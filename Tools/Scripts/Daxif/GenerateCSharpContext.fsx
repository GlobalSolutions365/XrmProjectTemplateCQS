(**
SolutionUpdateCustomContext
*)

#load @"_Config.fsx"
open _Config
open DG.Daxif
open DG.Daxif.Common.Utility

let xrmContext = Path.toolsFolder ++ @"XrmContext\XrmContext.exe"
let businessDomain = Path.solutionRoot ++ @"Xrm.Models\Crm"

Solution.GenerateCSharpContext(Env.dev, xrmContext, businessDomain,
  solutions = [
    SolutionInfo.name
    ],
  entities = [
        "systemuser,businessunit,team,account,contact"
    ],
  extraArguments = [
    "namespace", "Xrm.Models.Crm"    
    "servicecontextname", "XrmContext"
    ])