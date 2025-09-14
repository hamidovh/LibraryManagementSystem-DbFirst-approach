using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DAL
{
    [MetadataType(typeof(KitabMetaData))]
    public partial class Kitab
    {
        // Burada heç nə yazmağa ehtiyac yoxdur,
        // sadəcə MetadataType attributu ilə MetaData class-a bağlayırıq.
    }

    public class KitabMetaData
    {
        public int KitabID { get; set; }

        [Display(Name = "Kitab")]
        [Required(ErrorMessage = "Boş buraxılmamalıdır!")]
        public string KitabAdi { get; set; }

        [Display(Name = "İcarə Qiyməti")]
        public Nullable<decimal> IcareQiymeti { get; set; }

        [Display(Name = "Stokda Var")]
        public bool StokdaVarmi { get; set; }

        [Display(Name = "Haqqında")]
        public string Haqqinda { get; set; }

        // Navigation property-lər
        [Display(Name = "Kateqoriya")]
        [Required(ErrorMessage = "Kateqoriya təyin edin!")]
        public virtual ICollection<Kateqoriya> Kateqoriya { get; set; }

        [Display(Name = "Müəllif")]
        [Required(ErrorMessage = "Müəllif təyin edin!")]
        public virtual ICollection<Muellif> Muellif { get; set; }

        // Yeni əlavə olunacaq sütun (məsələn foto yolu və ya binary)
        //[Display(Name = "Kitab Fotosu")]
        [Display(Name = "Kitab")]
        public string Foto { get; set; }  // əgər DB-də nvarchar və ya varbinary kimi saxlayacaqsansa uyğun tip seç
    }
}
