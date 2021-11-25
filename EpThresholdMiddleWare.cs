using EpThreshold;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EpThreshold
{
    public class EpThresholdMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly EpThresholdOptions options;

        public EpThresholdMiddleWare(RequestDelegate next, Action<EpThresholdOptions> settings)
        {
            _next = next;
            options = new EpThresholdOptions();
            settings.Invoke(options);

        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                await _next(context);

                if(context.Response.HasStarted)
                {
                    watch.Stop();
                }
                else
                {
                    context.Response.OnStarting(() =>
                    {
                        watch.Stop();
                        return Task.CompletedTask;
                    });
                }

                if(options.ThresholdMetAction != null)
                {
                    var attribute = context.Features.Get<IEndpointFeature>()?.Endpoint?.Metadata?.GetMetadata<ThresholdAttribute>();
                    if (watch.ElapsedMilliseconds >= (attribute?.Threshold ?? options.Threshold))
                    {
                        options.ThresholdMetAction.Invoke(
                            new ThresholdLogModel
                            {
                                Url = context.Request.Path,
                                Method = context.Request.Method,
                                Started = DateTimeOffset.Now.AddMilliseconds(-watch.ElapsedMilliseconds),
                                StatusCode = context.Response.StatusCode,
                                TimeMs = watch.ElapsedMilliseconds
                            });
                    }
                }  
            }
            catch (Exception) { }
        }
    }

    public static class EpThresholdMiddleWareExtensions
    {
        public static IApplicationBuilder UseEpThreshold(this IApplicationBuilder builder, Action<EpThresholdOptions> options) => builder.UseMiddleware<EpThresholdMiddleWare>(options);
    }








}
