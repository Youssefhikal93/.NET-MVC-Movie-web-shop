
using Lexiflix.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
