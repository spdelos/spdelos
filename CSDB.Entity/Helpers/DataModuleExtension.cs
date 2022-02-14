// Decompiled with JetBrains decompiler
// Type: CSDB.Entity.Helpers.DataModuleExtension
// Assembly: CSDB.Entity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F87B454-8F51-476B-A340-3FC2E4AAC63A
// Assembly location: C:\Users\asus\Downloads\123\publish\CSDB.Entity.dll

using CSDB.Entity.Models;
using CSDB.Entity.ViewModels;
using System;
using System.Xml;

namespace CSDB.Entity.Helpers
{
  public static class DataModuleExtension
  {
    public static XmlDocument AddDefaultDataFor5(
      this XmlDocument doc,
      DataModule module,
      Project project,
      InformationCodes codes,
      LocationCode locCode)
    {
      DMCDataViewModel dmcDataViewModel = new DMCDataViewModel();
      doc.GetElementsByTagName("techName")[0].InnerXml = module.Title.Split(new[] { '-' }, StringSplitOptions.None)[0].Trim();
      if (((XmlNode) doc).SelectNodes("//dmodule/idstatus/dmtitle/infoName").Count == 0)
      {
        doc.GetElementsByTagName("dmTitle")[0].AppendChild((XmlNode) doc.CreateElement("infoName"));
        doc.GetElementsByTagName("infoName")[0].InnerXml = module.Title.Split(new[] { '-' }, StringSplitOptions.None)[1].Trim();
      }
      else
        doc.GetElementsByTagName("infoName")[0].InnerXml = module.Title.Split(new[] { '-' }, StringSplitOptions.None)[1].Trim();
      XmlAttribute attribute1 = doc.CreateAttribute("assyCode");
      ((XmlNode) attribute1).Value = module.DMC.Split(new[] { '-' }, StringSplitOptions.None)[4].Trim();
      XmlAttribute attribute2 = doc.CreateAttribute("disassyCode");
      ((XmlNode) attribute2).Value = module.DMC.Split(new[] { '-' }, StringSplitOptions.None)[5].Replace(module.DisassemblyCodeVariant, "").Trim();
      XmlAttribute attribute3 = doc.CreateAttribute("disassyCodeVariant");
      ((XmlNode) attribute3).Value = module.DisassemblyCodeVariant;
      XmlAttribute attribute4 = doc.CreateAttribute("infoCode");
      ((XmlNode) attribute4).Value = codes.InformationCode;
      XmlAttribute attribute5 = doc.CreateAttribute("infoCodeVariant");
      ((XmlNode) attribute5).Value = codes.Variant;
      XmlAttribute attribute6 = doc.CreateAttribute("itemLocationCode");
      ((XmlNode) attribute6).Value = locCode.Code;
      XmlAttribute attribute7 = doc.CreateAttribute("learnCode");
      XmlAttribute attribute8 = doc.CreateAttribute("learnEventCode");
      XmlAttribute attribute9 = doc.CreateAttribute("modelIdentCode");
      ((XmlNode) attribute9).Value = project.ModelIdentification;
      XmlAttribute attribute10 = doc.CreateAttribute("subSubSystemCode");
      ((XmlNode) attribute10).Value = module.DMC.Split(new[] { '-' }, StringSplitOptions.None)[3].Trim()[1].ToString();
      XmlAttribute attribute11 = doc.CreateAttribute("subSystemCode");
      ((XmlNode) attribute11).Value = module.DMC.Split(new[] { '-' }, StringSplitOptions.None)[3].Trim()[0].ToString();
      XmlAttribute attribute12 = doc.CreateAttribute("systemCode");
      ((XmlNode) attribute12).Value = module.DMC.Split(new[] { '-' }, StringSplitOptions.None)[2].Trim();
      XmlAttribute attribute13 = doc.CreateAttribute("systemDiffCode");
      ((XmlNode) attribute13).Value = project.SDC;
      doc.GetElementsByTagName("dmCode")[0].Attributes.Append(attribute1);
      doc.GetElementsByTagName("dmCode")[0].Attributes.Append(attribute2);
      doc.GetElementsByTagName("dmCode")[0].Attributes.Append(attribute3);
      doc.GetElementsByTagName("dmCode")[0].Attributes.Append(attribute4);
      doc.GetElementsByTagName("dmCode")[0].Attributes.Append(attribute5);
      doc.GetElementsByTagName("dmCode")[0].Attributes.Append(attribute6);
      doc.GetElementsByTagName("dmCode")[0].Attributes.Append(attribute7);
      doc.GetElementsByTagName("dmCode")[0].Attributes.Append(attribute8);
      doc.GetElementsByTagName("dmCode")[0].Attributes.Append(attribute9);
      doc.GetElementsByTagName("dmCode")[0].Attributes.Append(attribute10);
      doc.GetElementsByTagName("dmCode")[0].Attributes.Append(attribute11);
      doc.GetElementsByTagName("dmCode")[0].Attributes.Append(attribute12);
      doc.GetElementsByTagName("dmCode")[0].Attributes.Append(attribute13);
      return doc;
    }

