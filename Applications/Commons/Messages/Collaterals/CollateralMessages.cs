using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Commons.Messages.Collaterals
{
    public static class CollateralMessages
    {
        public const string NotFound = "Collateral not found.";
        public const string AlreadyExists = "Collateral already exists.";
        public const string Created = "Collateral created successfully.";
        public const string Updated = "Collateral updated successfully.";
        public const string Deleted = "Collateral deleted successfully.";
        public const string ErrorRetrieve = "An error occurred while retrieving collateral.";
        public const string ErrorCreate = "An error occurred while creating collateral.";
        public const string ErrorUpdate = "An error occurred while updating collateral.";
        public const string ErrorDelete = "An error occurred while deleting collateral.";
        public const string ConfirmDelete = @"Are you sure you want to delete this collateral line?
                                              Your Collateral Line will not recover after this action.";
        public const string ConfirmDisable = @"Collateral was used by other loan, so you cannot delete it from system.
                                            Do you want to disable instead of delete?"; 

    }
}
