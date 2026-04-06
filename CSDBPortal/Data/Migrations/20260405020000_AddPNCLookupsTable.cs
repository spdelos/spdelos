using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSDBPortal.Data.Migrations
{
    public partial class AddPNCLookupsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PNCLookups",
                schema: "dbo",
                columns: table => new
                {
                    Id          = table.Column<int>(nullable: false)
                                       .Annotation("SqlServer:Identity", "1, 1"),
                    LookupType  = table.Column<string>(type: "nvarchar(20)",  maxLength: 20,  nullable: false),
                    Code        = table.Column<string>(type: "nvarchar(10)",  maxLength: 10,  nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SortOrder   = table.Column<int>(nullable: false, defaultValue: 0),
                    CreatedBy   = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedOn   = table.Column<DateTime>(nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_PNCLookups", x => x.Id));

            migrationBuilder.CreateIndex(
                name: "IX_PNCLookups_Type_Code",
                schema: "dbo",
                table: "PNCLookups",
                columns: new[] { "LookupType", "Code" },
                unique: true);

            // ── Seed data ────────────────────────────────────────────────────
            var now = new DateTime(2026, 4, 5, 0, 0, 0, DateTimeKind.Utc);

            // Equipment Codes
            var eqRows = new (string Code, string Desc, int Sort)[] {
                ("70","Standard Practices",1), ("71","Power Plant",2), ("72","Engine",3),
                ("73","Engine Fuel and Control",4), ("74","Ignition",5), ("75","Air General",6),
                ("76","Engine Controls",7), ("77","Engine Indicating",8), ("78","Exhaust",9),
                ("79","Oil",10), ("80","Starting",11)
            };
            foreach (var r in eqRows)
                migrationBuilder.InsertData("PNCLookups", new[] { "LookupType","Code","Description","SortOrder","CreatedOn" }, new object[] { "EqCode", r.Code, r.Desc, r.Sort, now }, "dbo");

            // Module Codes
            var modRows = new (string Code, string Desc, int Sort)[] {
                ("EGN","Engine General",1), ("EEX","Engine Exhaust",2), ("LPC","Low Pressure Compressor",3),
                ("COU","Coupler",4), ("FAN","Fan",5), ("ICA","Intermediate Casing",6),
                ("HPC","High Pressure Compressor",7), ("DCO","Diffuser & Combustor",8),
                ("TNZ","Transition Zone",9), ("HPT","High Pressure Turbine",10),
                ("LPT","Low Pressure Turbine",11), ("TEC","Turbine Exhaust Case",12),
                ("MGB","Main Gearbox",13), ("AGB","Accessory Gearbox",14)
            };
            foreach (var r in modRows)
                migrationBuilder.InsertData("PNCLookups", new[] { "LookupType","Code","Description","SortOrder","CreatedOn" }, new object[] { "ModCode", r.Code, r.Desc, r.Sort, now }, "dbo");

            // Sub-Assembly Codes
            var subRows = new (string Code, string Desc, int Sort)[] {
                ("FCA","Fan Case",1), ("FBL","Fan Blades",2), ("FHB","Fan Hub",3),
                ("FSH","Fan Shaft",4), ("BRG","Bearings",5),
                ("TTG","Test Tools & Ground Equipment (TT&GE)",6),
                ("FDU","Fan Duct",7), ("FVA","Fan Vane",8),
                ("POL","POL (Petroleum, Oil, Lubricants)",9),
                ("STP","Standard Fasteners (Bolts, Nuts, Washers)",10),
                ("CON","Consumables (Class C items)",11),
                ("SPR","Spares (Replacement Kits)",12)
            };
            foreach (var r in subRows)
                migrationBuilder.InsertData("PNCLookups", new[] { "LookupType","Code","Description","SortOrder","CreatedOn" }, new object[] { "SubAsm", r.Code, r.Desc, r.Sort, now }, "dbo");

            // Maintenance Levels
            var mlRows = new (string Code, string Desc, int Sort)[] {
                ("O","O Level - Operational",1), ("I","I Level - Intermediate",2), ("D","D Level - Depot",3)
            };
            foreach (var r in mlRows)
                migrationBuilder.InsertData("PNCLookups", new[] { "LookupType","Code","Description","SortOrder","CreatedOn" }, new object[] { "MaintLevel", r.Code, r.Desc, r.Sort, now }, "dbo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "PNCLookups", schema: "dbo");
        }
    }
}
