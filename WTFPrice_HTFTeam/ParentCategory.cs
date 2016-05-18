using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WTFPrice_HTFTeam.Models;

namespace WTFPrice_HTFTeam
{
    public class ParentCategory
    {
        public ParentCategory(int id)
        {
            this.Id = id;
            this.Name = getParentName(id);
        }

        public int Id { get; set; }
        public string Name { get; set; }

        private string getParentName(int id)
        {
            if (id == 0)
            {
                return "Root";
            }

            var db = new WTFPriceEntities();
            string parentName = "";

            var cate = db.Categories.FirstOrDefault(x => x.Id == id);
            parentName = cate.Name;

            int pid = cate.ParentCategoryId;
            var pcate = db.Categories.FirstOrDefault(x => x.Id == pid);

            while (pcate != null)
            {
                parentName = parentName.Insert(0, pcate.Name + " >> ");
                pcate = db.Categories.FirstOrDefault(x => x.Id == pcate.ParentCategoryId);
            }

            return parentName;
        }
    }
}