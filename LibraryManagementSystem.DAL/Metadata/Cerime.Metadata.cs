using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DAL
{
    public class CerimeMetadata
    {
        [Key]
        public int CerimeID { get; set; }

        [Display(Name = "Cərimənin Məbləği")]
        [Range(0, 9999.99, ErrorMessage = "Cərimə məbləği 0 ilə 9999.99 arasında olmalıdır!")]
        public decimal Mebleg { get; set; }

        [Display(Name = "Ödənilibmi")]
        public bool Odenilibmi { get; set; }

        [Display(Name = "Cərimələnmə Tarixi")]
        [Required(ErrorMessage = "Cərimələnmə tarixini qeyd edin!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime CerimeTarixi { get; set; }

        [Display(Name = "Ödənmə Tarixi")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> OdenmeTarixi { get; set; }

        [Display(Name = "Cərimələnmə Səbəbi")]
        [Required(ErrorMessage = "Səbəb boş buraxıla bilməz!")]
        [StringLength(100, ErrorMessage = "Səbəb maksimum 100 simvol ola bilər!")]
        public string Sebeb { get; set; } // "Gecikir", "İtirilib"

        [Display(Name = "İstifadəçi")]
        [Required(ErrorMessage = "İstifadəçi seçilməlidir!")]
        public int IstifadechiID { get; set; }

        [Display(Name = "İcarə")]
        [Required(ErrorMessage = "İcarə seçilməlidir!")]
        public int IcareID { get; set; }

        [Display(Name = "İcarə")]
        public virtual Icare Icare { get; set; }

        [Display(Name = "İstifadəçi")]
        public virtual Istifadechi Istifadechi { get; set; }
    }

    [MetadataType(typeof(CerimeMetadata))]
    public partial class Cerime
    {
    }
}
