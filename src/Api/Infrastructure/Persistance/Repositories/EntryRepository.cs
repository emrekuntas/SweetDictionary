using Api.Core.Domain.Models;
using Application.Interfaces.Repositories;
using Persistance.Context;
using Persistance.Repositories;

namespace Persistance.Repositories;


public class EntryRepository : GenericRepository<Entry>, IEntryRepository
{
    public EntryRepository(SweetDictionaryContext dbContext) : base(dbContext)
    {
    }
}
