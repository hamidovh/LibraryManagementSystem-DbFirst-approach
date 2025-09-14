using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DAL
{
    [MetadataType(typeof(KateqoriyaMetaData))]
    public partial class Kateqoriya
    {
        // Burada heç nə yazmağa ehtiyac yoxdur.
        // Bu class sadəcə MetaData ilə əlaqələndirmək üçündür.
    }

    public class KateqoriyaMetaData
    {
        public int KateqoriyaID { get; set; }

        [Display(Name = "Kateqoriyanın Adı")]
        [Required(ErrorMessage = "Boş buraxılmamalıdır!")]
        public string KateqoriyaAdi { get; set; }

        [Display(Name = "Kateqoriya Haqqında")]
        public string KateqoriyaTesviri { get; set; }

        // Navigation property-lər
        [Display(Name = "Kitablar")]
        public virtual ICollection<Kitab> Kitab { get; set; }
    }
}
