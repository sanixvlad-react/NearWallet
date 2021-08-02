using System;

using Android.Runtime;
using Android.Animation;

namespace MaterialSearchView
{
    internal sealed class MyValueAnimatorUpdateListener : Java.Lang.Object, ValueAnimator.IAnimatorUpdateListener, IJavaObject, IDisposable
    {
        private readonly Action<ValueAnimator> _onAnimationUpdateAction;

        public MyValueAnimatorUpdateListener(Action<ValueAnimator> onAnimationUpdateAction)
        {
            this._onAnimationUpdateAction = onAnimationUpdateAction;
        }

        public void OnAnimationUpdate(ValueAnimator animation)
        {
            Action<ValueAnimator> animationUpdateAction = this._onAnimationUpdateAction;
            if (animationUpdateAction == null)
                return;
            animationUpdateAction(animation);
        }
    }
}