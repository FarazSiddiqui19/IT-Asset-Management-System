using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IT_Asset_Management_System.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionAndCancelledAtToAssignmentRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledAt",
                table: "AssignmentRequests",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AssignmentRequests",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelledAt",
                table: "AssignmentRequests");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AssignmentRequests");
        }
    }
}
