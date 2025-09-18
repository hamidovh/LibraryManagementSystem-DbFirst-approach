using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.MVCUI.Areas.User.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [Display(Name = "Adı")]
        public string Adi { get; set; }

        [Required]
        [Display(Name = "Soyadı")]
        public string Soyadi { get; set; }

        [Required]
        [Display(Name = "Doğum Tarixi")]
        [DataType(DataType.Date)]
        public DateTime DoghumTarixi { get; set; }

        [Display(Name = "Cinsi")]
        public string Cins { get; set; }

        [Required]
        [Display(Name = "FİN")]
        [StringLength(7)]
        public string FinKod { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Əlaqə Nömrəsi")]
        [StringLength(10)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Yalnız 10 rəqəm daxil edin.")]
        public string TelefonNo { get; set; }

        [Display(Name = "Ünvan")]
        public string Adres { get; set; }

        [Required]
        [Display(Name = "İstifadəçi Adı")]
        public string IstifadechiAdi { get; set; }

        [Required]
        [MinLength(6)]
        [Display(Name = "Şifrə")]
        [DataType(DataType.Password)]
        public string Shifre { get; set; }

        [Required]
        [Display(Name = "Şifrə Təsdiqi")]
        [DataType(DataType.Password)]
        [Compare("Shifre", ErrorMessage = "Şifrələr eyni olmalıdır.")]
        public string ShifreTəsdiq { get; set; }
    }
}
