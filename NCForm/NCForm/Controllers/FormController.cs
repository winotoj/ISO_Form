using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NCForm.Models;
using NCForm.DAL;
using NCForm.ViewModels;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
//CHANGED LOG LEVEL TO WARN IN PRODUCTION
namespace NCForm.Controllers
{
    [Authorize]
    public class FormController : Controller
    {
        private static readonly log4net.ILog log
       = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private NonConformanceContext _context;

        public FormController()
        {
            _context = new NonConformanceContext();
        }
        // GET: Form
        public ActionResult Index()
        {
            return View();
        }
        //Get all issues where status is in-progress
        //
        //GET: Form/List
        public ActionResult List(string id)
        {
            if (!(id.Equals("InProgress") || id.Equals("All")) || id == null)
                return View("~/ErrorHandler/Notfound");
            List<ListViewModel> issue = null;
            try
            {
                issue = _context.Issues.AsQueryable().WhereIf(id == "InProgress", i => i.Status == 0).Select(i => new ListViewModel
                {
                    ID = i.ID,
                    Title = i.Title.Value.ToString(),
                    CreatedDate = i.CreatedDate,
                    DueDate = i.DueDate,
                    InitiatedBy = i.InitiatedBy.Value.ToString(),
                    Status = i.Status.ToString(),
                   // Status = i.Status.Value.ToString()
                }).ToList();                
            }
            catch(Exception e)
            {
                log.Error("Error Getting list" + e.Message);
            }
            return View(issue);
        }


        //Detail action to display the issue detail
        //GET: Form/Details/1
        public ActionResult Details(int id)
        {
            Issue detail = null;
            DetailViewModel dvm = null;
            try
            {
                detail = _context.Issues.SingleOrDefault(i => i.ID == id);
                if (detail == null)
                {
                    return View("Error");
                }
                dvm = new DetailViewModel();
                dvm.ID = detail.ID;
                dvm.Title = detail.Title.Value.ToString();
                dvm.CreatedDate = detail.CreatedDate;
                dvm.DueDate = detail.DueDate;
                dvm.ClosedDate = detail.ClosedDate;
                dvm.InitiatedBy = detail.InitiatedBy.Value.ToString();
                dvm.IsLate = detail.DueDate.Date < DateTime.Today ? (DateTime.Today - detail.DueDate.Date).Days + " day(s) late" : "Due in " + (detail.DueDate.Date - DateTime.Today.Date).Days + " day(s)";
                dvm.Status = detail.Status;
                dvm.Employee = detail.EmployeeName;
                dvm.Iso = detail.RefIsoProc;
                dvm.Correction = detail.Correction ? "Required" : "Not Required";
                dvm.CarNo = detail.CarNo;
                dvm.PoNo = detail.PurchaseOrderNo;
                dvm.Vendor = detail.Vendor;
                dvm.Customer = detail.Customer;
                dvm.Oe = detail.OrderEntryNo;
                dvm.Carrier = detail.Carrier;
                dvm.Description = detail.Description;
                dvm.QM = detail.QMNote;
                if (detail.CostErr)
                    dvm.OeError.Add("Cost Error, PO price differ from Invoice");
                if (detail.Quality1)
                    dvm.OeError.Add("Quality not as expected in PO");
                if (detail.Quality2)
                    dvm.OeError.Add("Poor quality/defective in PO");
                if (detail.Quality3)
                    dvm.OeError.Add("Quality not as expected in OE");
                if (detail.Quality4)
                    dvm.OeError.Add("Poor quality/defective in OE");
                if (detail.CustChange)
                    dvm.OeError.Add("Customer changed mind");
                if (detail.ShipDateErr)
                    dvm.OeError.Add("Ship too early/late in OE");
                if (detail.CustMistake)
                    dvm.OeError.Add("Customer made mistake");
                if (detail.Error1)
                    dvm.OeError.Add("Incorrect product or quantity");
                if (detail.Error2)
                    dvm.OeError.Add("Did not enter instructions");
                if (detail.Error3)
                    dvm.OeError.Add("Misunderstood what cust. wanted");
                if (detail.Error4)
                    dvm.OeError.Add("Key punch error");
                if (detail.InstErr)
                    dvm.OeError.Add("Note/Inst. not followed");
                if (detail.PriceErr)
                    dvm.OeError.Add("Pricing error");
                if (detail.AddressErr)
                    dvm.OeError.Add("Incorrect eddress");
                if (detail.Duplicate)
                    dvm.OeError.Add("Duplicate order");
                if (detail.SOPOther)
                    dvm.OeError.Add("Sales/PO/OE Other");
                if (detail.DelNoApp)
                    dvm.WhError.Add("Delivery no appointment");
                if (detail.DelTime)
                    dvm.WhError.Add("Delivery too early/late");
                if (detail.DelShortShip)
                    dvm.WhError.Add("Delivery quantity short");
                if (detail.DelOverShip)
                    dvm.WhError.Add("Delivery quantity over");
                if (detail.PackNotAsOrdered)
                    dvm.WhError.Add("Packaging not as ordered");
                if (detail.PackNotBilingual)
                    dvm.WhError.Add("Packaging not bilingual");
                if (detail.PackDirty)
                    dvm.WhError.Add("Packaging is dirty");
                if (detail.MarkNotAsReq)
                    dvm.WhError.Add("Marking not as required");
                if (detail.MarkTooSmall)
                    dvm.WhError.Add("Marking is too small");
                if (detail.NotWhereReq)
                    dvm.WhError.Add("Marking is not where required");
                if (detail.DamagePallet)
                    dvm.WhError.Add("Damage, pallet damaged");
                if (detail.DamageBoxesCrush)
                    dvm.WhError.Add("Damage, boxes crushed");
                if (detail.DamageInTransit)
                    dvm.WhError.Add("Damage, damaged in transit");
                if (detail.PalletNotpalletized)
                    dvm.WhError.Add("Pallet, not palletized");
                if (detail.PalletRcvIncorrect)
                    dvm.WhError.Add("Pallet received incorrect");
                if (detail.WMNotAssigned)
                    dvm.WhError.Add("Wh manager was not assigned");
                if (detail.WhOther)
                    dvm.WhError.Add("Other warehouse issue");
                dvm.Histories = _context.Histories.Where(i => i.IssueId == id).ToList();
            }catch(Exception e)
            {
                log.Fatal("Error Getting detail" + e.Message, e);
            }
            return View(dvm);
        }

