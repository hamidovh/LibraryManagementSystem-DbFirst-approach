using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DAL
{
    public class RolMetadata
    {
        [Key]
        public int RolID { get; set; }

        [Display(Name = "Rol Adı")]
        [Required(ErrorMessage = "Rol adı boş buraxıla bilməz!")]
        [StringLength(50, ErrorMessage = "Rol adı maksimum 50 simvol ola bilər!")]
        public string RolAdi { get; set; }

        [Display(Name = "İstifadəçilər")]
        public virtual ICollection<Istifadechi> Istifadechi { get; set; }
    }

    [MetadataType(typeof(RolMetadata))]
    public partial class Rol
    {
    }
}
