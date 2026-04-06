using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSDBPortal.Data.Migrations
{
    public partial class AddPNSDesignOffDropdown : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Widen DesignOffice to nvarchar(10) — now a dropdown code, not just a digit
            migrationBuilder.AlterColumn<string>(
                name: "DesignOffice",
                schema: "dbo",
                table: "PartNumberCodes",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1);

            // Narrow RevSuffix back to nvarchar(1) — single letter colour scheme code
            migrationBuilder.AlterColumn<string>(
                name: "RevSuffix",
                schema: "dbo",
                table: "PartNumberCodes",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DesignOffice",
                schema: "dbo",
                table: "PartNumberCodes",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "RevSuffix",
                schema: "dbo",
                table: "PartNumberCodes",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1);
        }
    }
}
