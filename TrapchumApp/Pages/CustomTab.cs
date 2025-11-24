using TrapchumApp.Helpers.Constants;

namespace TrapchumApp.Pages
{
    internal enum TabActions
    {
        Unknown,
        List,
        Create,
        Details,
        Edit,
        Delete,
    }
    internal sealed class CustomTab
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
            Icon = icon;
            TabAction = tabAction;
            Content = content;
        }

        public string Key { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Icon
        {
            get => _icon;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _icon = TabAction switch
                    {
                        TabActions.List => IconNames.List,
                        TabActions.Create => IconNames.Add,
                        TabActions.Details => IconNames.Info,
                        TabActions.Edit => IconNames.Edit,
                        TabActions.Delete => IconNames.Delete,
                        _ => IconNames.Undo
                    };
                }
                else
                {
                    _icon = value;
                }


            }
        }
        public TabActions TabAction { get; set; } = TabActions.Unknown;
        public RenderFragment? Content { get; set; }

    }
}
