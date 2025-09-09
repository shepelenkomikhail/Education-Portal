using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using EducationPortal.Data.Models;

namespace EducationPortal.WebMVC.Services
{
    public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, IdentityRole<int>>
    {
        public CustomUserClaimsPrincipalFactory(
            UserManager<User> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = new ClaimsIdentity("Identity.Application", Options.ClaimsIdentity.UserNameClaimType, Options.ClaimsIdentity.RoleClaimType);

            var userId = await UserManager.GetUserIdAsync(user);
            var userName = await UserManager.GetUserNameAsync(user);
            var email = await UserManager.GetEmailAsync(user);

            identity.AddClaim(new Claim(Options.ClaimsIdentity.UserIdClaimType, userId ?? user.Id.ToString()));

            var nameValue = !string.IsNullOrEmpty(userName) ? userName :
                           !string.IsNullOrEmpty(email) ? email :
                           user.Id.ToString();
            identity.AddClaim(new Claim(Options.ClaimsIdentity.UserNameClaimType, nameValue));

            if (!string.IsNullOrEmpty(email))
            {
                identity.AddClaim(new Claim(ClaimTypes.Email, email));
            }

            if (!string.IsNullOrEmpty(user.SecurityStamp))
            {
                identity.AddClaim(new Claim(Options.ClaimsIdentity.SecurityStampClaimType, user.SecurityStamp));
            }

            if (!string.IsNullOrEmpty(user.FirstName))
            {
                identity.AddClaim(new Claim("FirstName", user.FirstName));
            }

            if (!string.IsNullOrEmpty(user.Surname))
            {
                identity.AddClaim(new Claim("Surname", user.Surname));
            }

            if (UserManager.SupportsUserRole)
            {
                var roles = await UserManager.GetRolesAsync(user);
                foreach (var roleName in roles)
                {
                    if (!string.IsNullOrEmpty(roleName))
                    {
                        identity.AddClaim(new Claim(Options.ClaimsIdentity.RoleClaimType, roleName));
                        if (RoleManager.SupportsRoleClaims)
                        {
                            var role = await RoleManager.FindByNameAsync(roleName);
                            if (role != null)
                            {
                                identity.AddClaims(await RoleManager.GetClaimsAsync(role));
                            }
                        }
                    }
                }
            }

            if (UserManager.SupportsUserClaim)
            {
                var userClaims = await UserManager.GetClaimsAsync(user);
                foreach (var claim in userClaims)
                {
                    if (!string.IsNullOrEmpty(claim.Value))
                    {
                        identity.AddClaim(claim);
                    }
                }
            }

            return identity;
        }
    }
}
