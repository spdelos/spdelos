// Decompiled with JetBrains decompiler
// Type: CSDB.Business.Implementation.DataModuleTypeBizModel
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
  public class DataModuleTypeBizModel : IDataModuleTypeBizModel
  {
    public IDataModuleTypeDataAccess DataAccess { get; set; }

    public DataModuleTypeBizModel(IDataModuleTypeDataAccess dataAccess) => this.DataAccess = dataAccess;

    public IQueryable<DataModuleType> AsQueryable() => this.DataAccess.AsQueryable();

    public void Attach(DataModuleType entity) => throw new NotImplementedException();

    public bool BulkUpdate(IEnumerable<DataModuleType> entList) => this.DataAccess.BulkUpdate(entList);

    public void Delete(DataModuleType entity) => this.DataAccess.Delete(entity);

    public DataModuleType First(Expression<Func<DataModuleType, bool>> predicate) => this.DataAccess.First(predicate);

    public IEnumerable<DataModuleType> GetAll() => this.DataAccess.GetAll();

    public DataModuleType GetById(int id) => this.DataAccess.GetById(id);

    public DataModuleType GetById(string id) => this.DataAccess.GetById(id);

    public void Insert(DataModuleType entity) => this.DataAccess.Insert(entity);

    public IEnumerable<DataModuleType> SearchFor(
      Expression<Func<DataModuleType, bool>> predicate)
    {
      return this.DataAccess.SearchFor(predicate);
    }

    public DataModuleType Single(Expression<Func<DataModuleType, bool>> predicate) => this.DataAccess.Single(predicate);

    public DataModuleType SingleOrDefault(
      Expression<Func<DataModuleType, bool>> predicate)
    {
      return this.DataAccess.SingleOrDefault(predicate);
    }

    public bool Update(DataModuleType entity) => this.DataAccess.Update(entity);
  }
}
