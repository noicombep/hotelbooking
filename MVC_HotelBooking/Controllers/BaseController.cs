using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

public class BaseController : Controller
{
    public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
    {
        var token = HttpContext.Session.GetString("token");

        if (!string.IsNullOrEmpty(token))
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            ViewBag.User = new
            {
                Email = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                UserId = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                Role = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value
            };
        }

        base.OnActionExecuting(context);
    }
}