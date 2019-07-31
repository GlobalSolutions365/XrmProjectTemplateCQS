using Xrm.Domain.Commands;
using Xrm.Domain.Events;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.CommandHandlers
{
    public class SampleCommandHandler : CommandHandler<SampleCommand, SampleEvent>
    {
        public SampleCommandHandler(IOrganizationServiceWrapper orgServiceWrapper, IEventBus eventBus) : base(orgServiceWrapper, eventBus)
        {
        }

        public override SampleEvent Execute(SampleCommand command)
        {
            return new SampleEvent();
        }
    }
}
