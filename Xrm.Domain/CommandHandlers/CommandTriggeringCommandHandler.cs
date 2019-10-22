using Xrm.Domain.Commands;
using Xrm.Domain.Events;
using Xrm.Models.Flow;

namespace Xrm.Domain.CommandHandlers
{
    public class CommandTriggeringCommandHandler : CommandHandler<CommandTriggeringCommand, VoidEvent>
    {
        public CommandTriggeringCommandHandler(FlowArguments flowArgs) : base(flowArgs)
        {
        }

        public override VoidEvent Execute(CommandTriggeringCommand command)
        {
            SetAccountNrOfContactsCommand childCommand = new SetAccountNrOfContactsCommand { FromContact = command.FromContact };

            TriggerCommand(childCommand);

            return VoidEvent;
        }
    }
}
