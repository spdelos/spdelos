// Decompiled with JetBrains decompiler
// Type: CSDB.Business.Implementation.ResponsiblePartnerCodeBizModel
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
  public class ResponsiblePartnerCodeBizModel : IResponsiblePartnerCodeBizModel
  {
    public IResponsiblePartnerCodeDataAccess DataAccess { get; set; }

    public ResponsiblePartnerCodeBizModel(IResponsiblePartnerCodeDataAccess dataAccess) => this.DataAccess = dataAccess;

    public IQueryable<ResponsiblePartnerCode> AsQueryable() => this.DataAccess.AsQueryable();

    public void Attach(ResponsiblePartnerCode entity) => throw new NotImplementedException();

    public bool BulkUpdate(IEnumerable<ResponsiblePartnerCode> entList) => this.DataAccess.BulkUpdate(entList);

    public void Delete(ResponsiblePartnerCode entity) => this.DataAccess.Delete(entity);

    public ResponsiblePartnerCode First(
      Expression<Func<ResponsiblePartnerCode, bool>> predicate)
    {
      return this.DataAccess.First(predicate);
    }

    public IEnumerable<ResponsiblePartnerCode> GetAll() => this.DataAccess.GetAll();

    public ResponsiblePartnerCode GetById(int id) => this.DataAccess.GetById(id);

    public ResponsiblePartnerCode GetById(string id) => this.DataAccess.GetById(id);

    public void Insert(ResponsiblePartnerCode entity) => this.DataAccess.Insert(entity);

    public IEnumerable<ResponsiblePartnerCode> SearchFor(
      Expression<Func<ResponsiblePartnerCode, bool>> predicate)
    {
      return this.DataAccess.SearchFor(predicate);
    }

    public ResponsiblePartnerCode Single(
      Expression<Func<ResponsiblePartnerCode, bool>> predicate)
    {
      return this.DataAccess.Single(predicate);
    }

    public ResponsiblePartnerCode SingleOrDefault(
      Expression<Func<ResponsiblePartnerCode, bool>> predicate)
    {
      return this.DataAccess.SingleOrDefault(predicate);
    }

    public bool Update(ResponsiblePartnerCode entity) => this.DataAccess.Update(entity);
  }
}
