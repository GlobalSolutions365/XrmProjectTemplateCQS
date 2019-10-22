using Xrm.Models.Crm;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.Commands
{
    public class CommandTriggeringCommand : ICommand
    {
        public Contact FromContact { get; set; }
    }
}
