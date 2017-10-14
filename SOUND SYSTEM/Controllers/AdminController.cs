using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using SOUND_SYSTEM.Model;

namespace SOUND_SYSTEM.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        soundsystemEntities mdi = new soundsystemEntities();


        public ActionResult Index()
        {
            Session["products"] = null;
            
            ViewBag.ordercount = mdi.customers.Count();



            return View();
        }


      

        public ActionResult AddProducttodb(SOUND_SYSTEM.Model.Product p)
        {

            mdi.Products.Add(p);

            return View();
        }

        public ActionResult Manage()
        {




            return View(mdi.Categories.ToList());
        }


        public ActionResult order()
        {


           


            return View(mdi.customers.ToList());
        }
    
        public ActionResult addproduct(SOUND_SYSTEM.Model.Product p )
        {

            Category obj = mdi.Categories.Where(x => x.Name == p.Category).SingleOrDefault();


            obj.Qty++;


    

            mdi.Products.Add(p);

                   mdi.SaveChanges();



            for (int i = 0; i < Request.Files.Count; i++)
            {

                string s = i.ToString();


                string concat = p.Name + s;
                HttpPostedFileBase file1 = Request.Files[i];
                file1.SaveAs(Server.MapPath(@"~\images\product\"+concat+".jpg"));

            }





            return View("Manage", mdi.Categories.ToList());

        }

        public PartialViewResult product()
        {

            Session["category"] = mdi.Categories.ToList();

            List<Product> p = mdi.Products.ToList(); 

            return PartialView("product",p);
        }

        public PartialViewResult corder()
        {


            Session["ptotal"] = 0;           
            return PartialView("corder", mdi.customers.ToList());
        }


        public PartialViewResult co(int id)
        {

            ViewBag.name = mdi.Order_Product.Where(x => x.Orderno == id).ToList();


            Session["ptotal"] = 0;
            foreach (Order_Product obj in (List<SOUND_SYSTEM.Model.Order_Product>)ViewBag.name)
            {

                Session["ptotal"] = (int)Session["ptotal"] + (obj.Price * obj.Quanitiy);


            }

            return PartialView("corder", mdi.customers.ToList());

        }


        public JsonResult deleteorder(string Orderno)
        {
                           int y=      Convert.ToInt32(Orderno);
          customer    obj  = mdi.customers.Where(x => x.orderno ==y ).SingleOrDefault();
          List<Order_Product> temp = mdi.Order_Product.Where(x => x.Orderno == y).AsEnumerable().ToList();
           
            
            mdi.customers.Remove(obj);

            mdi.Order_Product.RemoveRange(temp);

            return Json(mdi.SaveChanges(), JsonRequestBehavior.AllowGet);

        }

        public JsonResult fillvalues(int id)
        {

            var c=   mdi.customers.Find(id);




         return Json(new
         {

             fname = c.fname,
             sname=c.sname,
              address= c.address,
                   city =   c.city,
               email=    c.email,
               mobile=c.mobile,
              province= c.province,
              orderno= c.orderno,
             status=c.status,


         }, JsonRequestBehavior.AllowGet);


        }
      

       
        public JsonResult Update(Product p)
        {


            Product obj = mdi.Products.Where(x => x.Product_id == p.Product_id).SingleOrDefault() ;

            mdi.Products.Remove(obj);

            mdi.Products.Add(p);


            return Json(mdi.SaveChanges(), JsonRequestBehavior.AllowGet);

        }

        public JsonResult statusupdate(customer c)
        {



             mdi.customers.Where(x => x.orderno == c.orderno).Single().status=c.status;
      

            mdi.SaveChanges();
            return Json(new
            {

                order =c.orderno,
                status = c.status,


            }, JsonRequestBehavior.AllowGet);


        }
        public JsonResult delete(string id)
        {

            Product obj = mdi.Products.Where(x => x.Product_id == id).SingleOrDefault();


            mdi.Products.Remove(obj);
            mdi.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);

        }
        public JsonResult addimage()
        {


            for (int i = 0; i < Request.Files.Count; i++)
            {

                string s = i.ToString();
               
                            
                string concat = Session["name"] + s;
                HttpPostedFileBase file1 = Request.Files[i];
                file1.SaveAs(Server.MapPath(@"~\images\product\" + concat + ".jpg"));

            }


            return Json(1, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetbyID(string ID)
        {

            var p = mdi.Products.Find(ID);


            return Json(new
            {
          
                 Name=p.Name,
                  Price =    p.Price,
                            Qty =    p.Qty,
              
                                   Product_id=     p.Product_id,
                                   
                                  category= p.Category,
              
                                  Description=p.Description,

               } , JsonRequestBehavior.AllowGet );

        }
        public JsonResult add(Product p)
        {

           Category obj = mdi.Categories.Where(x => x.Name == p.Category).SingleOrDefault();


            obj.Qty++;
            mdi.Products.Add(p);
            mdi.SaveChanges();
            Session["name"] = p.Name;
           

            return Json (true, JsonRequestBehavior.AllowGet);

        }

    }
}
