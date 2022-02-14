// Decompiled with JetBrains decompiler
// Type: CSDB.Entity.Models.DataModule
// Assembly: CSDB.Entity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F87B454-8F51-476B-A340-3FC2E4AAC63A
// Assembly location: C:\Users\asus\Downloads\123\publish\CSDB.Entity.dll

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDB.Entity.Models
{
  public class DataModule
  {
    [Key]
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public int StandardNumberingSystemId { get; set; }

    public int InformationCodeId { get; set; }

    public string InformationCodeDesc { get; set; }

    public string SortInfoCodeBy { get; set; }

    [Required(ErrorMessage = "Title Required")]
    public string Title { get; set; }

    public string DMC { get; set; }

    public int DataModuleTypeId { get; set; }

    [Required(ErrorMessage = "Dissembly Code Variant is Required")]
    public string DisassemblyCodeVariant { get; set; }

    public int ItemLocationId { get; set; }

    public string AssocateEagleTalk { get; set; }

    public string LCN { get; set; }

    public string ALC { get; set; }

    public string LCNType { get; set; }

    public string TaskID { get; set; }

    public string RefNo { get; set; }

    public string Cage { get; set; }

    public int? Status { get; set; }

    public int IssueNoId { get; set; }

    public string InWork { get; set; }

    public string Code { get; set; }

    public string ENT { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string UpdatedBy { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string AssignedTo { get; set; }

    public string Path { get; set; }

    [NotMapped]
    public string InformationCodeVarient { get; set; }
  }
}
