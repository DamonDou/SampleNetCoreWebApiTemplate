using Microsoft.AspNetCore.Builder;


namespace SampleNetCoreWebApiTemplate.WebApi.MiddleWare
{
    public static class MiddleWareExtend
    {
        public static void UseCustomerExceptionMiddleWare(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomerExceptionMiddleWare>();
        }

        public static void UseTraceLogMiddleWare(this IApplicationBuilder app)
        {
            app.UseMiddleware<TraceLogMiddleWare>();
        }
    }
}
