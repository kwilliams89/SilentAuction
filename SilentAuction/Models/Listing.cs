using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilentAuction.Models
{
    public class Listing
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Auction")]
        [Display(Name = "Auction")]
        public int AuctionId { get; set; }

        public Auction Auction { get; set; }

        [Required]
        [ForeignKey("Item")]
        [Display(Name = "Item")]
        public int ItemId { get; set; }

        public Item Item { get; set; }

        [Required]
        [Display(Name = "Starting Bid")]
        [DataType(DataType.Currency)]
        public decimal StartingBid { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Increment { get; set; }
    }
}
