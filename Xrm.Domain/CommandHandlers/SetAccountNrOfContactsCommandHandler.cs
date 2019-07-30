using System;
using Xrm.Domain.Commands;
using Xrm.Domain.Events;
using Xrm.Domain.Queries;
using Xrm.Models.Crm;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.CommandHandlers
{
    public class SetAccountNrOfContactsCommandHandler : CommandHandler<SetAccountNrOfContactsCommand, AccountNrOfContactsSetEvent>
    {
        private readonly AccountQueries accountQueries;

        public SetAccountNrOfContactsCommandHandler(IOrganizationServiceWrapper orgServiceWrapper, IEventBus eventBus,
            AccountQueries accountQueries)
            : base(orgServiceWrapper, eventBus)
        {
            this.accountQueries = accountQueries ?? throw new ArgumentNullException(nameof(accountQueries));
        }

        public override AccountNrOfContactsSetEvent Execute(SetAccountNrOfContactsCommand command)
        {
            command.FromContact = command.FromContact ?? throw new ArgumentNullException(nameof(command.FromContact));

            if (command.FromContact.ParentCustomerId == null)
            {
                return null;
            }

            int nrOfContacts = accountQueries.GetNrOfContacts(command.FromContact.ParentCustomerId.Id);

            Account account = new Account
            {
                Id = command.FromContact.ParentCustomerId.Id,
                Name = $"I have {nrOfContacts} contacts"
            };
            orgServiceWrapper.OrgServiceAsSystem.Update(account);

            return new AccountNrOfContactsSetEvent { TargetContact = command.FromContact };
        }
    }
}
