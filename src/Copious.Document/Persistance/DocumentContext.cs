using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Copious.Document.Interface.State;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Copious.Document.Persistance.Configurations;

namespace Copious.Document.Persistance
{
    // Add-Migration -Name Initial-Create -Context DocumentContext -Project Copious.Document -StartupProject Copious.Tests.Web
    public class DocumentContext : DbContext
    {
        public DocumentContext(DbContextOptions<DocumentContext> options) : base(options)
        {
        }


        public virtual DbSet<Index> Index { get; set; }
        public virtual DbSet<VersionedDocument> VersionedDocuments { get; set; }
        public virtual DbSet<Draft> Drafts { get; set; }
        public virtual DbSet<DocumentAccess> Logs { get; set; }

        //https://docs.microsoft.com/en-us/ef/core/modeling/relational/fk-constraints
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ForNpgsqlUseSerialColumns();

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;

            modelBuilder.AddConfiguration(new IndexMap());
            modelBuilder.AddConfiguration(new VersionedDocumentMap());
            modelBuilder.AddConfiguration(new DraftMap());
            modelBuilder.AddConfiguration(new LogMap());
            
            base.OnModelCreating(modelBuilder);
        }
    }
}