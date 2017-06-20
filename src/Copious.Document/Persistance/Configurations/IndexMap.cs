using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Copious.Document.Persistance.Configurations
{
    public class IndexMap : EntityTypeConfiguration<Interface.State.Index>
    {
        public override void Map(EntityTypeBuilder<Interface.State.Index> builder)
        {
            builder.HasKey(i => i.Id);            
        }
    }
}