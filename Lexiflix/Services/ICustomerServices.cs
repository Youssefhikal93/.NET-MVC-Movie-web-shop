using Lexiflix.Models.Db;
using Microsoft.EntityFrameworkCore;
namespace Lexiflix;


public interface ICustomerServices
{
    public void AddNewCustomer(Customer customer);
    public void UpdateCustomer(Customer customer);
    public List<Customer> GetAllCustomers();
    public Customer GetCustomerById(int id);
    public void DeleteCustomer(int id);
    public Customer GetCustomerByEmail(string email);

    public bool Exists(int customerId);
}