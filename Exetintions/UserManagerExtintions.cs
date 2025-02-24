using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Exetintions
{
    public static class UserManagerExtintions
    {
        public static async Task<AppUser> FindUserWithAddressByEmailAsync(this UserManager<AppUser> userManager,ClaimsPrincipal User )
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users.Include(u => u.Address).SingleOrDefaultAsync(u => u.Email == email);

            return user;
        }
    }
}
