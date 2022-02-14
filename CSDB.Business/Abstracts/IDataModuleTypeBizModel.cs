// Decompiled with JetBrains decompiler
// Type: CSDB.Business.Abstracts.IDataModuleTypeBizModel
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
  public interface IDataModuleTypeBizModel
  {
    IQueryable<DataModuleType> AsQueryable();

    IEnumerable<DataModuleType> GetAll();

    IEnumerable<DataModuleType> SearchFor(
      Expression<Func<DataModuleType, bool>> predicate);

    DataModuleType Single(Expression<Func<DataModuleType, bool>> predicate);

    DataModuleType SingleOrDefault(Expression<Func<DataModuleType, bool>> predicate);

    DataModuleType First(Expression<Func<DataModuleType, bool>> predicate);

    DataModuleType GetById(int id);

    DataModuleType GetById(string id);

    void Insert(DataModuleType entity);

    bool Update(DataModuleType entity);

    bool BulkUpdate(IEnumerable<DataModuleType> entList);

    void Delete(DataModuleType entity);

    void Attach(DataModuleType entity);
  }
}
