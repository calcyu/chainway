using System;
using ChainFx.Web;

namespace ChainPort
{
    /// <summary>
    /// To implement principal authorization of access to the target resources.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        // org role requirement (bitwise)
        readonly short orgly;

        // platform admin role requirement (bitwise)
        readonly short admly;


        public UserAuthorizeAttribute(short orgly = 0, short admly = 0)
        {
            this.orgly = orgly;
            this.admly = admly;
        }

        public override bool Do(WebContext wc, bool mock)
        {
            var prin = (User) wc.Principal;

            if (prin == null) // auth required
            {
                return false;
            }

            if (admly > 0)
            {
                return (prin.admly & admly) == admly;
            }

            // require sign-in only
            if (orgly == 0) return true;

            // check access to org
            var org = wc[typeof(OrglyVarWork)].As<Org>();

            // is the manager of the org
            if (org.mgrid == prin.id)
            {
                if (!mock)
                {
                    // wc.Role = "管理员";
                }
                return true;
            }

            // is of any role for the org
            if (org.id == prin.orgid && (prin.orgly & orgly) == orgly)
            {
                if (!mock)
                {
                    // wc.Role = User.Orgly[prin.orgly];
                }
                return true;
            }

            return false;
        }
    }
}