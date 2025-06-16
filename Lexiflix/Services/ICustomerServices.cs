using Lexiflix.Models;
namespace Lexiflix;


public interface ICustomerServices
{   
    public void AddNewCustomer(Customer customer);
    public void UpdateCustomer(Customer customer);
    public List<Customer> GetAllCustomers();
    public Customer GetCustomerById(int id);
}