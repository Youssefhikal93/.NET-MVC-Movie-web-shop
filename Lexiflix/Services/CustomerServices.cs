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

    public List<Customer> GetAllCustomers()
    {
        return  _db.Customers.ToList();
        
    }

  
 }
