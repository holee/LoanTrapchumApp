namespace Applications.Responses.ViewModels.OtherNames
{
    public class EntityNameVm
    {
        public long? EntityNameId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string TypeOf { get; set; } = string.Empty;
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }

        }
    }
}
