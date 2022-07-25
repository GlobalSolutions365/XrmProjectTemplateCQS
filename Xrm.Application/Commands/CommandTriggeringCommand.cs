using Xrm.Domain.Crm;
using Xrm.Domain.Interfaces;

namespace Xrm.Application.Commands
{
    public class CommandTriggeringCommand : ICommand
    {
        public Contact FromContact { get; set; }
    }
}
