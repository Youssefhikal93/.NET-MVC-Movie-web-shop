using Lexiflix.Data.Db;
using Lexiflix.Models;
using Lexiflix.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace Lexiflix.Services
{
    public class HomeService : IHomeService
    {
        private readonly MovieDbContext _db;
        

        public HomeService(MovieDbContext db )
        {
            _db = db;
           
        }

        public List<MovieWithOrderCount> GetMostPopularMovies()
        {
            return _db.OrderRows
                .Include(row => row.Movie)
                .GroupBy(row => row.Movie)
                .Select(group => new MovieWithOrderCount
                {
                    Movie = group.Key,
                    OrderCount = group.Sum(row => row.Quantity)
                })
                .OrderByDescending(x => x.OrderCount)
                .Take(5)
                .ToList();
        }

        public TopCustomerViewModel GetTopCustomer()
        {
            var topCustomer = _db.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderRows)
                .GroupBy(o => o.Customer)
                .Select(group => new TopCustomerViewModel
                {
                    Customer = group.Key,
                    TotalOrders = group.Count(),
                    TotalSpent = group.SelectMany(o => o.OrderRows).Sum(row => row.Price * row.Quantity)
                })
                .OrderByDescending(x => x.TotalSpent)
                .FirstOrDefault();

            return topCustomer;
        }

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

        
    }

}
