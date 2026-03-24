using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSDBPortal.Data.Migrations
{
    public partial class AddImageAssetsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImageAssets",
                schema: "dbo",
                columns: table => new
                {
                    Id         = table.Column<int>(nullable: false)
                                     .Annotation("SqlServer:Identity", "1, 1"),
                    Name       = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FileName   = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    MimeType   = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Data       = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks    = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    UploadedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UploadedOn = table.Column<DateTime>(nullable: false),
                    UpdatedBy  = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    UpdatedOn  = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageAssets", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "ImageAssets", schema: "dbo");
        }
    }
}
