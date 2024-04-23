using Store.Data.Entities.identityEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.TokenServices
{
    public interface ITokenServices
    {
        string GenrateToken(AppUser appUser);
    }
}
