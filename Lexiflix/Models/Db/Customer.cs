using System.ComponentModel.DataAnnotations;

namespace Lexiflix.Models.Db
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Billing Address")]
        public string BillingAddress { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Billing City")]
        public string BillingCity { get; set; }
        [Required]
        [StringLength(10)]
        [Display(Name = "Billing Postal Code")]
        public string BillingZip { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Delivery Address")]
        public string DeliveryAddress { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Delivery City")]
        public string DeliveryCity { get; set; }
        [Required]
        [StringLength(10)]
        [Display(Name = "Delivery postal code")]
        public string DeliveryZip { get; set; }

        [Required]
        [StringLength(255)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [StringLength(20)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        //many to many realtionship with order
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}

