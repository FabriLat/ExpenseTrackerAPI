using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public partial class GestionGastosContext : DbContext
{
    public GestionGastosContext(DbContextOptions<GestionGastosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

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

            entity.HasOne(d => d.Group).WithMany(p => p.Expenses).HasConstraintName("group_Id");

            entity.HasOne(d => d.User).WithMany(p => p.Expenses).HasConstraintName("user_id");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.IdGroup).HasName("PRIMARY");

            entity.HasOne(d => d.Owner).WithMany(p => p.Groups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("owner_id");
        });

        modelBuilder.Entity<Invitation>(entity =>
        {
            entity.HasKey(e => e.IdInvitation).HasName("PRIMARY");

            entity.HasOne(d => d.Group).WithMany(p => p.Invitations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_invitations_groups");

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

            entity.HasMany(d => d.GroupsNavigation).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UsersGroup",
                    r => r.HasOne<Group>().WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("groupId"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("userId"),
                    j =>
                    {
                        j.HasKey("UserId", "GroupId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("users_groups");
                        j.HasIndex(new[] { "GroupId" }, "groupId_idx");
                        j.HasIndex(new[] { "UserId" }, "userId_idx");
                        j.IndexerProperty<int>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<int>("GroupId").HasColumnName("group_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
