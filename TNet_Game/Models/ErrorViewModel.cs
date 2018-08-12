using System;

namespace TNet_Web.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
public  class HttpContext2
{
    public static IServiceProvider ServiceProvider;

    public static Microsoft.AspNetCore.Http.HttpContext Current
    {
        get
        {
            object factory = ServiceProvider.GetService(typeof(Microsoft.AspNetCore.Http.IHttpContextAccessor));
            Microsoft.AspNetCore.Http.HttpContext context = ((Microsoft.AspNetCore.Http.HttpContextAccessor)factory).HttpContext;
            return context;
        }
    }

}