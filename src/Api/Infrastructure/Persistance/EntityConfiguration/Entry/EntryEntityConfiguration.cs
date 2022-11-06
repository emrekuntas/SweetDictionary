using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistance.Context;
using Persistence.EntityConfigurations;

namespace Persistence.EntityConfigurations.Entry;

public class EntryEntityConfiguration : BaseEntityConfiguration<Api.Core.Domain.Models.Entry>
{
    public override void Configure(EntityTypeBuilder<Api.Core.Domain.Models.Entry> builder)
    {
        base.Configure(builder);

        builder.ToTable("entry", SweetDictionaryContext.DEFAULT_SCHEMA);


        builder.HasOne(i => i.CreatedBy)
            .WithMany(i => i.Entries)
            .HasForeignKey(i => i.CreatedById);
    }
}
