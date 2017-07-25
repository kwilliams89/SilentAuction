using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SilentAuction.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}
