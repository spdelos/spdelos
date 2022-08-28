using CSDBPortal.Data;
using CSDBPortal.Models;
using CSDBPortal.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CSDBPortal.Business
{
    public class ICNManager
    {

        public ICNViewModel GetProjects()
        {
            ICNViewModel _iCNViewModel = new ICNViewModel();
            _iCNViewModel.ProjectList  = new List<ProjectDropDown>();
            try
            {
                using (ApplicationDbContext _db = new())
                {
                    var result = (from p in _db.Projects
                                  select new { p.Id, p.Name }).ToList();
                    foreach (var obj in result)
                    {
                        _iCNViewModel.ProjectList.Add(new ProjectDropDown { ProjecctId = obj.Id, Name = obj.Name });
                    }
                }
            }
            catch (Exception ex)
            {
                //
            }
            return _iCNViewModel;
        }

        public int GetMaxSequenceNumber(int projectId)
        {
            int maxSequenceNumber = 0;
            try
            {
                using (ApplicationDbContext _db = new())
                {
                    var result = (from icn in _db.IcnNumbers
                                         join p in _db.Projects on icn.ProjectId equals p.Id
                                  where icn.ProjectId == projectId
                                  select icn.SeqNo).Max();

                    if (result != null)
                    {
                        maxSequenceNumber = result;
                    }
                }
            }
            catch (Exception ex)
            {
                //
            }
            return maxSequenceNumber;
        }

        public string GetMaxVarcode(int projectId, int sequenceNumber)
        {
            string maxVarcode = string.Empty;
            try
            {
                using (ApplicationDbContext _db = new())
                {
                    var result = (from icn in _db.IcnNumbers
                                  join p in _db.Projects on icn.ProjectId equals p.Id
                                  where icn.ProjectId == projectId && icn.SeqNo == sequenceNumber
                                  select icn.VarCode).Max();

                    if (result != null)
                    {
                        maxVarcode = result;
                    }
                }
            }
            catch (Exception ex)
            {
                //
            }
            return maxVarcode;
        }

        public int GetMaxIssueNo(int projectId, int sequenceNumber, string varCode)
        {
            int maxIssueNo = 0;
            try
            {
                using (ApplicationDbContext _db = new())
                {
                    var result = (from icn in _db.IcnNumbers
                                  join p in _db.Projects on icn.ProjectId equals p.Id
                                  where icn.ProjectId == projectId && icn.SeqNo == sequenceNumber && icn.VarCode.Equals(varCode)
                                  select icn.IssueNo).Max();

                    if (result != null)
                    {
                        maxIssueNo = result;
                    }
                }
            }
            catch (Exception ex)
            {
                //
            }
            return maxIssueNo;
        }

        public List<int> GetSequenceNumbers(int projectId)
        {
            List<int> sequenceNumbers = new List<int>();
            try
            {
                using (ApplicationDbContext _db = new())
                {
                    var result = (from icn in _db.IcnNumbers
                                  join p in _db.Projects on icn.ProjectId equals p.Id
                                  where icn.ProjectId == projectId
                                  select new { icn.SeqNo }).ToList();
                    foreach (var obj in result)
                    {
                        if (!sequenceNumbers.Contains(obj.SeqNo))
                        {
                            sequenceNumbers.Add(obj.SeqNo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //
            }
            return sequenceNumbers;
        }

        public List<string> GetVarcodes(int projectId, int sequenceNumber)
        {
            List<string> varCodes = new List<string>();
            try
            {
                using (ApplicationDbContext _db = new())
                {
                    var result = (from icn in _db.IcnNumbers
                                  join p in _db.Projects on icn.ProjectId equals p.Id
                                  where icn.ProjectId == projectId && icn.SeqNo == sequenceNumber
                                  select new { icn.VarCode }).ToList();
                    foreach (var obj in result)
                    {
                        if (!varCodes.Contains(obj.VarCode))
                        {
                            varCodes.Add(obj.VarCode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //
            }
            return varCodes;
        }

        public List<CustomICNNumber> ICNNumberByProjectId(int projectid)
        {
            List<CustomICNNumber> customICNNumberList = new List<CustomICNNumber>();
            try
            {
                using (ApplicationDbContext _db = new())
                {
                    var result = (from icn in _db.IcnNumbers
                                  join  p in _db.Projects on icn.ProjectId equals p.Id
                                  where icn.ProjectId == projectid
                                  select new { icn.Id,icn.Number,icn.UpdatedBy,icn.ProjectId,p.Name }).ToList();
                    foreach (var obj in result)
                    {
                        customICNNumberList.Add(new CustomICNNumber { Id = obj.Id,
                            Number = obj.Number,
                            UpdatedBy=obj.UpdatedBy,
                            ProjectId=obj.ProjectId,
                            ProjectName=obj.Name });
                    }
                }
            }
            catch (Exception ex)
            {
                //
            }
            return customICNNumberList;
        }


        public void GenerateICNNumber(ICNumberGenerationModel icnNumberGenerationModel, string userName)
        {
            using (ApplicationDbContext applicationDbContext = new())
            {
                Project project = applicationDbContext.Projects.Where(p => p.Id == icnNumberGenerationModel.ProjectId).FirstOrDefault();
                if (project != null)
                {
                    StringBuilder icnNumber = new StringBuilder("ICN-");

                    Icnformat icnFormat = applicationDbContext.Icnformats.Where(icn => icn.Id == project.IcnformatId).FirstOrDefault();
                    if (icnFormat != null)
                    {
                        List<ICNFormatField> fieldList = applicationDbContext.ICNFormatFields.Where(icn => icn.ICNFormatId == icnFormat.Id).OrderBy(o => o.DisplayOrder).ToList();
                        if (fieldList != null && fieldList.Count > 0)
                        {
                            foreach (ICNFormatField field in fieldList)
                            {
                                ICNFormatMasterField icnField = applicationDbContext.ICNFormatMasterFields.Where(i => i.Id == field.ICNFormatMasterFieldId).FirstOrDefault();
                                if (icnField != null)
                                {
                                    if (icnField.IsForeignKey.HasValue == false)
                                    {
                                        switch(icnField.Field)
                                        {
                                            case "SEC":
                                                icnNumber.Append("Y");
                                                icnNumber.Append("-");
                                                break;
                                            case "ISSUENO":
                                                icnNumber.Append("<<ISSUENO>>");
                                                icnNumber.Append("-");
                                                break;
                                            case "SEQNO":
                                                icnNumber.Append("<<SEQNO>>");
                                                icnNumber.Append("-");
                                                break;
                                            case "VARCODE":
                                                icnNumber.Append("<<VARCODE>>");
                                                icnNumber.Append("-");
                                                break;
                                        }
                                    } 
                                    else if (icnField.IsForeignKey == false)
                                    {
                                        var value = project.GetType().GetProperty(icnField.ProjectFieldReference).GetValue(project, null);

                                        icnNumber.Append(value);
                                        icnNumber.Append("-");
                                    }
                                    else
                                    {
                                        var value = project.GetType().GetProperty(icnField.ProjectFieldReference).GetValue(project, null);

                                        StringBuilder query = new StringBuilder("Select ");
                                        query.Append(icnField.ReferenceColumn);
                                        query.Append(" From ");
                                        query.Append(icnField.ReferenceTable);
                                        query.Append(" where ");
                                        query.Append(icnField.ReferenceKey);
                                        query.Append(" = ");
                                        query = query.Append(value);

                                        using (SqlConnection connection = new SqlConnection(applicationDbContext.Database.GetConnectionString()))
                                        {
                                            SqlCommand command = new SqlCommand(query.ToString(), connection);
                                            connection.Open();
                                            SqlDataReader reader = command.ExecuteReader();
                                            try
                                            {
                                                while (reader.Read())
                                                {
                                                    icnNumber.Append(reader[0]);
                                                    icnNumber.Append("-");
                                                }
                                            }
                                            finally
                                            {
                                                reader.Close();
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        string newIcnNumber = icnNumber.ToString();
                        int sequenceNumber = GetMaxSequenceNumber(icnNumberGenerationModel.ProjectId);
                        string varCode = GetMaxVarcode(icnNumberGenerationModel.ProjectId, sequenceNumber);
                        int issueNo = GetMaxIssueNo(icnNumberGenerationModel.ProjectId, sequenceNumber,varCode);

                        if (string.IsNullOrWhiteSpace(varCode))
                        {
                            varCode = "A";
                        }

                        if (issueNo <=0 )
                        {
                            issueNo = 0;
                        }

                        switch (icnNumberGenerationModel.GenerateBy)
                        {
                            case ICNNumberGenerateBy.SEQUENCENUMBER:
                                for (int i = 1; i <= icnNumberGenerationModel.Count; i++)
                                {
                                    int newSequenceNumber = sequenceNumber + i;

                                    string templateNumber = newIcnNumber.Replace("<<SEQNO>>", $"{newSequenceNumber:00000}");
                                    templateNumber = templateNumber.Replace("<<VARCODE>>", varCode);
                                    templateNumber = templateNumber.Replace("<<ISSUENO>>", $"{issueNo:00}");

                                    IcnNumber newIcnNumberRecord = new IcnNumber()
                                    {
                                        Number = templateNumber,
                                        DataModuleId = 0,
                                        ProjectId = icnNumberGenerationModel.ProjectId,
                                        //ImagePath
                                        IsAllocated = true,
                                        UpdatedBy = userName,
                                        UpdatedOn = DateTime.UtcNow,
                                        ICNFormatId = icnFormat.Id,
                                        SeqNo = newSequenceNumber,
                                        VarCode = varCode,
                                        IssueNo = issueNo
                                    };

                                    applicationDbContext.Add(newIcnNumberRecord);
                                }
                                break;
                            case ICNNumberGenerateBy.VARCODE:
                                int newSequenceNumberForVarCode = icnNumberGenerationModel.SequenceNumber;

                                var newIssueNo = (from icn in applicationDbContext.IcnNumbers
                                            join p in applicationDbContext.Projects on icn.ProjectId equals p.Id
                                            where icn.ProjectId == icnNumberGenerationModel.ProjectId && icn.SeqNo == icnNumberGenerationModel.SequenceNumber
                                            select icn.IssueNo).FirstOrDefault();

                                varCode = GetMaxVarcode(icnNumberGenerationModel.ProjectId, newSequenceNumberForVarCode);
                                string newVarCode = ((char)(((int)varCode[0]) + 1)).ToString();

                                string templateNumberForVarcode = newIcnNumber.Replace("<<SEQNO>>", $"{newSequenceNumberForVarCode:00000}");
                                templateNumberForVarcode = templateNumberForVarcode.Replace("<<VARCODE>>", newVarCode);
                                templateNumberForVarcode = templateNumberForVarcode.Replace("<<ISSUENO>>", $"{newIssueNo:00}");

                                IcnNumber newIcnNumberRecordForVarcode = new IcnNumber()
                                {
                                    Number = templateNumberForVarcode,
                                    DataModuleId = 0,
                                    ProjectId = icnNumberGenerationModel.ProjectId,
                                    //ImagePath
                                    IsAllocated = true,
                                    UpdatedBy = userName,
                                    UpdatedOn = DateTime.UtcNow,
                                    ICNFormatId = icnFormat.Id,
                                    SeqNo = newSequenceNumberForVarCode,
                                    VarCode = newVarCode,
                                    IssueNo = newIssueNo
                                };

                                applicationDbContext.Add(newIcnNumberRecordForVarcode);
                                break;
                            case ICNNumberGenerateBy.ISSUENO:
                                int newSequenceNumberForIssueNumber = icnNumberGenerationModel.SequenceNumber;
                                varCode = icnNumberGenerationModel.VarCode;
                                issueNo = GetMaxIssueNo(icnNumberGenerationModel.ProjectId, newSequenceNumberForIssueNumber, varCode);
                                issueNo = issueNo + 1;

                                string templateNumberForIssueNumber = newIcnNumber.Replace("<<SEQNO>>", $"{newSequenceNumberForIssueNumber:00000}");
                                templateNumberForIssueNumber = templateNumberForIssueNumber.Replace("<<VARCODE>>", varCode);
                                templateNumberForIssueNumber = templateNumberForIssueNumber.Replace("<<ISSUENO>>", $"{issueNo:00}");

                                IcnNumber newIcnNumberRecordForIssueNumber = new IcnNumber()
                                {
                                    Number = templateNumberForIssueNumber,
                                    DataModuleId = 0,
                                    ProjectId = icnNumberGenerationModel.ProjectId,
                                    //ImagePath
                                    IsAllocated = true,
                                    UpdatedBy = userName,
                                    UpdatedOn = DateTime.UtcNow,
                                    ICNFormatId = icnFormat.Id,
                                    SeqNo = newSequenceNumberForIssueNumber,
                                    VarCode = varCode,
                                    IssueNo = issueNo
                                };
                                applicationDbContext.Add(newIcnNumberRecordForIssueNumber);
                                break;
                        }

                        applicationDbContext.SaveChanges();
                    }
                }
            }
        }

    }
    public class CustomICNNumber : IcnNumber
    {
        public string ProjectName { set; get; }
    }
}
