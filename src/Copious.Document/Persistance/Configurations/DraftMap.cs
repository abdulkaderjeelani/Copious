using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Copious.Document.Persistance.Configurations {
    public class DraftMap : EntityTypeConfiguration<Interface.State.Draft> {
        public override void Map (EntityTypeBuilder<Interface.State.Draft> builder) {
            builder.HasKey (d => d.Id);
            builder.Property (d => d.Name).IsRequired ().HasMaxLength (200);
            builder.Property (d => d._File).HasColumnName ("File");
        }
    }
}