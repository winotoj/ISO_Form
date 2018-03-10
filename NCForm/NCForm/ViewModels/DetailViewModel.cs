using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NCForm.Models;
using NCForm.Controllers;

namespace NCForm.ViewModels
{
    public class DetailViewModel
    {
        private List<string> _WhError = new List<string>();
        private List<string> _OeError = new List<string>();
        public List<History> Histories { get; set; }
        public string Message { get; set; }
        public string FileLoc { get; set; }
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string InitiatedBy { get; set; }
        public string IsLate { get; set; }
        public EnumStatus Status { get; set; }
        public string Employee { get; set; }
        public string Iso { get; set; }
        public string Correction { get; set; }
        public string CarNo { get; set; }
        public string PoNo { get; set; }
        public string Vendor { get; set; }
        public string Customer { get; set; }
        public string Oe { get; set; }
        public string Carrier { get; set; }
        public string Description { get; set; }
        public string QM { get; set; }
        public List<string> WhError
        {
            get => _WhError; set => _WhError = value;
        }
    public List<string> OeError
        {
            get => _OeError; set => _OeError = value;
        }
    }
}