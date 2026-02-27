using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Owin;
using Microsoft.IdentityModel.Tokens;

[assembly: OwinStartup(typeof(Task_4.Startup))]
namespace Task_4
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var issuer = "VanriseInternshipApp";
            var audience = "VanriseInternshipApp";
            var secret = Encoding.UTF8.GetBytes("MySuperSecretInternshipKey2026!!");

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(secret)
                }
            });
        }
    }
}