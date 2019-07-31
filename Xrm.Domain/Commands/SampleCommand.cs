using Microsoft.Xrm.Sdk;
using Xrm.Models.Interfaces;

namespace Xrm.Domain.Commands
{
    public class SampleCommand : ICommand
    {
        public EntityReference TargetRef { get; set; }
    }
}
