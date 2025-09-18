using LibraryManagementSystem.BL.Repositories;
using LibraryManagementSystem.DAL;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagementSystem.BL
{
    public class KitabManager : Repository<Kitab>
    {
        private Repository<Kitab> repo = new Repository<Kitab>();

        public List<Kitab> GetPaged(int page, int pageSize)
        {
            return repo.GetPaged(page, pageSize, k => k.KitabID).ToList();
        }
    }
}
