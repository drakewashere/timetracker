using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Policy;
using UI.Data.DTOs;
using UI.Services.Interfaces;

namespace UI.Pages.TimeTracking
{
    public partial class CurrentShift
    {
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


        public CurrentShift() { }

        protected string? ErrorState { get; set; }
        protected string? ShiftStartTime { get; set; }
        protected string? BreakStartTime { get; set; }

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

        private static string FormatDateTime(DateTime date)
            => date.ToString("MMMM dd, yyyy 'at' HH:mm:ss 'UTC'");

        private Shift? OpenShift { get; set; }
        protected async Task<Shift?> GetOpenShift()
            => OpenShift ??= await ShiftService.GetOpenShiftForUser((await GetUserId()) ?? string.Empty);

        private Break? OpenBreak { get; set; }
        protected async Task<Break?> GetOpenBreak()
            => OpenBreak ??= await BreakService.GetOpenBreakForUser((await GetUserId()) ?? string.Empty);

        protected async Task<Break?> CreateBreak(BreakTypeId breakType)
        {
            try
            {
                OpenBreak = await BreakService.CreateBreakForUser((await GetUserId()) ?? string.Empty, breakType);
                BreakStartTime = FormatDateTime(OpenBreak!.StartTime!.Value);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, $"Could not open break of type {breakType} for user {UserId}");
            }
            await InvokeAsync(StateHasChanged);
            return OpenBreak;
        }

        protected async Task<Break?> CloseBreak()
        {
            Break? endedBreak = null;
            try
            {
                endedBreak = await BreakService.EndCurrentBreakForUser((await GetUserId()) ?? string.Empty);
                OpenBreak = null;
                BreakStartTime = null;
                TimeOnBreakForOpenShift = await GetTimeOnBreakForOpenShift();
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, $"Could not close break {OpenBreak?.BreakId} for user {UserId}");
                ErrorState = "Could not close current break";
            }
            await InvokeAsync(StateHasChanged);
            return endedBreak;
        }

        protected async Task<Shift?> CreateShift()
        {
            try
            {
                OpenShift = await ShiftService.CreateShiftForUser((await GetUserId()) ?? string.Empty);
                ShiftStartTime = FormatDateTime(OpenShift!.StartTime!.Value);
                TimeOnBreakForOpenShift = await GetTimeOnBreakForOpenShift();
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, $"Could not open shift for user {UserId}");
            }
            await InvokeAsync(StateHasChanged);
            return OpenShift;
        }

        protected async Task<Shift?> CloseShift()
        {
            if (OpenBreak != null)
            {
                await CloseBreak();
            }

            Shift? endedShift = null;
            try
            {
                endedShift = await ShiftService.EndCurrentShiftForUser(await GetUserId() ?? string.Empty);
                OpenShift = null;
                ShiftStartTime = null;
                TimeOnBreakForOpenShift = null;
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, $"Could not close shift {OpenShift?.ShiftId} for user {UserId}");
                ErrorState = "Could not close current break";
            }
            await InvokeAsync(StateHasChanged);
            return endedShift;
        }

        protected TimeSpan? TimeOnBreakForOpenShift { get; set; }
        protected async Task<TimeSpan?> GetTimeOnBreakForOpenShift()
        {
            var breaks = await BreakService.GetBreaksForShift(OpenShift?.ShiftId ?? 0);

            var timespanTicks = breaks
                .Where(b => b.StartTime != null && b.EndTime != null)
                .Sum(b => (b!.EndTime!.Value - b!.StartTime!.Value).Ticks);

            TimeOnBreakForOpenShift = new TimeSpan(timespanTicks);

            return TimeOnBreakForOpenShift;
        }
    }
}
