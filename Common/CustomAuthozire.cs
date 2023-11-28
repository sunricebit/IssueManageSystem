using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MySqlX.XDevAPI;
using System.Net;
using System.Web.Mvc;

namespace IMS.Common
{
    public class CustomAuthorize
    {
        private readonly string[] allowedroles;

        public CustomAuthorize(params string[] roles) => allowedroles = roles;

        public bool Authorize(HttpContext context)
        {
            string role = context.Session.GetString("role");
            if (!string.IsNullOrEmpty(role))
            {
                if (allowedroles.Contains(role))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
