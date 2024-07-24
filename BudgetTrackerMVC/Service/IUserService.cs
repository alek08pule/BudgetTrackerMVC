namespace BudgetTrackerMVC.Service
{
   

public interface IUserService
    {
        Task<string> GetCurrentUserIdAsync();

        bool IsUserAuthenticated();
    }

}

