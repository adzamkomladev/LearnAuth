/*
 * In this part I did:
 *
 * - Authentication Cookie
 */

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/username", (HttpContext ctx) =>
{
    var cookie = ctx.Request.Headers.Cookie.FirstOrDefault(x => x.Contains("auth="));
    var authCookie = cookie?[cookie.IndexOf("auth=", StringComparison.Ordinal)..];

    char[] delimiterChars = {'=', ':'};
    return authCookie?.Split(delimiterChars) switch
    {
        [_, _, var username] => username
    };
});

app.MapGet("/login", (HttpContext ctx) =>
{
    ctx.Response.Headers["set-cookie"] = "auth=usr:komla";
    return "ok";
});

app.Run();