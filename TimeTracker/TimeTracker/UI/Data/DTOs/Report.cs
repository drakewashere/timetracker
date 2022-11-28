#nullable disable

using Microsoft.AspNetCore.Identity;

namespace UI.Data.DTOs
{
    public class Report
    {
        public IdentityUser User { get; set; }
        public Shift Shift { get; set; }
        public IEnumerable<Break> Breaks { get; set; }
    }
}
