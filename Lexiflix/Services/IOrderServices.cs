
using Lexiflix.Models;

namespace Lexiflix.Services
{
    public interface IOrderServices
    {
        public void CreateOrder(Order order)
        {

        }
        public List<Order> GetAllOrders();
        Movie GetMovieById(int v);

    }
}

