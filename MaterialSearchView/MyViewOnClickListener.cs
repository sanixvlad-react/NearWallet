using System;

using Android.Views;
using Android.Runtime;

namespace MaterialSearchView
{
    internal sealed class MyViewOnClickListener : Java.Lang.Object, View.IOnClickListener, IJavaObject, IDisposable
    {
        private readonly Action<View> _onClickAction;

        public MyViewOnClickListener(Action<View> onClickAction)
        {
            this._onClickAction = onClickAction;
        }

        public void OnClick(View v)
        {
            Action<View> onClickAction = this._onClickAction;
            if (onClickAction == null)
                return;
            onClickAction(v);
        }
    }
}