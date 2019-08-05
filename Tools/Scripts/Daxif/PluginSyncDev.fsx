(**
PluginSyncDev
*)

#load @"_Config.fsx"
open _Config
open DG.Daxif
open DG.Daxif.Common.Utility

let pluginProjFile = Path.solutionRoot ++ @"Xrm.Plugins\Xrm.Plugins.csproj"
let pluginDll = Path.solutionRoot ++ @"Xrm.Plugins\bin\Release\ILMerge\Xrm.Plugin.dll"

Plugin.Sync(Env.dev, pluginDll, pluginProjFile, SolutionInfo.name)
