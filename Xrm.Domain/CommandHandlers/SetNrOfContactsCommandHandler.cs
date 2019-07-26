using Xrm.Domain.Commands;
using Xrm.Domain.Events;
using Xrm.Domain.Queries;
using Xrm.Models.Attrbutes;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.CommandHandlers
{
    public class SetNrOfContactsCommandHandler : CommandHandler<SetNrOfContactsCommand, VoidEvent>
    {
        public SetNrOfContactsCommandHandler(IOrganizationServiceWrapper orgServiceWrapper, IEventBus eventBus, 
            AccountQueries accountQueries, [InUserContext] AccountQueries accountQueriesAsUser) 
            : base(orgServiceWrapper, eventBus)
        {
        }

        public override VoidEvent Execute(SetNrOfContactsCommand command)
        {
            return VoidEvent;
        }
    }
}
