using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using Express.Core.ViewModels;
using System.Web.Mvc;
using ExpressCoreBank.Models;
using ExpressLogic;
using Express.Core.ViewModels.CustomerAccountViewModel;
using Express.Core.Models;

namespace ExpressCoreBank.Controllers
{
    public class CustomerAccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        

        CustomerAccountLogic custActLogic = new CustomerAccountLogic();
        BusinessLogic busLogic = new BusinessLogic();
        FinancialReportLogic reportLogic = new FinancialReportLogic();

        public ActionResult SelectCustomerAccount()
        {
            return View(db.Customers.ToList());
        }

        // GET: CustomerAccount
        public ActionResult Index()
        {
            var customerAccounts = db.CustomerAccounts.Include(c => c.Branch).Include(c => c.Customer).Include(c => c.ServicingAccount);
            return View(customerAccounts.ToList());
        }

        // GET: CustomerAccount/Details/5
        public ActionResult Details(int? id)
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
            return View(customerAccount);
        }

        public ActionResult CreateSavings(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            CreateAccountViewModel model = new CreateAccountViewModel();
            model.CustomerID = customer.ID;

            ViewBag.BranchID = new SelectList(db.Branches, "ID", "Name");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSavings([Bind(Include = "AccountName,BranchID,CustomerID")]CreateAccountViewModel model)
        {
            ViewBag.BranchID = new SelectList(db.Branches, "ID", "Name", model.BranchID);

            if (ModelState.IsValid)
            {
                try
                {
                    CustomerAccount customerAccount = new CustomerAccount();
                    customerAccount.AccountName = model.AccountName;
                    customerAccount.AccountType = AccountType.Savings;
                    customerAccount.CustomerID = model.CustomerID;
                    customerAccount.AccountNumber = custActLogic.GenerateCustomerAccountNumber(AccountType.Savings, model.CustomerID);
                    customerAccount.DateCreated = DateTime.Now;
                    customerAccount.BranchID = model.BranchID;
                    customerAccount.AccountStatus = AccountStatus.Closed;
                    customerAccount.AccountBalance = 0;

                    db.CustomerAccounts.Add(customerAccount);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    AddError(ex.ToString());
                    return View(model);
                }
            }
            AddError("Please enter valid data");
            return View(model);
        }

        public ActionResult CreateCurrent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            CreateAccountViewModel model = new CreateAccountViewModel();
            model.CustomerID = customer.ID;

            ViewBag.BranchID = new SelectList(db.Branches, "ID", "Name");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCurrent([Bind(Include = "AccountName,BranchID,CustomerID")]CreateAccountViewModel model)
        {
            ViewBag.BranchID = new SelectList(db.Branches, "ID", "Name", model.BranchID);

            if (ModelState.IsValid)
            {
                try
                {
                    CustomerAccount customerAccount = new CustomerAccount();
                    customerAccount.AccountName = model.AccountName;
                    customerAccount.AccountType = AccountType.Current;
                    customerAccount.CustomerID = model.CustomerID;
                    customerAccount.AccountNumber = custActLogic.GenerateCustomerAccountNumber(AccountType.Current, model.CustomerID);
                    customerAccount.DateCreated = DateTime.Now;
                    customerAccount.BranchID = model.BranchID;
                    customerAccount.AccountStatus = AccountStatus.Closed;
                    customerAccount.AccountBalance = 0;

                    db.CustomerAccounts.Add(customerAccount);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    AddError(ex.ToString());
                    return View(model);
                }
            }
            AddError("Please enter valid data");
            return View(model);
        }

        public ActionResult CreateLoan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            CreateLoanAccountViewModel model = new CreateLoanAccountViewModel();
            model.CustomerID = customer.ID;
            model.InterestRate = db.AccountConfigurations.First().LoanDebitInterestRate;
            model.NumberOfYears = 1;

            ViewBag.TermsOfLoan = string.Empty;
            ViewBag.BranchID = new SelectList(db.Branches, "ID", "Name");

            // for loan accounts only
            // servicing account, user will input account number, in the post account number is checked to see if it is a savings or current account that belongs to customer
            //ViewBag.ServicingAccountID = new SelectList(db.CustomerAccounts, "ID", "AccountName");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateLoan([Bind(Include = "AccountName,LoanAmount,TermsOfLoan,NumberOfYears,InterestRate,BranchID,CustomerID,ServicingAccountNumber")]CreateLoanAccountViewModel model)
        {
            ViewBag.BranchID = new SelectList(db.Branches, "ID", "Name", model.BranchID);

            if (ModelState.IsValid)
            {
                try
                {
                    // general settings
                    CustomerAccount customerAccount = new CustomerAccount();
                    customerAccount.AccountName = model.AccountName;
                    customerAccount.AccountType = AccountType.Loan;
                    customerAccount.CustomerID = model.CustomerID;
                    customerAccount.AccountNumber = custActLogic.GenerateCustomerAccountNumber(AccountType.Loan, model.CustomerID);
                    customerAccount.DateCreated = DateTime.Now;
                    customerAccount.BranchID = model.BranchID;
                    customerAccount.AccountStatus = AccountStatus.Closed;
                    customerAccount.AccountBalance = 0;

                    // loan specific settings
                    customerAccount.LoanAmount = model.LoanAmount;
                    customerAccount.TermsOfLoan = model.TermsOfLoan;

                    long actNo = Convert.ToInt64(model.ServicingAccountNumber);

                    var servAct = db.CustomerAccounts.Where(a => a.AccountNumber == actNo).SingleOrDefault();
                    if (servAct == null)
                    {
                        AddError("Servicing account number does not exist");
                        return View(model);
                    }
                    // check if servicing account number actually belongs to customer and is either savings or current.
                    if (servAct.AccountType == AccountType.Loan || servAct.CustomerID != model.CustomerID)
                    {
                        AddError("Invalid servicing account");
                        return View(model);
                    }
                    // this checks if a Customer can get loans. it avoids creation of new accounts and taking loans
                    decimal loanEligible = 2000;
                    if (servAct.AccountBalance < loanEligible)
                    {
                        AddError("Service Account Balance Below Loan Eligible range");
                        return View(model);
                    }
                    // checks if the customer account linked with loan is active
                    if (servAct.AccountStatus == AccountStatus.Closed)
                    {
                        AddError("Servicing account is closed");
                        return View(model);
                    }

                    // get the id
                    customerAccount.ServicingAccountID = servAct.Id;

                    customerAccount.LoanInterestRatePerMonth = Convert.ToDecimal(model.InterestRate);
                    switch (model.TermsOfLoan)
                    {
                        case TermsOfLoan.Fixed:
                            custActLogic.ComputeFixedRepayment(customerAccount, model.NumberOfYears, model.InterestRate);
                            break;
                        case TermsOfLoan.Reducing:
                            custActLogic.ComputeReducingRepayment(customerAccount, model.NumberOfYears, model.InterestRate);
                            break;
                        default:
                            break;
                    }

                    // loan disbursement which happens after validations.
                    busLogic.DebitCustomerAccount(customerAccount, customerAccount.LoanAmount);
                    busLogic.CreditCustomerAccount(servAct, customerAccount.LoanAmount);

                    db.Entry(servAct).State = EntityState.Modified;
                    db.CustomerAccounts.Add(customerAccount);
                    db.SaveChanges();

                    // financial report logging
                    reportLogic.CreateTransaction(customerAccount, customerAccount.LoanAmount, TransactionType.Debit);
                    reportLogic.CreateTransaction(servAct, customerAccount.LoanAmount, TransactionType.Credit);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    AddError(ex.ToString());
                    return View(model);
                }
            }
            AddError("Please enter valid data");
            return View(model);
        }

        // GET: CustomerAccount/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.BranchID = new SelectList(db.Branches, "ID", "Name", customerAccount.BranchID);

            return View(customerAccount);
        }

        // POST: CustomerAccount/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AccountNumber,AccountName,BranchID,AccountBalance,DateCreated,DaysCount,dailyInterestAccrued,LoanInterestRatePerMonth,AccountType,AccountStatus,SavingsWithdrawalCount,CurrentLien,CustomerID,LoanAmount,LoanMonthlyRepay,LoanMonthlyInterestRepay,LoanMonthlyPrincipalRepay,LoanPrincipalRemaining,TermsOfLoan,ServicingAccountID")] CustomerAccount customerAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BranchID = new SelectList(db.Branches, "ID", "Name", customerAccount.BranchID);
            return View(customerAccount);
        }

        public ActionResult ChangeAccountStatus(int? id)
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
            if (customerAccount.AccountStatus == AccountStatus.Active)
            {
                customerAccount.AccountStatus = AccountStatus.Closed;
            }
            else
            {
                customerAccount.AccountStatus = AccountStatus.Active;
            }
            db.Entry(customerAccount).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: CustomerAccount/Delete/5
        public ActionResult Delete(int? id)
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
            return View(customerAccount);
        }

        // POST: CustomerAccount/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerAccount customerAccount = db.CustomerAccounts.Find(id);
            db.CustomerAccounts.Remove(customerAccount);
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
