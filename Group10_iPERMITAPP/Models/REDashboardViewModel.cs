using System;
using System.Collections.Generic;

namespace Group10_Project.Models
{
    public class REDashboardViewModel
    {
        public string REID { get; set; }
        public string ContactPersonName { get; set; }
        public string Email { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationAddress { get; set; }

        public List<REPermitRequestItemViewModel> Requests { get; set; } = new List<REPermitRequestItemViewModel>();
    }

    public class REPermitRequestItemViewModel
    {
        public string RequestNo { get; set; }
        public string PermitName { get; set; }
        public DateTime? DateOfRequest { get; set; }
        public string ActivityDescription { get; set; }
        public DateTime? ActivityStartDate { get; set; }
        public DateTime? ActivityDuration { get; set; }
        public double? PermitFee { get; set; }

        public string CurrentStatus { get; set; }
        public DateTime? StatusDate { get; set; }
        public string StatusDescription { get; set; }

        public bool CanPay { get; set; }
        public bool CanViewPermit { get; set; }
        public bool CanViewDecision { get; set; }
    }
}