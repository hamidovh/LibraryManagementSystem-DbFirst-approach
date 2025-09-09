using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DAL
{
    public partial class Muellif
    {
        [Display(Name = "Müəllif")]
        public string MuellifAdSoyadi
        {
            get
            {
                return $"{MuellifAdi} {MuellifSoyadi}";
            }
        }
    }
}
