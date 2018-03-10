using System;
using System.Security.Claims;
using System.DirectoryServices.AccountManagement;
using Microsoft.Owin.Security;
using System.Collections.Generic;

namespace NCForm.Models
{
    public class AdAuthenticationService
    {
        private static readonly log4net.ILog log
      = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public class AuthenticationResult
        {
            public AuthenticationResult(string errorMessage = null)
            {
                ErrorMessage = errorMessage;
            }

            public String ErrorMessage { get; private set; }
            public Boolean IsSuccess => String.IsNullOrEmpty(ErrorMessage);
        }

        private readonly IAuthenticationManager authenticationManager;

        public AdAuthenticationService(IAuthenticationManager authenticationManager)
        {
            this.authenticationManager = authenticationManager;
        }


        /// <summary>
        /// Check if username and password matches existing account in AD. 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public AuthenticationResult SignIn(String username, String password)
        {
#if DEBUG
            // authenticates against your local machine - for development time
            ContextType authenticationType = ContextType.Machine;
#else
            // authenticates against your Domain AD
            ContextType authenticationType = ContextType.Domain;
#endif
            PrincipalContext principalContext = new PrincipalContext(authenticationType);
            bool isAuthenticated = false;
             UserPrincipal userPrincipal = null;
            // replace with 3 lines below
            //UserPrincipal userPrincipal = new UserPrincipal(principalContext);
            //userPrincipal.SamAccountName = username;
            //var searcher = new PrincipalSearcher(userPrincipal);
            try
            {
                 userPrincipal = UserPrincipal.FindByIdentity(principalContext, username);
                //userPrincipal = searcher.FindOne() as UserPrincipal;
                if (userPrincipal != null)
                {
                    isAuthenticated = principalContext.ValidateCredentials(username, password, ContextOptions.Negotiate);
                }
            }
            catch (Exception exception)
            {
                log.Error("Error connecting to DC" + exception.Message, exception);
                return new AuthenticationResult("Username or Password is not correct");
            }

            if (!isAuthenticated || userPrincipal == null)
            {
                return new AuthenticationResult("Username or Password is not correct");
            }

            if (userPrincipal.IsAccountLockedOut())
            {
                // here can be a security related discussion weather it is worth 
                // revealing this information
                return new AuthenticationResult("Your account is locked.");
            }

            if (userPrincipal.Enabled.HasValue && userPrincipal.Enabled.Value == false)
            {
                // here can be a security related discussion weather it is worth 
                // revealing this information
                return new AuthenticationResult("Your account is disabled");
            }

            var identity = CreateIdentity(userPrincipal);
            
            authenticationManager.SignOut(MyAuthentication.ApplicationCookie);
            authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);


            return new AuthenticationResult();
        }


        private ClaimsIdentity CreateIdentity(UserPrincipal userPrincipal)
        {
            var identity = new ClaimsIdentity(MyAuthentication.ApplicationCookie, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            identity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "Active Directory"));
            identity.AddClaim(new Claim(ClaimTypes.Name, userPrincipal.SamAccountName));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userPrincipal.SamAccountName));
            if (!String.IsNullOrEmpty(userPrincipal.EmailAddress))
            {
                identity.AddClaim(new Claim(ClaimTypes.Email, userPrincipal.EmailAddress));
            }

            //var groups = userPrincipal.GetAuthorizationGroups();
            //foreach (var @group in groups)
            //{
            //    identity.AddClaim(new Claim(ClaimTypes.Role, @group.Name));
            //}
            var claims = new List<Claim>();
            var groups = new GroupPrincipal(userPrincipal.Context);
            var searcher = new PrincipalSearcher(groups);
            foreach(Principal item in searcher.FindAll())
            {
                var foundGroup = item as GroupPrincipal;
                if(foundGroup != null && foundGroup.IsSecurityGroup == true)
                {
                    claims.Add(new Claim(ClaimTypes.Role, foundGroup.Name));
                }
            }
            if (claims.Count > 0)
            {
                identity.AddClaims(claims);
            }

            // add your own claims if you need to add more information stored on the cookie

            return identity;
        }

    }
}