using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Copious.Document.Migrations {
    public partial class InitialCreate : Migration {
        protected override void Up (MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable (
                name: "Logs",
                columns : table => new {
                    Id = table.Column<Guid> (nullable: false),
                        AccessType = table.Column<int> (nullable: false),
                        AccessedOn = table.Column<DateTimeOffset> (nullable: false),
                        ActorId = table.Column<Guid> (nullable: false),
                        ActorKind = table.Column<int> (nullable: false),
                        DocumentId = table.Column<Guid> (nullable: false),
                        IpAddress = table.Column<string> (nullable: true),
                        Location = table.Column<string> (nullable: true),
                        Reason = table.Column<string> (nullable: true),
                        RequestedByActorId = table.Column<Guid> (nullable: false),
                        RequestedByActorKind = table.Column<int> (nullable: false),
                        SubSystemId = table.Column<Guid> (nullable: false),
                        SystemId = table.Column<Guid> (nullable: false),
                        Version = table.Column<int> (nullable: false)
                },
                constraints : table => {
                    table.PrimaryKey ("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable (
                name: "Drafts",
                columns : table => new {
                    Id = table.Column<Guid> (nullable: false),
                        ActorId = table.Column<Guid> (nullable: false),
                        ActorKind = table.Column<int> (nullable: false),
                        DocumentId = table.Column<Guid> (nullable: false),
                        Name = table.Column<string> (maxLength: 200, nullable: false),
                        Version = table.Column<int> (nullable: false),
                        File = table.Column<string> (nullable: true)
                },
                constraints : table => {
                    table.PrimaryKey ("PK_Drafts", x => x.Id);
                });

            migrationBuilder.CreateTable (
                name: "Index",
                columns : table => new {
                    Id = table.Column<Guid> (nullable: false),
                        AccessedOn = table.Column<DateTimeOffset> (nullable: false),
                        ActorId = table.Column<Guid> (nullable: false),
                        ActorKind = table.Column<int> (nullable: false),
                        Alias = table.Column<string> (nullable: true),
                        Author = table.Column<string> (nullable: true),
                        Code = table.Column<string> (nullable: true),
                        ComponentId = table.Column<Guid> (nullable: false),
                        ComponentName = table.Column<string> (nullable: true),
                        DocumentKind = table.Column<int> (nullable: false),
                        Name = table.Column<string> (nullable: true),
                        RevisionCode = table.Column<string> (nullable: true),
                        SubSystemId = table.Column<Guid> (nullable: false),
                        SystemId = table.Column<Guid> (nullable: false),
                        Tags = table.Column<string> (nullable: true),
                        Version = table.Column<int> (nullable: false),
                        VersionNo = table.Column<int> (nullable: false),
                        VersionedOn = table.Column<DateTimeOffset> (nullable: false)
                },
                constraints : table => {
                    table.PrimaryKey ("PK_Index", x => x.Id);
                });

            migrationBuilder.CreateTable (
                name: "VersionedDocuments",
                columns : table => new {
                    Id = table.Column<Guid> (nullable: false),
                        DocumentId = table.Column<Guid> (nullable: false),
                        DocumentKind = table.Column<int> (nullable: false),
                        Version = table.Column<int> (nullable: false),
                        VersionNo = table.Column<int> (nullable: false),
                        Access = table.Column<string> (nullable: true),
                        DocumentDetail = table.Column<string> (nullable: true),
                        File = table.Column<string> (nullable: true),
                        Metadata = table.Column<string> (nullable: false),
                        RelatedDocumentIds = table.Column<string> (nullable: true),
                        Security = table.Column<string> (nullable: true)
                },
                constraints : table => {
                    table.PrimaryKey ("PK_VersionedDocuments", x => x.Id);
                });

            migrationBuilder.CreateIndex (
                name: "IX_VersionedDocuments_DocumentId_VersionNo",
                table: "VersionedDocuments",
                columns : new [] { "DocumentId", "VersionNo" });
        }

        protected override void Down (MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable (
                name: "Logs");

            migrationBuilder.DropTable (
                name: "Drafts");

            migrationBuilder.DropTable (
                name: "Index");

            migrationBuilder.DropTable (
                name: "VersionedDocuments");
        }
    }
}