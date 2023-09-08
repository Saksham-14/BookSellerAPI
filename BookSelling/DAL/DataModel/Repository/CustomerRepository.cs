using DAL.Common;
using DAL.Database;
using DAL.IRepository;
using DAL.Models;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace DAL.Repository
{
    public class CustomerRepository : DbContext, ICustomerRepository
    {
        private ILogger<CustomerRepository> _logger;
        #region Constructor
        public CustomerRepository(ILogger<CustomerRepository> logger)
        {
            _logger = logger;
        }
        #endregion

        #region LoginCheck
        public CustomerModel LoginCheck(CustomerModel customer)
        {
            try
            {
                _logger.LogInformation("LoginCheck initiated for {Username}", customer.Username);
                var result = new CustomerModel();
                //Set up DynamicParameters object to pass parameters  
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("uname", customer.Username);
                parameters.Add("pwd", customer.Password);
                //Execute stored procedure and map the returned result to a Customer object  
                var response = mConnection.Query<CustomerModel>("sp_signin", parameters, commandType: CommandType.StoredProcedure).ToList();
                if (response != null && response.Count > 0 && response.FirstOrDefault() != null)
                    result = response.FirstOrDefault()!;

                _logger.LogInformation("LoginCheck finished for {Username}", customer.Username);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured in LoginCheck function and error is {error}", CommonMethod.GetErrorMessage(ex));
                return new CustomerModel();
            }
        }
        #endregion

        #region RegisterCustomer
        public bool RegisterCustomer(CustomerModel customer)
        {
            try
            {
                _logger.LogInformation("RegisterCustomer initiated for {Username}", customer.Username);
                var result = false;
                //Set up DynamicParameters object to pass parameters  
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("uname", customer.Username);
                parameters.Add("pwd", customer.Password);
                //Execute stored procedure and map the returned result to a Customer object  
                var response = mConnection.Query<int>("sp_signup", parameters, commandType: CommandType.StoredProcedure).ToList();
                if (response != null && response.Count > 0)
                    result = response.FirstOrDefault()! > 0;
                else
                    result = false;
                _logger.LogInformation("RegisterCustomer finished for {Username}", customer.Username);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured in RegisterCustomer function and error is {error}", CommonMethod.GetErrorMessage(ex));
                return false;
            }
        }
        #endregion
    }
}