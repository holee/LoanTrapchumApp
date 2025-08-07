namespace TrapchumApp.Helpers.Validations
{
    using Microsoft.AspNetCore.Components.Forms;

    public static class ValidationHelper
    {
        public static void ApplyFieldErrors<TModel>(EditContext editContext, Dictionary<string, string> fieldErrors)
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

        public static void ApplyFieldErrors<TModel>(this EditContext editContext, IDictionary<string, string[]> fieldErrors)
        {
            var messageStore = new ValidationMessageStore(editContext);

            messageStore.Clear();

            foreach (var error in fieldErrors)
            {
                var field = new FieldIdentifier(editContext.Model, error.Key);

                foreach (var err in error.Value)
                {
                    messageStore.Add(field, err);
                }

            }

            editContext.NotifyValidationStateChanged();
        }

        public static void ApplyGeneralError<TModel>(EditContext editContext, string errorMessage)
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

        public static void ApplyValidationSummary<TModel>(this EditContext editContext, params IList<string> errors)
        {
            var messageStore = new ValidationMessageStore(editContext);

            messageStore.Clear();

            var field = new FieldIdentifier(editContext.Model, string.Empty);

            foreach (var err in errors)
            {
                messageStore.Add(field, err);
            }

            editContext.NotifyValidationStateChanged();
        }

        public static void ApplyValidationSummary(this ValidationMessageStore store, EditContext editContext, IList<string> errors)
        {
            store.Clear();

            var field = new FieldIdentifier(editContext.Model, string.Empty);

            foreach (var err in errors)
            {
                store.Add(field, err);
            }
            editContext.NotifyValidationStateChanged();
        }


        public static void ClearMessages(this ValidationMessageStore store, EditContext editContext)
        {
            store.Clear();

            editContext.NotifyValidationStateChanged();
        }

        public static void Clear<TModel>(this EditContext editContext)
        {
            // You MUST use the same FieldIdentifier used when adding
            var field = new FieldIdentifier(editContext.Model, string.Empty);

            // Create a new message store, even though it's hacky
            var messageStore = new ValidationMessageStore(editContext);

            // Only clears messages for this specific FieldIdentifier
            messageStore.Clear(field);

            editContext.NotifyValidationStateChanged();
        }
    }

}
