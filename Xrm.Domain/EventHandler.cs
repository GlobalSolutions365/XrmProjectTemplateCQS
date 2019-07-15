using Microsoft.Xrm.Sdk;
using System;
using Xrm.Models.Interfaces;

namespace Xrm.Domain
{
    public class EventHandler<TEvent, TPostEvent> : IHandleEvent<TEvent> 
        where TEvent : IEvent 
        where TPostEvent : IEvent
    {
        protected readonly IOrganizationService orgService;
        private readonly IEventBus eventBus;

        public EventHandler(IOrganizationService orgService, IEventBus eventBus)
        {
            this.orgService = orgService ?? throw new ArgumentNullException(nameof(orgService));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public void Handle(TEvent @event)
        {
            if (!Validate(@event)) { return; }

            TPostEvent postEvent = Execute(@event);

            if (postEvent != null)
            {
                eventBus.NotifyListenersAbout(postEvent);
            }
        }

        public virtual bool Validate(TEvent @event) { return true; }

        public virtual TPostEvent Execute(TEvent @event) { return default; }
    }
}
