namespace SportHub.Data.Contexts;

using Microsoft.EntityFrameworkCore;

using SportHub.Data.Common.Interfaces;
using SportHub.Data.Models.DbEntities.OlympicGames;
using SportHub.Data.Models.DbEntities.OlympicGames.Enumerations;

public class OlympicGamesDbContext : DbContext
{
    public OlympicGamesDbContext(DbContextOptions<OlympicGamesDbContext> options)
        : base(options)
    {
    }

    //public virtual DbSet<FlagBearer> FlagBearers { get; set; }

    public virtual DbSet<Discipline> Disciplines { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<NOC> NOCs { get; set; }

    public virtual DbSet<Phase> Phases { get; set; }


    //public virtual DbSet<NOCAdministration> NOCAdministrations { get; set; }

    //public virtual DbSet<ProtocolOrder> ProtocolOrders { get; set; }

    // Enumerations
    public virtual DbSet<Administration> Administrations { get; set; }

    public virtual DbSet<Ceremony> Ceremonies { get; set; }

    public virtual DbSet<Decision> Decisions { get; set; }

    public virtual DbSet<EventGender> EventGenders { get; set; }

    public virtual DbSet<FinishStatus> FinishStatuses { get; set; }

    public virtual DbSet<GameType> GameTypes { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Medal> Medals { get; set; }

    public virtual DbSet<Record> Records { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Round> Rounds { get; set; }

    public override int SaveChanges()
    {
        return this.SaveChanges(true);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        this.ApplyCheckRules();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return this.SaveChangesAsync(true, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        this.ApplyCheckRules();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void ApplyCheckRules()
    {
        var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e => e.Entity is ICreatableEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in changedEntries)
        {
            var entity = (ICreatableEntity)entry.Entity;
            if (entry.State == EntityState.Added && entity.CreatedOn == default)
            {
                entity.CreatedOn = DateTime.UtcNow;
            }
            else
            {
                entity.ModifiedOn = DateTime.UtcNow;
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //builder.Entity<Game>()
        //    .HasMany(x => x.Cities)
        //    .WithMany(x => x.Games)
        //    .UsingEntity("Hosts",
        //        x => x.HasOne(typeof(City))
        //            .WithMany()
        //            .HasForeignKey("CitiesId")
        //            .HasPrincipalKey(nameof(City.Id))
        //            .OnDelete(DeleteBehavior.Restrict),
        //        x => x.HasOne(typeof(Game))
        //            .WithMany()
        //            .HasForeignKey("GamesId")
        //            .HasPrincipalKey(nameof(Game.Id))
        //            .OnDelete(DeleteBehavior.Restrict),
        //        x => x.HasKey("CitiesId", "GamesId"));

        //builder.Entity<Game>()
        //    .HasMany(x => x.NOCs)
        //    .WithMany(x => x.Games)
        //    .UsingEntity("GamesNOCs",
        //        x => x.HasOne(typeof(NOC))
        //            .WithMany()
        //            .HasForeignKey("NOCsId")
        //            .HasPrincipalKey(nameof(NOC.Id))
        //            .OnDelete(DeleteBehavior.Restrict),
        //        x => x.HasOne(typeof(Game))
        //            .WithMany()
        //            .HasForeignKey("GamesId")
        //            .HasPrincipalKey(nameof(Game.Id))
        //            .OnDelete(DeleteBehavior.Restrict),
        //        x => x.HasKey("NOCsId", "GamesId"));

        //builder.Entity<Participation>()
        //   .HasMany(x => x.Teams)
        //   .WithMany(x => x.Participations)
        //   .UsingEntity("Squads",
        //       x => x.HasOne(typeof(Team))
        //           .WithMany()
        //           .HasForeignKey("TeamsId")
        //           .HasPrincipalKey(nameof(Team.Id))
        //           .OnDelete(DeleteBehavior.Restrict),
        //       x => x.HasOne(typeof(Participation))
        //           .WithMany()
        //           .HasForeignKey("ParticipationsId")
        //           .HasPrincipalKey(nameof(Participation.Id))
        //           .OnDelete(DeleteBehavior.Restrict),
        //       x => x.HasKey("TeamsId", "ParticipationsId"));
    }
}