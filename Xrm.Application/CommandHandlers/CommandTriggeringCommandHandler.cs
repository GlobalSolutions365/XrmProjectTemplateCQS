using Xrm.Application.Commands;
using Xrm.Application.Events;
using Xrm.Domain.Flow;

namespace Xrm.Application.CommandHandlers
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
