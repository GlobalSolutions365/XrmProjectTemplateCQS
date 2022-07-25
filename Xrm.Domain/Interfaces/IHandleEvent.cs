namespace Xrm.Domain.Interfaces
{
    public interface IHandleEvent<TEvent> where TEvent : IEvent
    {
        void Handle(TEvent @event);
    }
}
