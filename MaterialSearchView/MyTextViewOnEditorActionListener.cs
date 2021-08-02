using System;

using Android.Views;
using Android.Widget;
using Android.Runtime;
using Android.Views.InputMethods;

namespace MaterialSearchView
{
    internal sealed class MyTextViewOnEditorActionListener : Java.Lang.Object, TextView.IOnEditorActionListener, IJavaObject, IDisposable
    {
        private readonly Func<TextView, ImeAction, KeyEvent, bool> _onEditorActionFunc;

        public MyTextViewOnEditorActionListener(
          Func<TextView, ImeAction, KeyEvent, bool> onEditorActionFunc)
        {
            this._onEditorActionFunc = onEditorActionFunc;
        }

        public bool OnEditorAction(TextView v, ImeAction actionId, KeyEvent e)
        {
            return this._onEditorActionFunc(v, actionId, e);
        }
    }
}