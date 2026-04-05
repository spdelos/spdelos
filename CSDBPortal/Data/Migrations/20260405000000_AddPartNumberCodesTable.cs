using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSDBPortal.Data.Migrations
{
    public partial class AddPartNumberCodesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PartNumberCodes",
                schema: "dbo",
                columns: table => new
                {
                    Id         = table.Column<int>(nullable: false)
                                     .Annotation("SqlServer:Identity", "1, 1"),
                    ModelId    = table.Column<string>(type: "nvarchar(10)",  maxLength: 10,  nullable: false),
                    EqCode     = table.Column<string>(type: "nvarchar(2)",   maxLength: 2,   nullable: false),
                    ModCode    = table.Column<string>(type: "nvarchar(3)",   maxLength: 3,   nullable: false),
                    SubAsmCode = table.Column<string>(type: "nvarchar(3)",   maxLength: 3,   nullable: false),
                    SeqDigits  = table.Column<string>(type: "nvarchar(5)",   maxLength: 5,   nullable: false),
                    MaintLevel = table.Column<string>(type: "nvarchar(1)",   maxLength: 1,   nullable: false),
                    RevSuffix  = table.Column<string>(type: "nvarchar(1)",   maxLength: 1,   nullable: false),
                    FullPNC    = table.Column<string>(type: "nvarchar(50)",  maxLength: 50,  nullable: false),
                    CreatedBy  = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedOn  = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartNumberCodes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PartNumberCodes_FullPNC",
                schema: "dbo",
                table: "PartNumberCodes",
                column: "FullPNC",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "PartNumberCodes", schema: "dbo");
        }
    }
}