        //GET : Form/New
        //[Authorize(Roles = RoleNames.CanManageMedia)]
        public ActionResult New()
        {
            Issue issue = new Issue() { ID = 0, DueDate = DateTime.Today };

            return View(issue);
        }


        //POST : Form/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = RoleNames.CanManageMedia)]
        public ActionResult Save(Issue issue)
        {
            //server side validation
            if (!ModelState.IsValid)
            {
                return View("New", issue);
            }
            //server side validation end
            try
            {
                if (issue.ID == 0)
                {
                    issue.CreatedDate = DateTime.Now;
                    _context.Issues.Add(issue);
                }
                _context.SaveChanges();
                int id = issue.ID;
                //Send email
                //var body = "<p>Email From: {0} ({1})</p><p>{2}</p>";
                //var message = new MailMessage();
                //message.To.Add(new MailAddress("minx1208@hotmail.com"));
                //message.Subject = issue.Title.Value.ToString();


            }catch(Exception e)
            {
                log.Error("Error saving New issue " + e.Message, e);
            }
            
            return RedirectToAction("List/InProgress", "Form");

        }
        public ActionResult SaveDetails(DetailViewModel dvm)
        {
            int counter = 0;
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details/" + dvm.ID);
            }
            
            Issue dvmInDb = null;
            try
            {
                dvmInDb = _context.Issues.Single(c => c.ID == dvm.ID);
                if (dvmInDb == null)
                {
                    return View("Error");
                }
                if (dvm.Message != null)
                {
                    History history = new History()
                    {
                        IssueId = dvmInDb.ID,
                        Creator = User.Identity.Name,
                        MsgDate = DateTime.Now,
                        Message = dvm.Message,
                        FileLoc = dvm.FileLoc
                    };
                    _context.Histories.Add(history);
                    counter++;
                }
                if(dvm.QM != null)
                {
                    dvmInDb.QMNote = dvm.QM;
                    counter++;
                }

                if (dvm.Status.ToString().Equals("Solved"))
                {
                    dvmInDb.ClosedDate = DateTime.Now;
                    dvmInDb.Status = dvm.Status;
                    counter++;
                }
                if (counter > 0) {
                _context.SaveChanges();
                return RedirectToAction("List/InProgress");
                }
            }
            catch(Exception e)
            {
                log.Error("Error saving edit " + e.Message, e);
            }
            return RedirectToAction("Details/" + dvm.ID);

        }

    }
    public static class LinqExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> whereClause)
        {
            if (condition)
            {
                return query.Where(whereClause);
            }
            return query;
        }
    }

}