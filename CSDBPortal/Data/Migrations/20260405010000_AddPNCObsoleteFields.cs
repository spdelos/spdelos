using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSDBPortal.Data.Migrations
{
    public partial class AddPNCObsoleteFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsObsolete",
                schema: "dbo",
                table: "PartNumberCodes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ObsoletedBy",
                schema: "dbo",
                table: "PartNumberCodes",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ObsoletedOn",
                schema: "dbo",
                table: "PartNumberCodes",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "IsObsolete",   schema: "dbo", table: "PartNumberCodes");
            migrationBuilder.DropColumn(name: "ObsoletedBy",  schema: "dbo", table: "PartNumberCodes");
            migrationBuilder.DropColumn(name: "ObsoletedOn",  schema: "dbo", table: "PartNumberCodes");
        }
    }
}
