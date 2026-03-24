using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSDBPortal.Data.Migrations
{
    public partial class AddAllocationFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedTo",
                table: "DataModuleCode",
                schema: "dbo",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignedTo",
                table: "IcnNumbers",
                schema: "dbo",
                type: "nvarchar(450)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "AssignedTo", table: "DataModuleCode", schema: "dbo");
            migrationBuilder.DropColumn(name: "AssignedTo", table: "IcnNumbers",    schema: "dbo");
        }
    }
}
