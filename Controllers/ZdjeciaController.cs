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
    public class ZdjeciaController : Controller
    {
        private KomisContext db = new KomisContext();

        // GET: Zdjecia
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var zdjecia = db.Zdjecia.Include(z => z.Samochody);
            return View(zdjecia.ToList());
        }

        // GET: Zdjecia/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Zdjecia zdjecia = db.Zdjecia.Find(id);
            if (zdjecia == null)
            {
                return HttpNotFound();
            }
            return View(zdjecia);
        }

        // GET: Zdjecia/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.Id_samochodu = new SelectList(db.Samochody, "Id_samochodu", "Tytul");
            return View();
        }

        // POST: Zdjecia/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_zdjecia,Id_samochodu,Tytul,Nazwa_zdjecia")] Zdjecia zdjecia)
        {
            if (ModelState.IsValid)
            {
                db.Zdjecia.Add(zdjecia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_samochodu = new SelectList(db.Samochody, "Id_samochodu", "Tytul", zdjecia.Id_samochodu);
            return View(zdjecia);
        }

        // GET: Zdjecia/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Zdjecia zdjecia = db.Zdjecia.Find(id);
            if (zdjecia == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_samochodu = new SelectList(db.Samochody, "Id_samochodu", "Tytul", zdjecia.Id_samochodu);
            return View(zdjecia);
        }

        // POST: Zdjecia/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_zdjecia,Id_samochodu,Tytul,Nazwa_zdjecia")] Zdjecia zdjecia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(zdjecia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_samochodu = new SelectList(db.Samochody, "Id_samochodu", "Tytul", zdjecia.Id_samochodu);
            return View(zdjecia);
        }

        // GET: Zdjecia/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Zdjecia zdjecia = db.Zdjecia.Find(id);
            if (zdjecia == null)
            {
                return HttpNotFound();
            }
            return View(zdjecia);
        }

        // POST: Zdjecia/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Zdjecia zdjecia = db.Zdjecia.Find(id);
            db.Zdjecia.Remove(zdjecia);
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
