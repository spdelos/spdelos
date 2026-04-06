using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSDBPortal.Data.Migrations
{
    public partial class AddPNSTechSpecRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Widen MaintLevel to 2 chars (Technical Specification Number)
            migrationBuilder.AlterColumn<string>(
                name: "MaintLevel",
                schema: "dbo",
                table: "PartNumberCodes",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1);

            // Widen RevSuffix to 10 chars (Customised Colour Scheme)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MaintLevel",
                schema: "dbo",
                table: "PartNumberCodes",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2)",
                oldMaxLength: 2);

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
    }
}
