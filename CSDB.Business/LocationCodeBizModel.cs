// Decompiled with JetBrains decompiler
// Type: CSDB.Business.Implementation.LocationCodeBizModel
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
  public class LocationCodeBizModel : ILocationCodeBizModel
  {
    public ILocationCodeDataAccess DataAccess { get; set; }

    public LocationCodeBizModel(ILocationCodeDataAccess dataAccess) => this.DataAccess = dataAccess;

    public IQueryable<LocationCode> AsQueryable() => this.DataAccess.AsQueryable();

    public void Attach(LocationCode entity) => throw new NotImplementedException();

    public bool BulkUpdate(IEnumerable<LocationCode> entList) => this.DataAccess.BulkUpdate(entList);

    public void Delete(LocationCode entity) => this.DataAccess.Delete(entity);

    public LocationCode First(Expression<Func<LocationCode, bool>> predicate) => this.DataAccess.First(predicate);

    public IEnumerable<LocationCode> GetAll() => this.DataAccess.GetAll();

    public LocationCode GetById(int id) => this.DataAccess.GetById(id);

    public LocationCode GetById(string id) => this.DataAccess.GetById(id);

    public void Insert(LocationCode entity) => this.DataAccess.Insert(entity);

    public IEnumerable<LocationCode> SearchFor(
      Expression<Func<LocationCode, bool>> predicate)
    {
      return this.DataAccess.SearchFor(predicate);
    }

    public LocationCode Single(Expression<Func<LocationCode, bool>> predicate) => this.DataAccess.Single(predicate);

    public LocationCode SingleOrDefault(Expression<Func<LocationCode, bool>> predicate) => this.DataAccess.SingleOrDefault(predicate);

    public bool Update(LocationCode entity) => this.DataAccess.Update(entity);
  }
}
