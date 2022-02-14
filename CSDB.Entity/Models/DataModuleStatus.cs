// Decompiled with JetBrains decompiler
// Type: CSDB.Entity.Models.DataModuleStatus
// Assembly: CSDB.Entity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F87B454-8F51-476B-A340-3FC2E4AAC63A
// Assembly location: C:\Users\asus\Downloads\123\publish\CSDB.Entity.dll

using System;
using System.ComponentModel.DataAnnotations;

namespace CSDB.Entity.Models
{
  public class DataModuleStatus
  {
    [Key]
    public int Id { get; set; }

    public int DataModuleId { get; set; }

    public string UploadedFile { get; set; }

    public string Status { get; set; }

    public string UploadedBy { get; set; }

    public DateTime UploadedOn { get; set; }

    public bool IsCurrentVersion { get; set; }
  }
}
