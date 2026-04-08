using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingApp.Server.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIdforVerificationCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VerificationCodes",
                table: "VerificationCodes");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "VerificationCodes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_VerificationCodes",
                table: "VerificationCodes",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VerificationCodes",
                table: "VerificationCodes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "VerificationCodes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VerificationCodes",
                table: "VerificationCodes",
                column: "UserId");
        }
    }
}
