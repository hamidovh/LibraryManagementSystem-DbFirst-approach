using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DAL
{
    public class Elaqe
    {
        public int ElaqeID { get; set; }

        [EmailAddress, StringLength(50), Required]
        public string Email { get; set; }

        [StringLength(50), Required]
        public string Adi { get; set; }

        [StringLength(50)]
        public string Soyadi { get; set; }

        [Required]
        public string Mesaj { get; set; }

        public DateTime ElaqeTarixi { get; set; }
    }
}
