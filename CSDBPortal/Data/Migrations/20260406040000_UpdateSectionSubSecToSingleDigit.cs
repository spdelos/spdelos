using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSDBPortal.Data.Migrations
{
    public partial class UpdateSectionSubSecToSingleDigit : Migration
    {
        protected override void Up(MigrationBuilder m)
        {
            m.AlterColumn<string>("ModCode", schema: "dbo", table: "PartNumberCodes",
                type: "nvarchar(1)", maxLength: 1, nullable: false,
                oldClrType: typeof(string), oldType: "nvarchar(3)", oldMaxLength: 3);

            m.AlterColumn<string>("SubAsmCode", schema: "dbo", table: "PartNumberCodes",
                type: "nvarchar(1)", maxLength: 1, nullable: false,
                oldClrType: typeof(string), oldType: "nvarchar(3)", oldMaxLength: 3);
        }

        protected override void Down(MigrationBuilder m)
        {
            m.AlterColumn<string>("ModCode", schema: "dbo", table: "PartNumberCodes",
                type: "nvarchar(3)", maxLength: 3, nullable: false,
                oldClrType: typeof(string), oldType: "nvarchar(1)", oldMaxLength: 1);

            m.AlterColumn<string>("SubAsmCode", schema: "dbo", table: "PartNumberCodes",
                type: "nvarchar(3)", maxLength: 3, nullable: false,
                oldClrType: typeof(string), oldType: "nvarchar(1)", oldMaxLength: 1);
        }
    }
}
