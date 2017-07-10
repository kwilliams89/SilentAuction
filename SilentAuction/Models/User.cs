using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SilentAuction.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "First name cannot be longer than 20 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Last name cannot be longer than 20 characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "E-mail cannot be longer than 40 characters.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [StringLength(11, ErrorMessage = "Phone cannot be longer than 11 characters.")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Password cannot be longer than 20 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public UserType Type { get; set; }

        [DataType(DataType.Currency)]
        public decimal AutoBidAmt { get; set; }

        // Has Many
        public ICollection<BidHistory> BidHistories { get; set; }
    }
}
