using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DAL
{
    public class ElaqeMetadata
    {
        [Key]
        public int ElaqeID { get; set; }

        [EmailAddress, StringLength(50), Required]
        public string Email { get; set; }

        [StringLength(50), Required]
        public string Adi { get; set; }

        [StringLength(50)]
        public string Soyadi { get; set; }

        [Required]
        public string Mesaj { get; set; }

        public System.DateTime ElaqeTarixi { get; set; }
    }

    [MetadataType(typeof(ElaqeMetadata))]
    public partial class Elaqe
    {
    }
}
