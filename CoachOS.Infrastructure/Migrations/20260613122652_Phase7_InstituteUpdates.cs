using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoachOS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Phase7_InstituteUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Institutes");

            migrationBuilder.RenameColumn(
                name: "Mobile",
                table: "Institutes",
                newName: "Pincode");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Institutes",
                newName: "EmailAddress");

            migrationBuilder.AlterColumn<string>(
                name: "LogoPath",
                table: "Institutes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AadhaarNumber",
                table: "Institutes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicSessionEndMonth",
                table: "Institutes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AcademicSessionStartMonth",
                table: "Institutes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine1",
                table: "Institutes",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "Institutes",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AlternateMobileNumber",
                table: "Institutes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Institutes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactPersonName",
                table: "Institutes",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Institutes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Institutes",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "INR");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Institutes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailEnabled",
                table: "Institutes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "EstablishedYear",
                table: "Institutes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "Institutes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InstituteCode",
                table: "Institutes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InstituteType",
                table: "Institutes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsTrial",
                table: "Institutes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MaxStudentsAllowed",
                table: "Institutes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxTeachersAllowed",
                table: "Institutes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MobileNumber",
                table: "Institutes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OwnerEmail",
                table: "Institutes",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerMobile",
                table: "Institutes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PANNumber",
                table: "Institutes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlanName",
                table: "Institutes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "SMSEnabled",
                table: "Institutes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "Institutes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Institutes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WebsiteUrl",
                table: "Institutes",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WhatsAppEnabled",
                table: "Institutes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AadhaarNumber",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "AcademicSessionEndMonth",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "AcademicSessionStartMonth",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "AddressLine1",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "AddressLine2",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "AlternateMobileNumber",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "ContactPersonName",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "EmailEnabled",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "EstablishedYear",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "InstituteCode",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "InstituteType",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "IsTrial",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "MaxStudentsAllowed",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "MaxTeachersAllowed",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "MobileNumber",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "OwnerEmail",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "OwnerMobile",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "PANNumber",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "PlanName",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "SMSEnabled",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "ShortName",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "WebsiteUrl",
                table: "Institutes");

            migrationBuilder.DropColumn(
                name: "WhatsAppEnabled",
                table: "Institutes");

            migrationBuilder.RenameColumn(
                name: "Pincode",
                table: "Institutes",
                newName: "Mobile");

            migrationBuilder.RenameColumn(
                name: "EmailAddress",
                table: "Institutes",
                newName: "Email");

            migrationBuilder.AlterColumn<string>(
                name: "LogoPath",
                table: "Institutes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Institutes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
