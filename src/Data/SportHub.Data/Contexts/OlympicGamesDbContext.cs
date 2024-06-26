namespace SportHub.Data.Contexts;

using Microsoft.EntityFrameworkCore;

using SportHub.Data.Common.Interfaces;
using SportHub.Data.Models.Entities.OlympicGames;

public class OlympicGamesDbContext : DbContext
{
    public OlympicGamesDbContext(DbContextOptions<OlympicGamesDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Athlete> Athletes { get; set; }

    public virtual DbSet<AthleteClub> AthleteClubs { get; set; }

    public virtual DbSet<AthleteType> AthleteTypes { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Club> Clubs { get; set; }

    public virtual DbSet<Discipline> Disciplines { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventGenderType> EventGenderTypes { get; set; }

    public virtual DbSet<EventVenue> EventsVenues { get; set; }

    public virtual DbSet<FinishType> FinishTypes { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Host> Hosts { get; set; }

    public virtual DbSet<Medal> Medals { get; set; }

    public virtual DbSet<NOC> NOCs { get; set; }

    public virtual DbSet<NOCPresident> NOCPresidents { get; set; }

    public virtual DbSet<OlympicGameType> OlympicGameTypes { get; set; }

    public virtual DbSet<Participation> Participations { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sport> Sports { get; set; }

    public virtual DbSet<Squad> Squads { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<Venue> Venues { get; set; }

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

        builder.Entity<AthleteClub>()
             .HasKey(ac => new { ac.AthleteId, ac.ClubId });

        builder.Entity<AthleteClub>()
            .HasOne(ac => ac.Athlete)
            .WithMany(a => a.AthletesClubs)
            .HasForeignKey(ac => ac.AthleteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<AthleteClub>()
            .HasOne(ac => ac.Club)
            .WithMany(c => c.AthletesClubs)
            .HasForeignKey(ac => ac.ClubId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<EventVenue>()
            .HasKey(ev => new { ev.EventId, ev.VenueId });

        builder.Entity<EventVenue>()
            .HasOne(ev => ev.Event)
            .WithMany(e => e.EventsVenues)
            .HasForeignKey(ev => ev.EventId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<EventVenue>()
            .HasOne(ev => ev.Venue)
            .WithMany(v => v.EventsVenues)
            .HasForeignKey(ev => ev.VenueId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Host>()
            .HasKey(h => new { h.CityId, h.GameId });

        builder.Entity<Host>()
            .HasOne(h => h.City)
            .WithMany(c => c.Hosts)
            .HasForeignKey(h => h.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Host>()
            .HasOne(h => h.Game)
            .WithMany(g => g.Hosts)
            .HasForeignKey(h => h.GameId)
            .OnDelete(DeleteBehavior.Restrict);

        //builder.Entity<ResultParticipation>()
        //    .HasKey(rp => new { rp.ResultId, rp.ParticipationId });

        //builder.Entity<ResultParticipation>()
        //    .HasOne(rp => rp.Result)
        //    .WithMany(r => r.ResultsParticipations)
        //    .HasForeignKey(rp => rp.ResultId)
        //    .OnDelete(DeleteBehavior.Restrict);

        //builder.Entity<ResultParticipation>()
        //    .HasOne(rp => rp.Participation)
        //    .WithMany(p => p.ResultsParticipations)
        //    .HasForeignKey(rp => rp.ParticipationId)
        //    .OnDelete(DeleteBehavior.Restrict);

        //builder.Entity<ResultTeam>()
        //    .HasKey(rt => new { rt.ResultId, rt.TeamId });

        //builder.Entity<ResultTeam>()
        //    .HasOne(rt => rt.Result)
        //    .WithMany(r => r.ResultsTeams)
        //    .HasForeignKey(rt => rt.ResultId)
        //    .OnDelete(DeleteBehavior.Restrict);

        //builder.Entity<ResultTeam>()
        //    .HasOne(rt => rt.Team)
        //    .WithMany(t => t.ResultsTeams)
        //    .HasForeignKey(rt => rt.TeamId)
        //    .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Role>()
            .HasKey(r => new { r.AthleteId, r.AthleteTypeId });

        builder.Entity<Role>()
            .HasOne(r => r.Athlete)
            .WithMany(a => a.Roles)
            .HasForeignKey(r => r.AthleteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Role>()
            .HasOne(r => r.AthleteType)
            .WithMany(at => at.Roles)
            .HasForeignKey(r => r.AthleteTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Squad>()
            .HasKey(s => new { s.ParticipationId, s.TeamId });

        builder.Entity<Squad>()
            .HasOne(s => s.Participation)
            .WithMany(p => p.Squads)
            .HasForeignKey(s => s.ParticipationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Squad>()
            .HasOne(s => s.Team)
            .WithMany(t => t.Squads)
            .HasForeignKey(s => s.TeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Event>()
            .HasOne(e => e.Game)
            .WithMany(g => g.Events)
            .HasForeignKey(e => e.GameId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}