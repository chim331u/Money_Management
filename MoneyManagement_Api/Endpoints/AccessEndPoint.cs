using MoneyManagement_Api.Interfaces;
using MoneyManagement_Api.Models.Access;

namespace MoneyManagement_Api.Endpoints;

public static class AccessEndPoint
{
    public static IEndpointRouteBuilder MapAccessEndPoint(this IEndpointRouteBuilder app)
    {
        // app.MapGet("/login", async ([FromBody]AuthRequest authRequest, IAccessService service ) =>
        // {
        //     var result = await service.Login(authRequest);
        //     return result != null ? Results.Ok(result) : Results.NotFound();
        // });

        app.MapPost("/login", async (AuthRequest authRequest, IIdentityService identityService) =>
        {
            var result = await identityService.Login(authRequest);
            return result != null ? Results.Ok(result) : Results.NotFound();
        });

        return app;
    }
}