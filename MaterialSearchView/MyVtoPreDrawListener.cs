using System;

using Android.Views;
using Android.Runtime;

namespace MaterialSearchView
{
    internal sealed class MyVtoPreDrawListener : Java.Lang.Object, ViewTreeObserver.IOnPreDrawListener, IJavaObject, IDisposable
    {
        public Func<bool> PreDraw { get; set; }

        public bool OnPreDraw()
        {
            if (this.PreDraw != null)
                return this.PreDraw();
            return false;
        }
    }
}