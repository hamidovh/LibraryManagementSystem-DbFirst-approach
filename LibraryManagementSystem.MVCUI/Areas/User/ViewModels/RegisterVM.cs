// ~/Areas/User/ViewModels/RegisterVM.cs (AccountController.cs-in Register.cshtml [HttpPost] methodu və UserController.cs-in EditProfile.cshtml [HttpPost] methodu üçün):

using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.MVCUI.Areas.User.ViewModels
{
    public class RegisterVM
    {
        [Display(Name = "Adı")]
        [Required(ErrorMessage = "Ad mütləqdir!")]
        public string Adi { get; set; }
                
        [Display(Name = "Soyadı")]
        [Required(ErrorMessage = "Soyad mütləqdir!")]
        public string Soyadi { get; set; }

        [Display(Name = "Doğum Tarixi")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Doğum tarixini daxil edin!")]
        [DataType(DataType.Date)]
        public DateTime? DoghumTarixi { get; set; } //sonradan nullable edildi

        [Display(Name = "Cinsi")]
        public string Cins { get; set; }

        [Display(Name = "FİN")]
        [Required(ErrorMessage = "FİN mütləqdir!")]
        [MinLength(7, ErrorMessage = "FİN kod minimum 7 simvoldan ibarət ola bilər!")]
        [StringLength(7, ErrorMessage = "FİN kod maksimum 7 simvoldan ibarət ola bilər!")]
        public string FinKod { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email mütləqdir!")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email formata uyğun deyil!")]
        public string Email { get; set; }

        [Display(Name = "Əlaqə Nömrəsi")]
        [RegularExpression(@"^\d{10}$")]
        [MinLength(10, ErrorMessage = "Əlaqə nömrəsi minimum 10 rəqəmdən ibarət ola bilər!")]
        [StringLength(10, ErrorMessage = "Əlaqə nömrəsi maksimum 10 rəqəmdən ibarət ola bilər!")]
        public string TelefonNo { get; set; }

        [Display(Name = "Ünvan")]
        public string Adres { get; set; }

        [Display(Name = "İstifadəçi Adı")]
        [Required(ErrorMessage = "İstifadəçi Adı təyin olunmalıdır!")]
        public string IstifadechiAdi { get; set; }

        [Display(Name = "Şifrə")]
        [MinLength(6)]
        [Required(ErrorMessage = "Şifrə minimum 6 simvoldan ibarət olmalıdır!")]
        [DataType(DataType.Password)]
        public string Shifre { get; set; }

        [Display(Name = "Şifrə Təsdiqi")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifrə Təsdiqi Tələb Olunur!")]
        [Compare("Shifre", ErrorMessage = "Şifrələr eyni olmalıdır.")]
        public string ShifreTesdiq { get; set; }
    }
}
