using Microsoft.Xrm.Sdk;
using System;
using Xrm.Domain.Commands;
using Xrm.Domain.Events;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.CommandHandlers
{
    public class UpdateAccountNameHandler : CommandHandler<UpdateAccountName, VoidEvent>
    {
        public UpdateAccountNameHandler(IOrganizationService orgService, IEventBus eventBus) : base(orgService, eventBus)
        {
        }

        public override bool Validate(UpdateAccountName command)
        {
            return !String.IsNullOrWhiteSpace(command.Prefix);
        }

        public override VoidEvent Execute(UpdateAccountName command)
        {
            string newName = $"{command.Prefix}{command.TargetAccount.Name}";

            command.TargetAccount.Name = newName;

            return VoidEvent;
        }
    }
}
