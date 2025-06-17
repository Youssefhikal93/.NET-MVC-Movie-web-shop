using Lexiflix.Models.Db;
namespace Lexiflix;


public interface ICustomerServices
{
    public void AddNewCustomer(Customer customer);
    public void UpdateCustomer(Customer customer);
    public List<Customer> GetAllCustomers();

   public Customer GetCustomerByEmail(string email)
    {
        throw new NotImplementedException();
    }
   
   



    public Customer GetCustomerById(int id);
    public void DeleteCustomer(int id);
}