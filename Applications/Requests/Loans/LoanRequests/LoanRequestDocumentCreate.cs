using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Requests.Loans.LoanRequests
{
    public sealed class LoanRequestDocumentCreate
    {
        public int BranchId { get; set; }
        public string? LoanRequestRefId { get; set; }
        public string? CustomerRefId { get; set; }

    }
}
