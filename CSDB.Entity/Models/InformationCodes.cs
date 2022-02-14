// Decompiled with JetBrains decompiler
// Type: CSDB.Entity.Models.InformationCodes
// Assembly: CSDB.Entity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F87B454-8F51-476B-A340-3FC2E4AAC63A
// Assembly location: C:\Users\asus\Downloads\123\publish\CSDB.Entity.dll

using System;
using System.ComponentModel.DataAnnotations;

namespace CSDB.Entity.Models
{
  public class InformationCodes
  {
    [Key]
    public int Id { get; set; }

    public string InformationCode { get; set; }

    public string Variant { get; set; }

    public string AssemblyCode { get; set; }

    public string AssemblyVariant { get; set; }

    public string DisAssemblyCode { get; set; }

    public string DisAssemblyVariant { get; set; }

    public string Description { get; set; }

    public int DataModuleId { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string UpdatedBy { get; set; }

    public DateTime UpdatedOn { get; set; }
  }
}
