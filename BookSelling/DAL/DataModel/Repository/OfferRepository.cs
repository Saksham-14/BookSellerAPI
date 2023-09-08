using DAL.Common;
using DAL.Database;
using DAL.Enums;
using DAL.IRepository;
using DAL.Models;
using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data;

namespace DAL.Repository
{
    public class OfferRepository : DbContext, IOfferRepository
    {
        private ILogger<OfferRepository> _logger;
        
        #region Constructor
        public OfferRepository(ILogger<OfferRepository> logger)
        {
            _logger = logger;
        }
        #endregion

        #region GetOfferStatus
        public OfferStatus? GetOfferStatus(int bookId)
        {
            try
            {
                _logger.LogInformation("GetOfferStatus initiated with BookId {bookId}", bookId);
                OfferStatus? result;
                //Set up DynamicParameters object to pass parameters  
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("bookid", bookId);
                var response = mConnection.Query<int>("sp_getofferstatus", parameters, commandType: CommandType.StoredProcedure).ToList();
                result = (OfferStatus)response.FirstOrDefault();

                _logger.LogInformation("GetOfferStatus finished with BookId {bookId}", bookId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured in GetOfferStatus function and error is {error}", CommonMethod.GetErrorMessage(ex));
                return null;
            }
        }
        #endregion

        #region UpdateOfferStatus
        public OfferStatus? UpdateOfferStatus(BookModel model)
        {
            try
            {
                _logger.LogInformation("GetOfferStatus initiated with model {model}", JsonConvert.SerializeObject(model));
                var result = 0;
                //Set up DynamicParameters object to pass parameters  
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("username", model.Username);
                parameters.Add("bookid", model.BookId);
                parameters.Add("offerstatus", model.OfferStatus);
                //Execute stored procedure and map the returned result to a Customer object  
                var response = mConnection.Query<int>("sp_updateofferstatus", parameters, commandType: CommandType.StoredProcedure).ToList();
                if (response != null && response.Count > 0)
                {
                    var status = GetOfferStatus(model.BookId);
                    switch (status)
                    {
                        case OfferStatus.Requested: //From here seller will get notification for request of the offer
                            break;
                        case OfferStatus.Accepted: //From here customer will get notification for acceptance of the offer
                            break;
                    }
                    _logger.LogInformation("GetOfferStatus finished with model {model}", JsonConvert.SerializeObject(model));
                    result = response.FirstOrDefault();
                }
                return (OfferStatus)result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured in UpdateOfferStatus function and error is {error}", CommonMethod.GetErrorMessage(ex));
                return 0;
            }
        }
        #endregion
    }
}