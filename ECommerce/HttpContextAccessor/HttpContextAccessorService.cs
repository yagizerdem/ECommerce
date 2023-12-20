using Azure.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.HttpContextAccessor
{
    public class HttpContextAccessorService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextAccessorService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetIp()
        {
            var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;

            // If the client is behind a proxy, use the X-Forwarded-For header
            if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ipAddress = IPAddress.Parse(_httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].First());
            }
            // Convert the IPAddress to a string
            string ipAddressString = ipAddress.ToString();
            return ipAddressString;
        }

    }
}
