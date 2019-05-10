using System;
using System.Activities;
using System.ComponentModel;
using System.DirectoryServices.AccountManagement;
using System.Security;

namespace LDAP_Authenticator
{
    public class LdapAuthenticator : CodeActivity
    {
        [Category("Input")]
        public InArgument<String> Username { get; set; }

        [Category("Input")]
        public InArgument<SecureString> Password { get; set; }

        [Category("Output")]
        public OutArgument<bool> Authenticated { get; set; }

        [Category("Output")]
        public OutArgument<String> Email { get; set; }

        [Category("Output")]
        public OutArgument<String> GivenName { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            using (var principalContext = new PrincipalContext(ContextType.Domain))
            {
                using (SecureStringToStringMarshaler sm = new SecureStringToStringMarshaler(Password.Get(context)))
                {
                    bool authenticated = principalContext.ValidateCredentials(Username.Get(context), sm.String);
                    UserPrincipal principal = UserPrincipal.FindByIdentity(principalContext, Username.Get(context));
                    Authenticated.Set(context, authenticated);
                    Email.Set(context, principal.EmailAddress);
                    GivenName.Set(context, principal.GivenName);
                }

            }
        }
    }
}
