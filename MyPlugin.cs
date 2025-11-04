using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace Sujan_Solution_Deployer
{
    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "Sujan Solution Deployer"),
        ExportMetadata("Description", "Automated solution deployment tool for Microsoft Dynamics 365 / Power Platform"),
        ExportMetadata("SmallImageBase64", SMALL_IMAGE),    // 32x32 pixels image
        ExportMetadata("BigImageBase64", BIG_IMAGE),        //  80x80 pixels image
        ExportMetadata("BackgroundColor", "#FFFFFF"),
        ExportMetadata("PrimaryFontColor", "Black"),
        ExportMetadata("SecondaryFontColor", "Gray")]
    public class MyPlugin : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new SolutionDeployerControl();
        }

        public MyPlugin()
        {

        }

        private const string SMALL_IMAGE = "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAMAAABEpIrGAAABuVBMVEVHcEw7n2EgeqAog8c9n2Angsoqgsc5n18nnYwempIogskvm28wnHJpYK47n2Ing8o7n2EmgckempM4nmE7nmE6nmAngskngsosm3gogshrXazwpzYijqs6nl8ngskeka05n2E5nmBcZrNmYK9oZLEngsk7n2MngckmgskdmpMdmpMwmm8dmZNoYK4ogsoogsnvpzRSZ7NoYK44nl1pYK7spjo6nmFgaLAfk6TVokU5n2Eqf8YogckngspoYbBfZLI7n2JpYbAsf8ZpYK/ypzIdmpMfm5EdmZIdmpPbpT7spDftpDcempM7n2JMb7UdmpXvpzYmgcklgcohjrK8olM6n2FpYK9pX67xqDY6nmAli7doY7JqY7JoYrI4ecM7n2FrXa9Fc7w7nmJjYrBOgoQlgsoogck6db8empHwpjTrpDggm44dmZLmoTuKkn0bmZLbnUNEh7PooTm8o1I/dL4ekqmJkoA6n2EdmZUmgcgej7AekawijrUika0ikashj7F2Z53jnz47n2JoYK05nF5Xabc4esOOSJoogsoog8rxpzZpY7Lgn0Cuh2MikK+ggG7BkFUjj7EjjrQDKAYWAAAAiHRSTlMAeQGFg/C7HRNfpAcYeYzKq2bRLqBRlb8KnSXqECWnHmhDSle9OPmRY9jmBIVn+torBK4Pm8CwOCxhOCiz4IRuuaJCkCOZN0PFDzSwV70H/kx0fqaTmKmJbGPD9OTLk5UQX+ctOFrWD3aCj3pq6FJs1pLGf3rNfl/6U+XjwO/tvQy+cBVdPoIHcRsjIQAAAfRJREFUOMvNk2dXGlEQhmdhi7KFXQHpvUrvHaTE3jVq1Bixpvfee7JgSfnFAT2c411DPuf5OO9z5s6dey5AF5qGf6HC1WrMTMt65Zr0c5XKNqNO9MjNaRWDfaKWZ/pA19dBJxFsmA73eDCGCcDtgQ7jN0yIgGvsaZyr5PN2GO3vEPSPIDNja3YPX/HksbVuZdo/dl6wqylOzeNmXtWtMEN3zgsBnKMoyryMR6PRCWs2awXT0H1kiATGc1u8LWc0Gh8/ffd2yTrtHxkNIAql0VBwpdVqXX59cvI+OzA3F0S3JsNscCa8+vX7zWz/i3FGsgsOA5hPpVK5pfV1eYwOjsFfhBpJkttVl6scgwuvotvCaRDEpnjt6uHh5ofJ+UsS4cvOjgaE5pngyhlbDyZRganwCdgnCOLb5saG6+PRz+NbkhZ7ew0wud21wleHIzN1/OPoJhJ/ZqvVFbYBbq2yjfbZo7tTyBCNJLtby5Tr3wXxlNLDexNIg1DZ5CUs1+tOn0gOtimKi0geSzosJW9p0bkqiAckqVVGmnJEmE0+EYsFktiuC2JRLvcSYQIVYHV4YUFQ7A+zPvFAqyXCFqVEyKwY4KV1N25oz+DVRwb1kiMAnHHWycYd4GtfodmMhC8IYAixIQNAQa9oY1Ho3T3/mOwU+J/4A5WHYFllvLW5AAAAAElFTkSuQmCC";
        private const string BIG_IMAGE = "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";

        private Assembly AssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            Assembly loadAssembly = null;
            Assembly currAssembly = Assembly.GetExecutingAssembly();

            var argName = args.Name.Substring(0, args.Name.IndexOf(","));

            List<AssemblyName> refAssemblies = currAssembly.GetReferencedAssemblies().ToList();
            var refAssembly = refAssemblies.Where(a => a.Name == argName).FirstOrDefault();

            // if the current unresolved assembly is referenced by our plugin, attempt to load
            if (refAssembly != null)
            {
                // load from the path to this plugin assembly, not host executable
                string dir = Path.GetDirectoryName(currAssembly.Location).ToLower();
                string folder = Path.GetFileNameWithoutExtension(currAssembly.Location);
                dir = Path.Combine(dir, folder);

                var assmbPath = Path.Combine(dir, $"{argName}.dll");

                if (File.Exists(assmbPath))
                {
                    loadAssembly = Assembly.LoadFrom(assmbPath);
                }
                else
                {
                    throw new FileNotFoundException($"Unable to locate dependency: {assmbPath}");
                }
            }

            return loadAssembly;
        }
    }
}