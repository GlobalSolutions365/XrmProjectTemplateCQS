using Xrm.Models.Interfaces;

namespace Xrm.Domain.Events
{
    public class TestCommandExecutedEvent : IEvent
    {
        public static bool IsHandled = false;
    }
}
