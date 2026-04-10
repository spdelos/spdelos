using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSDBPortal.Data.Migrations
{
    /// <inheritdoc />
    [Migration("20260410000000_AddIetpLicense")]
    public partial class AddIetpLicense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IETP_License",
                schema: "dbo",
                columns: table => new
                {
                    Id           = table.Column<int>(nullable: false)
                                       .Annotation("SqlServer:Identity", "1, 1"),
                    IETP         = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsSecured    = table.Column<bool>(nullable: false, defaultValue: false),
                    ClientKey    = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    LicenseKey   = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsTrial      = table.Column<bool>(nullable: false, defaultValue: false),
                    CreationTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IETP_License", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IETP_License_IETP",
                schema: "dbo",
                table: "IETP_License",
                column: "IETP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "IETP_License", schema: "dbo");
        }
    }
}
