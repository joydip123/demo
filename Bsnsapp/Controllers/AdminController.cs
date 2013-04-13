using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raven.Client;
using Newtonsoft.Json;
using Bsnsapp.Models;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;

namespace Bsnsapp.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        private readonly IDocumentSession _documentSession;
        public AdminController(IDocumentSession documentSession)
        {
             _documentSession = documentSession;
            
        }
        [ActionName("Login")]
        public ActionResult Login()
        {
            var model = new LoginPage();
            return View(model);
        }

        [ActionName("Business")]
        public ActionResult BusinessList(String message)
        {
            if (Session["user"] != null)
            {
                var mov = _documentSession.Query<Vendor>()
                    .Customize(x => x.WaitForNonStaleResults())
                    .Take(50).ToList();
                return View(mov);
            }
            else
            {
                // return View(adcl);
                return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
            }

        }
        private List<SelectListItem> GetManufacturerList()
        {
            List<SelectListItem> manuList = new List<SelectListItem>();
            manuList = (from categories in _documentSession.Query<Category>()
                        select
                            new SelectListItem
                            {
                                //Value = categories.Id.ToString(),
                                Text = categories.Categories
                            }
                      ).Distinct().ToList();
            return manuList;
        }
        public ActionResult CreateBusiness(FormCollection collection)
        {
            if (Session["user"] != null)
            {
                Vendor vd = new Vendor();
                vd.Manufacturers = GetManufacturerList();
                vd.StateModel = new List<State1>();
                vd.StateModel = GetAllState1();
                return View(vd);
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }

        //
        // POST: /Admin/Create

        [HttpPost]
        public ActionResult CreateBusiness(Vendor vndr)
        {
            if (Session["user"] != null)
            {
                if (!ModelState.IsValid)
                    //return View();

                vndr.Country = "";
                List<State1> objCountry = new List<State1>();
                objCountry = GetAllState1().Where(m => m.Id.ToString() == Request.Form["Country"]).ToList();
                var objCoun = objCountry[0].StateName;
                vndr.Country = objCoun;
                _documentSession.Store(vndr);
                _documentSession.SaveChanges();
                return RedirectToAction("Business", new { message = string.Format("Business Added Successfully", vndr.Id) });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }
        public ActionResult EditBusiness(int id)
        {
            if (Session["user"] != null)
            {
                var bsn = _documentSession.Load<Vendor>(id);
                if (bsn == null)
                    return RedirectToAction("Business", new { message = string.Format("Business {0} not found", id) });

                return View(bsn);
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }

        //
        // POST: /Admin/Edit/5

        [HttpPost]
        public ActionResult EditBusiness(Vendor bsn)
        {
            if (Session["user"] != null)
            {
                if (!ModelState.IsValid)


                    _documentSession.Store(bsn);
                _documentSession.SaveChanges();
                return RedirectToAction("Business", new { message = string.Format("Saved changes to Business {0}", bsn.Id) });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }
        [ActionName("DetailsBusiness")]
        public ActionResult DetailsBusiness(int id)
        {
            if (Session["user"] != null)
            {
                var bsn = _documentSession.Load<Vendor>(id);
                if (bsn == null)
                    return RedirectToAction("Business", new { message = string.Format("Business {0} not found", id) });
                return View(bsn);
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }
         public ActionResult DeleteBusiness(int id)
         {
             if (Session["user"] != null)
             {
                 var move = _documentSession.Load<Vendor>(id);
                 if (move == null)
                     return RedirectToAction("Business", new { message = string.Format("Business {0} not found", id) });
                 return View(move);
             }
             else
             {
                 return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
             }
         }

         //
         // POST: /Vendor/Delete/5

         [HttpPost]
         public ActionResult DeleteBusiness(int id, FormCollection collection)
         {

             if (Session["user"] != null)
             {
                 _documentSession.Delete(_documentSession.Load<Vendor>(id));
                 _documentSession.SaveChanges();
                 return RedirectToAction("Business", new { message = string.Format("Delete mov with the id {0}", id) });
             }
             else
             {
                 return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
             }
            
         }
        
       
        /// <summary>
        /// /////////////////////////////////////////////////////Category/////////////
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>

        [ActionName("Category")]
        public ActionResult CategoryList(String message)
        {
            if (Session["user"] != null)
            {
                var cat = _documentSession.Query<Category>()
                    .Customize(x => x.WaitForNonStaleResults())
                    .Take(50).ToList();
                return View(cat);
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }
        [HttpPost]
        public ActionResult Login(AdminLoginPage admn)
        {
            
            if (admn.UserID == "Admin" && admn.Password == "admin")
            {
                Session["user"] = "Admin";
               // Session["user1"] = "us";
                return RedirectToAction("Users", "Admin", new { message = string.Format("Logged In As {0}", admn.UserID) });
            }
            else
            {
                return View();
            }
        }
        [ActionName("LogOff")]
        public ActionResult LogOff()
        {
            //var model = new LoginPage();
            Session.Abandon();            
            if (Session["user"] != null)
            {
               // return View();
                return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
            
        }

        //[ActionName("DetailsCat")]
        public ActionResult DetailsCat(int id)
        {
            if (Session["user"] != null)
            {
                var cat = _documentSession.Load<Category>(id);
                if (cat == null)
                    return RedirectToAction("Category", new { message = string.Format("cat {0} not found", id) });
                return View(cat);
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }

        //
        // GET: /Admin/Create

        public ActionResult CreateCat(FormCollection collection)
        {
            if (Session["user"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        } 

        //
        // POST: /Admin/Create

        [HttpPost]
        public ActionResult CreateCat(Category cat)
        {
            if (Session["user"] != null)
            {
                if (!ModelState.IsValid)
                    return View();
                _documentSession.Store(cat);
                _documentSession.SaveChanges();
                return RedirectToAction("Category", new { message = string.Format("Category Successfully Created", cat.Categories) });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }
        
        //
        // GET: /Admin/Edit/5
 
        public ActionResult EditCat(int id)
        {
            if (Session["user"] != null)
            {
                var cat = _documentSession.Load<Category>(id);
                if (cat == null)
                    return RedirectToAction("Category", new { message = string.Format("cat {0} not found", id) });

                return View(cat);
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }

        //
        // POST: /Admin/Edit/5

        [HttpPost]
        public ActionResult EditCat(Category cat)
        {
            if (Session["user"] != null)
            {
                _documentSession.Store(cat);
                _documentSession.SaveChanges();
                return RedirectToAction("Category", new { message = string.Format("Saved changes to cat {0}", cat.Id) });
            }
            else
            {
                return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }

        //
        // GET: /Admin/Delete/5
        [HttpPost]
        public ActionResult DeleteCat(int id, FormCollection collection)
        {
            if (Session["user"] != null)
            {
                _documentSession.Delete(_documentSession.Load<Category>(id));
                _documentSession.SaveChanges();
                return RedirectToAction("Category", new { message = string.Format("Delete cat with the id {0}", id) });
            }
            else
            {
                return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
            //if(Session["user"]!=null)
            //{
            //    // TODO: Add delete logic here
               

            //    return RedirectToAction("Category");
            //}
            //else
            //{
            //    return RedirectToAction("Login", "Admin", new { message = string.Format("Please Login Again {0}", "Admin") });
            //}
        }

        //
        // POST: /Admin/Delete/5

        
        public ActionResult DeleteCat(int id)
        {
            if (Session["user"] != null)
            {
                var move = _documentSession.Load<Category>(id);
                if (move == null)
                    return RedirectToAction("Category", new { message = string.Format("Delete cat with the id {0}", id) });
                return View(move);
            }
            else
            {
                return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
            
            //if (Session["user"] != null)
            //{
            //    _documentSession.Delete(_documentSession.Load<Category>(id));
            //    _documentSession.SaveChanges();
            //    return RedirectToAction("Category", new { message = string.Format("Delete cat with the id {0}", id) });
            //}
            //else
            //{
            //    return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
            //}
        }



        /////////////////////////////////////////////////User//////////////////////////


        [ActionName("Users")]
        public ActionResult UserList(String message)
        {
            if (Session["user"] != null)
            {
                var mov = _documentSession.Query<Mov>()
                    .Customize(x => x.WaitForNonStaleResults())
                    .Take(50).ToList();
                return View(mov);
            }
            else
            {
                // return View(adcl);
                return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
            }



        }
        public ActionResult DetailsUser(int id)
        {
            //var mov=_documentSession.Query<>;
            if (Session["user"] != null)
            {
                var mov = _documentSession.Load<Mov>(id);
                if (mov == null)
                    return RedirectToAction("Users", new { message = string.Format("mov {0} not found", id) });
                return View(mov);
            }
            else
            {
                return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }

        public ActionResult CreateUser(FormCollection collection)
        {
            if (Session["user"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
            }

        }

        [HttpPost]
        public ActionResult CreateUser(Mov move)
        {
            if (Session["user"] != null)
            {
                if (!ModelState.IsValid)
                    return View();
                _documentSession.Store(move);
                _documentSession.SaveChanges();
                Sendmail(move.Email, move.Name);
                return RedirectToAction("Users", new { message = string.Format("Created User {0}", move.Email) });
            }
            else
            {
                return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }
        public ActionResult EditUser(int id)
        {
            if (Session["user"] != null)
            {
                var move = _documentSession.Load<Mov>(id);
                if (move == null)
                    return RedirectToAction("Users", new { message = string.Format("Mov {0} not found", id) });

                return View(move);
            }
            else
            {
                return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }
        [HttpPost]
        public ActionResult EditUser(Mov move)
        {
            if (Session["user"] != null)
            {
                if (!ModelState.IsValid)
                    _documentSession.Store(move);
                _documentSession.SaveChanges();
                return RedirectToAction("Users", new { message = string.Format("Saved changes to mov {0}", move.Id) });
            }
            else
            {
                return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
            }


        }

        [ActionName("DeleteUser")]
        public ActionResult ConfirmDelete(int id)
        {
            if (Session["user"] != null)
            {
                var move = _documentSession.Load<Mov>(id);
                if (move == null)
                    return RedirectToAction("Users", new { message = string.Format("move {0} not found", id) });
                return View(move);
            }
            else
            {
                return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
            }
        }

        [HttpPost]
        public ActionResult DeleteUser(int id)
        {
            if (Session["user"] != null)
            {
                _documentSession.Delete(_documentSession.Load<Mov>(id));
                _documentSession.SaveChanges();
                return RedirectToAction("Users", new { message = string.Format("Delete mov with the id {0}", id) });
            }
            else
            {
                return RedirectToAction("Login", new { message = string.Format("Please Login Again {0}", "Admin") });
            }

        }
        public void Sendmail(string Email, string U_Id)
        {
            String Pass = GetUniqueKey();
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("abc@gmail.com");
            msg.To.Add(Email);
            msg.Subject = "Registration successfull.";
            // msg.Body = "Your user Id :" + U_Id + " and Password :" + Pass;
            msg.Body = "Your registration is successfull.";
            msg.Priority = MailPriority.High;
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            // smtp.Credentials = new NetworkCredential("email@gmail.com", "password");
            client.Credentials = new NetworkCredential("globalcoders@gmail.com", "coders12345");
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Send(msg);

        }
        public static string GetUniqueKey()
        {

            int maxSize = 8;
            char[] chars = new char[62];
            string a = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];

            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();

            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }
            return result.ToString();
        }

        ///////////////////////////////////Change Password/////////////////////////////

        [ActionName("ChangePassword")]
        public ActionResult ChangePassword(int id)
        {


            var move = _documentSession.Load<Mov>(id);
            if (move == null)
                return RedirectToAction("ChangePassword", new { message = string.Format("Password Change For{0}", move.Id) });

            return View(move);


        }
        [HttpPost]
        public ActionResult ChangePassword(Mov move,int id)
        {

           // var log2 = _documentSession.Load<ChangePassword>(chng.UserId);
            var log1 = _documentSession.Query<Mov>()
                .Where(x => x.Email == move.Email && x.Password == Request["Name0"])
                .Take(1).ToList();
          
            if (log1.Count > 0)
            {
                if (!ModelState.IsValid)

                    move.Password = "";
                    move.Id = 0;
                    move.Password = Request["Name1"];
                    move.RetypePassword = Request["Name2"];

                _documentSession.Delete(_documentSession.Load<Mov>(id));
                _documentSession.SaveChanges();
                _documentSession.Store(move);
                _documentSession.SaveChanges();
                return RedirectToAction("Users", new { message = string.Format("Password Successfully Changed")});
            }

            else
            {
                // return RedirectToAction("Log","Log", new { message = string.Format("Wrong User Id Or Password {0}") });
                return View(move);
            }

        }
        public List<State1> GetAllState1()
        {

            List<State1> objstate = new List<State1>();
            objstate.Add(new State1 { Id = 0, StateName = "Select Country" });
            objstate.Add(new State1 { Id = 1, StateName = "AFGHANISTAN" });
            objstate.Add(new State1 { Id = 2, StateName = "ALAND ISLANDS" });
            objstate.Add(new State1 { Id = 3, StateName = "ALBANIA" });
            objstate.Add(new State1 { Id = 4, StateName = "ALGERIA" });
            objstate.Add(new State1 { Id = 5, StateName = "AMERICAN SAMOA" });
            objstate.Add(new State1 { Id = 6, StateName = "ANDORRA" });
            objstate.Add(new State1 { Id = 7, StateName = "AMERICAN SAMOA" });
            objstate.Add(new State1 { Id = 8, StateName = "ANGOLA" });
            objstate.Add(new State1 { Id = 9, StateName = "ANGUILLA" });
            objstate.Add(new State1 { Id = 10, StateName = "ANTARCTICA," });
            objstate.Add(new State1 { Id = 11, StateName = "ANTIGUA AND BARBUDA" });
            objstate.Add(new State1 { Id = 12, StateName = "ARGENTINA" });
            objstate.Add(new State1 { Id = 13, StateName = "ARMENIA" });
            objstate.Add(new State1 { Id = 14, StateName = "ARUBA" });
            objstate.Add(new State1 { Id = 15, StateName = "AUSTRALIA" });
            objstate.Add(new State1 { Id = 16, StateName = "AZERBAIJAN" });
            objstate.Add(new State1 { Id = 17, StateName = "BAHAMAS" });
            objstate.Add(new State1 { Id = 18, StateName = "BAHRAIN" });
            objstate.Add(new State1 { Id = 19, StateName = "BANGLADESH" });
            objstate.Add(new State1 { Id = 20, StateName = "BARBADOS" });
            objstate.Add(new State1 { Id = 21, StateName = "BELARUS" });
            objstate.Add(new State1 { Id = 22, StateName = "BELGIUM" });
            objstate.Add(new State1 { Id = 23, StateName = "BELIZE" });
            objstate.Add(new State1 { Id = 24, StateName = "BENIN" });
            objstate.Add(new State1 { Id = 25, StateName = "BERMUDA" });
            objstate.Add(new State1 { Id = 26, StateName = "BHUTAN" });
            objstate.Add(new State1 { Id = 27, StateName = "BONAIRE" });
            objstate.Add(new State1 { Id = 28, StateName = "BOLIVIA" });
            objstate.Add(new State1 { Id = 29, StateName = "BOSNIA AND HERZEGOVINA" });
            objstate.Add(new State1 { Id = 30, StateName = "BOTSWANA" });
            objstate.Add(new State1 { Id = 31, StateName = "BOUVET ISLAND" });
            objstate.Add(new State1 { Id = 32, StateName = "BRAZIL" });
            objstate.Add(new State1 { Id = 33, StateName = "BRITISH INDIAN OCEAN TERRITORY" });
            objstate.Add(new State1 { Id = 34, StateName = "BRUNEI DARUSSALAM" });
            objstate.Add(new State1 { Id = 35, StateName = "BULGARIA" });
            objstate.Add(new State1 { Id = 36, StateName = "BURKINA FASO" });
            objstate.Add(new State1 { Id = 37, StateName = "BURUNDI" });
            objstate.Add(new State1 { Id = 38, StateName = "CAMBODIA" });
            objstate.Add(new State1 { Id = 39, StateName = "CAMEROON" });
            objstate.Add(new State1 { Id = 40, StateName = "CANADA" });
            objstate.Add(new State1 { Id = 41, StateName = "CAPE VERDE" });
            objstate.Add(new State1 { Id = 42, StateName = "CAYMAN ISLANDS" });
            objstate.Add(new State1 { Id = 43, StateName = "CENTRAL AFRICAN REPUBLIC" });
            objstate.Add(new State1 { Id = 44, StateName = "CHAD" });
            objstate.Add(new State1 { Id = 45, StateName = "CHILE" });
            objstate.Add(new State1 { Id = 46, StateName = "CHINA" });
            objstate.Add(new State1 { Id = 47, StateName = "CHRISTMAS ISLAND" });
            objstate.Add(new State1 { Id = 48, StateName = "COCOS (KEELING) ISLANDS" });
            objstate.Add(new State1 { Id = 49, StateName = "COLOMBIA" });
            objstate.Add(new State1 { Id = 50, StateName = "COMOROS" });
            objstate.Add(new State1 { Id = 51, StateName = "CONGO" });
            objstate.Add(new State1 { Id = 52, StateName = "COOK ISLANDS" });
            objstate.Add(new State1 { Id = 53, StateName = "COSTA RICA" });
            objstate.Add(new State1 { Id = 54, StateName = "CÔTE D'IVOIRE" });
            objstate.Add(new State1 { Id = 55, StateName = "CROATIA" });
            objstate.Add(new State1 { Id = 56, StateName = "CUBA" });
            objstate.Add(new State1 { Id = 57, StateName = "CURAÇAO" });
            objstate.Add(new State1 { Id = 58, StateName = "CYPRUS" });
            objstate.Add(new State1 { Id = 59, StateName = "CZECH REPUBLIC" });
            objstate.Add(new State1 { Id = 60, StateName = "DENMARK" });
            objstate.Add(new State1 { Id = 61, StateName = "DJIBOUTI" });
            objstate.Add(new State1 { Id = 62, StateName = "DOMINICA" });
            objstate.Add(new State1 { Id = 63, StateName = "DOMINICAN REPUBLIC" });
            objstate.Add(new State1 { Id = 64, StateName = "ECUADOR" });
            objstate.Add(new State1 { Id = 65, StateName = "EGYPT" });
            objstate.Add(new State1 { Id = 66, StateName = "EL SALVADOR" });
            objstate.Add(new State1 { Id = 67, StateName = "EQUATORIAL GUINEA" });
            objstate.Add(new State1 { Id = 68, StateName = "ERITREA" });
            objstate.Add(new State1 { Id = 69, StateName = "ESTONIA" });
            objstate.Add(new State1 { Id = 70, StateName = "ETHIOPIA" });
            objstate.Add(new State1 { Id = 71, StateName = "FALKLAND ISLANDS (MALVINAS)" });
            objstate.Add(new State1 { Id = 72, StateName = "FAROE ISLANDS" });
            objstate.Add(new State1 { Id = 73, StateName = "FIJI" });
            objstate.Add(new State1 { Id = 74, StateName = "FINLAND" });
            objstate.Add(new State1 { Id = 75, StateName = "FRANCE" });
            objstate.Add(new State1 { Id = 76, StateName = "FRENCH GUIANA" });
            objstate.Add(new State1 { Id = 77, StateName = "FRENCH POLYNESIA," });
            objstate.Add(new State1 { Id = 78, StateName = "FRENCH SOUTHERN TERRITORIES" });
            objstate.Add(new State1 { Id = 79, StateName = "GABON" });
            objstate.Add(new State1 { Id = 80, StateName = "GAMBIA" });
            objstate.Add(new State1 { Id = 81, StateName = "GEORGIA" });
            objstate.Add(new State1 { Id = 82, StateName = "GERMANY" });
            objstate.Add(new State1 { Id = 83, StateName = "GHANA" });
            objstate.Add(new State1 { Id = 84, StateName = "GIBRALTAR" });
            objstate.Add(new State1 { Id = 85, StateName = " GREECE" });
            objstate.Add(new State1 { Id = 86, StateName = "GREENLAND" });
            objstate.Add(new State1 { Id = 87, StateName = "GRENADA" });
            objstate.Add(new State1 { Id = 88, StateName = "GUADELOUPE" });
            objstate.Add(new State1 { Id = 89, StateName = "GUAM" });
            objstate.Add(new State1 { Id = 90, StateName = "GUATEMALA" });
            objstate.Add(new State1 { Id = 91, StateName = "GUERNSEY" });
            objstate.Add(new State1 { Id = 92, StateName = "GUINEA" });
            objstate.Add(new State1 { Id = 93, StateName = "GUINEA-BISSAU" });
            objstate.Add(new State1 { Id = 94, StateName = "GUYANA" });
            objstate.Add(new State1 { Id = 95, StateName = "HAITI" });
            objstate.Add(new State1 { Id = 96, StateName = "HEARD ISLAND AND MCDONALD ISLANDS" });
            objstate.Add(new State1 { Id = 97, StateName = "HOLY SEE (VATICAN CITY STATE)" });
            objstate.Add(new State1 { Id = 98, StateName = "HONDURAS" });
            objstate.Add(new State1 { Id = 99, StateName = "HONG KONG" });
            objstate.Add(new State1 { Id = 100, StateName = "HUNGARY" });
            objstate.Add(new State1 { Id = 101, StateName = "ICELAND" });
            objstate.Add(new State1 { Id = 102, StateName = "INDIA" });
            objstate.Add(new State1 { Id = 103, StateName = "INDONESIA" });
            objstate.Add(new State1 { Id = 104, StateName = "IRAN" });
            objstate.Add(new State1 { Id = 105, StateName = "ISLAMIC REPUBLIC OF" });
            objstate.Add(new State1 { Id = 106, StateName = "IRAQ" });
            objstate.Add(new State1 { Id = 107, StateName = "IRELAND" });
            objstate.Add(new State1 { Id = 108, StateName = "ISLE OF MAN" });
            objstate.Add(new State1 { Id = 109, StateName = "ISRAEL" });
            objstate.Add(new State1 { Id = 110, StateName = "ITALY" });
            objstate.Add(new State1 { Id = 111, StateName = " JAMAICA" });
            objstate.Add(new State1 { Id = 112, StateName = "JAPAN" });
            objstate.Add(new State1 { Id = 113, StateName = "JERSEY" });
            objstate.Add(new State1 { Id = 114, StateName = "JORDAN" });
            objstate.Add(new State1 { Id = 115, StateName = "KAZAKHSTAN" });
            objstate.Add(new State1 { Id = 116, StateName = "KENYA" });
            objstate.Add(new State1 { Id = 117, StateName = "KIRIBATI" });
            objstate.Add(new State1 { Id = 118, StateName = "KOREA" });
            objstate.Add(new State1 { Id = 119, StateName = "DEMOCRATIC PEOPLE'S REPUBLIC OF" });
            objstate.Add(new State1 { Id = 120, StateName = "KOREA, REPUBLIC OF" });
            objstate.Add(new State1 { Id = 121, StateName = "KUWAIT" });
            objstate.Add(new State1 { Id = 123, StateName = "KYRGYZSTAN" });
            objstate.Add(new State1 { Id = 124, StateName = "LAO PEOPLE'S DEMOCRATIC REPUBLIC" });
            objstate.Add(new State1 { Id = 125, StateName = "LATVIA" });
            objstate.Add(new State1 { Id = 126, StateName = "LEBANON" });
            objstate.Add(new State1 { Id = 127, StateName = "LESOTHO" });
            objstate.Add(new State1 { Id = 128, StateName = "LIBERIA" });
            objstate.Add(new State1 { Id = 129, StateName = "LIBYA" });
            objstate.Add(new State1 { Id = 130, StateName = "LIECHTENSTEIN" });
            objstate.Add(new State1 { Id = 131, StateName = "LITHUANIA" });
            objstate.Add(new State1 { Id = 132, StateName = "LUXEMBOURG" });
            objstate.Add(new State1 { Id = 133, StateName = "MACAO" });
            objstate.Add(new State1 { Id = 134, StateName = "MACEDONIA" });
            objstate.Add(new State1 { Id = 135, StateName = "MADAGASCAR" });
            objstate.Add(new State1 { Id = 136, StateName = " MALAWI" });
            objstate.Add(new State1 { Id = 137, StateName = "MALAYSIA" });
            objstate.Add(new State1 { Id = 138, StateName = "MALDIVES" });
            objstate.Add(new State1 { Id = 139, StateName = "MALI" });
            objstate.Add(new State1 { Id = 140, StateName = "MALTA" });
            objstate.Add(new State1 { Id = 141, StateName = "MARSHALL ISLANDS" });
            objstate.Add(new State1 { Id = 142, StateName = "MARTINIQUE" });
            objstate.Add(new State1 { Id = 143, StateName = "MAURITANIA" });
            objstate.Add(new State1 { Id = 144, StateName = "MAURITIUS" });
            objstate.Add(new State1 { Id = 145, StateName = "MAYOTTE" });
            objstate.Add(new State1 { Id = 146, StateName = "MEXICO" });
            objstate.Add(new State1 { Id = 147, StateName = "MICRONESIA" });
            objstate.Add(new State1 { Id = 148, StateName = "FEDERATED STATES OF" });
            objstate.Add(new State1 { Id = 149, StateName = "MOLDOVA, REPUBLIC OF" });
            objstate.Add(new State1 { Id = 150, StateName = "MONACO" });
            objstate.Add(new State1 { Id = 151, StateName = "MONGOLIA" });
            objstate.Add(new State1 { Id = 152, StateName = "MONTENEGRO" });
            objstate.Add(new State1 { Id = 153, StateName = "MONTSERRAT" });
            objstate.Add(new State1 { Id = 154, StateName = " MOROCCO" });
            objstate.Add(new State1 { Id = 155, StateName = "MOZAMBIQUE" });
            objstate.Add(new State1 { Id = 156, StateName = "MYANMAR" });
            objstate.Add(new State1 { Id = 157, StateName = "NAMIBIA" });
            objstate.Add(new State1 { Id = 158, StateName = "NAURU" });
            objstate.Add(new State1 { Id = 159, StateName = "NEPAL" });
            objstate.Add(new State1 { Id = 160, StateName = "NETHERLANDS" });
            objstate.Add(new State1 { Id = 161, StateName = "NEW CALEDONIA" });
            objstate.Add(new State1 { Id = 162, StateName = "NEW ZEALAND" });
            objstate.Add(new State1 { Id = 163, StateName = "NICARAGUA" });
            objstate.Add(new State1 { Id = 164, StateName = "NIGER" });
            objstate.Add(new State1 { Id = 165, StateName = "NIGERIA" });
            objstate.Add(new State1 { Id = 166, StateName = "NIUE" });
            objstate.Add(new State1 { Id = 167, StateName = "NORFOLK ISLAND" });
            objstate.Add(new State1 { Id = 168, StateName = "NORTHERN MARIANA ISLANDS" });
            objstate.Add(new State1 { Id = 169, StateName = "NORWAY" });
            objstate.Add(new State1 { Id = 170, StateName = "OMAN" });
            objstate.Add(new State1 { Id = 171, StateName = "PAKISTAN" });
            objstate.Add(new State1 { Id = 172, StateName = "PALAU" });
            objstate.Add(new State1 { Id = 173, StateName = "PALESTINE" });
            objstate.Add(new State1 { Id = 174, StateName = "PANAMA" });
            objstate.Add(new State1 { Id = 175, StateName = "PAPUA NEW GUINEA" });
            objstate.Add(new State1 { Id = 176, StateName = "PARAGUAY" });
            objstate.Add(new State1 { Id = 177, StateName = "PERU" });
            objstate.Add(new State1 { Id = 178, StateName = "PHILIPPINES" });
            objstate.Add(new State1 { Id = 179, StateName = " PITCAIRN" });
            objstate.Add(new State1 { Id = 180, StateName = "POLAND" });
            objstate.Add(new State1 { Id = 181, StateName = "PORTUGAL" });
            objstate.Add(new State1 { Id = 182, StateName = "PUERTO RICO" });
            objstate.Add(new State1 { Id = 183, StateName = "QATAR" });
            objstate.Add(new State1 { Id = 184, StateName = "RÉUNION" });
            objstate.Add(new State1 { Id = 185, StateName = "ROMANIA" });
            objstate.Add(new State1 { Id = 186, StateName = "RUSSIAN FEDERATION" });
            objstate.Add(new State1 { Id = 187, StateName = "RWANDA" });
            objstate.Add(new State1 { Id = 188, StateName = "SAINT BARTHÉLEMY" });
            objstate.Add(new State1 { Id = 189, StateName = "SAINT HELENA, ASCENSION AND TRISTAN DA CUNHA" });
            objstate.Add(new State1 { Id = 190, StateName = "SAINT KITTS AND NEVIS" });
            objstate.Add(new State1 { Id = 191, StateName = "SAINT LUCIA" });
            objstate.Add(new State1 { Id = 192, StateName = "SAINT MARTIN (FRENCH PART)" });
            objstate.Add(new State1 { Id = 193, StateName = "SAINT PIERRE AND MIQUELON" });
            objstate.Add(new State1 { Id = 194, StateName = "SAINT VINCENT AND THE GRENADINES" });
            objstate.Add(new State1 { Id = 195, StateName = "SAMOA" });
            objstate.Add(new State1 { Id = 196, StateName = "SAN MARINO" });
            objstate.Add(new State1 { Id = 197, StateName = "SAO TOME AND PRINCIPE" });
            objstate.Add(new State1 { Id = 198, StateName = "SAUDI ARABIA" });
            objstate.Add(new State1 { Id = 199, StateName = "SENEGAL" });
            objstate.Add(new State1 { Id = 200, StateName = "SERBIA" });
            objstate.Add(new State1 { Id = 201, StateName = "SEYCHELLES" });
            objstate.Add(new State1 { Id = 202, StateName = "SIERRA LEONE" });
            objstate.Add(new State1 { Id = 203, StateName = "SINGAPORE" });
            objstate.Add(new State1 { Id = 204, StateName = "SINT MAARTEN (DUTCH PART)" });
            objstate.Add(new State1 { Id = 205, StateName = "SLOVAKIA" });
            objstate.Add(new State1 { Id = 206, StateName = "SOLOMON ISLANDS" });
            objstate.Add(new State1 { Id = 207, StateName = "SOMALIA" });
            objstate.Add(new State1 { Id = 208, StateName = "SOUTH AFRICA" });
            objstate.Add(new State1 { Id = 209, StateName = "SOUTH GEORGIA AND THE SOUTH SANDWICH ISLANDS" });
            objstate.Add(new State1 { Id = 210, StateName = "SOUTH SUDAN" });
            objstate.Add(new State1 { Id = 211, StateName = "SPAIN" });
            objstate.Add(new State1 { Id = 212, StateName = "SRI LANKA" });
            objstate.Add(new State1 { Id = 213, StateName = "SUDAN" });
            objstate.Add(new State1 { Id = 214, StateName = "SURINAME" });
            objstate.Add(new State1 { Id = 215, StateName = "SVALBARD AND JAN MAYEN" });
            objstate.Add(new State1 { Id = 216, StateName = "SWAZILAND" });
            objstate.Add(new State1 { Id = 217, StateName = "SWEDEN" });
            objstate.Add(new State1 { Id = 218, StateName = "SWITZERLAND" });
            objstate.Add(new State1 { Id = 219, StateName = "SYRIAN ARAB REPUBLIC" });
            objstate.Add(new State1 { Id = 220, StateName = "TAIWAN, PROVINCE OF CHINA" });
            objstate.Add(new State1 { Id = 221, StateName = "TAJIKISTAN" });
            objstate.Add(new State1 { Id = 222, StateName = "TANZANIA" });
            objstate.Add(new State1 { Id = 223, StateName = "THAILAND" });
            objstate.Add(new State1 { Id = 224, StateName = "TIMOR-LESTE" });
            objstate.Add(new State1 { Id = 225, StateName = "TOGO" });
            objstate.Add(new State1 { Id = 226, StateName = "TOKELAU" });
            objstate.Add(new State1 { Id = 227, StateName = "TONGA" });
            objstate.Add(new State1 { Id = 228, StateName = "TRINIDAD AND TOBAGO" });
            objstate.Add(new State1 { Id = 229, StateName = "TUNISIA" });
            objstate.Add(new State1 { Id = 230, StateName = "TURKEY" });
            objstate.Add(new State1 { Id = 231, StateName = "TURKMENISTAN" });
            objstate.Add(new State1 { Id = 232, StateName = "TURKS AND CAICOS ISLANDS" });
            objstate.Add(new State1 { Id = 233, StateName = "TUVALU" });
            objstate.Add(new State1 { Id = 234, StateName = "UGANDA" });
            objstate.Add(new State1 { Id = 235, StateName = "UKRAINE" });
            objstate.Add(new State1 { Id = 236, StateName = "UNITED ARAB EMIRATES" });
            objstate.Add(new State1 { Id = 237, StateName = "UNITED KINGDOM" });
            objstate.Add(new State1 { Id = 238, StateName = "UNITED STATES" });
            objstate.Add(new State1 { Id = 239, StateName = "UNITED STATES MINOR OUTLYING ISLANDS" });
            objstate.Add(new State1 { Id = 240, StateName = "URUGUAY" });
            objstate.Add(new State1 { Id = 241, StateName = "UZBEKISTAN" });
            objstate.Add(new State1 { Id = 242, StateName = "VANUATU" });
            objstate.Add(new State1 { Id = 243, StateName = "VENEZUELA" });
            objstate.Add(new State1 { Id = 244, StateName = "VIET NAM" });
            objstate.Add(new State1 { Id = 245, StateName = "VIRGIN ISLANDS" });
            objstate.Add(new State1 { Id = 246, StateName = "WALLIS AND FUTUNA" });
            objstate.Add(new State1 { Id = 247, StateName = "WESTERN SAHARA" });
            objstate.Add(new State1 { Id = 248, StateName = "YEMEN" });
            objstate.Add(new State1 { Id = 249, StateName = "ZAMBIA" });
            objstate.Add(new State1 { Id = 250, StateName = "ZIMBABWE" });

            return objstate;
        } 
    }
}
