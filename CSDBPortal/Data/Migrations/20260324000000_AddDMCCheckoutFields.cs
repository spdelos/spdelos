using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSDBPortal.Data.Migrations
{
    public partial class AddDMCCheckoutFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CheckoutStatus",
                table: "DataModuleCode",
                schema: "dbo",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckedOutBy",
                table: "DataModuleCode",
                schema: "dbo",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckedOutOn",
                table: "DataModuleCode",
                schema: "dbo",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalXml",
                table: "DataModuleCode",
                schema: "dbo",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "CheckoutStatus", table: "DataModuleCode", schema: "dbo");
            migrationBuilder.DropColumn(name: "CheckedOutBy",   table: "DataModuleCode", schema: "dbo");
            migrationBuilder.DropColumn(name: "CheckedOutOn",   table: "DataModuleCode", schema: "dbo");
            migrationBuilder.DropColumn(name: "OriginalXml",    table: "DataModuleCode", schema: "dbo");
        }
    }
}
