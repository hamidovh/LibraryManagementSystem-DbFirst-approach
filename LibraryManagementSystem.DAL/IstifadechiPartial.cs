using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DAL
{
    public partial class Istifadechi
    {
        [Display(Name = "İstifadəçi")]
        public string AdSoyadi
        {
            get
            {
                return $"{Adi} {Soyadi}";
            }
        }
    }
}
