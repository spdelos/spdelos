// Decompiled with JetBrains decompiler
// Type: CSDB.Business.Abstracts.IResponsiblePartnerCodeBizModel
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
  internal interface IResponsiblePartnerCodeBizModel
  {
    IQueryable<ResponsiblePartnerCode> AsQueryable();

    IEnumerable<ResponsiblePartnerCode> GetAll();

    IEnumerable<ResponsiblePartnerCode> SearchFor(
      Expression<Func<ResponsiblePartnerCode, bool>> predicate);

    ResponsiblePartnerCode Single(
      Expression<Func<ResponsiblePartnerCode, bool>> predicate);

    ResponsiblePartnerCode SingleOrDefault(
      Expression<Func<ResponsiblePartnerCode, bool>> predicate);

    ResponsiblePartnerCode First(
      Expression<Func<ResponsiblePartnerCode, bool>> predicate);

    ResponsiblePartnerCode GetById(int id);

    ResponsiblePartnerCode GetById(string id);

    void Insert(ResponsiblePartnerCode entity);

    bool Update(ResponsiblePartnerCode entity);

    bool BulkUpdate(IEnumerable<ResponsiblePartnerCode> entList);

    void Delete(ResponsiblePartnerCode entity);

    void Attach(ResponsiblePartnerCode entity);
  }
}
