using Lexiflix.Models.Db;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Lexiflix.Data.Db
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext()
        {

        }

        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<OrderRow> OrderRows { get; set; }
        public virtual DbSet<Actor> Actors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        
            protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   //set up one to many order > orderRow/items and cascades on delete of order
            modelBuilder.Entity<OrderRow>()
                .HasOne(or => or.Order)
                .WithMany(o => o.OrderRows)
                .HasForeignKey(or => or.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            //deletes the order row /detail on removal of movie
            modelBuilder.Entity<OrderRow>()
            .HasOne(or => or.Movie)
            .WithMany()
            .HasForeignKey(or => or.MovieId)
            .OnDelete(DeleteBehavior.Cascade);


           //deletes the order row /detail on removal of customer
            modelBuilder.Entity<Order>()
            .HasOne(or => or.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(or => or.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        } 


    }
}

