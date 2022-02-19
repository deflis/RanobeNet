using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;


namespace RanobeNet.Utils
{
    public class FirebaseAuthenticationHandler : AuthenticationHandler<FirebaseSchemeOptions>
    {
        public FirebaseAuthenticationHandler(
            IOptionsMonitor<FirebaseSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
                : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string jwt;
            FirebaseToken token;

            try
            {
                jwt = Request.Headers.Authorization.ToString().Replace("Bearer ", string.Empty);
                token = await Options.FirebaseAuth.VerifyIdTokenAsync(jwt);
            }
            catch (Exception ex)
            {
                Context.Response.StatusCode = 401;
                return AuthenticateResult.Fail(ex);
            }

            var ticket = BuildTicket(token);

            return AuthenticateResult.Success(ticket);
        }

        private AuthenticationTicket BuildTicket(FirebaseToken token)
        {
            var claims = BuildClaims(token);
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);

            return new AuthenticationTicket(principal, Scheme.Name);
        }

        private static Claim[] BuildClaims(FirebaseToken token)
        {
            return new[]
            {
                new Claim(ClaimTypes.Name, token.Claims["name"]?.ToString() ?? ""),
                new Claim(ClaimTypes.NameIdentifier, token.Claims["user_id"]?.ToString() ?? "")
            };
        }
    }

    public class FirebaseSchemeOptions : AuthenticationSchemeOptions
    {
        public FirebaseAuth FirebaseAuth { get; set; } = FirebaseAuth.DefaultInstance;
    }

    public static class FirebaseAuthenticationExtension
    {
        public static AuthenticationBuilder AddFirebaseAuthentication(
            this IServiceCollection services)
        {
            return AddFirebaseAuthentication(services, "firebase", "firebase");
        }

        public static AuthenticationBuilder AddFirebaseAuthentication(
            this IServiceCollection services,
            Action<FirebaseSchemeOptions> options)
        {
            return AddFirebaseAuthentication(services, "firebase", "firebase", options);
        }

        public static AuthenticationBuilder AddFirebaseAuthentication(
            this IServiceCollection services,
            string schemeName)
        {
            return AddFirebaseAuthentication(services, schemeName, schemeName);
        }

        public static AuthenticationBuilder AddFirebaseAuthentication(
            this IServiceCollection services,
            string schemeName,
            string displayName)
        {
            return AddFirebaseAuthentication(services, schemeName, displayName);
        }

        public static AuthenticationBuilder AddFirebaseAuthentication(
            this IServiceCollection services,
            string schemeName,
            Action<FirebaseSchemeOptions> options)
        {
            return AddFirebaseAuthentication(services, schemeName, schemeName, options);
        }

        public static AuthenticationBuilder AddFirebaseAuthentication(
            this IServiceCollection services,
            string schemeName,
            string displayName,
            Action<FirebaseSchemeOptions>? options)
        {
            var authenticationBuilder =
                services
                    .AddAuthentication(schemeName)
                    .AddScheme<FirebaseSchemeOptions, FirebaseAuthenticationHandler>(schemeName, displayName, options);

            return authenticationBuilder;
        }
    }
}
