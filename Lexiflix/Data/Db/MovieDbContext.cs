using Lexiflix.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Lexiflix.Data.Db
{
    public class MovieDbContext:DbContext
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


    }
}

