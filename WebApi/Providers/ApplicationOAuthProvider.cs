using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        #region[GrantResourceOwnerCredentials] Actual
        //public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        //{
        //    #region Actual
        //    var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

        //    ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

        //    if (user == null)
        //    {
        //        context.SetError("invalid_grant", "The user name or password is incorrect.");
        //        return;
        //    }

        //    ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
        //       OAuthDefaults.AuthenticationType);
        //    ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
        //        CookieAuthenticationDefaults.AuthenticationType);

        //    AuthenticationProperties properties = CreateProperties(user.UserName);
        //    AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
        //    context.Validated(ticket);
        //    context.Request.Context.Authentication.SignIn(cookiesIdentity);
        //    #endregion
        //}
        #endregion[GrantResourceOwnerCredentials]

        #region [GrantResourceOwnerCredentials] token authentication with custom DB

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            #region My Code for token authentication with custom DB

            return Task.Factory.StartNew(() =>
            {
                var userName = context.UserName;
                var password = context.Password;
                var userService = new UserService(); // our created one
                var user = userService.ValidateUser(userName, password);

                if (user != null)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Sid, Convert.ToString(user.Id)),
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Email, user.Email)
                    };
                    ClaimsIdentity oAuthIdentity = new ClaimsIdentity(claims,
                                Startup.OAuthOptions.AuthenticationType);

                    var properties = CreateProperties(user.Name);
                    //var properties = CreateProperties(user.Email);

                    var ticket = new AuthenticationTicket(oAuthIdentity, properties);
                    context.Validated(ticket);
                }
                else
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect");
                }

            });
            #endregion
        }
        #endregion GrantResourceOwnerCredentials

        #region[TokenEndpoint]
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
        #endregion

        #region[ValidateClientAuthentication]
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }
        #endregion

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        #region[CreateProperties]
        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
        #endregion
    }
}