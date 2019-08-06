using Xrm.Models.Flow;

namespace Xrm.Models.Interfaces
{
    public interface ICommandBus
    {
        void Handle(ICommand command, FlowArguments flowArguments);
    }
}
