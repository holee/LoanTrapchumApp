using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Domains.Enumerations
{
    public enum ProductTypeOption
    {
        [Description("Mortgage")]
        Mortgage,
        [Description("Car Loan")]
        CarLoan,
        [Description("Student Loan")]
        StudentLoan
    }
}
