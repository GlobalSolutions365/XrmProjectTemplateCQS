using Xrm.Models.Interfaces;

namespace Xrm.Domain.Events
{
    public class SampleEvent : IEvent
    {
        public int SomeValue { get; set; }
    }
}
