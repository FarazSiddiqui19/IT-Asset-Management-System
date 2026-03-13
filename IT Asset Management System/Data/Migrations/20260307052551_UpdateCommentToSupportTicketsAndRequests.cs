using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IT_Asset_Management_System.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCommentToSupportTicketsAndRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "TicketId",
                table: "Comments",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "AssignmentRequestId",
                table: "Comments",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Comments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AssignmentRequestId",
                table: "Comments",
                column: "AssignmentRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AssignmentRequests_AssignmentRequestId",
                table: "Comments",
                column: "AssignmentRequestId",
                principalTable: "AssignmentRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AssignmentRequests_AssignmentRequestId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_AssignmentRequestId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "AssignmentRequestId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Comments");

            migrationBuilder.AlterColumn<Guid>(
                name: "TicketId",
                table: "Comments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
