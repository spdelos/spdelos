// Decompiled with JetBrains decompiler
// Type: CSDB.Business.Abstracts.IDataDispatchNodeBizModel
// Assembly: CSDB.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3E16F06E-96F5-45EA-8378-B8A3811262B8
// Assembly location: C:\Users\asus\Downloads\123\publish\CSDB.Business.dll

using CSDB.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CSDB.Business.Abstracts
{
  public interface IDataDispatchNodeBizModel
  {
    IQueryable<DataDispatchNode> AsQueryable();

    IEnumerable<DataDispatchNode> GetAll();

    IEnumerable<DataDispatchNode> SearchFor(
      Expression<Func<DataDispatchNode, bool>> predicate);

    DataDispatchNode Single(Expression<Func<DataDispatchNode, bool>> predicate);

    DataDispatchNode SingleOrDefault(
      Expression<Func<DataDispatchNode, bool>> predicate);

    DataDispatchNode First(Expression<Func<DataDispatchNode, bool>> predicate);

    DataDispatchNode GetById(int id);

    DataDispatchNode GetById(string id);

    void Insert(DataDispatchNode entity);

    bool Update(DataDispatchNode entity);

    bool BulkUpdate(IEnumerable<DataDispatchNode> entList);

    void Delete(DataDispatchNode entity);

    void Attach(DataDispatchNode entity);
  }
}
