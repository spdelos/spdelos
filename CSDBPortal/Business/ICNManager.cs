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
        private readonly ApplicationDbContext _db;

        public ICNManager(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<ICNViewModel> GetProjectsAsync()
        {
            ICNViewModel _iCNViewModel = new ICNViewModel();
            _iCNViewModel.ProjectList = new List<ProjectDropDown>();
            try
            {
                var result = await (from p in _db.Projects select new { p.Id, p.Name }).ToListAsync();
                foreach (var obj in result)
                    _iCNViewModel.ProjectList.Add(new ProjectDropDown { ProjecctId = obj.Id, Name = obj.Name });
            }
            catch { }
            return _iCNViewModel;
        }

        public async Task<int> GetMaxSequenceNumberAsync(int projectId)
        {
            try
            {
                var result = await (from icn in _db.IcnNumbers
                                    join p in _db.Projects on icn.ProjectId equals p.Id
                                    where icn.ProjectId == projectId
                                    select icn.SeqNo).MaxAsync();
                return result;
            }
            catch { return 0; }
        }

        public async Task<string> GetMaxVarcodeAsync(int projectId, int sequenceNumber)
        {
            try
            {
                var result = await (from icn in _db.IcnNumbers
                                    join p in _db.Projects on icn.ProjectId equals p.Id
                                    where icn.ProjectId == projectId && icn.SeqNo == sequenceNumber
                                    select icn.VarCode).MaxAsync();
                return result ?? string.Empty;
            }
            catch { return string.Empty; }
        }

        public async Task<int> GetMaxIssueNoAsync(int projectId, int sequenceNumber, string varCode)
        {
            try
            {
                var result = await (from icn in _db.IcnNumbers
                                    join p in _db.Projects on icn.ProjectId equals p.Id
                                    where icn.ProjectId == projectId && icn.SeqNo == sequenceNumber && icn.VarCode.Equals(varCode)
                                    select icn.IssueNo).MaxAsync();
                return result ?? 0;
            }
            catch { return 0; }
        }

        public async Task<List<int>> GetSequenceNumbersAsync(int projectId)
        {
            List<int> sequenceNumbers = new List<int>();
            try
            {
                var result = await (from icn in _db.IcnNumbers
                                    join p in _db.Projects on icn.ProjectId equals p.Id
                                    where icn.ProjectId == projectId
                                    select icn.SeqNo).Distinct().ToListAsync();
                sequenceNumbers = result;
            }
            catch { }
            return sequenceNumbers;
        }

        public async Task<List<string>> GetVarcodesAsync(int projectId, int sequenceNumber)
        {
            List<string> varCodes = new List<string>();
            try
            {
                var result = await (from icn in _db.IcnNumbers
                                    join p in _db.Projects on icn.ProjectId equals p.Id
                                    where icn.ProjectId == projectId && icn.SeqNo == sequenceNumber
                                    select icn.VarCode).Distinct().ToListAsync();
                varCodes = result;
            }
            catch { }
            return varCodes;
        }

        public async Task<List<CustomICNNumber>> ICNNumberByProjectIdAsync(int projectid)
        {
            List<CustomICNNumber> customICNNumberList = new List<CustomICNNumber>();
            try
            {
                var result = await (from icn in _db.IcnNumbers
                                    join p in _db.Projects on icn.ProjectId equals p.Id
                                    where icn.ProjectId == projectid
                                    select new { icn.Id, icn.Number, icn.UpdatedBy, icn.ProjectId, p.Name }).ToListAsync();
                foreach (var obj in result)
                {
                    customICNNumberList.Add(new CustomICNNumber
                    {
                        Id = obj.Id, Number = obj.Number, UpdatedBy = obj.UpdatedBy,
                        ProjectId = obj.ProjectId, ProjectName = obj.Name
                    });
                }
            }
            catch { }
            return customICNNumberList;
        }

        public async Task GenerateICNNumberAsync(ICNumberGenerationModel icnNumberGenerationModel, string userName)
        {
            Project project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == icnNumberGenerationModel.ProjectId);
            if (project == null) return;

            StringBuilder icnNumber = new StringBuilder("ICN-");
            Icnformat icnFormat = await _db.Icnformats.FirstOrDefaultAsync(icn => icn.Id == project.IcnformatId);
            if (icnFormat == null) return;

            List<ICNFormatField> fieldList = await _db.ICNFormatFields
                .Where(icn => icn.ICNFormatId == icnFormat.Id).OrderBy(o => o.DisplayOrder).ToListAsync();

            if (fieldList == null || fieldList.Count == 0) return;

            foreach (ICNFormatField field in fieldList)
            {
                ICNFormatMasterField icnField = await _db.ICNFormatMasterFields.FirstOrDefaultAsync(i => i.Id == field.ICNFormatMasterFieldId);
                if (icnField == null) continue;

                if (icnField.IsForeignKey.HasValue == false)
                {
                    switch (icnField.Field)
                    {
                        case "SEC": icnNumber.Append("Y-"); break;
                        case "ISSUENO": icnNumber.Append("<<ISSUENO>>-"); break;
                        case "SEQNO": icnNumber.Append("<<SEQNO>>-"); break;
                        case "VARCODE": icnNumber.Append("<<VARCODE>>-"); break;
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
                    query.Append(icnField.ReferenceColumn).Append(" From ").Append(icnField.ReferenceTable)
                         .Append(" where ").Append(icnField.ReferenceKey).Append(" = ").Append(value);

                    using (SqlConnection connection = new SqlConnection(_db.Database.GetConnectionString()))
                    {
                        SqlCommand command = new SqlCommand(query.ToString(), connection);
                        await connection.OpenAsync();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        try
                        {
                            while (await reader.ReadAsync())
                            {
                                icnNumber.Append(reader[0]);
                                icnNumber.Append("-");
                            }
                        }
                        finally { reader.Close(); }
                    }
                }
            }

            string newIcnNumber = icnNumber.ToString();
            int sequenceNumber = await GetMaxSequenceNumberAsync(icnNumberGenerationModel.ProjectId);
            string varCode = await GetMaxVarcodeAsync(icnNumberGenerationModel.ProjectId, sequenceNumber);
            int issueNo = await GetMaxIssueNoAsync(icnNumberGenerationModel.ProjectId, sequenceNumber, varCode);

            if (string.IsNullOrWhiteSpace(varCode)) varCode = "A";
            if (issueNo <= 0) issueNo = 0;

            switch (icnNumberGenerationModel.GenerateBy)
            {
                case ICNNumberGenerateBy.SEQUENCENUMBER:
                    for (int i = 1; i <= icnNumberGenerationModel.Count; i++)
                    {
                        int newSequenceNumber = sequenceNumber + i;
                        string templateNumber = newIcnNumber
                            .Replace("<<SEQNO>>", $"{newSequenceNumber:00000}")
                            .Replace("<<VARCODE>>", varCode)
                            .Replace("<<ISSUENO>>", $"{issueNo:00}");
                        _db.Add(new IcnNumber
                        {
                            Number = templateNumber, DataModuleId = 0, ProjectId = icnNumberGenerationModel.ProjectId,
                            IsAllocated = true, UpdatedBy = userName, UpdatedOn = DateTime.UtcNow,
                            ICNFormatId = icnFormat.Id, SeqNo = newSequenceNumber, VarCode = varCode, IssueNo = issueNo
                        });
                    }
                    break;
                case ICNNumberGenerateBy.VARCODE:
                    int newSeqForVarCode = icnNumberGenerationModel.SequenceNumber;
                    var newIssueNo = await (from icn in _db.IcnNumbers
                                     join p in _db.Projects on icn.ProjectId equals p.Id
                                     where icn.ProjectId == icnNumberGenerationModel.ProjectId && icn.SeqNo == icnNumberGenerationModel.SequenceNumber
                                     select icn.IssueNo).FirstOrDefaultAsync();
                    varCode = await GetMaxVarcodeAsync(icnNumberGenerationModel.ProjectId, newSeqForVarCode);
                    string newVarCode = ((char)(((int)varCode[0]) + 1)).ToString();
                    string templateForVarcode = newIcnNumber
                        .Replace("<<SEQNO>>", $"{newSeqForVarCode:00000}")
                        .Replace("<<VARCODE>>", newVarCode)
                        .Replace("<<ISSUENO>>", $"{newIssueNo:00}");
                    _db.Add(new IcnNumber
                    {
                        Number = templateForVarcode, DataModuleId = 0, ProjectId = icnNumberGenerationModel.ProjectId,
                        IsAllocated = true, UpdatedBy = userName, UpdatedOn = DateTime.UtcNow,
                        ICNFormatId = icnFormat.Id, SeqNo = newSeqForVarCode, VarCode = newVarCode, IssueNo = newIssueNo
                    });
                    break;
                case ICNNumberGenerateBy.ISSUENO:
                    int newSeqForIssueNumber = icnNumberGenerationModel.SequenceNumber;
                    varCode = icnNumberGenerationModel.VarCode;
                    issueNo = await GetMaxIssueNoAsync(icnNumberGenerationModel.ProjectId, newSeqForIssueNumber, varCode) + 1;
                    string templateForIssueNumber = newIcnNumber
                        .Replace("<<SEQNO>>", $"{newSeqForIssueNumber:00000}")
                        .Replace("<<VARCODE>>", varCode)
                        .Replace("<<ISSUENO>>", $"{issueNo:00}");
                    _db.Add(new IcnNumber
                    {
                        Number = templateForIssueNumber, DataModuleId = 0, ProjectId = icnNumberGenerationModel.ProjectId,
                        IsAllocated = true, UpdatedBy = userName, UpdatedOn = DateTime.UtcNow,
                        ICNFormatId = icnFormat.Id, SeqNo = newSeqForIssueNumber, VarCode = varCode, IssueNo = issueNo
                    });
                    break;
            }

            await _db.SaveChangesAsync();
        }
    }

    public class CustomICNNumber : IcnNumber
    {
        public string ProjectName { set; get; }
    }
}
