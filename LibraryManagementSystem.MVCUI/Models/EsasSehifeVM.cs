// ~/Models/EsasSehifeVM.cs:

using System.Collections.Generic;

namespace LibraryManagementSystem.MVCUI.Models
{
    public class EsasSehifeVM
    {
        public List<LibraryManagementSystem.DAL.Slider> Sliders { get; set; }
        public List<LibraryManagementSystem.DAL.Kitab> Kitablar { get; set; }

        // Paging üçün lazımdır:
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
