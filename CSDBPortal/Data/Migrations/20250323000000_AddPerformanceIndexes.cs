using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSDBPortal.Data.Migrations
{
    public partial class AddPerformanceIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // DataModuleCode
            migrationBuilder.CreateIndex("IX_DataModuleCode_ProjectId",           "DataModuleCodes", "ProjectId");
            migrationBuilder.CreateIndex("IX_DataModuleCode_InformationCodeId",   "DataModuleCodes", "InformationCodeId");
            migrationBuilder.CreateIndex("IX_DataModuleCode_LocationCodeId",      "DataModuleCodes", "LocationCodeId");
            migrationBuilder.CreateIndex("IX_DataModuleCode_IsDeleted",           "DataModuleCodes", "IsDeleted");
            migrationBuilder.CreateIndex("IX_DataModuleCode_ProjectId_IsDeleted", "DataModuleCodes", new[] { "ProjectId", "IsDeleted" });
            migrationBuilder.CreateIndex("IX_DataModuleCode_ProjectId_IsBrexXml", "DataModuleCodes", new[] { "ProjectId", "IsBrexXml" });

            // ProjectNavigation
            migrationBuilder.CreateIndex("IX_ProjectNavigation_ProjectId",          "ProjectNavigations", "ProjectId");
            migrationBuilder.CreateIndex("IX_ProjectNavigation_ParentId",           "ProjectNavigations", "ParentId");
            migrationBuilder.CreateIndex("IX_ProjectNavigation_ProjectId_ParentId", "ProjectNavigations", new[] { "ProjectId", "ParentId" });

            // StandardNumberingSystem
            migrationBuilder.CreateIndex("IX_StandardNumberingSystem_ParentId", "StandardNumberingSystems", "ParentId");

            // LocationCode
            migrationBuilder.CreateIndex("IX_LocationCode_LocationCodeSetId", "LocationCodes", "LocationCodeSetId");

            // InformationCode
            migrationBuilder.CreateIndex("IX_InformationCode_InformationCodeSetId", "InformationCodes", "InformationCodeSetId");

            // Project
            migrationBuilder.CreateIndex("IX_Project_IcnformatId",       "Projects", "IcnformatId");
            migrationBuilder.CreateIndex("IX_Project_InformationCodeId", "Projects", "InformationCodeId");
            migrationBuilder.CreateIndex("IX_Project_IssueNoId",         "Projects", "IssueNoId");

            // IcnNumber
            migrationBuilder.CreateIndex("IX_IcnNumber_ProjectId",       "IcnNumbers", "ProjectId");
            migrationBuilder.CreateIndex("IX_IcnNumber_ProjectId_SeqNo", "IcnNumbers", new[] { "ProjectId", "SeqNo" });

            // BrexRule
            migrationBuilder.CreateIndex("IX_BrexRule_ProjectId", "BrexRules", "ProjectId");

            // ProjectStandardNumberingSystem
            migrationBuilder.CreateIndex("IX_ProjectSNS_ProjectId",         "ProjectStandardNumberingSystems", "ProjectId");
            migrationBuilder.CreateIndex("IX_ProjectSNS_Snsid",             "ProjectStandardNumberingSystems", "Snsid");
            migrationBuilder.CreateIndex("IX_ProjectSNS_ProjectId_Snsid",   "ProjectStandardNumberingSystems", new[] { "ProjectId", "Snsid" });

            // IssueTypeFile
            migrationBuilder.CreateIndex("IX_IssueTypeFile_IssueNoId", "IssueTypeFiles", "IssueNoId");

            // ICNFormatField
            migrationBuilder.CreateIndex("IX_ICNFormatField_ICNFormatId", "ICNFormatFields", "ICNFormatId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex("IX_DataModuleCode_ProjectId",           "DataModuleCodes");
            migrationBuilder.DropIndex("IX_DataModuleCode_InformationCodeId",   "DataModuleCodes");
            migrationBuilder.DropIndex("IX_DataModuleCode_LocationCodeId",      "DataModuleCodes");
            migrationBuilder.DropIndex("IX_DataModuleCode_IsDeleted",           "DataModuleCodes");
            migrationBuilder.DropIndex("IX_DataModuleCode_ProjectId_IsDeleted", "DataModuleCodes");
            migrationBuilder.DropIndex("IX_DataModuleCode_ProjectId_IsBrexXml", "DataModuleCodes");
            migrationBuilder.DropIndex("IX_ProjectNavigation_ProjectId",          "ProjectNavigations");
            migrationBuilder.DropIndex("IX_ProjectNavigation_ParentId",           "ProjectNavigations");
            migrationBuilder.DropIndex("IX_ProjectNavigation_ProjectId_ParentId", "ProjectNavigations");
            migrationBuilder.DropIndex("IX_StandardNumberingSystem_ParentId",    "StandardNumberingSystems");
            migrationBuilder.DropIndex("IX_LocationCode_LocationCodeSetId",      "LocationCodes");
            migrationBuilder.DropIndex("IX_InformationCode_InformationCodeSetId","InformationCodes");
            migrationBuilder.DropIndex("IX_Project_IcnformatId",                 "Projects");
            migrationBuilder.DropIndex("IX_Project_InformationCodeId",           "Projects");
            migrationBuilder.DropIndex("IX_Project_IssueNoId",                   "Projects");
            migrationBuilder.DropIndex("IX_IcnNumber_ProjectId",                 "IcnNumbers");
            migrationBuilder.DropIndex("IX_IcnNumber_ProjectId_SeqNo",           "IcnNumbers");
            migrationBuilder.DropIndex("IX_BrexRule_ProjectId",                  "BrexRules");
            migrationBuilder.DropIndex("IX_ProjectSNS_ProjectId",                "ProjectStandardNumberingSystems");
            migrationBuilder.DropIndex("IX_ProjectSNS_Snsid",                    "ProjectStandardNumberingSystems");
            migrationBuilder.DropIndex("IX_ProjectSNS_ProjectId_Snsid",          "ProjectStandardNumberingSystems");
            migrationBuilder.DropIndex("IX_IssueTypeFile_IssueNoId",             "IssueTypeFiles");
            migrationBuilder.DropIndex("IX_ICNFormatField_ICNFormatId",          "ICNFormatFields");
        }
    }
}
