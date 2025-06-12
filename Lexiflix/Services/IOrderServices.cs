
using Lexiflix.Models;

namespace Lexiflix.Services
{
    public interface IOrderServices
    {
        public void CreateOrder(Order order)
        {
            throw new NotImplementedException();
        }
        public List<Order> GetAlOrders();
    }
}
