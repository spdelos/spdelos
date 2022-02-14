// Decompiled with JetBrains decompiler
// Type: CSDB.Entity.Models.Project
// Assembly: CSDB.Entity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F87B454-8F51-476B-A340-3FC2E4AAC63A
// Assembly location: C:\Users\asus\Downloads\123\publish\CSDB.Entity.dll

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDB.Entity.Models
{
  public class Project
  {
    [Key]
    public int Id { get; set; }

    public string EndItem { get; set; }

    public string Name { get; set; }

    public string Title { get; set; }

    public int IssueNoId { get; set; }

    public int ICNFormatId { get; set; }

    public int SNSSetId { get; set; }

    public int InformationCodeId { get; set; }

    public int LocationCodeId { get; set; }

    public string ModelIdentification { get; set; }

    public string SDC { get; set; }

    public int SubjectLength { get; set; }

    public string ProjectCode { get; set; }

    public int RPCId { get; set; }

    public bool TrackPercentComplete { get; set; }

    public bool CreateDefaultBrex { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string ModifiedBy { get; set; }

    public DateTime ModifiedDate { get; set; }

    [NotMapped]
    public string InformationCodeVarient { get; set; }
  }
}
