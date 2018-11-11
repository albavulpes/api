using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Models.Identity;
using AlbaVulpes.API.Models.Requests;
using Marten;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AlbaVulpes.API.Repositories.Identity
{
    public class AuthRepository : ApiRepository
    {
        public AuthRepository(IDocumentStore documentStore) : base(documentStore)
        {
        }

        public async Task<ClaimsPrincipal> AuthenticateUser(LoginRequest loginRequest)
        {
            using (var session = Store.QuerySession())
            {
                var user = await session.Query<User>()
                    .Where(u => u.Email == loginRequest.Email || u.UserName == loginRequest.Email)
                    .FirstOrDefaultAsync();

                // User doesn't exist
                if (user == null)
                {
                    return null;
                }

                var isPasswordValid = BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password);

                // Bad password
                if (!isPasswordValid)
                {
                    return null;
                }

                // User is legit, create a new claim identity
                var claims = new[]
                {
                    new Claim(ClaimTypes.Email, user.Email)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                return claimsPrincipal;
            }
        }

        public async Task<User> RegisterNewUser(RegisterRequest registerRequest)
        {
            using (var session = Store.QuerySession())
            {
                if (session.Query<User>().Any(u => u.Email == registerRequest.Email || u.UserName == registerRequest.UserName))
                {
                    return null;
                }


                using (var openSession = Store.OpenSession())
                {
                    var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

                    var newUser = new User
                    {
                        Email = registerRequest.Email,
                        UserName = registerRequest.UserName,
                        Password = passwordHash
                    };

                    openSession.Insert(newUser);
                    await openSession.SaveChangesAsync();

                    return newUser;
                }
            }
        }
    }
}