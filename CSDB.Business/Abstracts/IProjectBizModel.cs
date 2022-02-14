// Decompiled with JetBrains decompiler
// Type: CSDB.Business.Abstracts.IProjectBizModel
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
  public interface IProjectBizModel
  {
    IQueryable<Project> AsQueryable();

    IEnumerable<Project> GetAll();

    IEnumerable<Project> SearchFor(Expression<Func<Project, bool>> predicate);

    Project Single(Expression<Func<Project, bool>> predicate);

    Project SingleOrDefault(Expression<Func<Project, bool>> predicate);

    Project First(Expression<Func<Project, bool>> predicate);

    Project GetById(int id);

    Project GetById(string id);

    void Insert(Project entity);

    bool Update(Project entity);

    bool BulkUpdate(IEnumerable<Project> entList);

    void Delete(Project entity);

    void Attach(Project entity);
  }
}
