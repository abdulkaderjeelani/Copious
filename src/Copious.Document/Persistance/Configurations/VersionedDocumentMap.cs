using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Copious.Document.Persistance.Configurations
{
    public class VersionedDocumentMap : EntityTypeConfiguration<Interface.State.VersionedDocument>
    {
        public override void Map(EntityTypeBuilder<Interface.State.VersionedDocument> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d._Metadata).IsRequired();
            builder.Property(d => d._Metadata).HasColumnName("Metadata");
            builder.Property(d => d._RelatedDocumentIds).HasColumnName("RelatedDocumentIds");
            builder.Property(d => d._Detail).HasColumnName("DocumentDetail");
            builder.Property(d => d._File).HasColumnName("File");            
            builder.Property(d => d._Access).HasColumnName("Access");
            builder.Property(d => d._Security).HasColumnName("Security");
            builder.HasIndex("DocumentId", "VersionNo");
        }
    }
}