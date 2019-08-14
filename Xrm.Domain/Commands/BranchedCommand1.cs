using Xrm.Models.Interfaces;

namespace Xrm.Domain.Commands
{
    public class BranchedCommand1 : ICommand
    {
        public bool ExecuteBranch1 { get; set; }
    }
}
