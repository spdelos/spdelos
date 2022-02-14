// Decompiled with JetBrains decompiler
// Type: CSDB.Business.Implementation.DataDispatchNodeBizModel
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
  public class DataDispatchNodeBizModel : IDataDispatchNodeBizModel
  {
    public IDataDispatchNodeDataAccess DataAccess { get; set; }

    public DataDispatchNodeBizModel(IDataDispatchNodeDataAccess dataAccess) => this.DataAccess = dataAccess;

    public IQueryable<DataDispatchNode> AsQueryable() => this.DataAccess.AsQueryable();

    public void Attach(DataDispatchNode entity) => throw new NotImplementedException();

    public bool BulkUpdate(IEnumerable<DataDispatchNode> entList) => this.DataAccess.BulkUpdate(entList);

    public void Delete(DataDispatchNode entity) => this.DataAccess.Delete(entity);

    public DataDispatchNode First(Expression<Func<DataDispatchNode, bool>> predicate) => this.DataAccess.First(predicate);

    public IEnumerable<DataDispatchNode> GetAll() => this.DataAccess.GetAll();

    public DataDispatchNode GetById(int id) => this.DataAccess.GetById(id);

    public DataDispatchNode GetById(string id) => this.DataAccess.GetById(id);

    public void Insert(DataDispatchNode entity) => this.DataAccess.Insert(entity);

    public IEnumerable<DataDispatchNode> SearchFor(
      Expression<Func<DataDispatchNode, bool>> predicate)
    {
      return this.DataAccess.SearchFor(predicate);
    }

    public DataDispatchNode Single(
      Expression<Func<DataDispatchNode, bool>> predicate)
    {
      return this.DataAccess.Single(predicate);
    }

    public DataDispatchNode SingleOrDefault(
      Expression<Func<DataDispatchNode, bool>> predicate)
    {
      return this.DataAccess.SingleOrDefault(predicate);
    }

    public bool Update(DataDispatchNode entity) => this.DataAccess.Update(entity);
  }
}
