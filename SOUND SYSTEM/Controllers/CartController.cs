using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SOUND_SYSTEM.Model;
using System.Data.Entity;

namespace SOUND_SYSTEM.Controllers
{
    public class CartController : Controller
    {
        //
        // GET: /Cart/

        soundsystemEntities mdi = new soundsystemEntities();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult cart()
        {

            return View();
        }

        public ActionResult Add(string pid)
        {

            
               

            Product obj = mdi.Products.Where(p => p.Product_id==pid).SingleOrDefault();

            Session["subtotal"] = 0;
            item iobj = new item(obj, 1);




            if (Session["cart"] == null)
            {

                Session["subtotal"] = 0;

                List<item> cart = new List<item>();

               

                

                cart.Add(iobj);

                for (int i = 0; i < cart.Count; i++)
                {

                    Session["subtotal"] = (int)Session["subtotal"] + (cart[i].P.Price * cart[i].Quantity);



                }


               
               item.Total++;
                Session["cart"] = cart;
                Session["total"] = item.Total;
                
            }
            else
            {
                

                List<item> cart = (List<item>)Session["cart"];

                item.Total = (int)Session["total"];
                if ( cart.Exists(x=> x.P.Product_id==iobj.P.Product_id  ))
                {

                    item temp = cart.Find(x => x.P.Product_id == iobj.P.Product_id);

                    
                    if(temp.Quantity<temp.P.Qty)
                    {

                    cart.Find(x => x.P.Product_id == iobj.P.Product_id).Quantity++;
                    }
                    else
                    {
                        return   Redirect("/Home");
                    }
                   
                }
                else
                {
              
                    cart.Add(iobj);
                    
                }
                item.Total++;


                for (int i = 0; i < cart.Count; i++)
                {

                    Session["subtotal"] = (int)Session["subtotal"] + (cart[i].P.Price * cart[i].Quantity);



                }
               



                Session["cart"] = cart;
                Session["total"] = item.Total;
            }


            

                  return   Redirect("/Home");
        }



        public ActionResult Updatecart(FormCollection fc)
        {
            string[] quantites = fc.GetValues("quantity");

            List<item> cart = (List<item>)Session["cart"];
            Session["total"] = 0;
            for (int i = 0; i < cart.Count; i++)
            {

                cart[i].Quantity = Convert.ToInt32(quantites[i]);
                Session["total"] = (int)Session["total"] + cart[i].Quantity;
            }

            Session["subtotal"] = 0;

            for (int i = 0; i < cart.Count; i++)
            {

                Session["subtotal"] = (int)Session["subtotal"] + (cart[i].P.Price * cart[i].Quantity);



            }
            



            Session["cart"] = cart;

            return View("cart");

        }


        public ActionResult Delete(string pid)
        {

                             

            List<item> cart = (List<item>)Session["cart"];
            
            cart.RemoveAll(x => x.P.Product_id == pid);



            Session["cart"] = cart;
            Session["total"] = cart.Count;



           return View("cart");
        }

        public ActionResult total()
        {
            List<item> cart = (List<item>)Session["cart"];


            Session["subtotal"] = 0;
            for (int i = 0; i < cart.Count; i++)
            {

                Session["subtotal"] =(int)Session["subtotal"] + (cart[i].P.Price * cart[i].Quantity);



            }


            return View("cart");
        }


        public ActionResult checkout()
        {





            return View();
        }


        public ActionResult thankspage(SOUND_SYSTEM.Model.customer c)
        {
            c.status = "pending";

            mdi.customers.Add(c);

            mdi.SaveChanges();
            List<item> cart = (List<item>)Session["cart"];


           


            for (int i = 0; i < cart.Count; i++)
            {
                Order_Product obj = new Order_Product();
                obj.Product_id = cart[i].P.Product_id;
                obj.Quanitiy = cart[i].Quantity;
                obj.Orderno = c.orderno;
                obj.Price = cart[i].P.Price;
                obj.Name = cart[i].P.Name;
                obj.unitprice = cart[i].P.Price;
                
                mdi.Order_Product.Add(obj);

               
            }

            mdi.SaveChanges();






            return View();

        }
    }
}
