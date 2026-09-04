using Microsoft.AspNetCore.Components;
using Web.UI.Helpers.Constants;

namespace Web.UI.Pages
{
    public enum TabActions
    {
        Unknown,
        List,
        Create,
        Details,
        Edit,
        Delete,
    }

    public sealed class CustomTab
    {
        private string _icon = string.Empty;

        public CustomTab()
        {
        }

        public CustomTab(string key, string title, TabActions tabAction, RenderFragment? content)
        {
            Key = key;
            Title = title;
            TabAction = tabAction;
            Content = content;
        }

        public CustomTab(string key, string title, string icon, TabActions tabAction, RenderFragment? content)
        {
            Key = key;
            Title = title;
            TabAction = tabAction;
            Content = content;
        }

        public string Key { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Icon
        {
            get
            {
                return TabAction switch
                {
                    TabActions.List => IconNames.List,
                    TabActions.Create => IconNames.Add,
                    TabActions.Details => IconNames.Info,
                    TabActions.Edit => IconNames.Edit,
                    TabActions.Delete => IconNames.Delete,
                    _ => IconNames.Undo
                };
            }
        }
        public TabActions TabAction { get; set; } = TabActions.Unknown;
        public RenderFragment? Content { get; set; }
    }
}
