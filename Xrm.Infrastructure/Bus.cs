using Microsoft.Xrm.Sdk;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xrm.Models.Interfaces;

namespace Xrm.Base
{
    public class Bus : ICommandBus, IEventBus
    {
        private readonly Container container = new Container();

        public Bus(IOrganizationService orgService)
        {
            Assembly domain = typeof(Domain.Locator).Assembly;

            container.RegisterInstance(typeof(IEventBus), this);
            container.RegisterInstance(orgService);
            container.Register(typeof(IHandleCommand<>), domain);
            container.Collection.Register(typeof(IHandleEvent<>), domain);

            container.Verify();
        }

        public void Handle(ICommand command)
        {
            var type = typeof(IHandleCommand<>).MakeGenericType(command.GetType());
            dynamic handler = container.GetInstance(type);
            handler.Execute((dynamic)command);
        }

        public void NotifyListenersAbout(IEvent @event)
        {
            var type = typeof(IHandleEvent<>).MakeGenericType(@event.GetType());
            IEnumerable<dynamic> listeners = container.GetAllInstances(type);
            foreach(dynamic listener in listeners)
            {
                listener.Handle((dynamic)@event);
            }
        }
    }
}
