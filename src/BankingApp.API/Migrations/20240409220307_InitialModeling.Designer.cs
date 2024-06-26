﻿// <auto-generated />
using System;
using BankingApp.API.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BankingApp.API.Migrations
{
    [DbContext(typeof(AccountsDbContext))]
    [Migration("20240409220307_InitialModeling")]
    partial class InitialModeling
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("BankingApp.Domain.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("Balance")
                        .HasPrecision(19, 4)
                        .HasColumnType("decimal(19,4)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PixKey")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Accounts", (string)null);
                });

            modelBuilder.Entity("BankingApp.Domain.Transaction", b =>
                {
                    b.Property<Guid>("Identifier")
                        .HasColumnType("char(36)");

                    b.Property<int?>("AccountId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Occurence")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<decimal>("_amount")
                        .HasPrecision(19, 4)
                        .HasColumnType("decimal(19,4)")
                        .HasColumnName("Amount");

                    b.HasKey("Identifier");

                    b.HasIndex("AccountId");

                    b.ToTable("Transactions", (string)null);
                });

            modelBuilder.Entity("BankingApp.Domain.Transaction", b =>
                {
                    b.HasOne("BankingApp.Domain.Account", null)
                        .WithMany("_transactions")
                        .HasForeignKey("AccountId");
                });

            modelBuilder.Entity("BankingApp.Domain.Account", b =>
                {
                    b.Navigation("_transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
