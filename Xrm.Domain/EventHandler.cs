using Xrm.Models.Interfaces;

namespace Xrm.Domain
{
    public class EventHandler<TEvent, TResult> : IHandleEvent<TEvent> where TEvent : IEvent
    {
        public void Handle(IEvent @event)
        {
            if (!Validate(@event)) { return; }
            TResult result = Execute(@event);
            Notify(@event, result);
        }

        protected virtual bool Validate(IEvent @event) { return true; }

        protected virtual TResult Execute(IEvent command) { return default; }

        protected virtual void Notify(IEvent command, TResult result) { }
    }
}
