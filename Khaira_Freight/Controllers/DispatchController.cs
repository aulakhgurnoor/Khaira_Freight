using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Khaira_Freight.Models;

namespace Khaira_Freight.Controllers
{
    public class DispatchController : Controller
    {
        private KhairaFreightEntities db = new KhairaFreightEntities();

        // GET: Dispatch
        public ActionResult Index()
        {
            return View(db.trucks.ToList());
        }

        // GET: Dispatch/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            truck truck = db.trucks.Find(id);
            if (truck == null)
            {
                return HttpNotFound();
            }
            return View(truck);
        }

        // GET: Dispatch/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dispatch/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "unit_number,plate_number,registration,vin_number,make,year,weight,status,owner_company,driver,last_location,last_trip")] truck truck)
        {
            if (ModelState.IsValid)
            {
                db.trucks.Add(truck);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(truck);
        }

        // GET: Dispatch/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            truck truck = db.trucks.Find(id);
            if (truck == null)
            {
                return HttpNotFound();
            }
            return View(truck);
        }

        // POST: Dispatch/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "unit_number,plate_number,registration,vin_number,make,year,weight,status,owner_company,driver,last_location,last_trip")] truck truck)
        {
            if (ModelState.IsValid)
            {
                db.Entry(truck).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(truck);
        }

        // GET: Dispatch/Delete/5
    
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed.Try again.";
            }
            //
            
            truck truck = db.trucks.Find(id);
            if (truck == null)
            {
                return HttpNotFound();
            }
            return View(truck);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                

                truck truck = db.trucks.Find(id);
                db.trucks.Remove(truck);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DataException)
            {

                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }
        //

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
