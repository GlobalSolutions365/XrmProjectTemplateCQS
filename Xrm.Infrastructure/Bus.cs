using Autofac;
using Autofac.Core;
using DateProvider;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xrm.Application;
using Xrm.Application.Interfaces;
using Xrm.Domain.Attributes;
using Xrm.Domain.Flow;
using Xrm.Domain.Interfaces;

namespace Xrm.Infrastructure
{
    public class Bus : ICommandBus, IEventBus
    {
        public bool DoNotPropagateEvents { get; set; } = false; // Useful for unit testing

        private readonly IContainer container = null;

        public Bus(IDateProvider dateProvider = null, IConfigurationReader configurationReader = null, IHttpRequestExecutor httpRequestExector = null, IFileSystem fileSystem = null)
        {
            dateProvider = dateProvider ?? new SystemDateProvider();

            var builder = new ContainerBuilder();

            Assembly domain = typeof(Locator).Assembly;

            builder.RegisterInstance<IEventBus>(this);           
            builder.RegisterAssemblyTypes(domain).AsClosedTypesOf(typeof(IHandleCommand<>));
            builder.RegisterAssemblyTypes(domain).AsClosedTypesOf(typeof(IHandleEvent<>));
            builder.RegisterAssemblyTypes(domain).AsClosedTypesOf(typeof(CrmQuery<>));

            /// Add custom dependencies below
            builder.RegisterInstance(dateProvider);
            builder.RegisterInstance<IJsonHelper>(new JsonHelper.JsonHelper());
            builder.RegisterInstance<IXmlHelper>(new XmlHelper.XmlHelper());
            builder.RegisterInstance(configurationReader);
            builder.RegisterInstance(httpRequestExector ?? new PhysicalHttpRequestExecutor.HttpRequestExecutor());
            builder.RegisterInstance(fileSystem ?? new PhysicalFileSystem.FileSystem());
            /// --- End of custom added dependencties

            container = builder.Build();
        }

        public void Handle(ICommand command, FlowArguments flowArguments)
        {
            using (ILifetimeScope scope = container.BeginLifetimeScope())
            {
                var handlerType = typeof(IHandleCommand<>).MakeGenericType(command.GetType());

                dynamic handler = scope.Resolve(handlerType, FlowArgumentsResolver(flowArguments), QueryResolver(scope, flowArguments.OrgServiceWrapper));

                handler.Handle((dynamic)command);

                if(flowArguments.OrgServiceWrapper.TransactionalOrgService is TransactionalService transactionOrgService)
                {
                    transactionOrgService.Commit();
                }
                if (flowArguments.OrgServiceWrapper.TransactionalOrgServiceAsSystem is TransactionalService transactionOrgServiceAsSystem)
                {
                    transactionOrgServiceAsSystem.Commit();
                }
            }
        }

        public void NotifyListenersAbout(IEvent @event, FlowArguments flowArguments)
        {
            if (DoNotPropagateEvents) { return; }

            using (ILifetimeScope scope = container.BeginLifetimeScope())
            {
                var handlerType = typeof(IHandleEvent<>).MakeGenericType(@event.GetType());
                var enumerableOfHandlersType = typeof(IEnumerable<>).MakeGenericType(handlerType);

                dynamic listeners = scope.Resolve(enumerableOfHandlersType, FlowArgumentsResolver(flowArguments), QueryResolver(scope, flowArguments.OrgServiceWrapper));

                foreach (dynamic listener in listeners)
                {
                    listener.Handle((dynamic)@event);
                }
            }
        }

        private ResolvedParameter FlowArgumentsResolver(FlowArguments flowArguments)
        {
            return new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(FlowArguments),
                    (pi, ctx) => flowArguments                    
                );
        }

        private ResolvedParameter QueryResolver(ILifetimeScope scope, IOrganizationServiceWrapper orgServiceWrapper)
        {
            return new ResolvedParameter(
                    (pi, ctx) =>
                    {
                        // Determine if we're looking for a parameter that is of a type that extends CrmQuery<>
                        bool isCrmQuery = pi.ParameterType.IsClass
                                          && pi.ParameterType.BaseType.IsGenericType
                                          && pi.ParameterType.BaseType.GetGenericTypeDefinition() == typeof(CrmQuery<>);

                        return isCrmQuery;
                    },
                    (pi, ctx) =>
                    {
                        // Check if it has the [InUserContext] attribute
                        bool useUserContextService = pi.CustomAttributes.Any(attr => attr.AttributeType == typeof(InUserContextAttribute));

                        // Inject the correct CRM service reference
                        object resolvedQueryHandler = scope.Resolve(pi.ParameterType, new ResolvedParameter(
                            (_pi, _ctx) => _pi.ParameterType == typeof(IOrganizationService),
                            (_pi, _ctx) => useUserContextService ? orgServiceWrapper.OrgService : orgServiceWrapper.OrgServiceAsSystem
                        ));

                        return resolvedQueryHandler;
                    }
                );
        }
    }

}
