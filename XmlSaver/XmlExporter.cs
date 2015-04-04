using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Books;

namespace XmlHandlers
{
    public class XmlExporter : IXmlFormatExporter
    {
        public void Export(IEnumerable<Book> books, string fileName)
        {
            XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8);
            writer.WriteStartDocument();
            writer.WriteStartElement("Books");
            foreach (Book b in books)
            {

                writer.WriteStartElement("Book");

                writer.WriteStartElement("Author");
                writer.WriteString(b.Author);
                writer.WriteEndElement();

                writer.WriteStartElement("Title");
                writer.WriteString(b.Title);
                writer.WriteEndElement();

                writer.WriteStartElement("PageCount");
                writer.WriteString(b.PageCount.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("Price");
                writer.WriteString(b.Price.ToString());
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();

            writer.Close();
        }

        public void ReadXml(out string xmlData, string fileName)
        {
            XmlTextReader reader = new XmlTextReader(fileName);

            xmlData = string.Empty;
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.XmlDeclaration:
                        xmlData += "<?xml version='1.0'?>";
                        break;
                    case XmlNodeType.Element:
                        xmlData += string.Format("<{0}>", reader.Name);
                        break;
                    case XmlNodeType.Text:
                        xmlData += reader.Value;
                        break;
                    case XmlNodeType.EndElement:
                        xmlData += string.Format("</{0}>", reader.Name);
                        break;
                }
            }

            reader.Close();
        }
    }
}
