using Microsoft.EntityFrameworkCore.Migrations;

namespace StaffApi.Migrations
{
    public partial class RemovePositionFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Employees_EmployeeId",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Positions_EmployeeId",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Positions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Positions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Positions_EmployeeId",
                table: "Positions",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Employees_EmployeeId",
                table: "Positions",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
