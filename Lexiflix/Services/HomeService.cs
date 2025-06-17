using Lexiflix.Data.Db;
using Lexiflix.Models;
using Lexiflix.Models.Db;
using System;

namespace Lexiflix.Services
{
    public class HomeService : IHomeService
    {
        // Inject your DbContext
        private readonly MovieDbContext _db;
        

        public HomeService(MovieDbContext db )
        {
            _db = db;
           
        }

        //public  List<Movie> GetMostPopularMovies()
        //{
        //    return  _context.Movies
        //        .OrderByDescending(m => m.Orders.Count)
        //        .Take(5)
        //        .Select(m => new Movie
        //        {
        //            Title = m.Title,
        //            ReleaseYear = m.ReleaseDate.Year,
        //            DurationInMinutes = m.Duration,
        //            PosterUrl = m.PosterUrl,
        //            Price = m.Price,
        //            OrdersCount = m.Orders.Count,
        //            Label = ""
        //        })
        //        .ToList();
        //}

        public  List<Movie> GetNewestReleases()
        {
            return _db.Movies
        .OrderByDescending(m => m.ReleaseYear)
        .Take(5)
        .ToList();
        }

        public  List<Movie> GetClassicFilms()
        {
            return  _db.Movies
                .Where(m => m.ReleaseYear < 1990)
                .OrderBy(m => m.ReleaseYear)
                .Take(5)
                
                .ToList();
        }

        public  List<Movie> GetBestDeals()
        {
            return  _db.Movies
                .OrderBy(m => m.Price)
                .Take(5)
                
                .ToList();
        }

        //public  TopCustomerViewModel GetTopCustomer()
        //{
            //var customer =  _context.Customers
            //    .OrderByDescending(u => u.Orders.Sum(o => o.TotalPrice))
            //    .Select(u => new TopCustomerViewModel
            //    {
            //        FullName = u.FullName,
            //        TotalOrders = u.Orders.Count,
            //        TotalSpent = u.Orders.Sum(o => o.TotalPrice),
            //        BiggestOrder = u.Orders.Max(o => o.TotalPrice),
            //        MemberSince = u.CreatedAt.Year
            //    })
            //    .FirstOrDefault();

            //return customer!;
        //}
    }

}
