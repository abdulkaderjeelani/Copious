using System;
using Copious.Document.Interface.State;
using Copious.Document.Persistance;
using Copious.Foundation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Copious.Document.Migrations {
    [DbContext (typeof (DocumentContext))]
    [Migration ("20170516113752_Initial-Create")]
    partial class InitialCreate {
        protected override void BuildTargetModel (ModelBuilder modelBuilder) {
            modelBuilder
                .HasAnnotation ("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation ("ProductVersion", "1.1.1");

            modelBuilder.Entity ("Copious.Document.Interface.State.DocumentAccess", b => {
                b.Property<Guid> ("Id")
                    .ValueGeneratedOnAdd ();

                b.Property<int> ("AccessType");

                b.Property<DateTimeOffset> ("AccessedOn");

                b.Property<Guid> ("ActorId");

                b.Property<int> ("ActorKind");

                b.Property<Guid> ("DocumentId");

                b.Property<string> ("IpAddress");

                b.Property<string> ("Location");

                b.Property<string> ("Reason");

                b.Property<Guid> ("RequestedByActorId");

                b.Property<int> ("RequestedByActorKind");

                b.Property<Guid> ("SubSystemId");

                b.Property<Guid> ("SystemId");

                b.Property<int> ("Version");

                b.HasKey ("Id");

                b.ToTable ("Logs");
            });

            modelBuilder.Entity ("Copious.Document.Interface.State.Draft", b => {
                b.Property<Guid> ("Id")
                    .ValueGeneratedOnAdd ();

                b.Property<Guid> ("ActorId");

                b.Property<int> ("ActorKind");

                b.Property<Guid> ("DocumentId");

                b.Property<string> ("Name")
                    .IsRequired ()
                    .HasMaxLength (200);

                b.Property<int> ("Version");

                b.Property<string> ("_File")
                    .HasColumnName ("File");

                b.HasKey ("Id");

                b.ToTable ("Drafts");
            });

            modelBuilder.Entity ("Copious.Document.Interface.State.Index", b => {
                b.Property<Guid> ("Id")
                    .ValueGeneratedOnAdd ();

                b.Property<DateTimeOffset> ("AccessedOn");

                b.Property<Guid> ("ActorId");

                b.Property<int> ("ActorKind");

                b.Property<string> ("Alias");

                b.Property<string> ("Author");

                b.Property<string> ("Code");

                b.Property<Guid> ("ComponentId");

                b.Property<string> ("ComponentName");

                b.Property<int> ("DocumentKind");

                b.Property<string> ("Name");

                b.Property<string> ("RevisionCode");

                b.Property<Guid> ("SubSystemId");

                b.Property<Guid> ("SystemId");

                b.Property<string> ("Tags");

                b.Property<int> ("Version");

                b.Property<int> ("VersionNo");

                b.Property<DateTimeOffset> ("VersionedOn");

                b.HasKey ("Id");

                b.ToTable ("Index");
            });

            modelBuilder.Entity ("Copious.Document.Interface.State.VersionedDocument", b => {
                b.Property<Guid> ("Id")
                    .ValueGeneratedOnAdd ();

                b.Property<Guid> ("DocumentId");

                b.Property<int> ("DocumentKind");

                b.Property<int> ("Version");

                b.Property<int> ("VersionNo");

                b.Property<string> ("_Access")
                    .HasColumnName ("Access");

                b.Property<string> ("_Detail")
                    .HasColumnName ("DocumentDetail");

                b.Property<string> ("_File")
                    .HasColumnName ("File");

                b.Property<string> ("_Metadata")
                    .IsRequired ()
                    .HasColumnName ("Metadata");

                b.Property<string> ("_RelatedDocumentIds")
                    .HasColumnName ("RelatedDocumentIds");

                b.Property<string> ("_Security")
                    .HasColumnName ("Security");

                b.HasKey ("Id");

                b.HasIndex ("DocumentId", "VersionNo");

                b.ToTable ("VersionedDocuments");
            });
        }
    }
}