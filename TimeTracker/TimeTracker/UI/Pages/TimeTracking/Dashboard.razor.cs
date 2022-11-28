using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using UI.Services.Interfaces;

namespace UI.Pages.TimeTracking
{
    [Authorize]
    public partial class Dashboard
    {

        [Inject]
        private IUserService UserService { get; set; }
        [Inject]
        private IShiftService ShiftService { get; set; }
        [Inject]
        private IBreakService BreakService { get; set; }
        [Inject]
        private ILoggerFactory LoggerFactory { get; set; }
        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private ILogger _logger { get; set; }
        private ILogger Logger
            => _logger ??= LoggerFactory.CreateLogger<CurrentShift>();

        public Dashboard() { }

        protected bool ViewAll { get; set; }

        private string? UserId { get; set; }
        private async Task<string?> GetUserId()
        {
            if (string.IsNullOrWhiteSpace(UserId))
            {
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;

                if (user.Identity?.IsAuthenticated ?? false)
                {
                    UserId = user.FindFirst(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
                }
            }

            return UserId;
        }

        protected IEnumerable<Tuple<string, string>>? UserList { get; set; }
        protected async Task<IEnumerable<Tuple<string, string>>?> GetUserList()
            => UserList ??= (await UserService.GetUserList());
    }
}
