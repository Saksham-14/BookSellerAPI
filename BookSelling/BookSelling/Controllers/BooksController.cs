using DAL.Common;
using DAL.Enums;
using DAL.IRepository;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;

namespace BookSelling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        #region Variables
        private ILogger<BooksController> _logger;
        private readonly IOfferRepository _offerRepository;
        #endregion

        #region Constructor
        public BooksController(ILogger<BooksController> logger, IOfferRepository offerRepository)
        {
            _logger = logger;
            _offerRepository = offerRepository;
        }
        #endregion

        #region CheckBookAvailability
        [HttpGet("CheckBookAvailability/{id}")]
        public IActionResult CheckBookStatus(int id)
        {
            try
            {
                _logger.LogInformation("CheckBookStatus initiated with BookId {model}", id);
                var result = _offerRepository.GetOfferStatus(id)?.GetDescription();
                _logger.LogInformation("CheckBookStatus finished with Result {result} for BookId {id}", result, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured in UpdateOfferStatus function and error is {error}", CommonMethod.GetErrorMessage(ex));
                return BadRequest(CommonMethod.GetErrorMessage(ex));
            }
        }
        #endregion

        #region UpdateOfferStatus
        [HttpPost("UpdateOfferStatus")]
        public IActionResult UpdateBookStatus(BookModel model)
        {
            try
            {
                _logger.LogInformation("UpdateOfferStatus initiated with Input {model}", JsonConvert.SerializeObject(model));
                var result = _offerRepository.UpdateOfferStatus(model)?.GetDescription();
                _logger.LogInformation("UpdateOfferStatus finished with Result {result} for {Username}", result, model.Username);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured in UpdateOfferStatus function and error is {error}", CommonMethod.GetErrorMessage(ex));
                return BadRequest(CommonMethod.GetErrorMessage(ex));
            }
        }
        #endregion
    }
}
