using Microsoft.Xrm.Sdk;
using System;
using Xrm.Domain.Flow;
using Xrm.Domain.Interfaces;

namespace Xrm.Application
{
    public abstract class EventHandler<TEvent, TResultEvent> : IHandleEvent<TEvent>
        where TEvent : IEvent
        where TResultEvent : IEvent
    {        
        private readonly ICommandBus cmdBus;
        private readonly IEventBus eventBus;
        private readonly FlowArguments flowArgs;

        protected IOrganizationServiceWrapper OrgServiceWrapper { get; }
        protected ITracingService TracingService { get; }

        public EventHandler(FlowArguments flowArgs)
        {
            this.flowArgs = flowArgs ?? throw new ArgumentNullException(nameof(flowArgs));

            this.OrgServiceWrapper = flowArgs.OrgServiceWrapper;
            this.TracingService = flowArgs.TracingService ?? throw new ArgumentNullException(nameof(flowArgs.TracingService));
            this.cmdBus = flowArgs.CommandBus ?? throw new ArgumentNullException(nameof(flowArgs.CommandBus));
            this.eventBus = flowArgs.EventBus ?? throw new ArgumentNullException(nameof(flowArgs.EventBus));
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

        public void TriggerCommand(ICommand command)
        {
            cmdBus.Handle(command, flowArgs);
        }

        public virtual bool Validate(TEvent @event) { return true; }

        public abstract TResultEvent Execute(TEvent @event);

        protected Events.VoidEvent VoidEvent => new Events.VoidEvent();
    }
}
