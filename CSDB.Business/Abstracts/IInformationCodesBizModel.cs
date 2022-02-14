// Decompiled with JetBrains decompiler
// Type: CSDB.Business.Abstracts.IInformationCodesBizModel
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
  public interface IInformationCodesBizModel
  {
    IQueryable<InformationCodes> AsQueryable();

    IEnumerable<InformationCodes> GetAll();

    IEnumerable<InformationCodes> SearchFor(
      Expression<Func<InformationCodes, bool>> predicate);

    InformationCodes Single(Expression<Func<InformationCodes, bool>> predicate);

    InformationCodes SingleOrDefault(
      Expression<Func<InformationCodes, bool>> predicate);

    InformationCodes First(Expression<Func<InformationCodes, bool>> predicate);

    InformationCodes GetById(int id);

    InformationCodes GetById(string id);

    void Insert(InformationCodes entity);

    bool Update(InformationCodes entity);

    bool BulkUpdate(IEnumerable<InformationCodes> entList);

    void Delete(InformationCodes entity);

    void Attach(InformationCodes entity);
  }
}
