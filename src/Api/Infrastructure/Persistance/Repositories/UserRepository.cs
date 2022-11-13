using Api.Core.Domain.Models;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(SweetDictionaryContext dbContext) : base(dbContext)
        {
        }
    }
}
