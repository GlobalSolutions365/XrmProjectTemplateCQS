using Xrm.Domain.Flow;

namespace Xrm.Domain.Interfaces
{
    public interface ICommandBus
    {
        void Handle(ICommand command, FlowArguments flowArguments);
    }
}
