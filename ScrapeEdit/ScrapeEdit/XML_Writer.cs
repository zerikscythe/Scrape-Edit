using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace ScrapeEdit
{
    public static class XML_Writer
    {
        public static void Write(string xmlData, string xmlOutputPath)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(xmlOutputPath));

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlData);
                using (var writer = XmlWriter.Create(xmlOutputPath, new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "\t",
                    NewLineOnAttributes = false,
                    Encoding = Encoding.UTF8
                }))
                {
                    doc.Save(writer);
                }
            }
            catch (Exception ex)
            {
                //return $"Error writing XML file: {ex.Message}";
            }
        }

    }
}
