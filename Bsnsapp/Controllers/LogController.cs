using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raven.Client;
using Newtonsoft.Json;
using Bsnsapp.Models;
using System.ComponentModel.DataAnnotations;

namespace Bsnsapp.Controllers
{
    public class LogController : Controller 
    {
        //
        // GET: /Log/

        private readonly IDocumentSession _documentSession;
        public LogController(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }
        [ActionName("Log")]      
         
        public ActionResult Log()
        {
            var model = new LoginPage();
            List<SelectListItem> item = new List<SelectListItem>();

            item.Add(new SelectListItem() { Text = "English", Value = "English", Selected = true });
            item.Add(new SelectListItem() { Text = "French", Value = "French", Selected = false });
            ViewBag.Languages = item;
           // return View(model);
            return View();
        }
        [HttpPost]
        public ActionResult Log(LoginPage log,Mov move)
        {
            //var log2 = _documentSession.Load<LoginPage>(log.UserID);
            var log1 = _documentSession.Query<Mov>()
                .Where(x => x.Email==log.UserID && x.Password==log.Password)
                .Take(1).ToList();

           
            //var mov = _documentSession.Load<mov>(move.Email);
           // var mov1 = _documentSession.Load<LoginPage>(log.UserID);
            //var move = _documentSession.Store(mov);
            //var move1 = _documentSession.Load<LoginPage>(UserID);
            if (log1.Count > 0)
            {
                Session["Usr_Nm"] = move.Name;
                Mov[] Mov = _documentSession.Query<Mov>().Where(x => x.Email == log.UserID && x.Password == log.Password).ToArray() ;

               
                Session["Usr_Nm"] = Mov[0].Name;
                Session["vendor"] = log.UserID;
                Session["user1"] = "us";
                Session["user"] = "Admin";
                return RedirectToAction("Vendor", "Vendor", new { message = string.Format("Logged In As {0}", log.UserID) });
            }         

            else
            {
               // return RedirectToAction("Log","Log", new { message = string.Format("Wrong User Id Or Password {0}") });
                return View(log);
            }
          
        }


        //
        // GET: /Log/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Log/Create
        public ActionResult Create(FormCollection collection)
        {


            return View();

        }
        [HttpPost]
        public ActionResult Create(LoginPage log)
        {

            if (!ModelState.IsValid)
                return View();
            _documentSession.Store(log.UserID);
           // _documentSession.SaveChanges();
            return RedirectToAction("CreateLog", new { message = string.Format("LoginPage {0}", log.UserID)});

        } 

        //
        // POST: /Log/Create

       
       
        
        //
        // GET: /Log/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Log/Edit/5

        // [HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here
 
        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Log/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Log/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        //public ActionResult ALog()
        //{
        //    var model = new LoginPage();
        //    return View(model);
        //    // return View();
        //}
        //[HttpPost]
        //public ActionResult Log(AdminLoginPage log, Mov move)
        //{
        //    var log2 = _documentSession.Load<AdminLoginPage>(log.UserID);
        //    var log1 = _documentSession.Query<Mov>()
        //        .Where(x => x.Email == log.UserID && x.Password == log.Password)
        //        .Take(1).ToList();
        //    //var mov = _documentSession.Load<mov>(move.Email);
        //    // var mov1 = _documentSession.Load<LoginPage>(log.UserID);
        //    //var move = _documentSession.Store(mov);
        //    //var move1 = _documentSession.Load<LoginPage>(UserID);
        //    if (log1.Count > 0)
        //    {

        //        return RedirectToAction("Create", "Category", new { message = string.Format("Logged In As {0}", log.UserID) });
        //    }

        //    else
        //    {
        //        // return RedirectToAction("Log","Log", new { message = string.Format("Wrong User Id Or Password {0}") });
        //        return View(log);
        //    }

       //}
        // GET:Change Password
        public ActionResult ChangePassword(int id)
        {

           
                var move = _documentSession.Load<ChangePassword>(id);
                if (move == null)
                    return RedirectToAction("Users", new { message = string.Format("Mov {0} not found", id) });

                return View(move);
           

        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePassword chng, Mov move)
        {

            var log2 = _documentSession.Load<ChangePassword>(chng.UserId);
            var log1 = _documentSession.Query<Mov>()
                .Where(x => x.Email == chng.UserId && x.Password == chng.OldPassword)
                .Take(1).ToList();
            //var mov = _documentSession.Load<mov>(move.Email);
            // var mov1 = _documentSession.Load<LoginPage>(log.UserID);
            //var move = _documentSession.Store(mov);
            //var move1 = _documentSession.Load<LoginPage>(UserID);
            if (log1.Count > 0)
            {
                if (!ModelState.IsValid)
                    _documentSession.Store(chng);
                _documentSession.SaveChanges();
                return RedirectToAction("ChangePassword", "Log", new { message = string.Format("Password Successfully Changed {0}", chng.Id) });
            }

            else
            {
                // return RedirectToAction("Log","Log", new { message = string.Format("Wrong User Id Or Password {0}") });
                return View(chng);
            }

        }

        //public ActionResult Create(FormCollection collection)
        //{


        //    return View();

        //}
        [HttpPost]
        public ActionResult Create(ChangePassword chng)
        {
            if (!ModelState.IsValid)
                _documentSession.Store(chng);
            _documentSession.SaveChanges();
            //var log2 = _documentSession.Load<ChangePassword>(chng.Password);
            //var log1 = _documentSession.Query<ChangePassword>()
            //    .Where(x => x.Password == chng.Password)
            //    .Take(1).ToList();
            //var mov = _documentSession.Load<mov>(move.Email);
            // var mov1 = _documentSession.Load<LoginPage>(log.UserID);
            //var move = _documentSession.Store(mov);
            //var move1 = _documentSession.Load<LoginPage>(UserID);
            //if (log1.Count > 0)
            //{

            //return RedirectToAction("ChangePassword", "Log", new { message = string.Format("Password Successfully Changed {0}", chng.Password) });
            //}

            //else
            //{
            // return RedirectToAction("Log","Log", new { message = string.Format("Wrong User Id Or Password {0}") });
            return View(chng);
            //}

        }
      
    }
}
