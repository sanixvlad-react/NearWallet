using System;
using Android.Views;


namespace MaterialSearchView.Utils
{
    public sealed class MySimpleAnimationListener : SimpleAnimationUtils.IAnimationListener
    {
        private readonly Func<View, bool> _funcOnAnimationStart;
        private readonly Func<View, bool> _funcOnAnimationEnd;
        private readonly Func<View, bool> _funcOnAnimationCancel;

        public MySimpleAnimationListener(Func<View, bool> funcOnAnimationStart, Func<View, bool> funcOnAnimationEnd, Func<View, bool> funcOnAnimationCancel)
        {
            _funcOnAnimationStart = funcOnAnimationStart;
            _funcOnAnimationEnd = funcOnAnimationEnd;
            _funcOnAnimationCancel = funcOnAnimationCancel;
        }

        public bool OnAnimationStart(View view)
        {
            if (_funcOnAnimationStart != null)
                return _funcOnAnimationStart(view);
            return false;
        }

        public bool OnAnimationEnd(View view)
        {
            if (_funcOnAnimationEnd != null)
                return _funcOnAnimationEnd(view);
            return false;
        }

        public bool OnAnimationCancel(View view)
        {
            if (_funcOnAnimationCancel != null)
                return _funcOnAnimationCancel(view);
            return false;
        }
    }
}