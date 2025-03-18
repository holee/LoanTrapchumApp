namespace Applications.Extensions
{

    public enum TabAction
    {
        Create,
        Edit,
        Details
    }
    public class TabItemsBase
    {
        public TabItemsBase()
        {
            TabIndex = 0;
            Text = string.Empty;
            Icon = string.Empty;
        }
        public TabItemsBase(int tabIndex, string tabText, string? icon)
        {
            TabIndex = tabIndex;
            Text = tabText;
            Icon = icon;
        }
        public int TabIndex { get; } = 0;
        public string Text { get; } = string.Empty;
        public string? Icon { get; } = string.Empty;
    }


    public class TabItems: TabItemsBase
    {
        public TabItems(int tabIndex,string tabText, string? icon,TabAction action)
            : base(tabIndex, tabText, icon)
        {
            TabAction = action;
        }
        public TabAction TabAction { get; } 

    }
}
