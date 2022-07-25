using Xrm.Domain.Interfaces;

namespace Xrm.Application.Events
{
    public class TestEvent : IEvent
    {
        public bool IsHandled1 { get; set; }
        public bool IsHandled2 { get; set; }
    }
}
