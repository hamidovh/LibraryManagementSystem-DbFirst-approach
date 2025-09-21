using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DAL
{
    public partial class Cerime
    {
        // Bu property bazada saxlanmır, sadəcə UI-da göstərmək üçündür:
        [Display(Name = "Cərimə Məbləği (AZN)")]
        [DataType(DataType.Currency)]
        public string HesablanmisMebleg
        {
            get
            {
                // Əgər ödənilibsə, məbləği və "Ödənildi" sözünü göstər:
                if (Odenilibmi && OdenmeTarixi.HasValue)
                {
                    if (Mebleg == 0)
                        return "Ödənildi";
                    else
                        return $"{Mebleg:N2} AZN Ödənildi";
                }

                // Əks halda günə görə cərimə hesabla:
                var gunFerqi = (DateTime.Now.Date - CerimeTarixi.Date).Days;
                decimal mebleg = gunFerqi > 0 ? gunFerqi * 1 : 0; // hər gün üçün 1 AZN (standart)
                return $"{mebleg:N2} AZN";
            }
        }
    }
}
