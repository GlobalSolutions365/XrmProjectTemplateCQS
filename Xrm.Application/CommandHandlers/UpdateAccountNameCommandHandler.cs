using System;
using Xrm.Application.Commands;
using Xrm.Application.Events;
using Xrm.Domain.Interfaces;

namespace Xrm.Application.CommandHandlers
{
    public class UpdateAccountNameCommandHandler : CommandHandler<UpdateAccountNameCommand, VoidEvent>
    {
        public UpdateAccountNameCommandHandler(Domain.Flow.FlowArguments flowArgs) : base(flowArgs)
        {
        }

        public override bool Validate(UpdateAccountNameCommand command)
        {
            return !String.IsNullOrWhiteSpace(command.Prefix);
        }

        public override VoidEvent Execute(UpdateAccountNameCommand command)
        {
            string newName = $"{command.Prefix}{command.TargetAccount.Name}";

            command.TargetAccount.Name = newName;

            return VoidEvent;
        }
    }
}
