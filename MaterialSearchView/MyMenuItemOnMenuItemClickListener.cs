using System;

using Android.Views;
using Android.Runtime;

namespace MaterialSearchView
{
    internal sealed class MyMenuItemOnMenuItemClickListener : Java.Lang.Object, IMenuItemOnMenuItemClickListener, IJavaObject, IDisposable
    {
        private readonly Func<IMenuItem, bool> _onMenuItemClickFunc;

        public MyMenuItemOnMenuItemClickListener(Func<IMenuItem, bool> onMenuItemClickFunc)
        {
            this._onMenuItemClickFunc = onMenuItemClickFunc;
        }

        public bool OnMenuItemClick(IMenuItem item)
        {
            if (this._onMenuItemClickFunc != null)
                return this._onMenuItemClickFunc(item);
            return false;
        }
    }
}