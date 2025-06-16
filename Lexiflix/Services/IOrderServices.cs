using Lexiflix.Models.Db;

namespace Lexiflix.Services
{
    public interface IOrderServices
    {
        public List<Order>GetAlOrders();
    }
}
