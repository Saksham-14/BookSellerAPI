using DAL.Models;

namespace DAL.IRepository
{
    public interface ICustomerRepository
    {
        CustomerModel LoginCheck(CustomerModel customer);
        bool RegisterCustomer(CustomerModel customer);
    }
}