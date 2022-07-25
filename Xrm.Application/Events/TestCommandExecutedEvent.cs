using Xrm.Domain.Interfaces;

namespace Xrm.Application.Events
{
    public class TestCommandExecutedEvent : IEvent
    {
        public static bool IsHandled = false;
    }
}
