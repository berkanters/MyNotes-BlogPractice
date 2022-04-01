using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.DataAccessLayer
{
    public class BaseRepository
    {
        private static MyNotesContext _db;
        private static object _lock = new object();

        public BaseRepository()
        {

        }

        public static MyNotesContext CreateContext()
        {
            if (_db == null)
            {
                lock (_lock)
                {
                    if (_db == null)
                    {
                        _db = new MyNotesContext();
                    }
                }
            }

            return _db;
        }
    }
}
