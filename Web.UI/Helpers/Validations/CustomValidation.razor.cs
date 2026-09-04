using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Web.UI.Helpers.Validations
{
    public partial class CustomValidation
    {
        [CascadingParameter] private EditContext? EditContext { get; set; }

        private ValidationMessageStore? validationMessageStore;

        private bool hasErrors;

        public CustomValidation()
        {
            // We cannot instantiate validationMessageStore here because EditContext isn't set yet.
            // We'll assign it in OnInitialized.
        }

        protected override void OnInitialized()
        {
            if (EditContext == null)
            {
                throw new InvalidOperationException($"{nameof(CustomValidation)} must be used inside an EditForm.");
            }

            validationMessageStore = new ValidationMessageStore(EditContext);

            EditContext.OnValidationRequested += OnValidationRequested;

            EditContext.OnFieldChanged += OnFieldChanged;

            UpdateErrorDisplay();
        }

        public void DisplayError(string field, string message)
        {
            if (validationMessageStore == null || EditContext == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(field))
            {
                validationMessageStore.Add(EditContext.Field(string.Empty), message);
            }
            else
            {
                validationMessageStore.Add(EditContext.Field(field), message);
            }

            UpdateErrorDisplay();

            EditContext.NotifyValidationStateChanged();
        }


        public void DisplayError(string? field, string message, bool isLocalized = false)
        {

            if (validationMessageStore is null || EditContext is null)
            {
                return;
            }

            var localizedMessage = message;

            var fieldIdentifier = string.IsNullOrEmpty(field)
                ? EditContext.Field(string.Empty)
                : EditContext.Field(field);

            validationMessageStore.Add(fieldIdentifier, localizedMessage);

            UpdateErrorDisplay();

            EditContext.NotifyValidationStateChanged();
        }


        private void OnValidationRequested(object? sender, ValidationRequestedEventArgs e) => UpdateErrorDisplay();

        private void OnFieldChanged(object? sender, FieldChangedEventArgs e) => UpdateErrorDisplay();

        public void ClearErrors()
        {
            if (validationMessageStore == null || EditContext == null)
            {
                return;
            }

            validationMessageStore.Clear();

            UpdateErrorDisplay();

            EditContext.NotifyValidationStateChanged();
        }

        private void UpdateErrorDisplay()
        {
            hasErrors = EditContext?.GetValidationMessages().Any() ?? false;

            StateHasChanged();
        }

        public void Dispose()
        {
            // Unsubscribe to prevent memory leaks
            EditContext!.OnValidationRequested -= OnValidationRequested;
            EditContext!.OnFieldChanged -= OnFieldChanged;
        }
    }
}