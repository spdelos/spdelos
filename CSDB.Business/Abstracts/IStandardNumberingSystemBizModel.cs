// Decompiled with JetBrains decompiler
// Type: CSDB.Business.Abstracts.IStandardNumberingSystemBizModel
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
  public interface IStandardNumberingSystemBizModel
  {
    IQueryable<StandardNumberingSystem> AsQueryable();

    IEnumerable<StandardNumberingSystem> GetAll();

    IEnumerable<StandardNumberingSystem> SearchFor(
      Expression<Func<StandardNumberingSystem, bool>> predicate);

    StandardNumberingSystem Single(
      Expression<Func<StandardNumberingSystem, bool>> predicate);

    StandardNumberingSystem SingleOrDefault(
      Expression<Func<StandardNumberingSystem, bool>> predicate);

    StandardNumberingSystem First(
      Expression<Func<StandardNumberingSystem, bool>> predicate);

    StandardNumberingSystem GetById(int id);

    StandardNumberingSystem GetById(string id);

    void Insert(StandardNumberingSystem entity);

    bool Update(StandardNumberingSystem entity);

    bool BulkUpdate(IEnumerable<StandardNumberingSystem> entList);

    void Delete(StandardNumberingSystem entity);

    void Attach(StandardNumberingSystem entity);
  }
}
