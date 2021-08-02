using System;

using Android.Views;
using Android.Runtime;

namespace MaterialSearchView
{
    public sealed class MyOnFocusChangeListener : Java.Lang.Object, View.IOnFocusChangeListener, IJavaObject, IDisposable
    {
        private readonly Action<View, bool> _onFocusChangeAction;

        public MyOnFocusChangeListener(Action<View, bool> onFocusChangeAction)
        {
            this._onFocusChangeAction = onFocusChangeAction;
        }

        public void OnFocusChange(View v, bool hasFocus)
        {
            Action<View, bool> focusChangeAction = this._onFocusChangeAction;
            if (focusChangeAction == null)
                return;
            focusChangeAction(v, hasFocus);
        }
    }
}