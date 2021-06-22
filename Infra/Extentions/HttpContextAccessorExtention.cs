using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Infra.Extentions
{
    public static class HttpContextAccessorExtention
    {
        public static string GetUserId(this IHttpContextAccessor httpContextAccessor)
        {
            //get ID from token in Jwtservice class
            var cl = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault();
            return cl.Value;
        }
    }
}
