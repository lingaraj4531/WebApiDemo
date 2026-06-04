using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiDemo.Migrations
{
    /// <inheritdoc />
    public partial class InitialBaselineWithSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 🧠 UPGRADE STRATEGY: Skip table creation to preserve existing data!
            // We just add the two columns directly into your live table:

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedTime",
                table: "Students",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // If we roll back, we just remove the columns instead of dropping the whole table
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DeletedTime",
                table: "Students");
        }
    }
}