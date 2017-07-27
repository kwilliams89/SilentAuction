using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilentAuction.Models
{
    public class ItemMedia
    {
        [Key]
        [Column(Order = 0)]
        public int ItemId { get; set; }

        public virtual Item Item { get; set; }

        [Key]
        [Column(Order = 1)]
        public int MediaId { get; set; }

        public virtual Media Media { get; set; }
    }
}
