
using Api.Core.Domain.Models;
using Application.Interfaces.Repositories;
using Persistance.Context;
using Persistance.Repositories;

namespace Persistance.Repositories;


public class EntryCommentRepository : GenericRepository<EntryComment>, IEntryCommentRepository
{
    public EntryCommentRepository(SweetDictionaryContext dbContext) : base(dbContext)
    {
    }
}
