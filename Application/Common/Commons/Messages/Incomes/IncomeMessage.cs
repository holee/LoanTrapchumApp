using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.Messages.Incomes
{
    public static class IncomeMessage
    {
        public const string NotFound = "Income not found.";
        public const string AlreadyExists = "Income already exists.";
        public const string Created = "Income created successfully.";
        public const string Updated = "Income updated successfully.";
        public const string Deleted = "Income deleted successfully.";
        public const string Retrieved = "Income retrieved successfully.";
        public const string ListRetrieved = "Income list retrieved successfully.";
        public const string ServerError = "An error occurred while processing the income.";
        public const string confirmDelete = "Do you want to delete the income record?";
    }
}