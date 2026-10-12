using Microsoft.AspNetCore.Mvc;

public class HomeController : BaseController
{
    private readonly ApiService _api;

    public HomeController(ApiService api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        var rooms = await _api.GetRooms();
        return View(rooms);
    }
}