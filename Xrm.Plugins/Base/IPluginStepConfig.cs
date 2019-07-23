using System;
using System.Collections.Generic;
using ImageTuple = System.Tuple<string, string, int, string>;

namespace Xrm.Plugin.Base
{
    interface IPluginStepConfig
    {
        string _LogicalName { get; }
        string _EventOperation { get; }
        int _ExecutionStage { get; }

        string _Name { get; }
        int _Deployment { get; }
        int _ExecutionMode { get; }
        int _ExecutionOrder { get; }
        string _FilteredAttributes { get; }
        Guid _UserContext { get; }
        IEnumerable<ImageTuple> GetImages();
    }
}
