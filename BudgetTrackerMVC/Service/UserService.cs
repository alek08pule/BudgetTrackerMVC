using BudgetTrackerMVC.Domains;
using BudgetTrackerMVC.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<User> _userManager;

    public UserService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<string> GetCurrentUserIdAsync()
    {
        var email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
        if (email == null)
        {
            return null;
        }

        var user = await _userManager.FindByEmailAsync(email);
        return user?.Id;
    }

    public bool IsUserAuthenticated()
    {
        return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
    }
}
