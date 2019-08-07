using Microsoft.Xrm.Sdk;
using System;
using Xrm.Models.Flow;
using Xrm.Models.Interfaces;

namespace Xrm.Infrastructure
{
    public abstract class CommandHandler<TCommand, TResultEvent> : IHandleCommand<TCommand> where TCommand : ICommand where TResultEvent : IEvent
    {
        private readonly IEventBus eventBus;
        private readonly FlowArguments flowArgs;

        protected IOrganizationServiceWrapper OrgServiceWrapper { get; }
        protected ITracingService TracingService { get; }

        public CommandHandler(FlowArguments flowArgs)
        {
            this.flowArgs = flowArgs ?? throw new ArgumentNullException(nameof(flowArgs));

            this.OrgServiceWrapper = flowArgs.OrgServiceWrapper ?? throw new ArgumentNullException(nameof(flowArgs.OrgServiceWrapper));
            this.TracingService = flowArgs.TracingService ?? throw new ArgumentNullException(nameof(flowArgs.TracingService));
            this.eventBus = flowArgs.EventBus ?? throw new ArgumentNullException(nameof(flowArgs.EventBus));
        }

        public void Handle(TCommand command)
        {
            if (!Validate(command)) { return; }

            TResultEvent resultEvent = Execute(command);
            
            if(resultEvent != null && resultEvent.GetType() != typeof(VoidEvent))
            { 
                eventBus.NotifyListenersAbout(resultEvent, flowArgs);
            }
        }

        public virtual bool Validate(TCommand command) { return true; }

        public abstract TResultEvent Execute(TCommand command);

        protected VoidEvent VoidEvent => new VoidEvent();
    }
}
