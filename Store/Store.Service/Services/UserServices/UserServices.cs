using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient.Server;
using Store.Data.Entities.identityEntites;
using Store.Service.Services.TokenServices;
using Store.Service.Services.UserServices.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.UserServices
{
    public class UserServices : IUserServices
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenServices _tokenService;

        public UserServices(UserManager<AppUser> userManager ,
            SignInManager<AppUser>signInManager, ITokenServices tokenService)
            
           
        {
            _userManager = userManager;
            _signInManager = signInManager;
         
           _tokenService = tokenService;
        }
        public async Task<Userdto> login(LoginDto input)
        {
          var user= await _userManager.FindByEmailAsync(input.Email);
            if (user is null)

                return null;

                var result = await _signInManager.CheckPasswordSignInAsync(user, input.Password, false);
                if (!result.Succeeded)
                
                    throw new Exception("Login Failed");

                    return new Userdto
                    {
                        Email = user.Email,
                        DisplayName = user.DisplayName,
                        Token = _tokenService.GenrateToken(user)
                    };
                
            

        }

        public async Task<Userdto> Register(RegisterDto input)
        {
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user is null)
                return null;
            var appuser = new AppUser
            {
                DisplayName = input.DisplayName,
                Email = input.Email,
                UserName = input.DisplayName,

            };
            var result = await _userManager.CreateAsync(appuser , input.Password);
            if (!result.Succeeded)
                throw new Exception(result.Errors.Select(x => x.Description).FirstOrDefault());

            return new Userdto
            {
                Email = input.Email,
                DisplayName = input.DisplayName,
                Token = _tokenService.GenrateToken(appuser)
            };
        }
    }
}
