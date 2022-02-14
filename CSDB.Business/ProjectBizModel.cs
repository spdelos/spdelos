// Decompiled with JetBrains decompiler
// Type: CSDB.Business.Implementation.ProjectBizModel
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
  public class ProjectBizModel : IProjectBizModel
  {
    public IProjectDataAccess DataAccess { get; set; }

    public ProjectBizModel(IProjectDataAccess dataAccess) => this.DataAccess = dataAccess;

    public IQueryable<Project> AsQueryable() => this.DataAccess.AsQueryable();

    public void Attach(Project entity) => throw new NotImplementedException();

    public bool BulkUpdate(IEnumerable<Project> entList) => this.DataAccess.BulkUpdate(entList);

    public void Delete(Project entity) => this.DataAccess.Delete(entity);

    public Project First(Expression<Func<Project, bool>> predicate) => this.DataAccess.First(predicate);

    public IEnumerable<Project> GetAll() => this.DataAccess.GetAll();

    public Project GetById(int id) => this.DataAccess.GetById(id);

    public Project GetById(string id) => this.DataAccess.GetById(id);

    public void Insert(Project entity) => this.DataAccess.Insert(entity);

    public IEnumerable<Project> SearchFor(
      Expression<Func<Project, bool>> predicate)
    {
      return this.DataAccess.SearchFor(predicate);
    }

    public Project Single(Expression<Func<Project, bool>> predicate) => this.DataAccess.Single(predicate);

    public Project SingleOrDefault(Expression<Func<Project, bool>> predicate) => this.DataAccess.SingleOrDefault(predicate);

    public bool Update(Project entity) => this.DataAccess.Update(entity);
  }
}
