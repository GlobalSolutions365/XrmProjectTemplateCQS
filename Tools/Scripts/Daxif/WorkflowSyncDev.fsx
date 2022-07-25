(**
WorkflowSyncDev
*)

#load @"_Config.fsx"
open _Config
open DG.Daxif
open DG.Daxif.Common.Utility

let workflowDll = Path.solutionRoot ++ @"Xrm.WorkflowActivities\bin\Release\ILMerge\XRM.CONSIT.Xrm.WorkflowActivities.dll"

Workflow.Sync(Env.dev, workflowDll, SolutionInfo.name)
