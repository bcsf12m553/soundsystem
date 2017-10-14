using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using SOUND_SYSTEM.Model;
namespace SOUND_SYSTEM.Controllers
{



    public class item
    {
        private Product p = new Product();

        

       

        public Product P
        {
            get { return p; }
            set { p = value; }
        }

        private int quantity;

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }


        private static int total = 0;

        public static int Total
        {
            get { return item.total; }
            set { item.total = value; }
        } 

       public item()
       {

       }

        
       public item(Product p, int q)
       {
           this.p = p;
           this.quantity = q;
         
            



       }
    

       


    }

   










    public class HomeController : Controller
    {
        //
        // GET: /Home/
        

        soundsystemEntities mdi = new soundsystemEntities();

        

        public ActionResult category(string id)
        {
                     int count =  mdi.Products.Where(x => x.Category == id).Count();

                     ViewBag.catname = id;

                     int page = 1;
                     int pagesize = 6;
                     int skip = pagesize * (page - 1);
                                        int divide= count/pagesize;

                                        int mod = count % pagesize;

                     if (count < pagesize)
                     {
                         Session["page"]=null;
                         return View(mdi.Products.Where(x => x.Category == id).ToList());


                     }

                     
            Session["page"]=divide;
            Session["id"] = id;
            return View(mdi.Products.Where(x => x.Category == id).OrderBy(p=>p.Product_id).Skip(skip).Take(pagesize).ToList());

        }

        public ActionResult nextpage(string pg)
        {

            int pg1 = Convert.ToInt32(pg);
           
            int pagesize = 6;
            int skip = pagesize * (pg1 - 1);



            string id = (string)Session["id"];
         

            return View("category",mdi.Products.Where(x => x.Category == id).OrderBy(p => p.Product_id).Skip(skip).Take(pagesize).ToList());



        }
        public ActionResult Index()
        {
            Session["category"] = mdi.Categories.ToList();


            
            return View(mdi.Products.ToList());
        }


        public ActionResult Product()
        {

            return View();

        }

        public ActionResult Detail()
        {
            return View();

        }


        public ActionResult Login()
        {
            return View();


        }

        public ActionResult Validatesignin(SOUND_SYSTEM.Model.userdata p)
        {

            userdata si = mdi.userdatas.Where(x => x.Email == p.Email).SingleOrDefault();
            if (si != null)
            {
                if ((si.Email == p.Email) && (si.Password == p.Password))
                {
                    Session["uname"] = p.Username;

                    return Redirect("/Admin/Index");

                }
                else
                {

                    ViewBag.Error = " Incorrect Username and Password";
                    return View("login");
                }
            }
            else
                ViewBag.Error = " Incorrect Username and Password";
            return View("login");

        }


        public ActionResult signup(SOUND_SYSTEM.Model.userdata p)
        {


            userdata si = mdi.userdatas.Where(x => x.Username == p.Username).SingleOrDefault();
            if (si == null)
            {
                mdi.userdatas.Add(p);
                mdi.SaveChanges();
                Session["uname"] = p.Username;
                return Redirect("/Home/Index");
            }
            else
            {
                ViewBag.Error = "Username Already Exists ......";
                return View("signup");
            }

            
        }


        public PartialViewResult cartupdate()
        {

            
            return PartialView("_cart");
        }

        public ActionResult Product_detail(string pid)
        {



            return View(mdi.Products.Where(x=> x.Product_id==pid).ToList());
        }
        
    }
}
