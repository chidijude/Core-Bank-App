using Express.Core.Models;
using ExpressCoreBank.Models;
using ExpressLogic;
using JUCBA.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace ExpressCoreBank.Controllers
{
    public class GlPostingController : Controller
    {
       
        
            private ApplicationDbContext db = new ApplicationDbContext();

            BusinessLogic busLogic = new BusinessLogic();
            FinancialReportLogic reportLogic = new FinancialReportLogic();
            EodLogic eodLogic = new EodLogic();

            // GET: GlPosting
            /*public ActionResult Index()
            {
            var glPostings = db.GlPostings.Include(g => g.CrGlAccount).Include(g => g.DrGlAccount).Where(g => g.Status == PostStatus.Approved);
                return View(glPostings.ToList());
            }

            public ActionResult UserPosts()
            {
                string userId = GetLoggedInUserId();
                var userGlPostings = db.GlPostings.Include(g => g.CrGlAccount).Include(g => g.DrGlAccount).Where(g => g.PostInitiatorId.Equals(userId));
                //return View("Index", userGlPostings.ToList());
                return View(userGlPostings.ToList());
            }*/

            // GET: GlPosting/Details/5
            public ActionResult Details(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                GlPosting glPosting = db.GlPostings.Find(id);
                if (glPosting == null)
                {
                    return HttpNotFound();
                }
                return View(glPosting);
            }

            public ActionResult SelectFirstAccount()
            {
                if (eodLogic.isBusinessClosed())
                {

                    //return a non-populated select credit acct view
                    //return View(new List<GlAccount>());
                    // return a view(partial view ? ) saying that business is closed
                    return View("EODnotRUN");
                }
                //db.GlAccounts.Include(g => g.GlCategory).Include(g => g.Branch).ToList()
                return View(db.GlAccounts.ToList());
            }

            public ActionResult SelectSecondAccount(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                GlAccount crglAccount = db.GlAccounts.Find(id);
                if (crglAccount == null)
                {
                    return HttpNotFound();
                }

                // populate second account table based on first account choice
                // e.g if glaccount main cat is asset, populate with only asset accounts.
                // exclude first selected account with from view of second.
                // but for now populate with everybody

                ViewBag.CrGlAccountID = crglAccount.ID;
                //var acts = db.GlAccounts.ToList();
                var acts = db.GlAccounts.Where(g => g.ID != crglAccount.ID && g.GlCategory.MainCategory == crglAccount.GlCategory.MainCategory).ToList();
                return View(acts);
            }

            // GET: GlPosting/Create
            public ActionResult Create(int? crId, int? drId)
            {
                if (crId == null || drId == null || crId == drId)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                GlAccount drglAccount = db.GlAccounts.Find(drId);
                GlAccount crglAccount = db.GlAccounts.Find(crId);
                if (drglAccount == null || crglAccount == null)
                {
                    return HttpNotFound();
                }

                GlPosting model = new GlPosting();
                model.DrGlAccountID = drglAccount.ID;
                model.CrGlAccountID = crglAccount.ID;

                return View(model);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Create([Bind(Include = "ID,CreditAmount,DebitAmount,Narration,DrGlAccountID,CrGlAccountID")] GlPosting glPosting)
            {
                if (eodLogic.isBusinessClosed())
                {
                    // return business closed view , for now use model error.
                    AddError("Sorry, business closed");
                    return View(glPosting);
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        var drAct = db.GlAccounts.Find(glPosting.DrGlAccountID);
                        var crAct = db.GlAccounts.Find(glPosting.CrGlAccountID);

                        if (crAct.AccountName.ToLower().Contains("till") || crAct.AccountName.ToLower().Contains("vault"))
                        {
                            if (crAct.AccountBalance < glPosting.CreditAmount)
                            {// is it really better to return an insuficient balance partial view ?
                                AddError("Insufficient funds in asset account to be credited");
                                return View(glPosting);
                            }
                        }

                        glPosting.PostInitiatorId = GetLoggedInUserId();
                        glPosting.Date = DateTime.Now;

                        glPosting.Status = PostStatus.Pending;

                        db.GlPostings.Add(glPosting);
                        db.SaveChanges();

                        return RedirectToAction("UserPosts"); // success post partial view or pop-up on post page ?
                    }
                    catch (Exception ex)
                    {
                        AddError(ex.ToString());
                        return View(glPosting);
                    }
                }
                AddError("Please, enter valid data");
                return View(glPosting);
            }

            // GET: GlPosting/Delete/5
            public ActionResult Delete(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                GlPosting glPosting = db.GlPostings.Find(id);
                if (glPosting == null)
                {
                    return HttpNotFound();
                }
                return View(glPosting);
            }

            // POST: GlPosting/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public ActionResult DeleteConfirmed(int id)
            {
                GlPosting glPosting = db.GlPostings.Find(id);
                db.GlPostings.Remove(glPosting);
                db.SaveChanges();
                return RedirectToAction("UserPosts");
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }

            private string GetLoggedInUserId()
            {
                return (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value;
            }

            private void AddError(string error)
            {
                ModelState.AddModelError("", error);
            }
        
    }
}