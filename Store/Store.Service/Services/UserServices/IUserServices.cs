using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Service.Services.UserServices.DTOS;

namespace Store.Service.Services.UserServices
{
    public interface IUserServices
    {
        Task<Userdto> Register(RegisterDto input);
        Task<Userdto> login(LoginDto input);
    }

}
