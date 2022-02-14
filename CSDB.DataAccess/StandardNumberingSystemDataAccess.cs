// Decompiled with JetBrains decompiler
// Type: CSDB.DataAccess.Implementation.StandardNumberingSystemDataAccess
// Assembly: CSDB.DataAccess, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2DB40F69-A4CC-40F0-82B0-03FE7F8C67C3
// Assembly location: C:\Users\asus\Downloads\123\publish\CSDB.DataAccess.dll

using CSDB.DataAccess.Abstracts;
using CSDB.Entity;
using CSDB.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace CSDB.DataAccess.Implementation
{
  public class StandardNumberingSystemDataAccess : 
    DbRepository<StandardNumberingSystem>,
    IStandardNumberingSystemDataAccess,
    IRepository<StandardNumberingSystem>
  {
    public ApplicationDbContext Context { get; set; }

    public StandardNumberingSystemDataAccess(ApplicationDbContext context)
      : base((DbContext) context)
    {
      this.Context = context;
    }
  }
}
