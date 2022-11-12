using Api.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistance.Context;

namespace Persistence.EntityConfigurations.EntryComment;

public class EntryCommentVoteEntityConfiguration : BaseEntityConfiguration<EntryCommentVote>
{
    public override void Configure(EntityTypeBuilder<EntryCommentVote> builder)
    {
        base.Configure(builder);

        builder.ToTable("entrycommentvote", SweetDictionaryContext.DEFAULT_SCHEMA);

        builder.HasOne(i => i.EntryComment)
            .WithMany(i => i.EntryCommentVotes)
            .HasForeignKey(i => i.EntryCommentId);

        builder.HasOne(i => i.CreatedBy)
        .WithMany(i => i.EntryCommentVotes)
        .HasForeignKey(i => i.CreatedById)
        .OnDelete(DeleteBehavior.Restrict);
    }
}
