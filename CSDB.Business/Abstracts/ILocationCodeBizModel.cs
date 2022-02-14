// Decompiled with JetBrains decompiler
// Type: CSDB.Business.Abstracts.ILocationCodeBizModel
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
  public interface ILocationCodeBizModel
  {
    IQueryable<LocationCode> AsQueryable();

    IEnumerable<LocationCode> GetAll();

    IEnumerable<LocationCode> SearchFor(
      Expression<Func<LocationCode, bool>> predicate);

    LocationCode Single(Expression<Func<LocationCode, bool>> predicate);

    LocationCode SingleOrDefault(Expression<Func<LocationCode, bool>> predicate);

    LocationCode First(Expression<Func<LocationCode, bool>> predicate);

    LocationCode GetById(int id);

    LocationCode GetById(string id);

    void Insert(LocationCode entity);

    bool Update(LocationCode entity);

    bool BulkUpdate(IEnumerable<LocationCode> entList);

    void Delete(LocationCode entity);

    void Attach(LocationCode entity);
  }
}
