using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSDBPortal.Data.Migrations
{
    public partial class AddPNSDesignFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add Design Office Responsibility (single digit)
            migrationBuilder.AddColumn<string>(
                name: "DesignOffice",
                schema: "dbo",
                table: "PartNumberCodes",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "");

            // Add Drawing Sequential Number (3-digit)
            migrationBuilder.AddColumn<string>(
                name: "DrawingSeqNo",
                schema: "dbo",
                table: "PartNumberCodes",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            // Widen FullPNC to accommodate 9-segment format
            migrationBuilder.AlterColumn<string>(
                name: "FullPNC",
                schema: "dbo",
                table: "PartNumberCodes",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "DesignOffice",  schema: "dbo", table: "PartNumberCodes");
            migrationBuilder.DropColumn(name: "DrawingSeqNo",  schema: "dbo", table: "PartNumberCodes");

            migrationBuilder.AlterColumn<string>(
                name: "FullPNC",
                schema: "dbo",
                table: "PartNumberCodes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(80)",
                oldMaxLength: 80);
        }
    }
}
