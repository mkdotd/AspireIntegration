namespace SeveraCustomers.ApiService
{
    public class CustomerService
    {
        private readonly SeveraPGSQLContext _context;
        public CustomerService(SeveraPGSQLContext context)
        {
            _context = context;
        }
        public List<Models.Customers> GetCustomers()
        {
            return _context.Customers.ToList();
        }
    }
}
