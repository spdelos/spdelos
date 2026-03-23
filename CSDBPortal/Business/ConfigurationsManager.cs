using CSDBPortal.Data;
using CSDBPortal.Models;
using CSDBPortal.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CSDBPortal.Business
{
    public class ConfigurationsManager
    {
        private readonly ApplicationDbContext _db;

        public ConfigurationsManager(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<ConfigurationViewModel> GetConfigurationDetailInfoAsync()
        {
            ConfigurationViewModel configurationViewModel = new();
            try
            {
                configurationViewModel.IssueNos = new List<CustomIssueNo>();
                configurationViewModel.Designations = new List<Designation>();
                configurationViewModel.InformationCodes = new List<CustomInformationCode>();
                configurationViewModel.Icnformats = new List<CustomIcnFormat>();
                configurationViewModel.IcnFormatMasterDestinationFields = new List<ICNFormatMasterField>();
                configurationViewModel.IcnFormatMasterFields = new List<ICNFormatMasterField>();
                configurationViewModel.LocationCodes = new List<CustomLocationCode>();
                configurationViewModel.LocationCodesSets = new List<LocationCodeSet>();
                configurationViewModel.ResponsiblePartnerCodes = new List<ResponsiblePartnerCode>();

                configurationViewModel.Designations = await _db.Designations.ToListAsync();
                configurationViewModel.InformationCodesSets = await _db.InformationCodeSets.ToListAsync();
                configurationViewModel.DataModuleTypes = await _db.DataModuleTypes.ToListAsync();
                configurationViewModel.IcnFormatMasterSourceFields = await _db.ICNFormatMasterFields.ToListAsync();
                configurationViewModel.IcnFormatMasterFields = await _db.ICNFormatMasterFields.ToListAsync();

                var issueNos = await _db.IssueNos.ToListAsync();
                foreach (IssueNo issueNo in issueNos)
                {
                    configurationViewModel.IssueNos.Add(new CustomIssueNo()
                    {
                        Id = issueNo.Id,
                        Name = issueNo.Name,
                        BrexTemplate = issueNo.BrexTemplate,
                        IsDelete = issueNo.IsDelete,
                        CreatedBy = issueNo.CreatedBy,
                        CreateOn = issueNo.CreateOn
                    });
                }

                var icnformats = await _db.Icnformats.ToListAsync();
                foreach (Icnformat icnFormat in icnformats)
                {
                    var customIcnFormat = new CustomIcnFormat()
                    {
                        Id = icnFormat.Id,
                        Code = icnFormat.Code,
                        CreatedBy = icnFormat.CreatedBy,
                        CreatedOn = icnFormat.CreatedOn,
                        Description = icnFormat.Description,
                        UpdatedBy = icnFormat.UpdatedBy,
                        UpdatedOn = icnFormat.UpdatedOn,
                    };

                    var icnFormatFields = await (from icf in _db.ICNFormatFields
                                           join icmf in _db.ICNFormatMasterFields on icf.ICNFormatMasterFieldId equals icmf.Id
                                           where icf.ICNFormatId == customIcnFormat.Id
                                           orderby icf.DisplayOrder
                                           select new { icf.Id, icf.ICNFormatId, icf.DisplayOrder, icf.ICNFormatMasterFieldId, icmf.Field }).ToListAsync();

                    customIcnFormat.IcnFormatFields = new List<CustomIcnFormatField>();
                    foreach (var icnformatField in icnFormatFields)
                    {
                        customIcnFormat.IcnFormatFields.Add(new CustomIcnFormatField()
                        {
                            DisplayOrder = icnformatField.DisplayOrder,
                            Id = icnformatField.Id,
                            Field = icnformatField.Field,
                            ICNFormatId = icnformatField.ICNFormatId,
                            ICNFormatMasterFieldId = icnformatField.ICNFormatMasterFieldId,
                        });
                    }

                    customIcnFormat.CanEdit = !await _db.IcnNumbers.AnyAsync(icn => icn.ICNFormatId == customIcnFormat.Id);
                    configurationViewModel.Icnformats.Add(customIcnFormat);
                }

                var result = await (from ic in _db.InformationCodes
                              join ics in _db.InformationCodeSets on ic.InformationCodeSetId equals ics.Id
                              join d in _db.DataModuleTypes on ic.DataModuleId equals d.Id
                              select new { ic.Id, ic.Code, ic.Description, ic.InformationCodeSetId, InformationCodeSetDescription = ics.Description, d.Name, ic.DataModuleId }).ToListAsync();

                foreach (var ic in result)
                {
                    configurationViewModel.InformationCodes.Add(new CustomInformationCode
                    {
                        Id = ic.Id,
                        Code = ic.Code,
                        Description = ic.Description,
                        DataModuleName = ic.Name,
                        DataModuleId = ic.DataModuleId,
                        InformationCodeSetId = ic.InformationCodeSetId,
                        InformationCodeSetDescription = ic.InformationCodeSetDescription
                    });
                }

                configurationViewModel.LocationCodesSets = await _db.LocationCodeSets.ToListAsync();
                configurationViewModel.ResponsiblePartnerCodes = await _db.ResponsiblePartnerCodes.ToListAsync();

                var lcresult = await (from lc in _db.LocationCodes
                                join lcs in _db.LocationCodeSets on lc.LocationCodeSetId equals lcs.Id
                                select new { lc.Id, lc.Code, lc.Description, lc.LocationCodeSetId, LocationCodeSetDescription = lcs.Description }).ToListAsync();

                foreach (var lc in lcresult)
                {
                    configurationViewModel.LocationCodes.Add(new CustomLocationCode
                    {
                        Id = lc.Id,
                        Code = lc.Code,
                        Description = lc.Description,
                        LocationCodeSetId = lc.LocationCodeSetId,
                        LocationCodeSetDescription = lc.LocationCodeSetDescription
                    });
                }

                return configurationViewModel;
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> CheckDuplicateIssueAsync(IssueNo issueNo)
        {
            try { return await _db.IssueNos.CountAsync(a => a.Name == issueNo.Name); }
            catch { return 0; }
        }

        public async Task<int> CheckDuplicateDesinationAsync(Designation designation)
        {
            try { return await _db.Designations.CountAsync(a => a.Name == designation.Name); }
            catch { return 0; }
        }

        public async Task<int> CheckDuplicateICTFormatAsync(Icnformat icnformat)
        {
            try { return await _db.Icnformats.CountAsync(a => a.Code == icnformat.Code); }
            catch { return 0; }
        }

        public async Task<int> CheckDuplicateInfoCodeAsync(InformationCode infoCode)
        {
            try { return await _db.InformationCodes.CountAsync(a => a.Description == infoCode.Description); }
            catch { return 0; }
        }

        public async Task<int> CheckDuplicateInfoCodeSetAsync(InformationCodeSet informationCodeSet)
        {
            try { return await _db.InformationCodeSets.CountAsync(a => a.Description == informationCodeSet.Description && a.Variant == informationCodeSet.Variant); }
            catch { return 0; }
        }
    }
}
