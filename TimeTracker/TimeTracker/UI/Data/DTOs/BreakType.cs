#nullable disable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UI.Data.DTOs
{
    public enum BreakTypeId : int
    {
        Break = 1,
        Lunch = 2,
    }

    public class BreakType
    {
        [Key]
        public BreakTypeId BreakTypeId { get; set; }
        public string Name { get; set; }

        public virtual IEnumerable<Break> Breaks { get; set; }
    }
}
