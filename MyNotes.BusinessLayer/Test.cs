using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNotes.DataAccessLayer;

namespace MyNotes.BusinessLayer
{
    public class Test
    {
        public Test()
        {
            MyNotesContext db = new MyNotesContext();
            db.Catagories.ToList();
        }
    }
}
