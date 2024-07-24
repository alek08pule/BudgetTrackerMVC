using BudgetTrackerMVC.Domains;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTrackerMVC.DataAccess
{
    public class BudgetTrackerDbContext : IdentityDbContext<User>
    {
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<UserBalance> UserBalances { get; set; }

        public BudgetTrackerDbContext(DbContextOptions<BudgetTrackerDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Transaction>().ToTable("Transactions");
            modelBuilder.Entity<UserBalance>().ToTable("UserBalances");

            modelBuilder.Entity<User>()
               .Property(u => u.Id)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserBalances)
                .WithOne(b => b.User)
                .HasForeignKey<UserBalance>(b => b.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Transactions)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<Transaction>()
               .HasKey(t => t.Id);

            modelBuilder.Entity<UserBalance>()
               .Property(b => b.TotalExpense)
               .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<UserBalance>()
                .Property(b => b.TotalIncome)
                .HasColumnType("decimal(18,2)");
        }
    }
}
