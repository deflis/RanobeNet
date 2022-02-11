using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

/// <summary>
/// AspNetCore.Firebase.Authentication をコピーしてきたもの
/// </summary>
/// <see href="https://bitbucket.org/RAPHAEL_BICKEL/aspnetcore.firebase.authentication/src/master/src/AspNetCore.Firebase.Authentication/Extensions/ApplicationBuilderExtensions.cs"/>
namespace RanobeNet.Utils
{
    public static class FirebaseAuthenticationExtension
    {
        public static IServiceCollection AddFirebaseAuthentication(this IServiceCollection services, string issuer, string audience)
        {
            var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>($"{issuer}/.well-known/openid-configuration", new OpenIdConnectConfigurationRetriever());
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.IncludeErrorDetails = true;
                o.RefreshOnIssuerKeyNotFound = true;
                o.MetadataAddress = $"{issuer}/.well-known/openid-configuration";
                o.ConfigurationManager = configurationManager;
                o.Audience = audience;
            });
            return services;
        }

        public static IServiceCollection AddFirebaseAuthentication(this IServiceCollection services, string firebaseProject)
        {
            return services.AddFirebaseAuthentication("https://securetoken.google.com/" + firebaseProject, firebaseProject);
        }
    }
}
