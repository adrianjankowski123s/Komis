using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Komis;

namespace Komis.Controllers
{
    public class WyposazenieController : Controller
    {
        private KomisContext db = new KomisContext();

        // GET: Wyposazenie
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Wyposazenie.ToList());
        }

        // GET: Wyposazenie/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wyposazenie wyposazenie = db.Wyposazenie.Find(id);
            if (wyposazenie == null)
            {
                return HttpNotFound();
            }
            return View(wyposazenie);
        }

        // GET: Wyposazenie/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Wyposazenie/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_wyposazenia,Nazwa_wyposazenia")] Wyposazenie wyposazenie)
        {
            if (ModelState.IsValid)
            {
                db.Wyposazenie.Add(wyposazenie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(wyposazenie);
        }

        

        // GET: Wyposazenie/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wyposazenie wyposazenie = db.Wyposazenie.Find(id);
            if (wyposazenie == null)
            {
                return HttpNotFound();
            }
            return View(wyposazenie);
        }

        // POST: Wyposazenie/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Wyposazenie wyposazenie = db.Wyposazenie.Find(id);
            db.Wyposazenie.Remove(wyposazenie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
