using Lexiflix.Data.Db;
using Lexiflix.Models;
namespace Lexiflix.Services;

public class CustomerServices : ICustomerServices
{

    private readonly MovieDbContext _db;
    public CustomerServices(MovieDbContext db)
    {

        _db = db;
    }
    public void AddNewCustomer(Customer customer)
    {
        _db.Add(customer);
        
        _db.SaveChanges();
    }

    public List<Customer> GetAllCustomers()
    {
        return  _db.Customers.ToList();
        
    }
    Customer GetCustomerByEmail(string email)
    {
        var Customers = new List<Customer>
    {
        new Customer {Id = 1, Email = "Test@example.com" , FirstName = "Test", LastName = "User" }
    };
        return Customers.FirstOrDefault(c => c.Email == email);
    }

    Customer ICustomerServices.GetCustomerByEmail(string email)
    {
        return GetCustomerByEmail(email);
    }
}
