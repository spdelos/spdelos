// Decompiled with JetBrains decompiler
// Type: CSDB.Business.Implementation.StandardNumberingSystemBizModel
// Assembly: CSDB.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3E16F06E-96F5-45EA-8378-B8A3811262B8
// Assembly location: C:\Users\asus\Downloads\123\publish\CSDB.Business.dll

using CSDB.Business.Abstracts;
using CSDB.DataAccess.Abstracts;
using CSDB.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CSDB.Business.Implementation
{
  public class StandardNumberingSystemBizModel : IStandardNumberingSystemBizModel
  {
    public IStandardNumberingSystemDataAccess DataAccess { get; set; }

    public StandardNumberingSystemBizModel(IStandardNumberingSystemDataAccess dataAccess) => this.DataAccess = dataAccess;

    public IQueryable<StandardNumberingSystem> AsQueryable() => this.DataAccess.AsQueryable();

    public void Attach(StandardNumberingSystem entity) => throw new NotImplementedException();

    public bool BulkUpdate(IEnumerable<StandardNumberingSystem> entList) => this.DataAccess.BulkUpdate(entList);

    public void Delete(StandardNumberingSystem entity) => this.DataAccess.Delete(entity);

    public StandardNumberingSystem First(
      Expression<Func<StandardNumberingSystem, bool>> predicate)
    {
      return this.DataAccess.First(predicate);
    }

    public IEnumerable<StandardNumberingSystem> GetAll() => this.DataAccess.GetAll();

    public StandardNumberingSystem GetById(int id) => this.DataAccess.GetById(id);

    public StandardNumberingSystem GetById(string id) => this.DataAccess.GetById(id);

    public void Insert(StandardNumberingSystem entity) => this.DataAccess.Insert(entity);

    public IEnumerable<StandardNumberingSystem> SearchFor(
      Expression<Func<StandardNumberingSystem, bool>> predicate)
    {
      return this.DataAccess.SearchFor(predicate);
    }

    public StandardNumberingSystem Single(
      Expression<Func<StandardNumberingSystem, bool>> predicate)
    {
      return this.DataAccess.Single(predicate);
    }

    public StandardNumberingSystem SingleOrDefault(
      Expression<Func<StandardNumberingSystem, bool>> predicate)
    {
      return this.DataAccess.SingleOrDefault(predicate);
    }

    public bool Update(StandardNumberingSystem entity) => this.DataAccess.Update(entity);
  }
}
