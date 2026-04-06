using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSDBPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.CreateTable(
                name: "ApplicationSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BrexRules",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Group = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RuleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Xml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dmtype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssueNoId = table.Column<int>(type: "int", nullable: true),
                    XmlTag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubXmlTag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Length = table.Column<int>(type: "int", nullable: true),
                    RangeValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MatchValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttributeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrexRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyInformation",
                schema: "dbo",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SharedPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyInformation", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "DataModule",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    StandardNumberingSystemId = table.Column<int>(type: "int", nullable: false),
                    InformationCodeId = table.Column<int>(type: "int", nullable: false),
                    InformationCodeDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortInfoCodeBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dmc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataModuleTypeId = table.Column<int>(type: "int", nullable: false),
                    DisassemblyCodeVariant = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemLocationId = table.Column<int>(type: "int", nullable: false),
                    AssocateEagleTalk = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lcn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ALC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lcntype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    IssueNoId = table.Column<int>(type: "int", nullable: false),
                    InWork = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssignedTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataModule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataModuleCode",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DMC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ModelIdentification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SDC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StandardNumberingSystem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DCV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InformationCodeId = table.Column<int>(type: "int", nullable: false),
                    ICV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationCodeId = table.Column<int>(type: "int", nullable: false),
                    TechName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InfoName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssueFileId = table.Column<int>(type: "int", nullable: false),
                    xml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsBrexXml = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CheckoutStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckedOutBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckedOutOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OriginalXml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataModuleCode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataModuleStatus",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataModuleId = table.Column<int>(type: "int", nullable: false),
                    UploadedFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCurrentVersion = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataModuleStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataModuleTypes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IssueNo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataModuleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Designations",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Designations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ICNFormatMasterFields",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Field = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Template = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectFieldReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsForeignKey = table.Column<bool>(type: "bit", nullable: true),
                    ReferenceTable = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceColumn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ICNFormatMasterFields", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ICNFormats",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ICNFormats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IcnNumbers",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataModuleId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAllocated = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ICNFormatId = table.Column<int>(type: "int", nullable: false),
                    SeqNo = table.Column<int>(type: "int", nullable: false),
                    VarCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssueNo = table.Column<int>(type: "int", nullable: false),
                    AssignedTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckedInBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckedInOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IcnNumbers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImageAssets",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageAssets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InformationCodes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataModuleId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InformationCodeSetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformationCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InformationCodeSets",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Variant = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformationCodeSets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IssueNo",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrexTemplate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueNo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LicenseManagement",
                schema: "dbo",
                columns: table => new
                {
                    LicenseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Encrypted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseManagement", x => x.LicenseId);
                });

            migrationBuilder.CreateTable(
                name: "LocationCodes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocationCodeSetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LocationCodeSets",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationCodeSets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartNumberCodes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    EqCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    ModCode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SubAsmCode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    DesignOffice = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DrawingSeqNo = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    SeqDigits = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    MaintLevel = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    RevSuffix = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    FullPNC = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    PartName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsObsolete = table.Column<bool>(type: "bit", nullable: false),
                    ObsoletedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ObsoletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartNumberCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PNCLookups",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LookupType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PNCLookups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EndItem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssueNoId = table.Column<int>(type: "int", nullable: false),
                    IcnformatId = table.Column<int>(type: "int", nullable: false),
                    SnssetId = table.Column<int>(type: "int", nullable: false),
                    InformationCodeId = table.Column<int>(type: "int", nullable: false),
                    LocationCodeId = table.Column<int>(type: "int", nullable: false),
                    ModelIdentification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sdc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubjectLength = table.Column<int>(type: "int", nullable: false),
                    RPCId = table.Column<int>(type: "int", nullable: false),
                    BrexTemplate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NavigationXml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrackPercentComplete = table.Column<bool>(type: "bit", nullable: false),
                    CreateDefaultBrex = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectNavigation",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    DMCId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectNavigation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectStandardNumberingSystems",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Snsid = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectStandardNumberingSystems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuickAccessItems",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Href = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuickAccessItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResponsiblePartnerCodes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rpccage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrginatorCage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponsiblePartnerCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StandardNumberingSystems",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardNumberingSystems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stylesheets",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stylesheets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserDetails",
                schema: "dbo",
                columns: table => new
                {
                    UserDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DesignationFk = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDetails", x => x.UserDetailId);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowInstances",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CurrentStep = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowInstances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowSteps",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowInstanceId = table.Column<int>(type: "int", nullable: false),
                    StepNumber = table.Column<int>(type: "int", nullable: false),
                    StepName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    ReferenceType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowSteps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "XmlValidation",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataModuleId = table.Column<int>(type: "int", nullable: false),
                    UploadStatus = table.Column<bool>(type: "bit", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XmlValidation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ICNFormatFields",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ICNFormatMasterFieldId = table.Column<int>(type: "int", nullable: false),
                    ICNFormatId = table.Column<int>(type: "int", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ICNFormatFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ICNFormatFields_ICNFormats_ICNFormatId",
                        column: x => x.ICNFormatId,
                        principalSchema: "dbo",
                        principalTable: "ICNFormats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IssueTypeFiles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssueNoId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueTypeFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssueTypeFiles_IssueNo_IssueNoId",
                        column: x => x.IssueNoId,
                        principalSchema: "dbo",
                        principalTable: "IssueNo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrexRule_ProjectId",
                schema: "dbo",
                table: "BrexRules",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_DataModuleCode_InformationCodeId",
                schema: "dbo",
                table: "DataModuleCode",
                column: "InformationCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_DataModuleCode_IsDeleted",
                schema: "dbo",
                table: "DataModuleCode",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_DataModuleCode_LocationCodeId",
                schema: "dbo",
                table: "DataModuleCode",
                column: "LocationCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_DataModuleCode_ProjectId",
                schema: "dbo",
                table: "DataModuleCode",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_DataModuleCode_ProjectId_IsBrexXml",
                schema: "dbo",
                table: "DataModuleCode",
                columns: new[] { "ProjectId", "IsBrexXml" });

            migrationBuilder.CreateIndex(
                name: "IX_DataModuleCode_ProjectId_IsDeleted",
                schema: "dbo",
                table: "DataModuleCode",
                columns: new[] { "ProjectId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_ICNFormatField_ICNFormatId",
                schema: "dbo",
                table: "ICNFormatFields",
                column: "ICNFormatId");

            migrationBuilder.CreateIndex(
                name: "IX_IcnNumber_ProjectId",
                schema: "dbo",
                table: "IcnNumbers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_IcnNumber_ProjectId_SeqNo",
                schema: "dbo",
                table: "IcnNumbers",
                columns: new[] { "ProjectId", "SeqNo" });

            migrationBuilder.CreateIndex(
                name: "IX_InformationCode_InformationCodeSetId",
                schema: "dbo",
                table: "InformationCodes",
                column: "InformationCodeSetId");

            migrationBuilder.CreateIndex(
                name: "IX_IssueTypeFile_IssueNoId",
                schema: "dbo",
                table: "IssueTypeFiles",
                column: "IssueNoId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationCode_LocationCodeSetId",
                schema: "dbo",
                table: "LocationCodes",
                column: "LocationCodeSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_IcnformatId",
                schema: "dbo",
                table: "Project",
                column: "IcnformatId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_InformationCodeId",
                schema: "dbo",
                table: "Project",
                column: "InformationCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_IssueNoId",
                schema: "dbo",
                table: "Project",
                column: "IssueNoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectNavigation_ParentId",
                schema: "dbo",
                table: "ProjectNavigation",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectNavigation_ProjectId",
                schema: "dbo",
                table: "ProjectNavigation",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectNavigation_ProjectId_ParentId",
                schema: "dbo",
                table: "ProjectNavigation",
                columns: new[] { "ProjectId", "ParentId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSNS_ProjectId",
                schema: "dbo",
                table: "ProjectStandardNumberingSystems",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSNS_ProjectId_Snsid",
                schema: "dbo",
                table: "ProjectStandardNumberingSystems",
                columns: new[] { "ProjectId", "Snsid" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSNS_Snsid",
                schema: "dbo",
                table: "ProjectStandardNumberingSystems",
                column: "Snsid");

            migrationBuilder.CreateIndex(
                name: "IX_QuickAccessItem_IsActive",
                schema: "dbo",
                table: "QuickAccessItems",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_QuickAccessItem_SortOrder",
                schema: "dbo",
                table: "QuickAccessItems",
                column: "SortOrder");

            migrationBuilder.CreateIndex(
                name: "IX_StandardNumberingSystem_ParentId",
                schema: "dbo",
                table: "StandardNumberingSystems",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstance_CreatedOn",
                schema: "dbo",
                table: "WorkflowInstances",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstance_ProjectId",
                schema: "dbo",
                table: "WorkflowInstances",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstance_Status",
                schema: "dbo",
                table: "WorkflowInstances",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStep_InstanceId_StepNumber",
                schema: "dbo",
                table: "WorkflowSteps",
                columns: new[] { "WorkflowInstanceId", "StepNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStep_WorkflowInstanceId",
                schema: "dbo",
                table: "WorkflowSteps",
                column: "WorkflowInstanceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationSettings");

            migrationBuilder.DropTable(
                name: "BrexRules",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CompanyInformation",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DataModule",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DataModuleCode",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DataModuleStatus",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DataModuleTypes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Designations",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ICNFormatFields",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ICNFormatMasterFields",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "IcnNumbers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ImageAssets",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InformationCodes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InformationCodeSets",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "IssueTypeFiles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "LicenseManagement",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "LocationCodes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "LocationCodeSets",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PartNumberCodes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PNCLookups",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Project",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProjectNavigation",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProjectStandardNumberingSystems",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "QuickAccessItems",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ResponsiblePartnerCodes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "StandardNumberingSystems",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Stylesheets",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserDetails",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "WorkflowInstances",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "WorkflowSteps",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "XmlValidation",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ICNFormats",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "IssueNo",
                schema: "dbo");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
