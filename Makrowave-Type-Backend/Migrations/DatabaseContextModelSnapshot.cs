﻿// <auto-generated />
using System;
using Makrowave_Type_Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Makrowave_Type_Backend.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Makrowave_Type_Backend.Models.Entities.DailyRecord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("daily_record_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<float>("Accuracy")
                        .HasColumnType("real")
                        .HasColumnName("accuracy");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<int>("Score")
                        .HasColumnType("integer")
                        .HasColumnName("score");

                    b.Property<int>("Time")
                        .HasColumnType("integer")
                        .HasColumnName("time");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_daily_record_id");

                    b.HasIndex("UserId");

                    b.ToTable("daily_record", (string)null);
                });

            modelBuilder.Entity("Makrowave_Type_Backend.Models.Entities.GradientColor", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer")
                        .HasColumnName("gradient_color_id");

                    b.Property<Guid>("UserThemeId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_theme_id");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("character varying(7)")
                        .HasColumnName("color");

                    b.HasKey("Id", "UserThemeId")
                        .HasName("pk_gradient_color_id");

                    b.HasIndex("UserThemeId");

                    b.ToTable("gradient_color", (string)null);
                });

            modelBuilder.Entity("Makrowave_Type_Backend.Models.Entities.Session", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<Guid>("SessionId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId")
                        .HasName("pk_session_id");

                    b.ToTable("session", (string)null);
                });

            modelBuilder.Entity("Makrowave_Type_Backend.Models.Entities.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("user_id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasColumnName("username");

                    b.HasKey("UserId")
                        .HasName("pk_user_id");

                    b.ToTable("user", (string)null);
                });

            modelBuilder.Entity("Makrowave_Type_Backend.Models.Entities.UserTheme", b =>
                {
                    b.Property<Guid>("UserThemeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("user_theme_id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("ActiveText")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("character varying(7)")
                        .HasColumnName("active_text");

                    b.Property<string>("InactiveKey")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("character varying(7)")
                        .HasColumnName("inactive_key");

                    b.Property<string>("InactiveText")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("character varying(7)")
                        .HasColumnName("inactive_text");

                    b.Property<string>("TextComplete")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("character varying(7)")
                        .HasColumnName("text_complete");

                    b.Property<string>("TextIncomplete")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("character varying(7)")
                        .HasColumnName("text_incomplete");

                    b.Property<string>("TextIncorrect")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("character varying(7)")
                        .HasColumnName("text_incorrect");

                    b.Property<string>("UiBackground")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("character varying(7)")
                        .HasColumnName("ui_background");

                    b.Property<string>("UiText")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("character varying(7)")
                        .HasColumnName("ui_text");

                    b.HasKey("UserThemeId")
                        .HasName("pk_user_theme_id");

                    b.ToTable("user_theme", (string)null);
                });

            modelBuilder.Entity("Makrowave_Type_Backend.Models.Entities.DailyRecord", b =>
                {
                    b.HasOne("Makrowave_Type_Backend.Models.Entities.User", "User")
                        .WithMany("DailyRecords")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Makrowave_Type_Backend.Models.Entities.GradientColor", b =>
                {
                    b.HasOne("Makrowave_Type_Backend.Models.Entities.UserTheme", "Theme")
                        .WithMany("GradientColors")
                        .HasForeignKey("UserThemeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Theme");
                });

            modelBuilder.Entity("Makrowave_Type_Backend.Models.Entities.Session", b =>
                {
                    b.HasOne("Makrowave_Type_Backend.Models.Entities.User", "User")
                        .WithMany("Sessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Makrowave_Type_Backend.Models.Entities.UserTheme", b =>
                {
                    b.HasOne("Makrowave_Type_Backend.Models.Entities.User", "User")
                        .WithOne("Theme")
                        .HasForeignKey("Makrowave_Type_Backend.Models.Entities.UserTheme", "UserThemeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Makrowave_Type_Backend.Models.Entities.User", b =>
                {
                    b.Navigation("DailyRecords");

                    b.Navigation("Sessions");

                    b.Navigation("Theme");
                });

            modelBuilder.Entity("Makrowave_Type_Backend.Models.Entities.UserTheme", b =>
                {
                    b.Navigation("GradientColors");
                });
#pragma warning restore 612, 618
        }
    }
}
