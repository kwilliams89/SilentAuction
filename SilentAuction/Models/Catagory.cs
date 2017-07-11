using System.ComponentModel.DataAnnotations;

namespace SilentAuction.Models
{
    public class Catagory
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }
    }
}
