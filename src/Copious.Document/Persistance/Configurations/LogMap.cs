using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Copious.Document.Persistance.Configurations {
    public class LogMap : EntityTypeConfiguration<Interface.State.DocumentAccess> {
        public override void Map (EntityTypeBuilder<Interface.State.DocumentAccess> builder) {
            builder.HasKey (d => d.Id);
        }
    }
}