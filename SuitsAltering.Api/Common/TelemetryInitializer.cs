using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using SuitsAltering.Infrastructure.Extensions;

namespace SuitsAltering.API.Common;

public class TelemetryInitializer : ITelemetryInitializer
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TelemetryInitializer(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Initialize(ITelemetry telemetry)
    {
        var requestTelemetry = telemetry as RequestTelemetry;
        if (requestTelemetry == null)
        {
            return;
        }

        var context = _httpContextAccessor.HttpContext;
        if (context == null)
        {
            return;
        }

        AddHeaders(context, requestTelemetry);
        AddClaims(context, requestTelemetry);
    }

    private void AddHeaders(HttpContext context, RequestTelemetry telemetry)
    {
        var headers = context.Request.Headers;
        if (headers == null)
        {
            return;
        }

        foreach (var header in headers)
        {
            telemetry.Context.GlobalProperties.TryAdd($"Request-{header.Key}", header.Value.JoinWith(Environment.NewLine));
        }
    }

    private void AddClaims(HttpContext context, RequestTelemetry telemetry)
    {
        var claims = context.User.Claims;
        if (claims == null)
        {
            return;
        }

        foreach (var claim in claims)
        {
            telemetry.Context.GlobalProperties.TryAdd($"Claim-{claim.Type}", claim.Value);
        }
    }
}