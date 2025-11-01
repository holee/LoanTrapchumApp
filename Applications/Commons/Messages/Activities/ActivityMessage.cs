using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Commons.Messages.Activities
{
    public static class ActivityMessage
    {
        public const string GetSuccess = "Activity retrieved successfully.";
        public const string GetFailed = "Failed to retrieve activity.";
        public const string CreateSuccess = "Activity created successfully.";
        public const string CreateFailed = "Failed to create activity.";
        public const string UpdateSuccess = "Activity updated successfully.";
        public const string UpdateFailed = "Failed to update activity.";
        public const string DeleteSuccess = "Activity deleted successfully.";
        public const string DeleteFailed = "Failed to delete activity."; 
        public const string NotFound = "Customer Activity not found.";

        public const string ConfirmDelete = "Are you sure you want to delete this activity?"; 

    }
}
