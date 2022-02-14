// Decompiled with JetBrains decompiler
// Type: CSDB.Entity.Models.BrexRules
// Assembly: CSDB.Entity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F87B454-8F51-476B-A340-3FC2E4AAC63A
// Assembly location: C:\Users\asus\Downloads\123\publish\CSDB.Entity.dll

using System;
using System.ComponentModel.DataAnnotations;

namespace CSDB.Entity.Models
{
  public class BrexRules
  {
    [Key]
    public int Id { get; set; }

    public string Group { get; set; }

    public string RuleName { get; set; }

    public string Xml { get; set; }

    public string DMTYPE { get; set; }

    public int IssueNoId { get; set; }

    public string XmlTag { get; set; }

    public string SubXmlTag { get; set; }

    public string Type { get; set; }

    public int Length { get; set; }

    public string RangeValue { get; set; }

    public string MatchValue { get; set; }

    public string AttributeName { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string UpdatedBy { get; set; }

    public DateTime UpdatedOn { get; set; }
  }
}
