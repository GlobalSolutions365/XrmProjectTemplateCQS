using Xrm.Models.Crm;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.Commands
{
    public class TestTransactionalCommand : ICommand
    {
        public Account TargetAccount { get; set; }
    }
}
