using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Data.Entities.identityEntites;
using Store.Service.HandelResponses;
using Store.Service.Services.UserServices;
using Store.Service.Services.UserServices.DTOS;
using System.Security.Claims;

namespace Store.API.Controllers
{
   
    public class AccountController : BaseController
    {
        private readonly IUserServices _userServices;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(IUserServices userServices , UserManager<AppUser> userManager)
        {
            _userServices = userServices;
            _userManager = userManager;
        }
        [HttpPost]
       public async Task<ActionResult<Userdto>>Login(LoginDto input)
        {
            var user = await _userServices.login(input);
            if (user is null)
            return Unauthorized(new CustomException(401));
            return Ok(user);
        }


        [HttpPost]
        public async Task<ActionResult<Userdto>> Register(RegisterDto input)
        {
            var user = await _userServices.Register(input);
            if (user is null)
                return BadRequest(new CustomException(400 , "Email Already Exists"));
            return Ok(user);
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<Userdto>>GetCurrentUserDetails()
        {
            var email = User?.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            return new Userdto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
               
            };
        }
    }
}
