using DAL.Common;
using DAL.IRepository;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookSelling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region Variables
        private ILogger<AccountController> _logger;
        private readonly IConfiguration _config;
        private readonly ICustomerRepository _customerRepository;
        #endregion

        #region Constructor
        public AccountController(ILogger<AccountController> logger, IConfiguration config, ICustomerRepository customerRepository)
        {
            _logger = logger;
            _config = config;
            _customerRepository = customerRepository;
        }
        #endregion

        #region Signup
        [AllowAnonymous]
        [HttpPost("SignUp")]
        public ActionResult SignUp([FromBody] CustomerModel model)
        {
            try
            {
                _logger.LogInformation("Signup initiated for {Username}", model.Username);
                var customer = _customerRepository.RegisterCustomer(model);
                var msg = string.Empty;
                if (customer)
                {
                    msg = "Customer created successfully";
                }
                else
                {
                    msg = "Customer already exists";
                }
                _logger.LogInformation("Signup finished for {Username}", model.Username);
                return Ok(msg);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured in Signup function and error is {error}", CommonMethod.GetErrorMessage(ex));
                return BadRequest(CommonMethod.GetErrorMessage(ex));
            }
        }
        #endregion

        #region Login
        [AllowAnonymous]
        [HttpPost("SignIn")]
        public ActionResult Login([FromBody] CustomerModel customerLogin)
        {
            try
            {
                _logger.LogInformation("Login initiated for {Username}", customerLogin.Username);
                var customer = Authenticate(customerLogin);
                if (customer != null)
                {
                    var token = GenerateToken(customer);
                    _logger.LogInformation("Login finished for {Username}", customerLogin.Username);
                    return Ok(token);
                }
                else
                    return NotFound("Invalid username or password");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured in Login function and error is {error}", CommonMethod.GetErrorMessage(ex));
                return BadRequest(CommonMethod.GetErrorMessage(ex));
            }
        }
        #endregion

        #region Logout
        [Authorize]
        [HttpPost("SignOut")]
        public ActionResult Logout()
        {
            try
            {
                string token = string.Empty;
                // Retrieve the JWT token from the client-side cookie or local storage
                if (HttpContext.Request.Headers["Authorization"].ToString().Contains("Bearer"))
                    token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                _logger.LogInformation("Logout initiated for token : {token}", token);

                // Set the expiration time of the JWT token to a past date to invalidate it
                var jwtHandler = new JwtSecurityTokenHandler();
                var tokenData = jwtHandler.ReadJwtToken(token);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Expires = DateTime.Now.AddMilliseconds(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256)
                };
                var newToken = jwtHandler.CreateJwtSecurityToken(tokenDescriptor);
                var newTokenString = jwtHandler.WriteToken(newToken);

                // Remove the JWT token from the client-side cookie or local storage
                HttpContext.Response.Cookies.Delete("access_token");
                _logger.LogInformation("Logout initiated for token : {token}", token);
                return Ok("Logged out successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured in Logout function and error is {error}", CommonMethod.GetErrorMessage(ex));
                return BadRequest(CommonMethod.GetErrorMessage(ex));
            }
        }
        #endregion

        #region GenerateToken
        private string? GenerateToken(CustomerModel customer)
        {
            try
            {
                _logger.LogInformation("Generate token initiated for {Username}", customer.Username);
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier,customer.Username!)
            };
                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(15),
                    signingCredentials: credentials);
                _logger.LogInformation("Generate token finished for {Username}", customer.Username);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured in GenerateToken function and error is {error}", CommonMethod.GetErrorMessage(ex));
                return null;
            }


        }
        #endregion

        #region Authentication
        private CustomerModel? Authenticate(CustomerModel customerLogin)
        {
            try
            {
                _logger.LogInformation("Authentication initiated for {Username}", customerLogin.Username);
                var currentUser = _customerRepository.LoginCheck(customerLogin);
                if (currentUser != null)
                {
                    _logger.LogInformation("Authentication finished for {Username}", currentUser.Username);
                    return currentUser;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured in Authenticate function and error is {error}", CommonMethod.GetErrorMessage(ex));
                return null;
            }
        }
        #endregion
    }
}