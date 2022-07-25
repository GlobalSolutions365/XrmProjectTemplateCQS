using Microsoft.Xrm.Sdk;
using Xrm.Domain.Interfaces;

namespace Xrm.Application.Interfaces
{
    public abstract class GenericActionCommand : ICommand
    {
        public IPluginExecutionContext PluginExecutionContext { get; set; }

        public string Request { get; set; }

        public string Response { get; set; }
    }
}
