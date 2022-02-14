// Decompiled with JetBrains decompiler
// Type: CSDB.Business.Abstracts.IApplicationSettingsBizModel
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
  public interface IApplicationSettingsBizModel
  {
    IQueryable<ApplicationSettings> AsQueryable();

    IEnumerable<ApplicationSettings> GetAll();

    IEnumerable<ApplicationSettings> SearchFor(
      Expression<Func<ApplicationSettings, bool>> predicate);

    ApplicationSettings Single(
      Expression<Func<ApplicationSettings, bool>> predicate);

    ApplicationSettings SingleOrDefault(
      Expression<Func<ApplicationSettings, bool>> predicate);

    ApplicationSettings First(
      Expression<Func<ApplicationSettings, bool>> predicate);

    ApplicationSettings GetById(int id);

    ApplicationSettings GetById(string id);

    void Insert(ApplicationSettings entity);

    bool Update(ApplicationSettings entity);

    bool BulkUpdate(IEnumerable<ApplicationSettings> entList);

    void Delete(ApplicationSettings entity);

    void Attach(ApplicationSettings entity);
  }
}
