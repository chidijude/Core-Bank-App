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
    public class GlAccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: GlAccounts
        public async Task<ActionResult> Index()
        {
            var glAccounts = db.GlAccounts.Include(g => g.Branch).Include(g => g.GlCategory);
            return View(await glAccounts.ToListAsync());
        }

        // GET: GlAccounts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GlAccount glAccount = await db.GlAccounts.FindAsync(id);
            if (glAccount == null)
            {
                return HttpNotFound();
            }
            return View(glAccount);
        }

        // GET: GlAccounts/Create
        public ActionResult Create()
        {
            ViewBag.BranchID = new SelectList(db.Branches, "Id", "Name");
            ViewBag.GlCategoryID = new SelectList(db.GlCategories, "ID", "Name");
            return View();
        }

        // POST: GlAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,AccountName,CodeNumber,AccountBalance,GlCategoryID,BranchID")] GlAccount glAccount)
        {
            if (ModelState.IsValid)
            {
                GlCategory glcategory = await db.GlCategories.FindAsync(glAccount.GlCategoryID);
                glAccount.CodeNumber = Generator.GenerateGLAccountCode(glcategory.MainCategory.ToString());
                db.GlAccounts.Add(glAccount);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.BranchID = new SelectList(db.Branches, "Id", "Name", glAccount.BranchID);
            ViewBag.GlCategoryID = new SelectList(db.GlCategories, "ID", "Name", glAccount.GlCategoryID);
            return View(glAccount);
        }

        // GET: GlAccounts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GlAccount glAccount = await db.GlAccounts.FindAsync(id);
            if (glAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchID = new SelectList(db.Branches, "Id", "Name", glAccount.BranchID);
            ViewBag.GlCategoryID = new SelectList(db.GlCategories, "ID", "Name", glAccount.GlCategoryID);
            return View(glAccount);
        }

        // POST: GlAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,AccountName,CodeNumber,AccountBalance,GlCategoryID,BranchID")] GlAccount glAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(glAccount).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BranchID = new SelectList(db.Branches, "Id", "Name", glAccount.BranchID);
            ViewBag.GlCategoryID = new SelectList(db.GlCategories, "ID", "Name", glAccount.GlCategoryID);
            return View(glAccount);
        }

        // GET: GlAccounts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GlAccount glAccount = await db.GlAccounts.FindAsync(id);
            if (glAccount == null)
            {
                return HttpNotFound();
            }
            return View(glAccount);
        }

        // POST: GlAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            GlAccount glAccount = await db.GlAccounts.FindAsync(id);
            db.GlAccounts.Remove(glAccount);
            await db.SaveChangesAsync();
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
