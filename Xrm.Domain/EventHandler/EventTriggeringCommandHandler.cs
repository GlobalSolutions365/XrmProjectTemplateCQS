using Xrm.Domain.Commands;
using Xrm.Domain.Events;
using Xrm.Models.Flow;

namespace Xrm.Domain.EventHandler
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
