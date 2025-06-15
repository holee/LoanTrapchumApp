using Applications.Requests.Lists.OtherNames;

namespace TrapchumApp.Pages.Lists.OtherNames
{
    public partial class GaurantorCreateComp
    {
        [Parameter] public EntityGuarantorCreate Model { get; set; } = new();
        
        void SetFirstName(string e)
        {
            var value = e ?? string.Empty;

            if (!string.IsNullOrEmpty(value))
            {
                var names = value.Split(" ");
                if (names.Length == 1)
                {
                    Model.FirstName = names[0];
                    Model.LastName = string.Empty;
                }
                if (names.Length == 2)
                {
                    Model.FirstName = names[0];
                    Model.LastName = names[1];
                }
            }
            else
            {
                Model.FirstName = string.Empty;
                Model.LastName = string.Empty;
            }

        }

    }
}