using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers
{
    public class BaseController : Controller
    {
        protected async Task<string> GetAccessToken()
        {
            return await HttpContext.GetTokenAsync("access_token");
        }

    }
}
