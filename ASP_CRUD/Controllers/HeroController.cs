using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ASP_CRUD.Data;
using ASP_CRUD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ASP_CRUD.Controllers
{
    [Authorize]

    public class HeroController : Controller
    {

        //to load the db
        private readonly ApplicationDbContext _db;
        public HeroController(ApplicationDbContext db)
        {
            _db = db;
        }

        string controller_name;
        string action_Name;

        public override void OnActionExecuting(ActionExecutingContext context) {
            controller_name = context.RouteData.Values["controller"].ToString();
            action_Name = context.RouteData.Values["action"].ToString();
            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    var name = claims.Where(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").FirstOrDefault()?.Value;
                    var role = claims.Where(p => p.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").FirstOrDefault()?.Value;

                    ActionHistory hist = new ActionHistory();
                    hist.Action = action_Name;
                    hist.Name = name;
                    hist.Role = role;
                    _db.ActionHistories.Add(hist);
                    _db.SaveChanges();
                }
            }
            base.OnActionExecuting(context);
        }





        public IActionResult Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as ClaimsIdentity;
               /* if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    var name = claims.Where(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").FirstOrDefault()?.Value;
                    ViewBag.userName = name;

                    ActionHistory hist = new ActionHistory();
                    hist.Action = "Hero.Index";
                    hist.Name = name;
                    _db.ActionHistories.Add(hist);
                    _db.SaveChanges();




                }*/

                IEnumerable<Hero> objList = _db.Heroes;

                return View(objList);
            }
            else
            {
                return RedirectToAction("LogIn", "Account", new { area = "" });

            }
        }

        [Authorize]
        public IActionResult View(int Id )
        {
            var obj = _db.Heroes.Find(Id);

            return View(obj);
        }

        public IActionResult Create()
        {
            //IEnumerable<Hero> objList = _db.Heroes;
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Account", new { area = "" });

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Hero obj)
        {

            if (User.Identity.IsAuthenticated)
            {
                _db.Heroes.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            else
            {
                return RedirectToAction("LogIn", "Account", new { area = "" });
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (Id == null || Id == 0)
                {
                    return NotFound();
                }

                var obj = _db.Heroes.Find(Id);

                if (obj == null)
                {
                    return NotFound();
                }

                return View(obj);
            }

            else
            {
                return RedirectToAction("LogIn", "Account", new { area = "" });

            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Hero obj)
        {
            if (User.Identity.IsAuthenticated)
            {
                _db.Heroes.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            else
            {
                return RedirectToAction("LogIn", "Account", new { area = "" });

            }
        }

        [Authorize(Roles = "Admin")]

        public IActionResult Delete(int Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (Id == null || Id == 0)
                {
                    return NotFound();
                }

                var obj = _db.Heroes.Find(Id);

                if (obj == null)
                {
                    return NotFound();
                }

                return View(obj);
            }

            else
            {
                return RedirectToAction("LogIn", "Account", new { area = "" });

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public IActionResult Delete(Hero obj)
        {
            if (User.Identity.IsAuthenticated)
            {
                _db.Heroes.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            else
            {
                return RedirectToAction("LogIn", "Account", new { area = "" });

            }
        }

    }
}
