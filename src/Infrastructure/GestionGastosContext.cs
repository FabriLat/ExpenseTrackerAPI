using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public partial class GestionGastosContext : DbContext
{
    public GestionGastosContext(DbContextOptions<GestionGastosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<Invitation> Invitations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.IdExpense).HasName("PRIMARY");

            entity.Property(e => e.Description).HasDefaultValueSql("'\"No description added.\"'");

            entity.HasOne(d => d.Team).WithMany(p => p.Expenses).HasConstraintName("team_Id");

            entity.HasOne(d => d.User).WithMany(p => p.Expenses).HasConstraintName("user_id");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.IdTeam).HasName("PRIMARY");

            entity.HasOne(d => d.Owner).WithMany(p => p.Teams)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("owner_id");
        });

        modelBuilder.Entity<Invitation>(entity =>
        {
            entity.HasKey(e => e.IdInvitation).HasName("PRIMARY");

            entity.Property(e => e.State).HasColumnName("state").HasDefaultValue(InvitationState.Pending);

            entity.HasOne(d => d.Team).WithMany(p => p.Invitations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_invitations_teams");

            entity.HasOne(d => d.InvitedUser).WithMany(p => p.InvitationInvitedUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("invited_user_id");

            entity.HasOne(d => d.OwnerUser).WithMany(p => p.InvitationOwnerUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("owner_user_id");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PRIMARY");

            entity.HasMany(d => d.TeamsNavigation).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UsersTeam",
                    r => r.HasOne<Team>().WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("TeamId"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("userId"),
                    j =>
                    {
                        j.HasKey("UserId", "TeamId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("users_teams");
                        j.HasIndex(new[] { "TeamId" }, "teamId_idx");
                        j.HasIndex(new[] { "UserId" }, "userId_idx");
                        j.IndexerProperty<int>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<int>("TeamId").HasColumnName("team_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
