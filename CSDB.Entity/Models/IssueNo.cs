// Decompiled with JetBrains decompiler
// Type: CSDB.Entity.Models.IssueNo
// Assembly: CSDB.Entity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F87B454-8F51-476B-A340-3FC2E4AAC63A
// Assembly location: C:\Users\asus\Downloads\123\publish\CSDB.Entity.dll

using System;
using System.ComponentModel.DataAnnotations;

namespace CSDB.Entity.Models
{
  public class IssueNo
  {
    [Key]
    public int Id { get; set; }

    [Display(Name = "Issue No")]
    public string Name { get; set; }

    public string Path { get; set; }

    public bool IsDelete { get; set; }

    public DateTime CreateOn { get; set; }

    public string CreatedBy { get; set; }
  }
}
