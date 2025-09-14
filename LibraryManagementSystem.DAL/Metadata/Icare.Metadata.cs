using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DAL
{
    public class IcareMetadata
    {
        [Key]
        [Display(Name = "İcarə ID")]
        public int IcareID { get; set; }

        [Display(Name = "İcarə Qiyməti")]
        [Range(0, 9999.99, ErrorMessage = "İcarə qiyməti 0 ilə 9999.99 arasında olmalıdır!")]
        public Nullable<decimal> IcareQiymeti { get; set; }

        [Display(Name = "İcarə Tarixi")]
        [Required(ErrorMessage = "İcarə tarixi seçilməlidir!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime IcareTarixi { get; set; }

        [Display(Name = "Son Tarix")]
        [Required(ErrorMessage = "Son tarix seçilməlidir!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime SonTarix { get; set; }

        [Display(Name = "Qaytarılma Tarixi")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> QaytarilmaTarixi { get; set; }

        [Display(Name = "Qaytarılıb")]
        public bool Qaytarilibmi { get; set; }

        [Display(Name = "İcarənin Statusu")]
        [Required(ErrorMessage = "İcarənin statusu boş buraxıla bilməz!")]
        [StringLength(20, ErrorMessage = "Status maksimum 20 simvol ola bilər!")]
        public string Statusu { get; set; } // Aktiv, Gecikir, Qaytarılıb

        [Display(Name = "İstifadəçi")]
        [Required(ErrorMessage = "İstifadəçi seçilməlidir!")]
        public int IstifadechiID { get; set; }

        [Display(Name = "Kitab")]
        [Required(ErrorMessage = "Kitab seçilməlidir!")]
        public int KitabID { get; set; }

        [Display(Name = "Cərimələr")]
        public virtual ICollection<Cerime> Cerime { get; set; }

        [Display(Name = "İstifadəçi")]
        public virtual Istifadechi Istifadechi { get; set; }

        [Display(Name = "Kitab")]
        public virtual Kitab Kitab { get; set; }
    }

    [MetadataType(typeof(IcareMetadata))]
    public partial class Icare
    {
    }
}
