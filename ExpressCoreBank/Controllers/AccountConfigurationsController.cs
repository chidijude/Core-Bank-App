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
    public class AccountConfigurationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: AccountConfigurations1
        public async Task<ActionResult> Index()
        {
            var accountConfigurations = db.AccountConfigurations.Include(a => a.CurrentCotIncomeGl).Include(a => a.CurrentInterestExpenseGl).Include(a => a.CurrentInterestPayableGl).Include(a => a.LoanInterestExpenseGl).Include(a => a.LoanInterestIncomeGl).Include(a => a.LoanInterestReceivableGl).Include(a => a.SavingsInterestExpenseGl).Include(a => a.SavingsInterestPayableGl);
            return View(await accountConfigurations.ToListAsync());
        }

       

        // GET: AccountConfigurations1/Create
        public ActionResult Create()
        {
            ViewBag.CurrentCotIncomeGlID = new SelectList(db.GlAccounts, "ID", "AccountName");
            ViewBag.CurrentInterestExpenseGlID = new SelectList(db.GlAccounts, "ID", "AccountName");
            ViewBag.CurrentInterestPayableGlID = new SelectList(db.GlAccounts, "ID", "AccountName");
            ViewBag.LoanInterestExpenseGLID = new SelectList(db.GlAccounts, "ID", "AccountName");
            ViewBag.LoanInterestIncomeGlID = new SelectList(db.GlAccounts, "ID", "AccountName");
            ViewBag.LoanInterestReceivableGlID = new SelectList(db.GlAccounts, "ID", "AccountName");
            ViewBag.SavingsInterestExpenseGlID = new SelectList(db.GlAccounts, "ID", "AccountName");
            ViewBag.SavingsInterestPayableGlID = new SelectList(db.GlAccounts, "ID", "AccountName");
            return View();
        }

        // POST: AccountConfigurations1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,IsBusinessOpen,FinancialDate,SavingsCreditInterestRate,SavingsMinimumBalance,SavingsInterestExpenseGlID,SavingsInterestPayableGlID,CurrentCreditInterestRate,CurrentMinimumBalance,CurrentCot,CurrentInterestExpenseGlID,CurrentCotIncomeGlID,CurrentInterestPayableGlID,LoanDebitInterestRate,LoanInterestIncomeGlID,LoanInterestExpenseGLID,LoanInterestReceivableGlID")] AccountConfiguration accountConfiguration)
        {
            if (ModelState.IsValid)
            {
                db.AccountConfigurations.Add(accountConfiguration);
                await db.SaveChangesAsync();
                return RedirectToAction("Details");
            }

            ViewBag.CurrentCotIncomeGlID = new SelectList(db.GlAccounts, "ID", "AccountName", accountConfiguration.CurrentCotIncomeGlID);
            ViewBag.CurrentInterestExpenseGlID = new SelectList(db.GlAccounts, "ID", "AccountName", accountConfiguration.CurrentInterestExpenseGlID);
            ViewBag.CurrentInterestPayableGlID = new SelectList(db.GlAccounts, "ID", "AccountName", accountConfiguration.CurrentInterestPayableGlID);
            ViewBag.LoanInterestExpenseGLID = new SelectList(db.GlAccounts, "ID", "AccountName", accountConfiguration.LoanInterestExpenseGLID);
            ViewBag.LoanInterestIncomeGlID = new SelectList(db.GlAccounts, "ID", "AccountName", accountConfiguration.LoanInterestIncomeGlID);
            ViewBag.LoanInterestReceivableGlID = new SelectList(db.GlAccounts, "ID", "AccountName", accountConfiguration.LoanInterestReceivableGlID);
            ViewBag.SavingsInterestExpenseGlID = new SelectList(db.GlAccounts, "ID", "AccountName", accountConfiguration.SavingsInterestExpenseGlID);
            ViewBag.SavingsInterestPayableGlID = new SelectList(db.GlAccounts, "ID", "AccountName", accountConfiguration.SavingsInterestPayableGlID);
            return View(accountConfiguration);
        }


        ConfigLogic configLogic = new ConfigLogic();

        // GET: AccountConfig/Details
        public ActionResult Details()
        {
            AccountConfiguration accountConfiguration = db.AccountConfigurations.First();
            //AccountConfiguration accountConfiguration = db.AccountConfigurations.Include(a => a.CurrentCotIncomeGl).Include(a => a.CurrentInterestExpenseGl).Include(a => a.LoanInterestExpenseGl).Include(a => a.LoanInterestIncomeGl).Include(a => a.LoanInterestReceivableGl).Include(a => a.SavingsInterestExpenseGl).Include(a => a.SavingsInterestPayableGl).First();
            if (accountConfiguration == null)
            {
                return HttpNotFound();
            }
            return View(accountConfiguration);
        }

        // GET: AccountConfig/Edit
        public ActionResult Edit()
        {
            AccountConfiguration accountConfiguration = db.AccountConfigurations.First();
            if (accountConfiguration == null)
            {
                return HttpNotFound();
            }
            SetGetGlActViewBags(accountConfiguration);
            return View(accountConfiguration);
        }

        // POST: AccountConfig/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IsBusinessOpen,FinancialDate,SavingsCreditInterestRate,SavingsMinimumBalance,SavingsInterestExpenseGlID,SavingsInterestPayableGlID,CurrentCreditInterestRate,CurrentMinimumBalance,CurrentCot,CurrentInterestExpenseGlID,CurrentCotIncomeGlID,CurrentInterestPayableGlID,LoanDebitInterestRate,LoanInterestIncomeGlID,LoanInterestExpenseGLID,LoanInterestReceivableGlID")] AccountConfiguration accountConfiguration)
        {
            if (ModelState.IsValid)
            {
                if (accountConfiguration.SavingsInterestExpenseGlID == 0) accountConfiguration.SavingsInterestExpenseGlID = null;
                if (accountConfiguration.SavingsInterestPayableGlID == 0) accountConfiguration.SavingsInterestPayableGlID = null;
                if (accountConfiguration.CurrentCotIncomeGlID == 0) accountConfiguration.CurrentCotIncomeGlID = null;
                if (accountConfiguration.CurrentInterestExpenseGlID == 0) accountConfiguration.CurrentInterestExpenseGlID = null;
                if (accountConfiguration.CurrentInterestPayableGlID == 0) accountConfiguration.CurrentInterestPayableGlID = null;
                if (accountConfiguration.LoanInterestExpenseGLID == 0) accountConfiguration.LoanInterestExpenseGLID = null;
                if (accountConfiguration.LoanInterestIncomeGlID == 0) accountConfiguration.LoanInterestIncomeGlID = null;
                if (accountConfiguration.LoanInterestReceivableGlID == 0) accountConfiguration.LoanInterestReceivableGlID = null;

                db.Entry(accountConfiguration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details");
            }
            SetPostGlActViewBags(accountConfiguration);
            return View(accountConfiguration);
        }

        // GET: AccountConfig/EditSavings
        public ActionResult EditSavings()
        {
            AccountConfiguration accountConfiguration = db.AccountConfigurations.First();
            if (accountConfiguration == null)
            {
                return HttpNotFound();
            }
            SetGetGlActViewBags(accountConfiguration);
            return View(accountConfiguration);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSavings([Bind(Include = "ID,IsBusinessOpen,FinancialDate,SavingsCreditInterestRate,SavingsMinimumBalance,SavingsInterestExpenseGlID,SavingsInterestPayableGlID,CurrentCreditInterestRate,CurrentMinimumBalance,CurrentCot,CurrentInterestExpenseGlID,CurrentCotIncomeGlID,CurrentInterestPayableGlID,LoanDebitInterestRate,LoanInterestIncomeGlID,LoanInterestExpenseGLID,LoanInterestReceivableGlID")] AccountConfiguration accountConfiguration)
        {
            if (ModelState.IsValid)
            {
                if(accountConfiguration.SavingsInterestExpenseGlID == 0) accountConfiguration.SavingsInterestExpenseGlID = null;
                if(accountConfiguration.SavingsInterestPayableGlID == 0) accountConfiguration.SavingsInterestPayableGlID = null;

                db.Entry(accountConfiguration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details");
            }
            SetPostGlActViewBags(accountConfiguration);
            return View(accountConfiguration);
        }

        // GET: AccountConfig/EditCurrent
        public ActionResult EditCurrent()
        {
            AccountConfiguration accountConfiguration = db.AccountConfigurations.First();
            if (accountConfiguration == null)
            {
                return HttpNotFound();
            }
            SetGetGlActViewBags(accountConfiguration);
            return View(accountConfiguration);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCurrent([Bind(Include = "ID,IsBusinessOpen,FinancialDate,SavingsCreditInterestRate,SavingsMinimumBalance,SavingsInterestExpenseGlID,SavingsInterestPayableGlID,CurrentCreditInterestRate,CurrentMinimumBalance,CurrentCot,CurrentInterestExpenseGlID,CurrentCotIncomeGlID,CurrentInterestPayableGlID,LoanDebitInterestRate,LoanInterestIncomeGlID,LoanInterestExpenseGLID,LoanInterestReceivableGlID")] AccountConfiguration accountConfiguration)
        {
            if (ModelState.IsValid)
            {
                if (accountConfiguration.CurrentCotIncomeGlID == 0) accountConfiguration.CurrentCotIncomeGlID = null;
                if (accountConfiguration.CurrentInterestExpenseGlID == 0) accountConfiguration.CurrentInterestExpenseGlID = null;
                if (accountConfiguration.CurrentInterestPayableGlID == 0) accountConfiguration.CurrentInterestPayableGlID = null;

                db.Entry(accountConfiguration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details");
            }
            SetPostGlActViewBags(accountConfiguration);
            return View(accountConfiguration);
        }

        // GET: AccountConfig/EditLoan
        public ActionResult EditLoan()
        {
            AccountConfiguration accountConfiguration = db.AccountConfigurations.First();
            if (accountConfiguration == null)
            {
                return HttpNotFound();
            }
            SetGetGlActViewBags(accountConfiguration);
            return View(accountConfiguration);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditLoan([Bind(Include = "ID,IsBusinessOpen,FinancialDate,SavingsCreditInterestRate,SavingsMinimumBalance,SavingsInterestExpenseGlID,SavingsInterestPayableGlID,CurrentCreditInterestRate,CurrentMinimumBalance,CurrentCot,CurrentInterestExpenseGlID,CurrentCotIncomeGlID,CurrentInterestPayableGlID,LoanDebitInterestRate,LoanInterestIncomeGlID,LoanInterestExpenseGLID,LoanInterestReceivableGlID")] AccountConfiguration accountConfiguration)
        {
            if (ModelState.IsValid)
            {
                if (accountConfiguration.LoanInterestExpenseGLID == 0) accountConfiguration.LoanInterestExpenseGLID = null;
                if (accountConfiguration.LoanInterestIncomeGlID == 0) accountConfiguration.LoanInterestIncomeGlID = null;
                if (accountConfiguration.LoanInterestReceivableGlID == 0) accountConfiguration.LoanInterestReceivableGlID = null;

                db.Entry(accountConfiguration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details");
            }
            SetPostGlActViewBags(accountConfiguration);
            return View(accountConfiguration);
        }

        // GET: AccountConfig/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountConfiguration accountConfiguration = db.AccountConfigurations.Find(id);
            if (accountConfiguration == null)
            {
                return HttpNotFound();
            }
            return View(accountConfiguration);
        }

        // POST: AccountConfig/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AccountConfiguration accountConfiguration = db.AccountConfigurations.Find(id);
            db.AccountConfigurations.Remove(accountConfiguration);
            db.SaveChanges();
            return RedirectToAction("Details");
        }

      
        private void SetGetGlActViewBags(AccountConfiguration accountConfiguration)
        {
            var allassets = configLogic.ExtractAssetGLs();
            var allincome = configLogic.ExtractIncomeGLs();
            var allexpense = configLogic.ExtractExpenseGLs();
            var allliability = configLogic.ExtractLiabilityGLs();

            //var allCurrentLiability = allliability.Where(g => g.GlCategory.Name.ToLowerInvariant().Contains("current")).ToList();
            //var x = allliability.Where(g => g.GlCategory.Name.ToLowerInvariant().Contains("savings")).ToList();
            //var allSavingsLiability = allliability.Where(g => g.GlCategory.Name.ToLowerInvariant().Contains("savings")).ToList();

            ViewBag.CurrentCotIncomeGlID = new SelectList(allincome, "ID", "AccountName", accountConfiguration.CurrentCotIncomeGlID == null ? 0 : accountConfiguration.CurrentCotIncomeGlID);
            ViewBag.CurrentInterestExpenseGlID = new SelectList(allexpense, "ID", "AccountName", accountConfiguration.CurrentInterestExpenseGlID == null ? 0 : accountConfiguration.CurrentInterestExpenseGlID);
            ViewBag.CurrentInterestPayableGlID = new SelectList(allliability, "ID", "AccountName", accountConfiguration.CurrentInterestPayableGlID == null ? 0 : accountConfiguration.CurrentInterestPayableGlID);
            ViewBag.LoanInterestExpenseGLID = new SelectList(allexpense, "ID", "AccountName", accountConfiguration.LoanInterestExpenseGLID == null ? 0 : accountConfiguration.LoanInterestExpenseGLID);
            ViewBag.LoanInterestIncomeGlID = new SelectList(allincome, "ID", "AccountName", accountConfiguration.LoanInterestIncomeGlID == null ? 0 : accountConfiguration.LoanInterestIncomeGlID);
            ViewBag.LoanInterestReceivableGlID = new SelectList(allassets, "ID", "AccountName", accountConfiguration.LoanInterestReceivableGlID == null ? 0 : accountConfiguration.LoanInterestReceivableGlID);
            ViewBag.SavingsInterestExpenseGlID = new SelectList(allexpense, "ID", "AccountName", accountConfiguration.SavingsInterestExpenseGlID == null ? 0 : accountConfiguration.SavingsInterestExpenseGlID);
            ViewBag.SavingsInterestPayableGlID = new SelectList(allliability, "ID", "AccountName", accountConfiguration.SavingsInterestPayableGlID == null ? 0 : accountConfiguration.SavingsInterestPayableGlID);
        }

        private void SetPostGlActViewBags(AccountConfiguration accountConfiguration)
        {
            var allassets = configLogic.ExtractAssetGLs();
            var allincome = configLogic.ExtractIncomeGLs();
            var allexpense = configLogic.ExtractExpenseGLs();
            var allliability = configLogic.ExtractLiabilityGLs();

            //var allCurrentLiability = allliability.Where(g => g.GlCategory.Name.ToLowerInvariant().Contains("current")).ToList();
            //var allSavingsLiability = allliability.Where(g => g.GlCategory.Name.ToLowerInvariant().Contains("savings")).ToList();

            ViewBag.CurrentCotIncomeGlID = new SelectList(allincome, "ID", "AccountName", accountConfiguration.CurrentCotIncomeGlID);
            ViewBag.CurrentInterestExpenseGlID = new SelectList(allexpense, "ID", "AccountName", accountConfiguration.CurrentInterestExpenseGlID);
            ViewBag.CurrentInterestPayableGlID = new SelectList(allliability, "ID", "AccountName", accountConfiguration.CurrentInterestPayableGlID);
            ViewBag.LoanInterestExpenseGLID = new SelectList(allexpense, "ID", "AccountName", accountConfiguration.LoanInterestExpenseGLID);
            ViewBag.LoanInterestIncomeGlID = new SelectList(allincome, "ID", "AccountName", accountConfiguration.LoanInterestIncomeGlID);
            ViewBag.LoanInterestReceivableGlID = new SelectList(allassets, "ID", "AccountName", accountConfiguration.LoanInterestReceivableGlID);
            ViewBag.SavingsInterestExpenseGlID = new SelectList(allexpense, "ID", "AccountName", accountConfiguration.SavingsInterestExpenseGlID);
            ViewBag.SavingsInterestPayableGlID = new SelectList(allliability, "ID", "AccountName", accountConfiguration.SavingsInterestPayableGlID);
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
