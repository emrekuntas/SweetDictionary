using Api.Core.Domain.Models;
using Application.Interfaces.Repositories;
using Persistance.Context;
using Persistance.Repositories;

namespace Persistance.Repositories;


public class EmailConfirmationRepository : GenericRepository<EmailConfirmation>, IEmailConfirmationRepository
{
    public EmailConfirmationRepository(SweetDictionaryContext dbContext) : base(dbContext)
    {
    }
}
