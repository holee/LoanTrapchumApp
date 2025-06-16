namespace TrapchumApp.Helpers.Validations
{
    using Microsoft.AspNetCore.Components.Forms;

    public static class ValidationHelper
    {
        public static void ApplyFieldErrors<TModel>(EditContext editContext,Dictionary<string, string> fieldErrors)
        {
            var messageStore = new ValidationMessageStore(editContext);

            messageStore.Clear();

            foreach (var error in fieldErrors)
            {
                var field = new FieldIdentifier(editContext.Model, error.Key);

                messageStore.Add(field, error.Value);
            }

            editContext.NotifyValidationStateChanged();
        }

        public static void ApplyGeneralError<TModel>(EditContext editContext,string errorMessage)
        {
            var messageStore = new ValidationMessageStore(editContext);

            messageStore.Clear();

            var field = new FieldIdentifier(editContext.Model, string.Empty);

            messageStore.Add(field, errorMessage);

            editContext.NotifyValidationStateChanged();
        }

        public static void ApplyValidationSummary<TModel>(this EditContext editContext, string errorMessage)
        {
            var messageStore = new ValidationMessageStore(editContext);
             
            messageStore.Clear();

            var field = new FieldIdentifier(editContext.Model, string.Empty);

            messageStore.Add(field, errorMessage);

            editContext.NotifyValidationStateChanged();
        }
    }

}
