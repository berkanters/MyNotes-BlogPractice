using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNotes.BusinessLayer.Abstract;
using MyNotes.BusinessLayer.ValueObject;
using MyNotes.DataAccessLayer;
using MyNotes.EntityLayer;

namespace MyNotes.BusinessLayer
{
    public class CategoryManager:ManagerBase<Category>
    {
        public Repository<Category> repo = new Repository<Category>();
        public List<Category> IndexCat()
        {
            return repo.List();
        }
        public int InsertCat(CategoryViewModel cat)
        {
            Category entity = new Category();
            entity.Title = cat.Category.Title;
            entity.Description = cat.Category.Description;
            entity.CreatedOn = null;
            entity.ModifiedUserName = null;
            entity.ModifiedOn = null;
            return repo.Insert(entity);
           
        }
        public CategoryViewModel FindCat(int? id)
        {
            var cat = repo.QList().FirstOrDefault(x => x.Id == id);
            CategoryViewModel cmv = new CategoryViewModel();
            cmv.Category.Id = cat.Id;
            cmv.Category.Title = cat.Title;
            cmv.Category.Description = cat.Description;
            cmv.Category.CreatedOn = cat.CreatedOn;
            cmv.Category.ModifiedUserName = cat.ModifiedUserName;
            cmv.Category.ModifiedOn = cat.ModifiedOn;
            return cmv;
        }
    }
}
