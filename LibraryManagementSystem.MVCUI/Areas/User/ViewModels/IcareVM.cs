// ~/Areas/User/ViewModels/IcareVM.cs (UserController.cs-in Icarelerim.cshtml methodu üçün):

using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.MVCUI.Areas.User.ViewModels
{
    public class IcareVM
    {
        [Display(Name = "İcarə ID")]
        public int IcareID { get; set; }

        [Display(Name = "Kitab")]
        public string KitabAdi { get; set; }

        [Display(Name = "İcarə Tarixi")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime IcareTarixi { get; set; }

        [Display(Name = "Son Tarix")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime SonTarix { get; set; }

        [Display(Name = "Qaytarılma Tarixi")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? QaytarilmaTarixi { get; set; }

        [Display(Name = "Qaytarılıb")]
        public bool Qaytarilibmi { get; set; }

        [Display(Name = "İcarənin Statusu")]
        public string Statusu { get; set; } // Aktiv, Gecikir, Qaytarılıb

        [Display(Name = "İcarə Qiyməti")]
        public decimal? IcareQiymeti { get; set; }
    }
}
