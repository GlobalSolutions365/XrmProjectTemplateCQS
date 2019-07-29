using System;
using Xrm.Domain.Commands;
using Xrm.Domain.Events;
using Xrm.Domain.Queries;
using Xrm.Models.Attrbutes;
using Xrm.Models.Crm;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.CommandHandlers
{
    public class SetAccountNrOfContactsCommandHandler : CommandHandler<SetAccountNrOfContactsCommand, VoidEvent>
    {
        private readonly AccountQueries accountQueriesAsUser;

        public SetAccountNrOfContactsCommandHandler(IOrganizationServiceWrapper orgServiceWrapper, IEventBus eventBus, 
            [InUserContext] AccountQueries accountQueriesAsUser) 
            : base(orgServiceWrapper, eventBus)
        {
            this.accountQueriesAsUser = accountQueriesAsUser ?? throw new ArgumentNullException(nameof(accountQueriesAsUser));
        }

        public override VoidEvent Execute(SetAccountNrOfContactsCommand command)
        {
            if(command?.FromContact?.ParentCustomerId == null)
            {
                return VoidEvent;
            }

            int nrOfContacts = accountQueriesAsUser.GetNrOfContacts(command.FromContact.ParentCustomerId.Id);

            Account account = new Account
            {
                Id = command.FromContact.ParentCustomerId.Id,
                Name = $"I have {nrOfContacts} contacts"
            };
            orgServiceWrapper.OrgServiceAsSystem.Update(account);

            return VoidEvent;
        }
    }
}
