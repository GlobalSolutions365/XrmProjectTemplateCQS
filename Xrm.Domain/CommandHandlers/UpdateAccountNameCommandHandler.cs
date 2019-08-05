using System;
using Xrm.Domain.Commands;
using Xrm.Domain.Events;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.CommandHandlers
{
    public class UpdateAccountNameCommandHandler : CommandHandler<UpdateAccountNameCommand, VoidEvent>
    {
        public UpdateAccountNameCommandHandler(Models.Flow.FlowArguments flowArgs) : base(flowArgs)
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
