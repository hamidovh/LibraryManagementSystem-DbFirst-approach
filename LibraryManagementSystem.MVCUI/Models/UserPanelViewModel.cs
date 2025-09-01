using System.Collections.Generic;
using System.Data.Entity;
using LibraryManagementSystem.DAL;
using LibraryManagementSystem.BL;

namespace LibraryManagementSystem.MVCUI.Models
{
    public class UserPanelViewModel //: LibraryManagementSystemDBEntities //: DbContext
    {
        public List<Icare> Icare { get; set; }
        public List<Cerime> Cerime { get; set; }
        public int AktivIcareSayi { get; set; }
        public int MaxIcareLimit { get; set; } = 5;
    }
}
