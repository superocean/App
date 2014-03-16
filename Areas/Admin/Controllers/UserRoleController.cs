using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XPGroup.Models;
using XPGroup.Models.Repository;

namespace XPGroup.Areas.Admin.Controllers
{
    public class UserRoleController : BaseController
    {
        private IUserRoleRepository userRoleRepository { get; set; }

        public UserRoleController(IUserRoleRepository userRole)
        {
            this.userRoleRepository = userRole;
        }

        //
        // GET: /Admin/UserRole/

        public ActionResult Index()
        {
            return View(userRoleRepository.GetAll());
        }

        //GET:/Admin/UserRole/Add
        public ActionResult Add()
        {
            return View();
        }

        //POST:/Admin/UserRole/Add
        [HttpPost]
        public ActionResult Add(UserRole userRole)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    userRoleRepository.Add(userRole);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return RedirectToAction("Add");
                }
            }
            return RedirectToAction("Add");
        }

        //GET:/Admin/UserRole/Edit/Id
        public ActionResult Edit(int id)
        {
            UserRole userRole = userRoleRepository.GetById(id);
            if (userRole == null)
            {
                return RedirectToAction("Index");
            }
            return View(userRole);
        }

        //POST:/Admin/UserRole/Edit
        [HttpPost]
        public ActionResult Edit(UserRole userRole)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    userRoleRepository.Update(userRole);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return RedirectToAction("Edit", new { id = userRole.RoleId });
                }
            }
            return RedirectToAction("Edit", new { id = userRole.RoleId });
        }

        //POST:/Admin/UserRole/Delete
        [HttpPost]
        public ActionResult Delete(int id)
        {
            UserRole userRole = userRoleRepository.GetById(id);
            if (userRole != null)
            {
                userRoleRepository.Delete(userRole);
                return Json("Delete user role " + userRole.RoleName + " successful");
            }
            else
            {
                return Json("Delete user role fail", JsonRequestBehavior.AllowGet);
            }
        }
    }
}