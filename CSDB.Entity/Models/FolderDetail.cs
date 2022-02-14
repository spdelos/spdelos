// Decompiled with JetBrains decompiler
// Type: CSDB.Entity.Models.FolderDetail
// Assembly: CSDB.Entity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F87B454-8F51-476B-A340-3FC2E4AAC63A
// Assembly location: C:\Users\asus\Downloads\123\publish\CSDB.Entity.dll

using System;
using System.ComponentModel.DataAnnotations;

namespace CSDB.Entity.Models
{
  public class FolderDetail : CommonFields
  {
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Path { get; set; }

    [Required]
    public bool IsFolder { get; set; }
  }
}
