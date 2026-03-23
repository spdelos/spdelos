using CSDBPortal.Models;

namespace CSDBPortal.ViewModels
{
    public class ConfigurationViewModel
    {
        public List<CustomIssueNo>? IssueNos { get; set; }
        public List<Designation>? Designations { get; set; }
        public List<CustomInformationCode>? InformationCodes { get; set; }
        public List<CustomIcnFormat>? Icnformats { get; set; }
        public List<InformationCodeSet>? InformationCodesSets { get; set; }
        public List<DataModuleType>? DataModuleTypes { get; set; }
        public List<ICNFormatMasterField>? IcnFormatMasterSourceFields { get; set; }
        public List<ICNFormatMasterField>? IcnFormatMasterDestinationFields { get; set; }
        public List<ICNFormatMasterField>? IcnFormatMasterFields { get; set; }
        public List<CustomLocationCode>? LocationCodes { get; set; }
        public List<LocationCodeSet>? LocationCodesSets { get; set; }
        public List<ResponsiblePartnerCode>? ResponsiblePartnerCodes { get; set; }
    }

    public class CustomInformationCode: InformationCode
    {
        public string? DataModuleName { get; set; }
        public string? InformationCodeSetDescription { get; set; }
    }

    public class CustomIssueNo : IssueNo
    {
        public IFormFileCollection Files { get; set; }
    }

    public class CustomIcnFormat : Icnformat
    {
        public List<CustomIcnFormatField>? IcnFormatFields { get; set; }

        public bool CanEdit { get; set; }
    }

    public class CustomIcnFormatField : ICNFormatField
    {
        public string? Field { get; set; }
    }
}
