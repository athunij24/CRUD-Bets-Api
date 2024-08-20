using Microsoft.EntityFrameworkCore.SqlServer;
using BetsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BetsApi.Data 
{
    public class BetsDbContext : DbContext
    {
        public BetsDbContext(DbContextOptions<BetsDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users {get; set;} = null;
        public DbSet<Bet> Bets {get; set;} = null;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Bet>()
            .HasOne(b => b.Sender)
            .WithMany(u => u.BetsSent)
            .HasForeignKey(b => b.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bet>()
            .HasOne(b => b.Receiver)
            .WithMany(u => u.BetsReceived)
            .HasForeignKey(b => b.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

        }
    }
}