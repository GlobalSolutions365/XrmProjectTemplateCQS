using Xrm.Domain.Crm;
using Xrm.Domain.Interfaces;

namespace Xrm.Application.Commands
{
    public class TestTransactionalCommand : ICommand
    {
        public Account TargetAccount { get; set; }
    }
}
