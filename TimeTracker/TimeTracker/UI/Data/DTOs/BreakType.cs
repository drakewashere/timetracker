#nullable disable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UI.Data.DTOs
{
    public class BreakType
    {
        [Key]
        public int BreakTypeId { get; set; }
        public string BreakTypeName { get; set; }

        public virtual IEnumerable<Break> Breaks { get; set; }
    }
}
