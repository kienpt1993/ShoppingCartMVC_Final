using ShopingCartEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

 
    public class PCMenu
    {

        static ShoppingCartEntities db = new ShoppingCartEntities();
       public static string ChildCategory(int parentID)
        {
           string str = null;
           
           return str;
       }

        static bool HasChild(Category cat, IEnumerable<Category>db)
       {
           return db.Any(x => x.ParentID.Value.Equals(cat.CategoryID));
       }

        public static string Category()
        {
            string str = null;
            var myDb =  db.Categories;

            foreach (var item in myDb.Where(x => x.ParentID.Value == 0))
            {
                str += "  <div class=\"panel panel-default\">";
                str += "               <div class=\"panel-heading\">";
                str += "            <h4 class=\"panel-title\">";

                str += "                   <a data-toggle=\"collapse\" data-parent=\"#accordian\" href=\"#cate_"+@item.CategoryID+"\">";

                if (HasChild(item, myDb))
                {
                    str += "                       <span class=\"badge pull-right\"><i class=\"fa fa-plus\"></i></span>";


                    str += item.Name;
                    str += "               </a>";
                    str += "            </h4>";
                    str += "         </div>";

                    foreach (var item2 in myDb.Where(x => x.ParentID.Value == item.CategoryID))
                    {


                        str += "         <div id=\"cate_" + item.CategoryID + "\" class=\"panel-collapse collapse\">";
                        str += "           <div class=\"panel-body\">";
                        str += "               <ul>";
                        str += "                   <li><a href=\"/Product/Category/"+item.CategoryID+"\">";
                        str += item2.Name;
                        str += "</a></li>";

                        str += "               </ul>";
                        str += "           </div>";
                        str += "       </div>";

                    }

                }

                else
                {

                    str += item.Name;
                    str += "               </a>";
                    str += "            </h4>";
                    str += "         </div>";
                }
              
                str += "   </div>";


            }


            return str;

        }
    }
