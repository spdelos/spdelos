using CSDBPortal.Models;
namespace CSDBPortal.ViewModels
{
    public class MaintenanceViewModel
    {
        public List<CustomLocationCode>? LocationCodes { get; set; }
        public List<LocationCodeSet>? LocationCodesSets { get; set; }
        public List<ResponsiblePartnerCode>? ResponsiblePartnerCodes { get; set; }
        public List<CustomStandardNumberingSystem>? StandardNumberingSystems { get; set; }
        public List<DataModuleType>? DataModuleTypes { get; set; }
        public List<CustomProjet>? Projects { get; set; }

        public List<CustomDataModuleCode>? DataModuleCodes { get; set; }
        public List<IssueNo>? Issues { get; set; }
        public List<InformationCodeSet>? InformationCodeSets { get; set;}
        public List<Icnformat>? Icnformats { get; set; }
    }

    public class CustomDataModuleCode : DataModuleCode
    {
        public string ProjectName { get; set; }
        public string InformationCodeDesc { get; set; }
        public string LocationCodeDesc { get; set; }
    }

    public class CustomProjet : Project
    {
        public string InformationCodeProp { get; set; }
        public string ICNFormatDescription { get; set; }
        public string Variant { get; set; }
    }

    public class CustomStandardNumberingSystem : StandardNumberingSystem
    {
        public string? ParentCode { get; set; }
        //public int id { get; set; }
        public int parent_id { get; set; }
        public string title { get; set; }
        public int level { get; set; }
        public int? SnsId { get; set; }
    }

    public class CustomLocationCode : LocationCode
    {
        public string LocationCodeSetDescription { get; set; }
    }
}
