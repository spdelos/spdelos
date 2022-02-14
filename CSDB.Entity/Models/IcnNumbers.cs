// Decompiled with JetBrains decompiler
// Type: CSDB.Entity.Models.IcnNumbers
// Assembly: CSDB.Entity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F87B454-8F51-476B-A340-3FC2E4AAC63A
// Assembly location: C:\Users\asus\Downloads\123\publish\CSDB.Entity.dll

using System;
using System.ComponentModel.DataAnnotations;

namespace CSDB.Entity.Models
{
  public class IcnNumbers
  {
    [Key]
    public int Id { get; set; }

    public string IcnNumber { get; set; }

    public int DataModuleId { get; set; }

    public int ProjectId { get; set; }

    public string ImagePath { get; set; }

    public bool IsAllocated { get; set; }

    public string UpdatedBy { get; set; }

    public DateTime UpdatedOn { get; set; }
  }
}
