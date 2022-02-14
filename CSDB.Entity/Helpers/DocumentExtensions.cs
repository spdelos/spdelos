// Decompiled with JetBrains decompiler
// Type: CSDB.Entity.Helpers.DocumentExtensions
// Assembly: CSDB.Entity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F87B454-8F51-476B-A340-3FC2E4AAC63A
// Assembly location: C:\Users\asus\Downloads\123\publish\CSDB.Entity.dll

using System.Xml;
using System.Xml.Linq;

namespace CSDB.Entity.Helpers
{
  public static class DocumentExtensions
  {
    public static void InsertAt(this XmlNode node, XmlNode insertingNode, int index = 0)
    {
      if (insertingNode == null)
        return;
      if (index < 0)
        index = 0;
      XmlNodeList childNodes = node.ChildNodes;
      int count = childNodes.Count;
      if (index >= count)
      {
        node.AppendChild(insertingNode);
      }
      else
      {
        XmlNode xmlNode = childNodes[index];
        node.InsertBefore(insertingNode, xmlNode);
      }
    }

    public static XmlDocument ToXmlDocument(this XDocument xDocument)
    {
      XmlDocument xmlDocument = new XmlDocument();
      using (XmlReader reader = xDocument.CreateReader())
        xmlDocument.Load(reader);
      return xmlDocument;
    }

    public static XDocument ToXDocument(this XmlDocument xmlDocument)
    {
      using (XmlNodeReader reader = new XmlNodeReader((XmlNode) xmlDocument))
      {
        int content = (int) ((XmlReader) reader).MoveToContent();
        return XDocument.Load((XmlReader) reader);
      }
    }
  }
}
