// Decompiled with JetBrains decompiler
// Type: CSDB.Business.Implementation.DataModuleBizModel
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
  public class DataModuleBizModel : IDataModuleBizModel
  {
    public IDataModuleDataAccess DataAccess { get; set; }

    public DataModuleBizModel(IDataModuleDataAccess dataAccess) => this.DataAccess = dataAccess;

    public IQueryable<DataModule> AsQueryable() => this.DataAccess.AsQueryable();

    public void Attach(DataModule entity) => throw new NotImplementedException();

    public bool BulkUpdate(IEnumerable<DataModule> entList) => this.DataAccess.BulkUpdate(entList);

    public void Delete(DataModule entity) => this.DataAccess.Delete(entity);

    public DataModule First(Expression<Func<DataModule, bool>> predicate) => this.DataAccess.First(predicate);

    public IEnumerable<DataModule> GetAll() => this.DataAccess.GetAll();

    public DataModule GetById(int id) => this.DataAccess.GetById(id);

    public DataModule GetById(string id) => this.DataAccess.GetById(id);

    public void Insert(DataModule entity) => this.DataAccess.Insert(entity);

    public IEnumerable<DataModule> SearchFor(
      Expression<Func<DataModule, bool>> predicate)
    {
      return this.DataAccess.SearchFor(predicate);
    }

    public DataModule Single(Expression<Func<DataModule, bool>> predicate) => this.DataAccess.Single(predicate);

    public DataModule SingleOrDefault(Expression<Func<DataModule, bool>> predicate) => this.DataAccess.SingleOrDefault(predicate);

    public bool Update(DataModule entity) => this.DataAccess.Update(entity);
  }
}
