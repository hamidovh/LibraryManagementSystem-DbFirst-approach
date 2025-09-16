using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DAL.Metadata
{
    public class SliderMetadata
    {
        [Key]
        public int SliderID { get; set; }

        [Display(Name = "Başlıq")]
        [Required(ErrorMessage = "Başlıq daxil edilməlidir!")]
        [StringLength(255, ErrorMessage = "Başlıq maksimum 255 simvol ola bilər!")]
        public string Basliq { get; set; }

        [Display(Name = "Açıqlama")]
        [StringLength(4000)]
        public string Achiqlama { get; set; }

        [Url]
        [Display(Name = "Link")]
        public string Link { get; set; }

        [Display(Name = "Şəkil")]
        public string Sekil { get; set; }
    }
}
