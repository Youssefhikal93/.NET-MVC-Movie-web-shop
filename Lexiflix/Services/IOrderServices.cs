using Lexiflix.Models.Db;
using Lexiflix.Models.ViewModels;

namespace Lexiflix.Services
{
    public interface IOrderServices
    {
        void CreateOrder(Order order);
        //string? GetAllOrders();
        public List<Order> GetAllOrders();
        Movie GetMovieById(int v);

        Order GetOrderWithDetails(int id);
        void AddOrderByAdmin(OrderVM ovm);




    }
}