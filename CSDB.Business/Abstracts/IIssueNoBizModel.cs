// Decompiled with JetBrains decompiler
// Type: CSDB.Business.Abstracts.IIssueNoBizModel
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
  public interface IIssueNoBizModel
  {
    IQueryable<IssueNo> AsQueryable();

    IEnumerable<IssueNo> GetAll();

    IEnumerable<IssueNo> SearchFor(Expression<Func<IssueNo, bool>> predicate);

    IssueNo Single(Expression<Func<IssueNo, bool>> predicate);

    IssueNo SingleOrDefault(Expression<Func<IssueNo, bool>> predicate);

    IssueNo First(Expression<Func<IssueNo, bool>> predicate);

    IssueNo GetById(int id);

    IssueNo GetById(string id);

    void Insert(IssueNo entity);

    bool Update(IssueNo entity);

    bool BulkUpdate(IEnumerable<IssueNo> entList);

    void Delete(IssueNo entity);

    void Attach(IssueNo entity);
  }
}
