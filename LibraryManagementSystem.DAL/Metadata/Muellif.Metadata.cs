using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DAL
{
    [MetadataType(typeof(MuellifMetaData))]
    public partial class Muellif
    {
        // Burada heç nə yazmağa ehtiyac yoxdur.
        // Sadəcə MetadataType ilə bağlayırıq.
    }

    public class MuellifMetaData
    {
        public int MuellifID { get; set; }

        [Display(Name = "Müəllifin Adı")]
        [Required(ErrorMessage = "Ad mütləqdir!")]
        public string MuellifAdi { get; set; }

        [Display(Name = "Müəllifin Soyadı")]
        [Required(ErrorMessage = "Soyad mütləqdir!")]
        public string MuellifSoyadi { get; set; }

        [Display(Name = "Müəllifin Doğum Tarixi")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public Nullable<DateTime> MuellifinDoghumTarixi { get; set; }

        // Navigation property
        [Display(Name = "Kitablar")]
        public virtual ICollection<Kitab> Kitab { get; set; }
    }
}
