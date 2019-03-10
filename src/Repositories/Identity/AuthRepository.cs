using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Models.Identity;
using AlbaVulpes.API.Models.Requests;
using AlbaVulpes.API.Services.AWS;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using Marten;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AlbaVulpes.API.Repositories.Identity
{
    public class AuthRepository : ApiRepository
    {
        private readonly ISecretsManagerService _secretsManager;

        public AuthRepository(IDocumentStore documentStore, ISecretsManagerService secretsManager) : base(documentStore)
        {
            _secretsManager = secretsManager;
        }

        public async Task<ClaimsPrincipal> AuthenticateUser(LoginRequest loginRequest)
        {
            using (var session = _store.QuerySession())
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
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var userRoleClaims = user.Roles.Select(role => new Claim(ClaimTypes.Role, role.ToString())).ToList();
                claims.AddRange(userRoleClaims);

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                return claimsPrincipal;
            }
        }

        public async Task<ClaimsPrincipal> AuthenticateGoogleUser(GoogleLoginRequest loginRequest)
        {
            var service = new Oauth2Service(new BaseClientService.Initializer());

            var request = service.Tokeninfo();
            request.AccessToken = loginRequest.AccessToken;

            var info = request.Execute();

            using (var session = _store.OpenSession())
            {
                var user = await session.Query<User>()
                    .Where(u => u.Email == info.Email)
                    .FirstOrDefaultAsync();

                // User doesn't exist, reject login
                if (user == null)
                {
                    return null;
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var userRoleClaims = user.Roles.Select(role => new Claim(ClaimTypes.Role, role.ToString())).ToList();
                claims.AddRange(userRoleClaims);

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                return claimsPrincipal;
            }
        }

        public async Task<User> RegisterNewUser(RegisterRequest registerRequest)
        {
            using (var session = _store.QuerySession())
            {
                if (session.Query<User>().Any(u => u.Email == registerRequest.Email || u.UserName == registerRequest.UserName))
                {
                    return null;
                }

                using (var openSession = _store.OpenSession())
                {
                    var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

                    var newUser = new User
                    {
                        Email = registerRequest.Email,
                        UserName = registerRequest.UserName,
                        Password = passwordHash,
                        Roles = new List<Role> { Role.Member }
                    };

                    openSession.Insert(newUser);
                    await openSession.SaveChangesAsync();

                    return newUser;
                }
            }
        }

        public async Task<User> GetUserByEmail(string emailAddress)
        {
            if (emailAddress == null)
            {
                return null;
            }

            using (var session = _store.QuerySession())
            {
                var user = await session.Query<User>()
                    .Where(u => u.Email == emailAddress)
                    .FirstOrDefaultAsync();

                return user;
            }
        }
    }
}