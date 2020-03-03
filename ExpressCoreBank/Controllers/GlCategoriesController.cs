using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Express.Core.Models;
using ExpressCoreBank.Models;
using ExpressLogic;

namespace ExpressCoreBank.Controllers
{
    public class GlCategoriesController : Controller
    {
        
        
            private ApplicationDbContext db = new ApplicationDbContext();
            GlCategoryLogic glCatLogic = new GlCategoryLogic();

            // GET: GlCategory
            public ActionResult Index()
            {
                return View(db.GlCategories.ToList());
            }

            // GET: GlCategory/Details/5
            public ActionResult Details(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                GlCategory glCategory = db.GlCategories.Find(id);
                if (glCategory == null)
                {
                    return HttpNotFound();
                }
                return View(glCategory);
            }

            // GET: GlCategory/Create
            public ActionResult Create()
            {
                return View();
            }

            // POST: GlCategory/Create
            // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
            // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Create([Bind(Include = "ID,Name,Description,MainCategory")] GlCategory glCategory)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (!glCatLogic.IsUniqueName(glCategory.Name))
                        {
                            AddError("Please select another name");
                            return View(glCategory);
                        }

                        db.GlCategories.Add(glCategory);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        AddError(ex.ToString());
                        return View(glCategory);
                    }
                }
                AddError("Please enter valid data");
                return View(glCategory);
            }

            // GET: GlCategory/Edit/5
            public ActionResult Edit(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                GlCategory glCategory = db.GlCategories.Find(id);
                if (glCategory == null)
                {
                    return HttpNotFound();
                }
                return View(glCategory);
            }

            // POST: GlCategory/Edit/5
            // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
            // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

            // there is now supposedly a more secure way to implement edit [post] using TryUpdateModel
            // as decribed in the microsoft's basic CRUD article.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Edit([Bind(Include = "ID,Name,Description,MainCategory")] GlCategory glCategory)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // original category is automatically being tracked by the db context. (because it was read from database, tracked with Unchanged status)
                        // its causing conflict when i track glCategory built by the model binder because they have the same primary key.
                        // my solution is to detach originalCategory from context tracking.

                        GlCategory originalCategory = db.GlCategories.Find(glCategory.ID);
                        db.Entry(originalCategory).State = EntityState.Detached;

                        string originalName = originalCategory.Name;
                        if (!glCategory.Name.ToLower().Equals(originalName.ToLower()))
                        {
                            if (!glCatLogic.IsUniqueName(glCategory.Name))
                            {
                                AddError("Please select another name");
                                return View(glCategory);
                            }
                        }

                        db.Entry(glCategory).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        AddError(ex.ToString());
                        return View(glCategory);
                    }
                }
                AddError("Please enter valid data");
                return View(glCategory);
            }

            // GET: GlCategory/Delete/5
            public ActionResult Delete(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                GlCategory glCategory = db.GlCategories.Find(id);
                if (glCategory == null)
                {
                    return HttpNotFound();
                }
                return View(glCategory);
            }

            // POST: GlCategory/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public ActionResult DeleteConfirmed(int id)
            {
                GlCategory glCategory = db.GlCategories.Find(id);
                db.GlCategories.Remove(glCategory);
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

            private void AddError(string error)
            {
                ModelState.AddModelError("", error);
            }
        
    }
}
