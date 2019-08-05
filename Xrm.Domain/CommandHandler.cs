using Microsoft.Xrm.Sdk;
using System;
using Xrm.Models.Flow;
using Xrm.Models.Interfaces;

namespace Xrm.Domain
{
    public abstract class CommandHandler<TCommand, TResultEvent> : IHandleCommand<TCommand> where TCommand : ICommand where TResultEvent : IEvent
    {
        protected readonly IOrganizationServiceWrapper orgServiceWrapper;
        private readonly IEventBus eventBus;

        public CommandHandler(FlowArguments flowArgs)
        {
            flowArgs = flowArgs ?? throw new ArgumentNullException(nameof(flowArgs));

            this.orgServiceWrapper = flowArgs.OrgServiceWrapper;
            this.eventBus = flowArgs.EventBus;
        }

        public void Handle(TCommand command)
        {
            if (!Validate(command)) { return; }

            TResultEvent resultEvent = Execute(command);
            
            if(resultEvent != null && resultEvent.GetType() != typeof(Events.VoidEvent))
            { 
                eventBus.NotifyListenersAbout(resultEvent);
            }
        }

        public virtual bool Validate(TCommand command) { return true; }

        public abstract TResultEvent Execute(TCommand command);

        protected Events.VoidEvent VoidEvent => new Events.VoidEvent();
    }
}
