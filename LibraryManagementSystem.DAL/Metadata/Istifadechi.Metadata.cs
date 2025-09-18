using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DAL
{
    [MetadataType(typeof(IstifadechiMetaData))]
    public partial class Istifadechi
    {
        // Burada heç nə yazmağa ehtiyac yoxdur.
        // Sadəcə MetadataType attributu ilə meta class-a bağlanır.
    }

    public class IstifadechiMetaData
    {
        public int IstifadechiID { get; set; }

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
        public Nullable<DateTime> DoghumTarixi { get; set; }

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
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Əlaqə nömrəsi 10 rəqəmdən ibarət olmalıdır!")]
        [MinLength(10, ErrorMessage = "Əlaqə nömrəsi minimum 10 rəqəmdən ibarət ola bilər!")]
        [StringLength(10, ErrorMessage = "Əlaqə nömrəsi maksimum 10 rəqəmdən ibarət ola bilər!")]
        public string TelefonNo { get; set; }

        [Display(Name = "Ünvan")]
        public string Adres { get; set; }

        [Display(Name = "İstifadəçi Adı")]
        [Required(ErrorMessage = "İstifadəçi Adı təyin olunmalıdır!")]
        public string IstifadechiAdi { get; set; }

        [Display(Name = "Şifrə")]
        [MinLength(6, ErrorMessage = "Şifrə minimum 6 simvoldan ibarət olmalıdır!")]
        [Required(ErrorMessage = "Şifrə mütləqdir!")]
        public string Shifre { get; set; }

        [Display(Name = "Aktivdir")]
        public bool Aktivdirmi { get; set; }

        [Display(Name = "Qeydiyyat Tarixi")]
        public Nullable<DateTime> QeydiyyatTarixi { get; set; }

        [Display(Name = "Rolu")]
        [Required(ErrorMessage = "İstifadəçinin rolunu təyin edin!")]
        public int RolID { get; set; }

        // Navigation property-lər
        [Display(Name = "Cərimələr")]
        public virtual ICollection<Cerime> Cerime { get; set; }

        [Display(Name = "İcarələr")]
        public virtual ICollection<Icare> Icare { get; set; }

        [Display(Name = "Rol")]
        public virtual Rol Rol { get; set; }
    }
}
