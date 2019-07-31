using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XmlSchemaValidationTool
{
    public class XmlFile
    {
        #region Properties..
        public XDocument InnerXml { get; private set; }

        public string FilePath { get; private set; }

        public List<string> ValidationExceptions { get; set; }
        #endregion Properties..

        #region Constructors..
        public XmlFile(string filePath)
        {
            FilePath = filePath;
            InnerXml = XDocument.Parse(File.ReadAllText(FilePath));
            ValidationExceptions = new List<string>();
        }
        #endregion Constructors..

        #region Methods..
        public string GetValidationLog()
        {
            string ValidationLog = $"---- {Path.GetFileName(FilePath)} ----{Environment.NewLine}" +
                                   $"---- Validation Exceptions: [{ValidationExceptions.Count}] ----{Environment.NewLine}{Environment.NewLine}";

            ValidationExceptions.ForEach((exception) =>
            {
                ValidationLog += $"{exception}{Environment.NewLine}{Environment.NewLine}";
            });

            return ValidationLog;
        }
        #endregion Methods..
    }
}
