using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSDBPortal.Data.Migrations
{
    /// <inheritdoc />
    [Migration("20260410010000_AddPackageCodes")]
    public partial class AddPackageCodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PackageCodes",
                schema: "dbo",
                columns: table => new
                {
                    Id              = table.Column<int>(nullable: false)
                                          .Annotation("SqlServer:Identity", "1, 1"),
                    Code            = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CodeType        = table.Column<string>(type: "nvarchar(3)",   maxLength: 3,   nullable: false),
                    CreatedAt       = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy       = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsPublished     = table.Column<bool>(nullable: false, defaultValue: false),
                    PublishedAt     = table.Column<DateTime>(nullable: true),
                    PublishedBy     = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PackageFilename = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProjectId       = table.Column<int>(nullable: true),
                    ProjectName     = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageCodes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "UX_PackageCodes_Code",
                schema: "dbo",
                table: "PackageCodes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PackageCodes_CodeType_IsPublished",
                schema: "dbo",
                table: "PackageCodes",
                columns: new[] { "CodeType", "IsPublished" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "PackageCodes", schema: "dbo");
        }
    }
}
