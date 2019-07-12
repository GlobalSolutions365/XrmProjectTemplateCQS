using Microsoft.Xrm.Sdk;
using System;
using Xrm.Domain.Commands;
using Xrm.Domain.Events;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.CommandHandlers
{
    public class UpdateLastNameHandler : CommandHandler<UpdateLastName, LastNameUpdated>
    {
        public UpdateLastNameHandler(IOrganizationService orgService, IEventBus eventBus) : base(orgService, eventBus)
        {
        }

        protected override bool Validate(UpdateLastName command)
        {
            return !String.IsNullOrWhiteSpace(command.Prefix);
        }

        protected override LastNameUpdated Execute(UpdateLastName command)
        {
            string newLastName = $"{command.Prefix}{DateTime.Now.ToString("yyyyMMMddHHmmss")}";

            Entity contact = new Entity("contact");
            contact.Id = command.ContactId;
            contact["lastname"] = newLastName;

            orgService.Update(contact);

            return new LastNameUpdated
            {
                ContactId = command.ContactId,
                NewLastnameName = newLastName
            };
        }
    }
}
