using Android.Views;

namespace MaterialSearchView.Utils
{
    public abstract class SimpleAnimationListener : SimpleAnimationUtils.IAnimationListener
    {
        public bool OnAnimationStart(View view)
        {
            return false;
        }

        public bool OnAnimationEnd(View view)
        {
            return false;
        }

        public bool OnAnimationCancel(View view)
        {
            return false;
        }
    }
}