using Lexiflix.Models.Db;
namespace Lexiflix;


public interface ICustomerServices
{   
    public void AddNewCustomer(Customer customer);
    public List<Customer> GetAllCustomers();
}