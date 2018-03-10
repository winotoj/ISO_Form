using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NCForm.Models
{
    public enum EnumTitle
    {
        [Display (Name ="Account Receivable")] AR,
        Customer,
        [Display(Name = "OD/Consumer ")] OD_Consumer,
        [Display(Name = "OD/Packaging ")] OD_Packaging,
        Purchasing,
        Receiving,
        [Display(Name = "Sales Consumer")] Sales_Consumer,
        [Display(Name = "Sales Packaging")] Sales_Packaging
    }
    public enum EnumInitiator
    {
        [Display(Name = "Account Payable")]
        AccountPayable,
        [Display(Name = "Customer Service")]
        CustomerService,
        Management,
        Purchasing,
        Sales,
        Warehouse
    }
    public enum EnumStatus
    {
        [Display(Name = "In Progress")]
        InProgress,
        Solved
    }
    public class Issue
    {
        public int ID { get; set; }
        [Required, Display(Description="Title / Subject")]
        //it was subject
        public EnumTitle? Title { get; set; }
        public DateTime CreatedDate { get; set; }       
        [Required, Display(Description = "Due Date")]
        public DateTime DueDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        [Required, Display(Description = "Initiated By")]
        public EnumInitiator? InitiatedBy { get; set; }
        [MaxLength(25)]
        public string ChangedBy { get; set; }
        [MaxLength(50), Display(Name = "Employee Name", Description = "Employee Name")]
        public string EmployeeName { get; set; }
        [MaxLength(25), Display(Description = "Ref ISO Proc")]
        public string RefIsoProc { get; set; }
        public bool Correction { get; set; } = false;
        [MaxLength(25), Display(Description = "Car Number")]
        public string CarNo { get; set; }
        [MaxLength(25)]
        [Display(Name ="PO No", Description = "Purchase Order No")]
        public string PurchaseOrderNo { get; set; }
        [MaxLength(25), Display(Description = "Vendor")]
        public string Vendor { get; set; }
        [MaxLength(25), Display(Description = "Customer")]
        public string Customer { get; set; }
        [MaxLength(25), Display(Name ="OE No", Description = "Order Entry Number")]
        public string OrderEntryNo { get; set; }
        [MaxLength(25), Display(Description = "Carrier")]
        public string Carrier { get; set; }
        [MaxLength(2500), Display(Description = "Description")]
        public string Description { get; set; }
        [MaxLength(2500), Display(Description = "QM Note")]
        public string QMNote { get; set; }
        [Display(Name = "Price on PO differ from Inv")]
        public bool CostErr { get; set; } = false;
        [Display(Name = "Not as expected in PO")]
        public bool Quality1 { get; set; } = false;
        [Display(Name = "Poor quality/defective in PO")]
        public bool Quality2 { get; set; } = false;
        [Display(Name = "Not as expected on OE")]
        public bool Quality3 { get; set; } = false;
        [Display(Name = "Poor quality/defective in OE")]
        public bool Quality4 { get; set; } = false;
        [Display(Name = "Customer changed mind")]
        public bool CustChange { get; set; } = false;
        [Display(Name = "Ship too early/late")]
        public bool ShipDateErr { get; set; }
        [Display(Name = "Customer ordered incorrectly")]
        public bool CustMistake { get; set; } = false;
        [Display(Name = "Incorrect product or Qty")]
        public bool Error1 { get; set; } = false;
        [Display(Name = "Did not enter instructions or notes")]
        public bool Error2 { get; set; } = false;
        [Display(Name = "Misunderstood what cust. wanted")]
        public bool Error3 { get; set; } = false;
        [Display(Name = "Key punch error")]
        public bool Error4 { get; set; } = false;
        [Display(Name = "Note/Instr. not followed")]
        public bool InstErr { get; set; } = false;
        [Display(Name = "Pricing error")]
        public bool PriceErr { get; set; } = false;
        [Display(Name = "Incorrect address")]
        public bool AddressErr { get; set; } = false;
        [Display(Name = "Duplicate order")]
        public bool Duplicate { get; set; } = false;
        [Display(Name = "Other")]
        public bool SOPOther { get; set; } = false;
		[Display(Name = "Delivery no appointment")]
        public bool DelNoApp { get; set; } = false;
        [Display(Name = "Delivery too early/late")]
        public bool DelTime { get; set; } = false;
        [Display(Name = "Delivery Qty short")]
        public bool DelShortShip { get; set; }
        [Display(Name = "Delivery Qty over")]
        public bool DelOverShip { get; set; }
        [Display(Name = "Packaging not as ordered")]
        public bool PackNotAsOrdered { get; set; } = false;
        [Display(Name = "Packaging not bilingual")]
        public bool PackNotBilingual { get; set; } = false;
        [Display(Name = "Packaging is dirty")]
        public bool PackDirty { get; set; } = false;
        [Display(Name = "Marking not as required")]
        public bool MarkNotAsReq { get; set; } = false;
        [Display(Name = "Marking too small")]
        public bool MarkTooSmall { get; set; } = false;
        [Display(Name = "Marking not where required")]
        public bool NotWhereReq { get; set; } = false;
        [Display(Name = "Damaged, Pallet damaged")]
        public bool DamagePallet { get; set; } = false;
        [Display(Name = "Damaged, Boxes crushed")]
        public bool DamageBoxesCrush { get; set; } = false;
        [Display(Name = "Damaged, Damage in transit")]
        public bool DamageInTransit { get; set; } = false;
        [Display(Name = "Pallet, not palletized")]
        public bool PalletNotpalletized { get; set; } = false;
        [Display(Name = "Pallet received incorrectly")]
        public bool PalletRcvIncorrect { get; set; } = false;
        [Display(Name = "other")]
        public bool WhOther { get; set; } = false;
        [Display(Name = "WM not assigned")]
        public bool WMNotAssigned { get; set; } = false;
        public EnumStatus Status { get; set; }
        public List<History> Histories { get; set; }

    }
}

