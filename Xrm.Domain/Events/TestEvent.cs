using Xrm.Models.Interfaces;

namespace Xrm.Domain.Events
{
    public class TestEvent : IEvent
    {
        public bool IsHandled1 { get; set; }
        public bool IsHandled2 { get; set; }
    }
}
