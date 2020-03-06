using Express.Core.Models;
using ExpressCoreBank.Models;
using ExpressLogic;
using JUCBA.Logic;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace ExpressCoreBank.Controllers
{
    public class TellerPostingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        CustomerAccountLogic custActLogic = new CustomerAccountLogic();
        TellerPostingLogic telPostLogic = new TellerPostingLogic();
        FinancialReportLogic reportLogic = new FinancialReportLogic();
        EodLogic eodLogic = new EodLogic();

        // GET: TellerPosting
        public ActionResult Index()
        {
            var tellerPostings = db.TellerPostings.Include(t => t.CustomerAccount); //.Where(t => t.Status == PostStatus.Approved);
            return View(tellerPostings.ToList());
        }

        public ActionResult UserPosts()
        {
            string userId = GetLoggedInUserId();
            var userTellerPostings = db.TellerPostings.Include(t => t.CustomerAccount).Where(t => t.PostInitiatorId.Equals(userId));
            //return View("Index", userTellerPostings.ToList());
            return View(userTellerPostings.ToList());
        }

        // GET: TellerPosting/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TellerPosting tellerPosting = db.TellerPostings.Find(id);
            if (tellerPosting == null)
            {
                return HttpNotFound();
            }
            return View(tellerPosting);
        }

        public ActionResult SelectAccount()
        {
            /*if (eodLogic.isBusinessClosed())
            {

                //return a non-populated select acct view
                //return View(new List<CustomerAccount>());
                // return a view(partial view ? ) saying that business is closed
                return View("EODnotRUN");
            }*/
            // check whether user has till here and bounce? or later in create's post ?

            return View(db.CustomerAccounts.Include(a => a.Branch).Where(a => a.AccountType != AccountType.Loan)); //&& a.AccountStatus == AccountStatus.Active)); //&& a.Customer.Status == CustomerStatus.Active && a.Branch.Status == BranchStatus.Open).ToList());
        }

        // GET: TellerPosting/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerAccount customerAccount = db.CustomerAccounts.Find(id);
            if (customerAccount == null)
            {
                return HttpNotFound();
            }

            TellerPosting model = new TellerPosting();
            model.CustomerAccountID = customerAccount.Id;

            ViewBag.PostingType = string.Empty;

            return View(model);
        }

        // POST: TellerPosting/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Amount,Narration,PostingType,CustomerAccountID")] TellerPosting tellerPosting)
        {
            /*if (eodLogic.isBusinessClosed())
            {
                AddError("Sorry, business is closed");
                return View(tellerPosting); // closed partial view ?
            }*/

            if (ModelState.IsValid)
            {
                try
                {
                    string tellerId = GetLoggedInUserId();
                    // if user doesn't have till, error.. implement user till checking earlier ?
                    bool tellerHasTill = db.TillUsers.Any(tu => tu.UserId.Equals(tellerId));
                    if (!tellerHasTill)
                    {
                        AddError("No till associated with logged in teller");
                        return View(tellerPosting);
                    }
                    // get user's till and do all the necessary jargons  
                    int tillId = db.TillUsers.Where(tu => tu.UserId.Equals(tellerId)).First().GlAccountID;
                    tellerPosting.TillAccountID = tillId;
                    var tillAct = db.GlAccounts.Find(tillId);

                    var custAct = db.CustomerAccounts.Find(tellerPosting.CustomerAccountID);

                    tellerPosting.PostInitiatorId = tellerId;
                    tellerPosting.Date = DateTime.Now;

                    var amt = tellerPosting.Amount;

                    if (tellerPosting.PostingType == TellerPostingType.Withdrawal)
                    {
                        if (custActLogic.CustomerAccountHasSufficientBalance(custAct, amt))
                        {
                            if (!(tillAct.AccountBalance >= amt))
                            {
                                AddError("Insuficient funds in till account");
                                return View(tellerPosting);
                            }

                            tellerPosting.Status = PostStatus.Approved;
                            //db.TellerPostings.Add(tellerPosting);
                            //db.SaveChanges();

                            //all this is happening after transaction is approved by checker.
                            string result = telPostLogic.PostTeller(custAct, tillAct, amt, TellerPostingType.Withdrawal);
                            if (!result.Equals("success"))
                            {
                               AddError(result);
                                return View(tellerPosting);
                            }

                            db.Entry(custAct).State = EntityState.Modified;
                            db.Entry(tillAct).State = EntityState.Modified;
                            db.TellerPostings.Add(tellerPosting);
                            db.SaveChanges();

                            reportLogic.CreateTransaction(tillAct, amt, TransactionType.Credit);
                            reportLogic.CreateTransaction(custAct, amt, TransactionType.Debit);

                            return RedirectToAction("UserPosts"); // suceess partial view or pop-up ?
                        }
                        else
                        {
                            AddError("Insufficient funds in customer account");
                            return View(tellerPosting);
                        }
                    }
                    else
                    {
                        tellerPosting.Status = PostStatus.Approved;
                        //db.TellerPostings.Add(tellerPosting);
                        //db.SaveChanges();

                        string result = telPostLogic.PostTeller(custAct, tillAct, amt, TellerPostingType.Deposit);
                        custAct.AccountStatus = AccountStatus.Active;
                        if (!result.Equals("success"))
                        {
                            AddError(result);
                           return View(tellerPosting);
                        }

                        db.Entry(custAct).State = EntityState.Modified;
                        db.Entry(tillAct).State = EntityState.Modified;
                        db.TellerPostings.Add(tellerPosting);
                        db.SaveChanges();

                        reportLogic.CreateTransaction(tillAct, amt, TransactionType.Debit);
                        reportLogic.CreateTransaction(custAct, amt, TransactionType.Credit);

                        return RedirectToAction("UserPosts");
                    }
                }
                catch (Exception ex)
                {
                    AddError(ex.ToString());
                    return View(tellerPosting);
                }
            }
            AddError("Please, enter valid data");
            return View(tellerPosting);
        }

        // GET: TellerPosting/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TellerPosting tellerPosting = db.TellerPostings.Find(id);
            if (tellerPosting == null)
            {
                return HttpNotFound();
            }
            return View(tellerPosting);
        }

        // POST: TellerPosting/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TellerPosting tellerPosting = db.TellerPostings.Find(id);
            db.TellerPostings.Remove(tellerPosting);
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