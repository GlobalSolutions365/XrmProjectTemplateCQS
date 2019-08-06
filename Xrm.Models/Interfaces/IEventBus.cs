using Xrm.Models.Flow;

namespace Xrm.Models.Interfaces
{
    public interface IEventBus
    {
        void NotifyListenersAbout(IEvent @event, FlowArguments flowArguments);
    }
}
