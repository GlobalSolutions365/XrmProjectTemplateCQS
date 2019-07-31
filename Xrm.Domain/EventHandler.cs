using Microsoft.Xrm.Sdk;
using System;
using Xrm.Models.Interfaces;

namespace Xrm.Domain
{
    public abstract class EventHandler<TEvent, TResultEvent> : IHandleEvent<TEvent> 
        where TEvent : IEvent 
        where TResultEvent : IEvent
    {
        protected readonly IOrganizationServiceWrapper orgServiceWrapper;
        private readonly IEventBus eventBus;

        public EventHandler(IOrganizationServiceWrapper orgServiceWrapper, IEventBus eventBus)
        {
            this.orgServiceWrapper = orgServiceWrapper ?? throw new ArgumentNullException(nameof(orgServiceWrapper));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public void Handle(TEvent @event)
        {
            if (!Validate(@event)) { return; }

            TResultEvent resultEvent = Execute(@event);

            if (resultEvent != null)
            {
                eventBus.NotifyListenersAbout(resultEvent);
            }
        }

        public virtual bool Validate(TEvent @event) { return true; }

        public abstract TResultEvent Execute(TEvent @event);

        protected Events.VoidEvent VoidEvent => new Events.VoidEvent();
    }
}
