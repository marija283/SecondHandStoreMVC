﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SecondHandStoreApp.Models;
using SecondHandStoreApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SecondHandStoreApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private UserRepository _userRepository = new UserRepository();
        private SellerRepository _sellerRepository = new SellerRepository();
        private StoreItemRepository _storeItemRepository = new StoreItemRepository();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListUsers()
        {
            var users = _userRepository.GetAll();

            return View(users);
        }

        //     USERS

        // GET: Admin/DisableUser/5
        public ActionResult DisableUser(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = UserManager.FindById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/DisableUser/5
        [HttpPost, ActionName("DisableUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DisableConfirmedUser(string id)
        {
            var user = UserManager.FindById(id);
            user.LockoutEndDateUtc = DateTime.Now.AddDays(3);
            UserManager.Update(user);
            return RedirectToAction("Index");
        }

        // GET: Admin/DetailsUser/5
        public ActionResult DetailsUser(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = UserManager.FindById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //    STORE ITEMS

        public ActionResult ListStoreItems()
        {
            var items = _storeItemRepository.GetAll();

            return View(items);
        }

        // GET: Admin/DisableItem/5
        public ActionResult DisableItem(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = _storeItemRepository.GetById((int)id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Admin/DisableItem/5
        [HttpPost, ActionName("DisableItem")]
        [ValidateAntiForgeryToken]
        public ActionResult DisableConfirmedItem(int id)
        {

            _storeItemRepository.Delete(id);
            return RedirectToAction("Index");
        }

        // GET: Admin/DetailsItem/5
        public ActionResult DetailsItem(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = _storeItemRepository.GetById((int)id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Admin/EditItem/5
        public ActionResult EditItem(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreItem storeItem = _storeItemRepository.GetById((int)id);
            if (storeItem == null)
            {
                return HttpNotFound();
            }
            return View(storeItem);
        }

        // POST: Admin/EditItem/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditItem(StoreItem storeItem)
        {
            if (ModelState.IsValid)
            {
                _storeItemRepository.Update(storeItem);
                return RedirectToAction("ListStoreItems");
            }
            return View(storeItem);
        }




    }
}