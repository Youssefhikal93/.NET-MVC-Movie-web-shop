using Lexiflix.Data.Db;
using Lexiflix.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Lexiflix.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly MovieDbContext _db;

        public OrderServices(MovieDbContext db) 
        {
            _db = db;
        }

        public List<Order> GetAllOrders()
        {
            var orderList = _db.Orders.Include(o => o.Customer).ToList();

            return orderList;
        }

        List<Order> IOrderServices.GetAlOrders()
        {
            return _db.Orders.ToList();


        }
   
    }
}