using Api.Core.Domain.Models;
using Bogus.DataSets;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Context;
public class SweetDictionaryContext : DbContext
{
    public const string DEFAULT_SCHEMA = "dbo";

    public SweetDictionaryContext()
    {

    }

    public SweetDictionaryContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Entry> Entries { get; set; }

    public DbSet<EntryVote> EntryVotes { get; set; }
    public DbSet<EntryFavorite> EntryFavorites { get; set; }

    public DbSet<EntryComment> EntryComments { get; set; }
    public DbSet<EntryCommentVote> EntryCommentVotes { get; set; }
    public DbSet<EntryCommentFavorite> EntryCommentFavorites { get; set; }

    public DbSet<EmailConfirmation> EmailConfirmations { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //migration olduğunda çalışacak
        if (!optionsBuilder.IsConfigured)
        {
            var connStr = "Data Source = (localdb)\\MSSQLLocalDB;Initial Catalog=SweetDictionary;Integrated Security=True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
            optionsBuilder.UseSqlServer(connStr, opt =>
            {
                opt.EnableRetryOnFailure();
            });
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override int SaveChanges()
    {
        OnBeforeSave();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        OnBeforeSave();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        OnBeforeSave();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSave();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void OnBeforeSave()
    {
        IEnumerable<BaseEntity> addedEntites = ChangeTracker.Entries()
                                .Where(i => i.State == EntityState.Added)
                                .Select(i => (BaseEntity)i.Entity);

        PrepareAddedEntities(addedEntites);
    }

    private void PrepareAddedEntities(IEnumerable<BaseEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (entity.CreateDate == DateTime.MinValue)
                entity.CreateDate = DateTime.Now;
        }
    }
}
