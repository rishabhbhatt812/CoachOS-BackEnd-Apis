using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoachOS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchemaFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstituteModules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Institutes",
                table: "Institutes");

            migrationBuilder.RenameTable(
                name: "Institutes",
                newName: "Organizations");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Modules",
                newName: "ModuleName");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Modules",
                newName: "ModuleCode");

            migrationBuilder.RenameIndex(
                name: "IX_Modules_Code",
                table: "Modules",
                newName: "IX_Modules_ModuleCode");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Organizations",
                newName: "OrganizationId");

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "Modules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefaultEnabled",
                table: "Modules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMenuItem",
                table: "Modules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentModuleId",
                table: "Modules",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoutePath",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Organizations",
                table: "Organizations",
                column: "OrganizationId");

            migrationBuilder.CreateTable(
                name: "OrganizationModules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    EnabledBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EnabledOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisabledBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DisabledOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationModules_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrganizationModules_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Modules_ParentModuleId",
                table: "Modules",
                column: "ParentModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationModules_ModuleId",
                table: "OrganizationModules",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationModules_OrganizationId_ModuleId",
                table: "OrganizationModules",
                columns: new[] { "OrganizationId", "ModuleId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Modules_ParentModuleId",
                table: "Modules",
                column: "ParentModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Modules_ParentModuleId",
                table: "Modules");

            migrationBuilder.DropTable(
                name: "OrganizationModules");

            migrationBuilder.DropIndex(
                name: "IX_Modules_ParentModuleId",
                table: "Modules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Organizations",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "IsDefaultEnabled",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "IsMenuItem",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "ParentModuleId",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "RoutePath",
                table: "Modules");

            migrationBuilder.RenameTable(
                name: "Organizations",
                newName: "Institutes");

            migrationBuilder.RenameColumn(
                name: "ModuleName",
                table: "Modules",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ModuleCode",
                table: "Modules",
                newName: "Code");

            migrationBuilder.RenameIndex(
                name: "IX_Modules_ModuleCode",
                table: "Modules",
                newName: "IX_Modules_Code");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "Institutes",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Institutes",
                table: "Institutes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "InstituteModules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstituteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstituteModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstituteModules_Institutes_InstituteId",
                        column: x => x.InstituteId,
                        principalTable: "Institutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstituteModules_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstituteModules_InstituteId",
                table: "InstituteModules",
                column: "InstituteId");

            migrationBuilder.CreateIndex(
                name: "IX_InstituteModules_ModuleId",
                table: "InstituteModules",
                column: "ModuleId");
        }
    }
}
