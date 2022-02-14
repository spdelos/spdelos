// Decompiled with JetBrains decompiler
// Type: CSDB.Entity.Models.ApplicationSettings
// Assembly: CSDB.Entity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F87B454-8F51-476B-A340-3FC2E4AAC63A
// Assembly location: C:\Users\asus\Downloads\123\publish\CSDB.Entity.dll

namespace CSDB.Entity.Models
{
  public class ApplicationSettings
  {
    [System.ComponentModel.DataAnnotations.Key]
    public int Id { get; set; }

    public string Key { get; set; }

    public string Value { get; set; }
  }
}
