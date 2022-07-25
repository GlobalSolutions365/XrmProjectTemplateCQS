using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebResourceSync
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args?.Length == 0)
            {
                Console.WriteLine("The path to the mappings.json file is required");
                return;
            }

            string mappingFilePath = args[0];
            FileInfo mappingFileInfo = new FileInfo(mappingFilePath);

            if (args.Length == 2)
            {
                int delay = int.Parse(args[1]);
                Console.WriteLine($"Waiting {delay}ms...");
                Thread.Sleep(delay);
            }

            string rootFolder = mappingFileInfo.DirectoryName;

            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["CRM"].ConnectionString;
            CrmServiceClient orgService = new CrmServiceClient(connStr);

            if (!orgService.IsReady)
            {
                Console.WriteLine(orgService.LastCrmError);
                return;
            }

            Console.WriteLine($"Connected to \"{orgService.ConnectedOrgFriendlyName}\" ({orgService.CrmConnectOrgUriActual.Host})");

            Mapping[] mappings = JsonConvert.DeserializeObject<Mapping[]>(File.ReadAllText(mappingFilePath));

            Console.WriteLine($"Processing mapping file...");

            using (XrmContext xrmContext = new XrmContext(orgService))
            {
                int mappingNr = 0;
                int mappingCount = mappings.Length;

                foreach (Mapping mapping in mappings)
                {
                    mappingNr++;

                    string fileContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(ReadFileContent(rootFolder, mapping.Source)));

                    var existing = xrmContext.WebResourceSet.Where(wr => wr.Name == mapping.Target).Select(wr => new { wr.WebResourceId, wr.Content }).FirstOrDefault();
                    Guid? webResourceId = existing?.WebResourceId;

                    if (webResourceId != null)
                    {
                        if (existing.Content != fileContent)
                        {
                            Console.WriteLine($"{mappingNr}/{mappingCount}: Updating {mapping.Target} ...");

                            WebResource webResource = new WebResource
                            {
                                WebResourceId = webResourceId.Value,
                                Content = fileContent
                            };
                            orgService.Update(webResource);
                        }
                        else
                        {
                            Console.WriteLine($"{mappingNr}/{mappingCount}: No changes detected in {mapping.Target} ...");
                            continue;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{mappingNr}/{mappingCount}: Creating {mapping.Target} ...");

                        WebResource webResource = new WebResource
                        {
                            Name = mapping.Target,
                            Content = fileContent,
                            DisplayName = mapping.Target,
                            WebResourceType = TypeFromExtension(mapping.Source)
                        };
                        webResourceId = orgService.Create(webResource);
                    }

                    Publish(orgService, webResourceId.Value);
                }
            }
        }

        private static string ReadFileContent(string rootFolder, string source)
        {
            source = source.Replace("/", "\\");
            string path = Path.Combine(rootFolder, source);

            return File.ReadAllText(path);
        }

        private static void Publish(CrmServiceClient orgService, Guid webResourceId)
        {
            Console.WriteLine("\t\tPublishing...");

            string webrescXml = $"<importexportxml><webresources><webresource>{webResourceId}</webresource></webresources></importexportxml>";

            PublishXmlRequest publishxmlrequest = new PublishXmlRequest
            {
                ParameterXml = String.Format(webrescXml)
            };

            orgService.Execute(publishxmlrequest);
        }

        private static WebResource_WebResourceType TypeFromExtension(string source)
        {
            if (source.EndsWith("js"))
            {
                return WebResource_WebResourceType.ScriptJScript;
            }
            else if (source.EndsWith("html"))
            {
                return WebResource_WebResourceType.WebpageHTML;
            }
            else if (source.EndsWith("css"))
            {
                return WebResource_WebResourceType.StyleSheetCSS;
            }
            else if (source.EndsWith("png"))
            {
                return WebResource_WebResourceType.PNGformat;
            }
            else if (source.EndsWith("jpg"))
            {
                return WebResource_WebResourceType.JPGformat;
            }
            else if (source.EndsWith("gif"))
            {
                return WebResource_WebResourceType.GIFformat;
            }
            else if (source.EndsWith("ico"))
            {
                return WebResource_WebResourceType.ICOformat;
            }
            else if (source.EndsWith("resx"))
            {
                return WebResource_WebResourceType.StringRESX;
            }
            else if (source.EndsWith("xap"))
            {
                return WebResource_WebResourceType.SilverlightXAP;
            }
            else if (source.EndsWith("xsl"))
            {
                return WebResource_WebResourceType.StyleSheetXSL;
            }
            else if (source.EndsWith("svg"))
            {
                return WebResource_WebResourceType.VectorformatSVG;
            }
            else if (source.EndsWith("xml"))
            {
                return WebResource_WebResourceType.DataXML;
            }
            else
            {
                throw new Exception($"File {source} is not of a supported type.");
            }
        }
    }
}
