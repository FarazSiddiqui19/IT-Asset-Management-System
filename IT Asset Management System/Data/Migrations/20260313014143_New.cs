using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IT_Asset_Management_System.Data.Migrations
{
    /// <inheritdoc />
    public partial class New : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelledAt",
                table: "AssignmentRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledAt",
                table: "AssignmentRequests",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
