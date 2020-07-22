using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryUOITC.Migrations
{
    public partial class init003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShelfNumber",
                table: "Books",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsDeleted", "Password" },
                values: new object[] { false, "HzK5SmhAssVbQO+YJyhfYuzwU2PqizK5Xc28EJtFrmTB1mVF" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShelfNumber",
                table: "Books");

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsDeleted", "Password" },
                values: new object[] { false, "dPers42tsv53Ig2cJtts/ZmtYJ2yasDM4ssEvJyN4H4Kvlip" });
        }
    }
}
