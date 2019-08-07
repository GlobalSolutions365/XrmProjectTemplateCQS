using Microsoft.Xrm.Sdk;
using System;
using Xrm.Models.Flow;
using Xrm.Models.Interfaces;

namespace Xrm.Infrastructure
{
    public abstract class EventHandler<TEvent, TResultEvent> : IHandleEvent<TEvent>
        where TEvent : IEvent
        where TResultEvent : IEvent
    {        
        private readonly IEventBus eventBus;
        private readonly FlowArguments flowArgs;

        protected IOrganizationServiceWrapper OrgServiceWrapper { get; }
        protected ITracingService TracingService { get; }

        public EventHandler(FlowArguments flowArgs)
        {
            this.flowArgs = flowArgs ?? throw new ArgumentNullException(nameof(flowArgs));

            this.OrgServiceWrapper = flowArgs.OrgServiceWrapper;
            this.TracingService = flowArgs.TracingService ?? throw new ArgumentNullException(nameof(flowArgs.TracingService));
            this.eventBus = flowArgs.EventBus;
        }

        public void Handle(TEvent @event)
        {
            if (!Validate(@event)) { return; }

            TResultEvent resultEvent = Execute(@event);

            if (resultEvent != null)
            {
                eventBus.NotifyListenersAbout(resultEvent, flowArgs);
            }
        }

        public virtual bool Validate(TEvent @event) { return true; }

        public abstract TResultEvent Execute(TEvent @event);

        protected VoidEvent VoidEvent => new VoidEvent();
    }
}
