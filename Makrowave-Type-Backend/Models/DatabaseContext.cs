using Makrowave_Type_Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Makrowave_Type_Backend.Models;

public class DatabaseContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserTheme> UserThemes { get; set; }
    public DbSet<GradientColor> GradientColors { get; set; }
    public DbSet<DailyRecord> DailyRecords { get; set; }
    public DbSet<Session> Sessions { get; set; }

    public DatabaseContext() {}
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("pk_user_id");
            entity.ToTable("user");
            entity.Property(e => e.UserId).IsRequired().HasColumnName("user_id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Username).IsRequired().HasColumnName("username").HasMaxLength(32);
            
            entity.HasOne(e => e.Theme).WithOne(e => e.User).HasForeignKey<UserTheme>(e => e.UserThemeId);
        });

        modelBuilder.Entity<UserTheme>(entity =>
        {
            entity.HasKey(e => e.UserThemeId).HasName("pk_user_theme_id");
            entity.ToTable("user_theme");
            entity.Property(e => e.UserThemeId).IsRequired().HasColumnName("user_theme_id")
                .HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.UiText).IsRequired().HasColumnName("ui_text").HasMaxLength(7);
            entity.Property(e => e.UiBackground).IsRequired().HasColumnName("ui_background").HasMaxLength(7);
            entity.Property(e => e.TextIncomplete).IsRequired().HasColumnName("text_incomplete").HasMaxLength(7);
            entity.Property(e => e.TextComplete).IsRequired().HasColumnName("text_complete").HasMaxLength(7);
            entity.Property(e => e.TextIncorrect).IsRequired().HasColumnName("text_incorrect").HasMaxLength(7);
            entity.Property(e => e.InactiveKey).IsRequired().HasColumnName("inactive_key").HasMaxLength(7);
            entity.Property(e => e.InactiveText).IsRequired().HasColumnName("inactive_text").HasMaxLength(7);
            entity.Property(e => e.ActiveText).IsRequired().HasColumnName("active_text").HasMaxLength(7);
        });

        modelBuilder.Entity<GradientColor>(entity =>
        {
            entity.HasKey(e =>  new {e.Id, e.UserThemeId}).HasName("pk_gradient_color_id");
            entity.ToTable("gradient_color");
            entity.Property(e => e.Id).IsRequired().HasColumnName("gradient_color_id");
            entity.Property(e => e.UserThemeId).IsRequired().HasColumnName("user_theme_id");
            entity.Property(e => e.Color).IsRequired().HasColumnName("color").HasMaxLength(7);

            entity.HasOne(e => e.Theme).WithMany(e => e.GradientColors).HasForeignKey(e => e.UserThemeId);

        });
        
        modelBuilder.Entity<DailyRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_daily_record_id");
            entity.ToTable("daily_record");
            entity.Property(e => e.Id).IsRequired().HasColumnName("daily_record_id");
            entity.Property(e => e.UserId).IsRequired().HasColumnName("user_id");
            entity.Property(e => e.Date).HasColumnType("date").HasColumnName("date");
            entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.Accuracy).HasColumnName("accuracy");
            entity.Property(e => e.Score).HasColumnName("score");
            
            entity.HasOne(e => e.User).WithMany(e => e.DailyRecords).HasForeignKey(e => e.UserId);
        });
        
        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("pk_session_id");
            entity.ToTable("session");
            entity.Property(e => e.UserId).IsRequired().HasColumnName("user_id");
            
            entity.HasOne(e => e.User).WithMany(e => e.Sessions).HasForeignKey(e => e.UserId);
        });
    }
}