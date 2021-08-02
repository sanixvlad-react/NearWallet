using System;

using Android.Runtime;

using Google.Android.Material.Tabs;

namespace MaterialSearchView
{
    [Obsolete]
    internal sealed class MyTabSelectedListener : Java.Lang.Object, TabLayout.IOnTabSelectedListener, IJavaObject, IDisposable
    {
        private readonly Action<TabLayout.Tab> _onTabSelectedAction;
        private readonly Action<TabLayout.Tab> _onTabUnselectedAction;
        private readonly Action<TabLayout.Tab> _onTabReselectedAction;

        public MyTabSelectedListener(
          Action<TabLayout.Tab> onTabSelectedAction,
          Action<TabLayout.Tab> onTabUnselectedAction,
          Action<TabLayout.Tab> onTabReselectedAction)
        {
            _onTabSelectedAction = onTabSelectedAction;
            _onTabUnselectedAction = onTabUnselectedAction;
            _onTabReselectedAction = onTabReselectedAction;
        }

        public void OnTabSelected(TabLayout.Tab tab)
        {
            var tabSelectedAction = _onTabSelectedAction;
            if (tabSelectedAction == null)
                return;
            tabSelectedAction(tab);
        }

        public void OnTabUnselected(TabLayout.Tab tab)
        {
            var unselectedAction = _onTabUnselectedAction;
            if (unselectedAction == null)
                return;
            unselectedAction(tab);
        }

        public void OnTabReselected(TabLayout.Tab tab)
        {
            var reselectedAction = _onTabReselectedAction;
            if (reselectedAction == null)
                return;
            reselectedAction(tab);
        }
    }
}