using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using BusinessObject;
public class AccountController : BaseController
{
    private readonly ApiService _api;

    public AccountController(ApiService api)
    {
        _api = api;
    }

    // 🟢 LOGIN PAGE
    public IActionResult Login()
    {
        return View();
    }

    // 🟢 LOGIN POST
    [HttpPost]
    public async Task<IActionResult> Login(BusinessObject.LoginRequest req)
    {
        var result = await _api.Login(req);

        if (!result.Success)
        {
            ViewBag.Error = result.Message;
            return View();
        }

        // 🔥 lưu JWT
        HttpContext.Session.SetString("token", result.Data);

        return RedirectToAction("Index", "Home");
    }

    // 🟢 REGISTER PAGE
    public IActionResult Register()
    {
        return View();
    }

    // 🟢 REGISTER POST
    [HttpPost]
    public async Task<IActionResult> Register(BusinessObject.RegisterRequest req)
    {
        var result = await _api.Register(req);

        if (!result.Success)
        {
            ViewBag.Error = result.Message;
            return View();
        }

        ViewBag.Success = result.Message;
        return View();
    }

    // 🟢 LOGOUT
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}