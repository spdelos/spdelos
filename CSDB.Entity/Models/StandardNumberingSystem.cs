// Decompiled with JetBrains decompiler
// Type: CSDB.Entity.Models.StandardNumberingSystem
// Assembly: CSDB.Entity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F87B454-8F51-476B-A340-3FC2E4AAC63A
// Assembly location: C:\Users\asus\Downloads\123\publish\CSDB.Entity.dll

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CSDB.Entity.Models
{
  public class StandardNumberingSystem
  {
    [Key]
    public int Id { get; set; }

    public string Code { get; set; }

    public string Description { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string UpdatedBy { get; set; }

    public DateTime UpdatedOn { get; set; }

    [DisplayName("Parent")]
    public int ParentId { get; set; }
  }
}
