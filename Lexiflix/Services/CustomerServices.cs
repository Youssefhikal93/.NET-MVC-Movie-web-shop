using Lexiflix.Data.Db;
using Lexiflix.Models.Db;
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


       public void UpdateCustomer(Customer customer)
    {
        _db.Update(customer);
        
        _db.SaveChanges();
    }


    public List<Customer> GetAllCustomers()
    {
        return  _db.Customers.ToList();
        
    }

    public Customer GetCustomerById(int id )
    {
         return _db.Customers.FirstOrDefault(c => c.Id == id);

    }

    
       public void DeleteCustomer(int id)
    {

         var customer = _db.Customers.FirstOrDefault(c => c.Id == id);
        if (customer != null)
        {
        _db.Remove(customer);
        _db.SaveChanges();

        }
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
