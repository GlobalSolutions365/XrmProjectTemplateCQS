using Xrm.Domain.Flow;

namespace Xrm.Domain.Interfaces
{
    public interface IEventBus
    {
        void NotifyListenersAbout(IEvent @event, FlowArguments flowArguments);
    }
}
