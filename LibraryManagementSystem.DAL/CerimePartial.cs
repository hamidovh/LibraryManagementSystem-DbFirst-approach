using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DAL
{
    public partial class Cerime
    {
        // Bu property bazada saxlanmır, sadəcə UI-da göstərmək üçündür:
        [Display(Name = "Cərimə Məbləği (AZN)")]
        public decimal HesablanmisMebleg
        {
            get
            {
                if (CerimeTarixi == null) return 0;
                var gunFerqi = (DateTime.Now.Date - CerimeTarixi.Date).Days;
                return gunFerqi > 0 ? gunFerqi * 1 : 0; // hər gün üçün 1 AZN
            }
        }
    }
}
