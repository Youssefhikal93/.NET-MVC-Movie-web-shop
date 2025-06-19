using Lexiflix.Models.Db;

namespace Lexiflix.Services
{
    public interface IOrderServices
    {
        void CreateOrder(Order order);
        //string? GetAllOrders();
        public List<Order> GetAllOrders();
        Movie GetMovieById(int v);

        Order GetOrderWithDetails(int id);

        void DeleteOrder(int id);

    }
}