﻿using Microsoft.Xrm.Sdk;
using Autofac;
using System.Collections.Generic;
using System.Reflection;
using Xrm.Models.Interfaces;
using Xrm.Domain;
using Autofac.Core;
using System.Linq;
using Xrm.Models.Attrbutes;
using Xrm.Domain.Queries;

namespace Xrm.Base
{
    public class Bus : ICommandBus, IEventBus
    {
        private readonly IContainer container = null;

        public Bus(IOrganizationServiceWrapper orgServiceWrapper)
        {
            var builder = new ContainerBuilder();

            Assembly domain = typeof(Locator).Assembly;

            builder.RegisterInstance<IEventBus>(this);
            builder.RegisterInstance(orgServiceWrapper);
            builder.RegisterAssemblyTypes(domain).AsClosedTypesOf(typeof(IHandleCommand<>));
            builder.RegisterAssemblyTypes(domain).AsClosedTypesOf(typeof(IHandleEvent<>));
            builder.RegisterAssemblyTypes(domain).AsClosedTypesOf(typeof(CrmQuery<>));

            container = builder.Build();
        }

        public void Handle(ICommand command)
        {
            using (ILifetimeScope scope = container.BeginLifetimeScope())
            {
                var handlerType = typeof(IHandleCommand<>).MakeGenericType(command.GetType());

                dynamic handler = scope.Resolve(handlerType, ContextBasedQueryOrgServiceResolver(scope));

                handler.Handle((dynamic)command);
            }
        }

        public void NotifyListenersAbout(IEvent @event)
        {
            using (ILifetimeScope scope = container.BeginLifetimeScope())
            {
                var handlerType = typeof(IHandleEvent<>).MakeGenericType(@event.GetType());
                var enumerableOfHandlersType = typeof(IEnumerable<>).MakeGenericType(handlerType);

                dynamic listeners = scope.Resolve(enumerableOfHandlersType, ContextBasedQueryOrgServiceResolver(scope));

                foreach (dynamic listener in listeners)
                {
                    listener.Handle((dynamic)@event);
                }
            }
        }

        private ResolvedParameter ContextBasedQueryOrgServiceResolver(ILifetimeScope scope)
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

                        // This contains both the system context and user context CRM service connections
                        IOrganizationServiceWrapper orgServiceWrapper = scope.Resolve<IOrganizationServiceWrapper>();

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
