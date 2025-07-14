using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using Domain.Entities;

namespace Infrastructure;

public partial class GestionGastosContext : DbContext
{
    public GestionGastosContext()
    {
    }

    public GestionGastosContext(DbContextOptions<GestionGastosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }


    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<Invitation> Invitations { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=gestion_gastos;uid=root;pwd=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.42-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategory).HasName("PRIMARY");

            entity.ToTable("categories");

            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(45)
                .HasColumnName("category_name");
        });


        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.IdExpense).HasName("PRIMARY");

            entity.ToTable("expenses");

            entity.HasIndex(e => e.CategoryId, "category_id_idx");

            entity.HasIndex(e => e.TeamId, "group_Id_idx");

            entity.HasIndex(e => e.UserId, "user_id_idx");

            entity.Property(e => e.IdExpense).HasColumnName("id_expense");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasColumnName("amount");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Description)
                .HasMaxLength(155)
                .HasDefaultValueSql("'\"No description added.\"'")
                .HasColumnName("description");
            entity.Property(e => e.ExpenseDate).HasColumnName("expense_date");
            entity.Property(e => e.ExpenseName)
                .HasMaxLength(45)
                .HasColumnName("expense_name");
            entity.Property(e => e.TeamId).HasColumnName("team_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Category).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("category_id");

            entity.HasOne(d => d.Team).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("group_Id");

            entity.HasOne(d => d.User).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_id");
        });

        modelBuilder.Entity<Invitation>(entity =>
        {
            entity.HasKey(e => e.IdInvitation).HasName("PRIMARY");

            entity.ToTable("invitations");

            entity.HasIndex(e => e.TeamId, "fk_invitations_groups");

            entity.HasIndex(e => e.InvitedUserId, "id_invited_idx");

            entity.HasIndex(e => e.OwnerUserId, "owner_user_id_idx");

            entity.Property(e => e.IdInvitation).HasColumnName("id_invitation");
            entity.Property(e => e.InvitedUserId).HasColumnName("invited_user_id");
            entity.Property(e => e.OwnerUserId).HasColumnName("owner_user_id");
            entity.Property(e => e.State).HasColumnName("state");
            entity.Property(e => e.TeamId).HasColumnName("team_id");

            entity.HasOne(d => d.InvitedUser).WithMany(p => p.InvitationInvitedUsers)
                .HasForeignKey(d => d.InvitedUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("invited_user_id");

            entity.HasOne(d => d.OwnerUser).WithMany(p => p.InvitationOwnerUsers)
                .HasForeignKey(d => d.OwnerUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("owner_user_id");

            entity.HasOne(d => d.Team).WithMany(p => p.Invitations)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_invitations_groups");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.IdTeam).HasName("PRIMARY");

            entity.ToTable("teams");

            entity.HasIndex(e => e.OwnerId, "owner_id_idx");

            entity.Property(e => e.IdTeam).HasColumnName("id_team");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.TeamName)
                .HasMaxLength(45)
                .HasColumnName("team_name");

            entity.HasOne(d => d.Owner).WithMany(p => p.Teams)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("owner_id");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "email_UNIQUE").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "phone_number_UNIQUE").IsUnique();

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(45)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");

            entity.HasMany(d => d.TeamsNavigation).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UsersTeam",
                    r => r.HasOne<Team>().WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("groupId"),
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
                        j.HasIndex(new[] { "TeamId" }, "groupId_idx");
                        j.HasIndex(new[] { "UserId" }, "userId_idx");
                        j.IndexerProperty<int>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<int>("TeamId").HasColumnName("team_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
