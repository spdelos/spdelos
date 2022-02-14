// Decompiled with JetBrains decompiler
// Type: CSDB.Business.Implementation.InformationCodesBizModel
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
  public class InformationCodesBizModel : IInformationCodesBizModel
  {
    public IInformationCodesDataAccess DataAccess { get; set; }

    public InformationCodesBizModel(IInformationCodesDataAccess dataAccess) => this.DataAccess = dataAccess;

    public IQueryable<InformationCodes> AsQueryable() => this.DataAccess.AsQueryable();

    public void Attach(InformationCodes entity) => throw new NotImplementedException();

    public bool BulkUpdate(IEnumerable<InformationCodes> entList) => this.DataAccess.BulkUpdate(entList);

    public void Delete(InformationCodes entity) => this.DataAccess.Delete(entity);

    public InformationCodes First(Expression<Func<InformationCodes, bool>> predicate) => this.DataAccess.First(predicate);

    public IEnumerable<InformationCodes> GetAll() => this.DataAccess.GetAll();

    public InformationCodes GetById(int id) => this.DataAccess.GetById(id);

    public InformationCodes GetById(string id) => this.DataAccess.GetById(id);

    public void Insert(InformationCodes entity) => this.DataAccess.Insert(entity);

    public IEnumerable<InformationCodes> SearchFor(
      Expression<Func<InformationCodes, bool>> predicate)
    {
      return this.DataAccess.SearchFor(predicate);
    }

    public InformationCodes Single(
      Expression<Func<InformationCodes, bool>> predicate)
    {
      return this.DataAccess.Single(predicate);
    }

    public InformationCodes SingleOrDefault(
      Expression<Func<InformationCodes, bool>> predicate)
    {
      return this.DataAccess.SingleOrDefault(predicate);
    }

    public bool Update(InformationCodes entity) => this.DataAccess.Update(entity);
  }
}
