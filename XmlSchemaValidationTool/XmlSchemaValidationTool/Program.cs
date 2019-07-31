using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace XmlSchemaValidationTool
{
    class Program
    {
        #region MemberVariables..
        private static string[] _XmlFilePaths;
        private static string[] _SchemaFilePaths;
        private static List<XmlFile> _XmlFiles;
        private static XmlSchemaSet _XmlSchemaSet;
        #endregion MemberVariables..

        #region Methods..
        private static void Initialize()
        { 
            _XmlFiles = new List<XmlFile>();
            _XmlSchemaSet = new XmlSchemaSet();

            _XmlFilePaths = Directory.GetFiles(Environment.CurrentDirectory, "*.xml", SearchOption.AllDirectories);
            _SchemaFilePaths = Directory.GetFiles(Environment.CurrentDirectory, "*.xsd", SearchOption.TopDirectoryOnly);
        }

        static void Main(string[] args)
        {
            Initialize();

            Console.WriteLine($"==== Xml Schema Validation Tool ===={Environment.NewLine}{Environment.NewLine}" +
                              $"This tool will validate all .XML files in the current directory against an .XSD file in the current directory{Environment.NewLine}" +
                              $"Press any key to begin..");
            Console.ReadKey();
            Console.Clear();

            if (_SchemaFilePaths.Length == 0)
            {
                Console.WriteLine("Error - Schema File(s) Not Found. Be sure the .xsd file is saved in the same directory as this executable");
            }
            else if (_SchemaFilePaths.Length > 1)
            {
                Console.WriteLine("Error - More than one .XSD file was found in the current directory");
            }
            else
            {
                ValidateXmlFiles();
                PublishValidationResults();
            }

            Console.ReadKey();
        }

        private static void PublishValidationResults()
        {
            _XmlFiles.ForEach(xmlFile =>
            {
                Console.WriteLine(xmlFile.GetValidationLog());
            });

            string ValidationSummary = $"==== Validation Report ===={Environment.NewLine}" +
                                       $"Total Files:            {_XmlFiles.Count}{Environment.NewLine}" +
                                       $"Successful Validations: {_XmlFiles.Where(x => x.ValidationExceptions.Count == 0).Count()}{Environment.NewLine}" +
                                       $"Failed Validations:     {_XmlFiles.Where(x => x.ValidationExceptions.Count > 0).Count()}{Environment.NewLine}";

            Console.WriteLine(ValidationSummary);
        }

        private static void ValidateXmlFiles()
        {
            _XmlSchemaSet.Add(string.Empty, XmlReader.Create(new StreamReader(_SchemaFilePaths[0])));

            foreach (string xmlFilePath in _XmlFilePaths)
            {
                XmlFile XmlFile = new XmlFile(xmlFilePath);
                _XmlFiles.Add(XmlFile);

                try
                {
                    XmlFile.InnerXml.Validate(_XmlSchemaSet, (obj, e) =>
                    {
                        XmlFile.ValidationExceptions.Add(e.Message);
                    });
                }
                catch (Exception ex)
                {
                    XmlFile.ValidationExceptions.Add(ex.Message);
                }
            }
        }
        #endregion Methods..
    }
}
