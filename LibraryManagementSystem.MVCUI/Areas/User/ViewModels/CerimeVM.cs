// ~/Areas/User/ViewModels/CerimeVM.cs (UserController.cs-in Cerimelerim.cshtml methodu üçün):

using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.MVCUI.Areas.User.ViewModels
{
    public class CerimeVM
    {
        public int CerimeID { get; set; }

        [Display(Name = "Cərimələnmiş Kitab")]
        public string KitabAdi { get; set; }

        [Display(Name = "Cərimə Məbləği (AZN)")]
        [DataType(DataType.Currency)]
        public string HesablanmisMebleg { get; set; }

        public decimal Mebleg { get; set; }

        [Display(Name = "Cərimə Tarixi")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime CerimeTarixi { get; set; }

        [Display(Name = "Ödənilibmi")]
        public bool Odenilibmi { get; set; }

        [Display(Name = "Ödənmə Tarixi")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? OdenmeTarixi { get; set; }

        [Display(Name = "Səbəb")]
        public string Sebeb { get; set; }
    }
}
