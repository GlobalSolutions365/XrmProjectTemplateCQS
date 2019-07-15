using Microsoft.Xrm.Sdk;
using Autofac;
using System.Collections.Generic;
using System.Reflection;
using Xrm.Models.Interfaces;

namespace Xrm.Base
{
    public class Bus : ICommandBus, IEventBus
    {
        private readonly IContainer container = null;

        public Bus(IOrganizationService orgService)
        {
            var builder = new ContainerBuilder();

            Assembly domain = typeof(Domain.Locator).Assembly;

            builder.RegisterInstance<IEventBus>(this);
            builder.RegisterInstance<IOrganizationService>(orgService);
            builder.RegisterAssemblyTypes(domain).AsClosedTypesOf(typeof(IHandleCommand<>));
            builder.RegisterAssemblyTypes(domain).AsClosedTypesOf(typeof(IHandleEvent<>));

            container = builder.Build();
        }

        public void Handle(ICommand command)
        {
            using(ILifetimeScope scope = container.BeginLifetimeScope())
            { 
                var handlerType = typeof(IHandleCommand<>).MakeGenericType(command.GetType());

                dynamic handler = scope.Resolve(handlerType);

                handler.Execute((dynamic)command);
            }
        }

        public void NotifyListenersAbout(IEvent @event)
        {
            using (ILifetimeScope scope = container.BeginLifetimeScope())
            {             
                var handlerType = typeof(IHandleEvent<>).MakeGenericType(@event.GetType());
                var enumerableOfHandlersType = typeof(IEnumerable<>).MakeGenericType(handlerType);

                dynamic listeners = scope.Resolve(enumerableOfHandlersType);

                foreach (dynamic listener in listeners)
                {
                    listener.Handle((dynamic)@event);
                }
            }
        }
    }
}