    public static XmlDocument AddDefaultDataFor3(
      this XmlDocument doc,
      DataModule module,
      Project project,
      InformationCodes codes,
      LocationCode locCode)
    {
      DMCDataViewModel dmcDataViewModel = new DMCDataViewModel();
      doc.GetElementsByTagName("techname")[0].InnerXml = module.Title.Split(new[] { '-' }, StringSplitOptions.None)[0].Trim();
      if (doc.GetElementsByTagName("infoname").Count == 0)
      {
        doc.GetElementsByTagName("techname")[0].AppendChild((XmlNode) doc.CreateElement("infoname"));
        doc.GetElementsByTagName("infoname")[0].InnerXml = module.Title.Split(new[] { '-' }, StringSplitOptions.None)[1].Trim();
      }
      else
        doc.GetElementsByTagName("infoname")[0].InnerXml = module.Title.Split(new[] { '-' }, StringSplitOptions.None)[1].Trim();
      if (doc.GetElementsByTagName("modelic").Count == 0)
      {
        doc.GetElementsByTagName("dmodule")[0].AppendChild((XmlNode) doc.CreateElement("modelic"));
        doc.GetElementsByTagName("modelic")[0].InnerXml = project.ModelIdentification;
      }
      else
        doc.GetElementsByTagName("modelic")[0].InnerXml = project.ModelIdentification;
      if (doc.GetElementsByTagName("sdc").Count == 0)
      {
        doc.GetElementsByTagName("dmodule")[0].AppendChild((XmlNode) doc.CreateElement("sdc"));
        doc.GetElementsByTagName("sdc")[0].InnerXml = project.SDC;
      }
      else
        doc.GetElementsByTagName("sdc")[0].InnerXml = project.SDC;
      if (doc.GetElementsByTagName("chapnum").Count == 0)
      {
        doc.GetElementsByTagName("dmodule")[0].AppendChild((XmlNode) doc.CreateElement("chapnum"));
        if (doc.GetElementsByTagName("chapnum")[0] != null)
          doc.GetElementsByTagName("chapnum")[0].InnerXml = module.DMC.Split(new[] { '-' }, StringSplitOptions.None)[2].Trim();
      }
      else if (doc.GetElementsByTagName("chapnum")[0] != null)
        doc.GetElementsByTagName("chapnum")[0].InnerXml = module.DMC.Split(new[] { '-' }, StringSplitOptions.None)[2].Trim();
      if (doc.GetElementsByTagName("section").Count == 0)
      {
        doc.GetElementsByTagName("dmodule")[0].AppendChild((XmlNode) doc.CreateElement("section"));
        doc.GetElementsByTagName("section")[0].InnerXml = module.DMC.Split(new[] { '-' }, StringSplitOptions.None)[2].Trim();
      }
      else
        doc.GetElementsByTagName("section")[0].InnerXml = module.DMC.Split(new[] { '-' }, StringSplitOptions.None)[3].Trim();
      char ch;
      if (doc.GetElementsByTagName("subsect").Count == 0)
      {
        doc.GetElementsByTagName("dmodule")[0].AppendChild((XmlNode) doc.CreateElement("subsect"));
        doc.GetElementsByTagName("subsect")[0].InnerXml = module.DMC.Split(new[] { '-' }, StringSplitOptions.None)[4].Trim()[0].ToString();
      }
      else
      {
        XmlNode xmlNode = doc.GetElementsByTagName("subsect")[0];
        ch = module.DMC.Split(new[] { '-' }, StringSplitOptions.None)[3].Trim()[0];
        string str = ch.ToString();
        xmlNode.InnerXml = str;
      }
      if (doc.GetElementsByTagName("subject").Count == 0)
      {
        doc.GetElementsByTagName("dmodule")[0].AppendChild((XmlNode) doc.CreateElement("subject"));
        XmlNode xmlNode = doc.GetElementsByTagName("subject")[0];
        ch = module.DMC.Split(new[] { '-' }, StringSplitOptions.None)[4].Trim()[1];
        string str = ch.ToString();
        xmlNode.InnerXml = str;
      }
      else
      {
        XmlNode xmlNode = doc.GetElementsByTagName("subject")[0];
        ch = module.DMC.Split(new[] { '-' }, StringSplitOptions.None)[3].Trim()[1];
        string str = ch.ToString();
        xmlNode.InnerXml = str;
      }
      if (doc.GetElementsByTagName("discode").Count == 0)
      {
        doc.GetElementsByTagName("dmodule")[0].AppendChild((XmlNode) doc.CreateElement("discode"));
        doc.GetElementsByTagName("discode")[0].InnerXml = module.DMC.Split(new[] { '-' }, StringSplitOptions.None)[5].Replace(module.DisassemblyCodeVariant, "").Trim();
      }
      else
        doc.GetElementsByTagName("discode")[0].InnerXml = module.DMC.Split(new[] { '-' }, StringSplitOptions.None)[5].Replace(module.DisassemblyCodeVariant, "").Trim();
      if (doc.GetElementsByTagName("discodev").Count == 0)
      {
        doc.GetElementsByTagName("dmodule")[0].AppendChild((XmlNode) doc.CreateElement("discodev"));
        doc.GetElementsByTagName("discodev")[0].InnerXml = module.DisassemblyCodeVariant;
      }
      else
        doc.GetElementsByTagName("discodev")[0].InnerXml = module.DisassemblyCodeVariant;
      if (doc.GetElementsByTagName("infocode").Count == 0)
      {
        doc.GetElementsByTagName("dmodule")[0].AppendChild((XmlNode) doc.CreateElement("infocode"));
        doc.GetElementsByTagName("infocode")[0].InnerXml = codes.InformationCode;
      }
      else
        doc.GetElementsByTagName("infocode")[0].InnerXml = codes.InformationCode;
      if (doc.GetElementsByTagName("infocodev").Count == 0)
      {
        doc.GetElementsByTagName("dmodule")[0].AppendChild((XmlNode) doc.CreateElement("infocodev"));
        doc.GetElementsByTagName("infocodev")[0].InnerXml = codes.Variant;
      }
      else
        doc.GetElementsByTagName("infocodev")[0].InnerXml = codes.Variant;
      if (doc.GetElementsByTagName("itemloc").Count == 0)
      {
        doc.GetElementsByTagName("dmodule")[0].AppendChild((XmlNode) doc.CreateElement("itemloc"));
        doc.GetElementsByTagName("itemloc")[0].InnerXml = locCode.Code;
      }
      else
        doc.GetElementsByTagName("itemloc")[0].InnerXml = locCode.Code;
      return doc;
    }
  }
}
