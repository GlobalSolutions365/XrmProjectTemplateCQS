using Microsoft.Xrm.Sdk;
using System;
using Xrm.Domain.Flow;
using Xrm.Domain.Interfaces;

namespace Xrm.Application
{
    public abstract class CommandHandler<TCommand, TResultEvent> : IHandleCommand<TCommand> where TCommand : ICommand where TResultEvent : IEvent
    {
        private readonly ICommandBus cmdBus;
        private readonly IEventBus eventBus;
        private readonly FlowArguments flowArgs;

        protected IOrganizationServiceWrapper OrgServiceWrapper { get; }
        protected ITracingService TracingService { get; }

        public CommandHandler(FlowArguments flowArgs)
        {
            this.flowArgs = flowArgs ?? throw new ArgumentNullException(nameof(flowArgs));

            this.OrgServiceWrapper = flowArgs.OrgServiceWrapper ?? throw new ArgumentNullException(nameof(flowArgs.OrgServiceWrapper));
            this.TracingService = flowArgs.TracingService ?? throw new ArgumentNullException(nameof(flowArgs.TracingService));
            this.cmdBus = flowArgs.CommandBus ?? throw new ArgumentNullException(nameof(flowArgs.CommandBus));
            this.eventBus = flowArgs.EventBus ?? throw new ArgumentNullException(nameof(flowArgs.EventBus));
        }

        public void Handle(TCommand command)
        {
            if (!Validate(command)) { return; }

            TResultEvent resultEvent = Execute(command);
            
            if(resultEvent != null && resultEvent.GetType() != typeof(Events.VoidEvent))
            { 
                eventBus.NotifyListenersAbout(resultEvent, flowArgs);
            }
        }

        public void TriggerCommand(ICommand command)
        {
            cmdBus.Handle(command, flowArgs);
        }

        public virtual bool Validate(TCommand command) { return true; }

        public abstract TResultEvent Execute(TCommand command);

        protected Events.VoidEvent VoidEvent => new Events.VoidEvent();
    }
}
