// Decompiled with JetBrains decompiler
// Type: CSDB.Business.Implementation.ApplicationSettingsBizModel
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
  public class ApplicationSettingsBizModel : IApplicationSettingsBizModel
  {
    public IApplicationSettingsDataAccess DataAccess { get; set; }

    public ApplicationSettingsBizModel(IApplicationSettingsDataAccess dataAccess) => this.DataAccess = dataAccess;

    public IQueryable<ApplicationSettings> AsQueryable() => this.DataAccess.AsQueryable();

    public void Attach(ApplicationSettings entity) => throw new NotImplementedException();

    public bool BulkUpdate(IEnumerable<ApplicationSettings> entList) => this.DataAccess.BulkUpdate(entList);

    public void Delete(ApplicationSettings entity) => this.DataAccess.Delete(entity);

    public ApplicationSettings First(
      Expression<Func<ApplicationSettings, bool>> predicate)
    {
      return this.DataAccess.First(predicate);
    }

    public IEnumerable<ApplicationSettings> GetAll() => this.DataAccess.GetAll();

    public ApplicationSettings GetById(int id) => this.DataAccess.GetById(id);

    public ApplicationSettings GetById(string id) => this.DataAccess.GetById(id);

    public void Insert(ApplicationSettings entity) => this.DataAccess.Insert(entity);

    public IEnumerable<ApplicationSettings> SearchFor(
      Expression<Func<ApplicationSettings, bool>> predicate)
    {
      return this.DataAccess.SearchFor(predicate);
    }

    public ApplicationSettings Single(
      Expression<Func<ApplicationSettings, bool>> predicate)
    {
      return this.DataAccess.Single(predicate);
    }

    public ApplicationSettings SingleOrDefault(
      Expression<Func<ApplicationSettings, bool>> predicate)
    {
      return this.DataAccess.SingleOrDefault(predicate);
    }

    public bool Update(ApplicationSettings entity) => this.DataAccess.Update(entity);
  }
}
