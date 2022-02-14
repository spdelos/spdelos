// Decompiled with JetBrains decompiler
// Type: CSDB.Entity.Models.FolderUserLink
// Assembly: CSDB.Entity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F87B454-8F51-476B-A340-3FC2E4AAC63A
// Assembly location: C:\Users\asus\Downloads\123\publish\CSDB.Entity.dll

using System;
using System.ComponentModel.DataAnnotations;

namespace CSDB.Entity.Models
{
  public class FolderUserLink
  {
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid FolderId { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public string SharedBy { get; set; }

    [Required]
    public DateTime SharedDate { get; set; }

    public bool IsReadablePermission { get; set; }

    public bool IsReadandWritePermission { get; set; }
  }
}
