using CSDBPortal.Data;
using CSDBPortal.Models;
using CSDBPortal.ViewModels;

namespace CSDBPortal.Business
{
    public class ConfigurationsManager
    {
        public ConfigurationViewModel GetConfigurationDetailInfo()
        {
            ConfigurationViewModel configurationViewModel = new();
            try
            {
                using (ApplicationDbContext applicationDbContext = new())
                {

                    configurationViewModel.IssueNos = new List<CustomIssueNo>();
                    configurationViewModel.Designations = new List<Designation>();
                    configurationViewModel.InformationCodes = new List<CustomInformationCode>();
                    configurationViewModel.Icnformats = new List<CustomIcnFormat>();
                    configurationViewModel.IcnFormatMasterDestinationFields = new List<ICNFormatMasterField>();
                    configurationViewModel.IcnFormatMasterFields = new List<ICNFormatMasterField>();

                    configurationViewModel.Designations = applicationDbContext.Designations.ToList();

                    configurationViewModel.InformationCodesSets = applicationDbContext.InformationCodeSets.ToList();

                    configurationViewModel.DataModuleTypes = applicationDbContext.DataModuleTypes.ToList();

                    configurationViewModel.IcnFormatMasterSourceFields = applicationDbContext.ICNFormatMasterFields.ToList();
                    configurationViewModel.IcnFormatMasterFields = applicationDbContext.ICNFormatMasterFields.ToList();
                    
                    foreach (IssueNo issueNo in applicationDbContext.IssueNos)
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

                    foreach (Icnformat icnFormat in applicationDbContext.Icnformats)
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

                        var icnFormatFields = (from icf in applicationDbContext.ICNFormatFields
                                               join icmf in applicationDbContext.ICNFormatMasterFields on icf.ICNFormatMasterFieldId equals icmf.Id
                                               where icf.ICNFormatId == customIcnFormat.Id
                                               orderby icf.DisplayOrder
                                               select new { icf.Id, icf.ICNFormatId, icf.DisplayOrder, icf.ICNFormatMasterFieldId, icmf.Field }).ToList();

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
                        
                        if (applicationDbContext.IcnNumbers.Where(icn => icn.ICNFormatId == customIcnFormat.Id).Count() > 0)
                        {
                            customIcnFormat.CanEdit = false;
                        }
                        else
                        {
                            customIcnFormat.CanEdit = true;
                        }

                        configurationViewModel.Icnformats.Add(customIcnFormat);
                    }


                    var result = (from ic in applicationDbContext.InformationCodes
                                  join ics in applicationDbContext.InformationCodeSets on ic.InformationCodeSetId equals ics.Id
                                  join d in applicationDbContext.DataModuleTypes on ic.DataModuleId equals d.Id
                                  select new { ic.Id, ic.Code, ic.Description, ic.InformationCodeSetId, InformationCodeSetDescription = ics.Description, d.Name, ic.DataModuleId }).ToList();

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

                    return configurationViewModel;
                }
            }
            catch
            {
                throw;
            }
        }
        public int CheckDuplicateIssue(IssueNo issueNo)
        {
            int result = 0;
            try
            {
                using (ApplicationDbContext applicationDbContext = new())
                {
                    result = applicationDbContext.IssueNos.Where(a => a.Name == issueNo.Name).Count();
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }
        public int CheckDuplicateDesination(Designation designation)
        {
            int result = 0;
            try
            {
                using (ApplicationDbContext applicationDbContext = new())
                {
                    result = applicationDbContext.Designations.Where(a => a.Name == designation.Name).Count();
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }
        public int CheckDuplicateICTFormat(Icnformat icnformat)
        {
            int result = 0;
            try
            {
                using (ApplicationDbContext applicationDbContext = new())
                {
                    result = applicationDbContext.Icnformats.Where(a => a.Code == icnformat.Code).Count();
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }
        public int CheckDuplicateInfoCode(InformationCode infoCode)
        {
            int result = 0;
            try
            {
                using (ApplicationDbContext applicationDbContext = new())
                {
                    result = applicationDbContext.InformationCodes.Where(a => a.Description == infoCode.Description).Count();
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int CheckDuplicateInfoCodeSet(InformationCodeSet informationCodeSet)
        {
            int result = 0;
            try
            {
                using (ApplicationDbContext applicationDbContext = new())
                {
                    result = applicationDbContext.InformationCodeSets.Where(a => a.Description == informationCodeSet.Description && a.Variant == informationCodeSet.Variant).Count();
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }
    }
}
