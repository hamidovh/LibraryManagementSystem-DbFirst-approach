using LibraryManagementSystem.DAL;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace LibraryManagementSystem.MVCUI.Areas.Admin.ViewModels
{
    public class KitabViewModel
    {
        public int KitabID { get; set; }

        [Display(Name = "Kitab")]
        [Required(ErrorMessage = "Boş buraxılmamalıdır!")]
        public string KitabAdi { get; set; }

        [Display(Name = "Haqqında")]
        public string Haqqinda { get; set; }

        [Display(Name = "İcarə Qiyməti")]
        public decimal? IcareQiymeti { get; set; }

        [Display(Name = "Stokda Var")]
        public bool StokdaVarmi { get; set; }

        [Display(Name = "Müəllif")]
        [Required(ErrorMessage = "Müəllif təyin edin!")]
        public int[] sechilmishMuellifIDler { get; set; }

        [Display(Name = "Kateqoriya")]
        [Required(ErrorMessage = "Kateqoriya təyin edin!")]
        public int[] sechilmishKateqoriyaIDler { get; set; }

        public MultiSelectList MuellifList { get; set; }
        public MultiSelectList KateqoriyaList { get; set; }

        //Silmə əməliyyatı üçün:
        public string sechilmishMuellifAdi { get; set; }
        public string sechilmishKateqoriyaAdi { get; set; }

    }
}
