// Decompiled with JetBrains decompiler
// Type: CSDB.Business.Abstracts.IDataModuleBizModel
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
  public interface IDataModuleBizModel
  {
    IQueryable<DataModule> AsQueryable();

    IEnumerable<DataModule> GetAll();

    IEnumerable<DataModule> SearchFor(
      Expression<Func<DataModule, bool>> predicate);

    DataModule Single(Expression<Func<DataModule, bool>> predicate);

    DataModule SingleOrDefault(Expression<Func<DataModule, bool>> predicate);

    DataModule First(Expression<Func<DataModule, bool>> predicate);

    DataModule GetById(int id);

    DataModule GetById(string id);

    void Insert(DataModule entity);

    bool Update(DataModule entity);

    bool BulkUpdate(IEnumerable<DataModule> entList);

    void Delete(DataModule entity);

    void Attach(DataModule entity);
  }
}
