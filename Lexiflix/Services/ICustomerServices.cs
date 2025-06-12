using Lexiflix.Models;
namespace Lexiflix;


public interface ICustomerServices
{   
    public void AddNewCustomer(Customer customer);
    public List<Customer> GetAllCustomers();
    public Customer GetCustomerByEmail( String email)
    {
        throw new NotImplementedException();
    }
    public void RegisterCustomer(Customer customer) {  throw new NotImplementedException(); }
}