using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IT_Asset_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class AddedAssignedbyAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProcessedByAdminId",
                table: "AssignmentRequests",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentRequests_ProcessedByAdminId",
                table: "AssignmentRequests",
                column: "ProcessedByAdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentRequests_Users_ProcessedByAdminId",
                table: "AssignmentRequests",
                column: "ProcessedByAdminId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentRequests_Users_ProcessedByAdminId",
                table: "AssignmentRequests");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentRequests_ProcessedByAdminId",
                table: "AssignmentRequests");

            migrationBuilder.DropColumn(
                name: "ProcessedByAdminId",
                table: "AssignmentRequests");
        }
    }
}
