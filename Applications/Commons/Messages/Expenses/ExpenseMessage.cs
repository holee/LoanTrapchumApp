using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Commons.Messages.Expenses
{
    public static class ExpenseMessage
    {
        public const string Created = "New Expense has been created.";
        public const string Updated = "Existing Expense has been updated.";
        public const string Deleted = "Existing Expense has been deleted.";
        public const string Error = "Something went wrong. Please try again later.";
        public const string NotFound = "Expense not found.";
        public const string ConfirmDelete = "Do you want to delete this expense?";
    }
}
