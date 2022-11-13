/*
 * In this part I did:
 *
 * - Authentication Cookie
 */

using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataProtection();

var app = builder.Build();

app.MapGet("/username", (HttpContext ctx, IDataProtectionProvider idp) =>
{
    var protector = idp.CreateProtector("auth-cookie");

    var cookie = ctx.Request.Headers.Cookie.FirstOrDefault(x => x.Contains("auth="));
    var authCookie = cookie?[cookie.IndexOf("auth=", StringComparison.Ordinal)..];


    var protectedPayload = authCookie?.Split("=").Last();
    var payload = protector.Unprotect(protectedPayload);

    return payload?.Split(":").Last();
});

app.MapGet("/login", (HttpContext ctx, IDataProtectionProvider idp) =>
{
    var protector = idp.CreateProtector("auth-cookie");
    ctx.Response.Headers["set-cookie"] = $"auth={protector.Protect("usr:komla")}";
    return "ok";
});

app.Run();