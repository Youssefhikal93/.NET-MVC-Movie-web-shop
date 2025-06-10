using Lexiflix.Models;
namespace Lexiflix;


public interface ICustomerServices
{   
    public void AddNewCustomer(Customer customer);
    public List<Customer> GetAllCustomers();
}