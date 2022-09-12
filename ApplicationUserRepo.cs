using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CostelloMVCWebProject.Models
{
    public class ApplicationUserRepo : IApplicationUserRepo
    {
        private IHttpContextAccessor httpContext;

        public ApplicationUserRepo(IHttpContextAccessor contextaccessor)
        {
            this.httpContext = contextaccessor;
        }

        public string FindUserID()
        {
            string userid = httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return userid;
        }
    }
}
