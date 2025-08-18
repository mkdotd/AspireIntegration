using Models;
namespace SeveraCustomers.ApiService
{
    public class CustomerService
    {
        private readonly SeveraPGSQLContext _context;
        public CustomerService(SeveraPGSQLContext context)
        {
            _context = context;
        }
        public List<Customers> GetCustomers()
        {
            return _context.Customers.ToList();
        }
        public Customers? GetCustomer(int Id)
        {
            return _context.Customers.FirstOrDefault(c => c.ID == Id);
        }

        public bool SeveraCustomerExists(Guid Id)
        {
            return _context.Customers.Any(c => c.Guid == Id);
        }

        public void UpdateCustomer(Customers customer)
        {
            var rec = _context.Customers.FirstOrDefault(c => c.ID == customer.ID);

            if (rec != null)
            {
                rec.Number = customer.Number;
                rec.OwnerName = customer.OwnerName;
                rec.IndustryName = customer.IndustryName;
                rec.Name = customer.Name;
                rec.Website = customer.Website;
                rec.Guid = customer.Guid;

                _context.Update(rec);
                _context.SaveChanges();
            }
        }

        public void CreateCustomer(Customers customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

    }
}
