using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UI.Migrations
{
    public partial class fkbreaktype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Breaks_BreakTypes_BreakTypeId",
                table: "Breaks");

            migrationBuilder.DropColumn(
                name: "BreakTypeName",
                table: "Breaks");

            migrationBuilder.AlterColumn<int>(
                name: "BreakTypeId",
                table: "Breaks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Breaks_BreakTypes_BreakTypeId",
                table: "Breaks",
                column: "BreakTypeId",
                principalTable: "BreakTypes",
                principalColumn: "BreakTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Breaks_BreakTypes_BreakTypeId",
                table: "Breaks");

            migrationBuilder.AlterColumn<int>(
                name: "BreakTypeId",
                table: "Breaks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "BreakTypeName",
                table: "Breaks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Breaks_BreakTypes_BreakTypeId",
                table: "Breaks",
                column: "BreakTypeId",
                principalTable: "BreakTypes",
                principalColumn: "BreakTypeId");
        }
    }
}
