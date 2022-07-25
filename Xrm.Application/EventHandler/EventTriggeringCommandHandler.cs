using Xrm.Application.Commands;
using Xrm.Application.Events;
using Xrm.Domain.Flow;

namespace Xrm.Application.EventHandler
{
    public class EventTriggeringCommandHandler : EventHandler<EventTriggeringCommand, VoidEvent>
    {
        public EventTriggeringCommandHandler(FlowArguments flowArgs) : base(flowArgs)
        {
        }

        public override VoidEvent Execute(EventTriggeringCommand @event)
        {
            SetAccountNrOfContactsCommand childCommand = new SetAccountNrOfContactsCommand { FromContact = @event.FromContact };

            TriggerCommand(childCommand);

            return VoidEvent;
        }
    }
}
