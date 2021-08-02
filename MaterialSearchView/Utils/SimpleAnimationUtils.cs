using System;
using System.Collections.Generic;

using Android.OS;
using Android.Views;
using Android.Graphics;
using Android.Animation;
using Android.Views.Animations;

using AndroidX.Annotations;
using AndroidX.Interpolator.View.Animation;

namespace MaterialSearchView.Utils
{
    public class SimpleAnimationUtils
    {
        public const int AnimationDurationDefault = 250;

        public static Animator RevealOrFadeIn([NonNull] View view)
        {
            return RevealOrFadeIn(view, 250);
        }

        public static Animator RevealOrFadeIn([NonNull] View view, int duration)
        {
            return RevealOrFadeIn(view, duration, null, null);
        }

        public static Animator RevealOrFadeIn([NonNull] View view, int duration, [Nullable] IAnimationListener listener)
        {
            return RevealOrFadeIn(view, duration, listener, null);
        }

        public static Animator RevealOrFadeIn([NonNull] View view, int duration, [Nullable] Point center)
        {
            return RevealOrFadeIn(view, duration, null, center);
        }

        public static Animator RevealOrFadeIn([NonNull] View view, [Nullable] IAnimationListener listener)
        {
            return RevealOrFadeIn(view, 250, listener, null);
        }

        public static Animator RevealOrFadeIn([NonNull] View view, [Nullable] Point center)
        {
            return RevealOrFadeIn(view, 250, null, center);
        }

        public static Animator RevealOrFadeIn([NonNull] View view, [Nullable] IAnimationListener listener, [Nullable] Point center)
        {
            return RevealOrFadeIn(view, 250, listener, center);
        }

        public static Animator RevealOrFadeIn([NonNull] View view, int duration, [Nullable] IAnimationListener listener, [Nullable] Point center)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
                return FadeIn(view, duration, listener);
            return Reveal(view, duration, listener, center);
        }

        public static Animator HideOrFadeOut([NonNull] View view, int duration)
        {
            return HideOrFadeOut(view, duration, null, null);
        }

        public static Animator HideOrFadeOut([NonNull] View view, int duration, [Nullable] IAnimationListener listener)
        {
            return HideOrFadeOut(view, duration, listener, null);
        }

        public static Animator HideOrFadeOut([NonNull] View view, int duration, [Nullable] Point center)
        {
            return HideOrFadeOut(view, duration, null, center);
        }

        public static Animator HideOrFadeOut([NonNull] View view)
        {
            return HideOrFadeOut(view, 250);
        }

        public static Animator HideOrFadeOut([NonNull] View view, [Nullable] IAnimationListener listener)
        {
            return HideOrFadeOut(view, 250, listener, null);
        }

        public static Animator HideOrFadeOut([NonNull] View view, [Nullable] Point center)
        {
            return HideOrFadeOut(view, 250, null, center);
        }

        public static Animator HideOrFadeOut([NonNull] View view, [Nullable] IAnimationListener listener, [Nullable] Point center)
        {
            return HideOrFadeOut(view, 250, listener, center);
        }

        public static Animator HideOrFadeOut([NonNull] View view, int duration, [Nullable] IAnimationListener listener, [Nullable] Point center)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
                return FadeOut(view, duration, listener);
            return Hide(view, duration, listener, center);
        }

        [RequiresApi(Api = 21)]
        public static Animator Reveal([NonNull] View view, int duration)
        {
            return Reveal(view, duration, null, null);
        }

        [RequiresApi(Api = 21)]
        public static Animator Reveal([NonNull] View view, int duration, [Nullable] Point center)
        {
            return Reveal(view, duration, null, center);
        }

        [RequiresApi(Api = 21)]
        public static Animator Reveal([NonNull] View view, int duration, [Nullable] IAnimationListener listener)
        {
            return Reveal(view, duration, listener, null);
        }

        [RequiresApi(Api = 21)]
        public static Animator Reveal([NonNull] View view)
        {
            return Reveal(view, 250);
        }

        [RequiresApi(Api = 21)]
        public static Animator Reveal([NonNull] View view, [Nullable] IAnimationListener listener)
        {
            return Reveal(view, 250, listener, null);
        }

        [RequiresApi(Api = 21)]
        public static Animator Reveal([NonNull] View view, [Nullable] Point center)
        {
            return Reveal(view, 250, null, center);
        }

        [RequiresApi(Api = 21)]
        public static Animator Reveal([NonNull] View view, [Nullable] IAnimationListener listener, [Nullable] Point center)
        {
            return Reveal(view, 250, listener, center);
        }

        [RequiresApi(Api = 21)]
        public static Animator Reveal([NonNull] View view, int duration, [Nullable] IAnimationListener listener, [Nullable] Point center)
        {
            if (center == null)
                center = GetDefaultCenter(view);
            var circularReveal = ViewAnimationUtils.CreateCircularReveal(view, center.X, center.Y, 0.0f, GetRevealRadius(center, view));
            circularReveal.AddListener(new DefaultActionAnimationListener(view, listener, v => v.Visibility = ViewStates.Visible, v => { }, v => { }));
            circularReveal.SetDuration(duration);
            circularReveal.SetInterpolator(GetDefaultInterpolator());
            return circularReveal;
        }

        [RequiresApi(Api = 21)]
        public static Animator Hide([NonNull] View view, int duration)
        {
            return Hide(view, duration, null, null);
        }

        [RequiresApi(Api = 21)]
        public static Animator Hide([NonNull] View view, int duration, [Nullable] Point center)
        {
            return Hide(view, duration, null, center);
        }

        [RequiresApi(Api = 21)]
        public static Animator Hide([NonNull] View view, int duration, [Nullable] IAnimationListener listener)
        {
            return Hide(view, duration, listener, null);
        }

        [RequiresApi(Api = 21)]
        public static Animator Hide([NonNull] View view)
        {
            return Hide(view, 250);
        }

        [RequiresApi(Api = 21)]
        public static Animator Hide([NonNull] View view, [Nullable] IAnimationListener listener)
        {
            return Hide(view, 250, listener, null);
        }

        [RequiresApi(Api = 21)]
        public static Animator Hide([NonNull] View view, [Nullable] Point center)
        {
            return Hide(view, 250, null, center);
        }

        [RequiresApi(Api = 21)]
        public static Animator Hide([NonNull] View view, [Nullable] IAnimationListener listener, [Nullable] Point center)
        {
            return Hide(view, 250, listener, center);
        }

        [RequiresApi(Api = 21)]
        public static Animator Hide([NonNull] View view, int duration, [Nullable] IAnimationListener listener, [Nullable] Point center)
        {
            if (center == null)
                center = GetDefaultCenter(view);
            var circularReveal = ViewAnimationUtils.CreateCircularReveal(view, center.X, center.Y, GetRevealRadius(center, view), 0.0f);
            circularReveal.AddListener(new DefaultActionAnimationListener(view, listener, v => { }, 
                v => v.Visibility = ViewStates.Gone, v => { }));
            circularReveal.SetDuration(duration);
            circularReveal.SetInterpolator(GetDefaultInterpolator());
            return circularReveal;
        }

        protected static Point GetDefaultCenter([NonNull] View view)
        {
            return new Point(view.Width / 2, view.Height / 2);
        }

        protected static int GetRevealRadius([NonNull] Point center, [NonNull] View view)
        {
            var num1 = 0.0f;
            var listPoints = new List<Point>
            {
              new Point(view.Left, view.Top),
              new Point(view.Right, view.Top),
              new Point(view.Left, view.Bottom),
              new Point(view.Right, view.Bottom)
            };

            foreach (Point second in listPoints)
            {
                float num2 = Distance(center, second);
                if ((double)num2 > num1)
                    num1 = num2;
            }
            return (int)Math.Ceiling(num1);
        }
    
        public static float Distance(Point first, Point second)
        {
            return (float)Math.Sqrt(Math.Pow(first.X - second.X, 2.0) + Math.Pow((first.Y - second.Y), 2.0));
        }

        public static Animator FadeIn([NonNull] View view)
        {
            return FadeIn(view, 250);
        }

        public static Animator FadeIn([NonNull] View view, int duration)
        {
            return FadeIn(view, duration, null);
        }

        public static Animator FadeIn([NonNull] View view, [Nullable] IAnimationListener listener)
        {
            return FadeIn(view, 250, listener);
        }

        public static Animator FadeIn([NonNull] View view, int duration, [Nullable] IAnimationListener listener)
        {
            if (view.Alpha == 1.0)
                view.Alpha = 0.0f;
            var objectAnimator = ObjectAnimator.OfFloat(view, "alpha", 1f);
            objectAnimator.AddListener(new DefaultActionAnimationListener(view, listener, v => v.Visibility = ViewStates.Visible, null, null));
            objectAnimator.SetDuration(duration);
            objectAnimator.SetInterpolator(GetDefaultInterpolator());
            return objectAnimator;
        }

        public static Animator FadeOut([NonNull] View view)
        {
            return FadeOut(view, 250);
        }

        public static Animator FadeOut([NonNull] View view, int duration)
        {
            return FadeOut(view, duration, null);
        }

        public static Animator FadeOut([NonNull] View view, [Nullable] IAnimationListener listener)
        {
            return FadeOut(view, 250, listener);
        }

        public static Animator FadeOut([NonNull] View view, int duration, [Nullable] IAnimationListener listener)
        {
            var objectAnimator = ObjectAnimator.OfFloat(view, "alpha", new float[1]);
            objectAnimator.AddListener(new DefaultActionAnimationListener(view, listener, null, 
                animation => view.Visibility = ViewStates.Gone, null));
            objectAnimator.SetDuration(duration);
            objectAnimator.SetInterpolator(GetDefaultInterpolator());
            return objectAnimator;
        }

        public static Animator VerticalSlideView([NonNull] View view, int fromHeight, int toHeight)
        {
            return VerticalSlideView(view, fromHeight, toHeight, null);
        }

        public static Animator VerticalSlideView([NonNull] View view, int fromHeight, int toHeight, int duration)
        {
            return VerticalSlideView(view, fromHeight, toHeight, duration, null);
        }

        public static Animator VerticalSlideView([NonNull] View view, int fromHeight, int toHeight, [Nullable] IAnimationListener listener)
        {
            return VerticalSlideView(view, fromHeight, toHeight, 250, listener);
        }

        public static Animator VerticalSlideView([NonNull] View view, int fromHeight, int toHeight, int duration, [Nullable] IAnimationListener listener)
        {
            var valueAnimator = ValueAnimator.OfInt(fromHeight, toHeight);
            valueAnimator.AddUpdateListener(new MyValueAnimatorUpdateListener(animation =>
            {
                view.LayoutParameters.Height = (int)animation.AnimatedValue;
                view.RequestLayout();
            }));
            valueAnimator.AddListener(new DefaultActionAnimationListener(view, listener, null, null, null));
            valueAnimator.SetDuration(duration);
            valueAnimator.SetInterpolator(GetDefaultInterpolator());
            return valueAnimator;
        }

        protected static IInterpolator GetDefaultInterpolator()
        {
            return new FastOutSlowInInterpolator();
        }

        public interface IAnimationListener
        {
            bool OnAnimationStart([NonNull] View view);

            bool OnAnimationEnd([NonNull] View view);

            bool OnAnimationCancel([NonNull] View view);
        }

        private sealed class DefaultActionAnimationListener : AnimatorListenerAdapter
        {
            private readonly View _view;
            private readonly IAnimationListener _listener;
            private readonly Action<View> _actionDefaultOnAnimationStart;
            private readonly Action<View> _actionDefaultOnAnimationEnd;
            private readonly Action<View> _actionDefaultOnAnimationCancel;

            public DefaultActionAnimationListener([NonNull] View view, [Nullable] IAnimationListener listener, Action<View> actionDefaultOnAnimationStart,
                Action<View> actionDefaultOnAnimationEnd, Action<View> actionDefaultOnAnimationCancel)
            {
                _view = view;
                _listener = listener;
                _actionDefaultOnAnimationStart = actionDefaultOnAnimationStart;
                _actionDefaultOnAnimationEnd = actionDefaultOnAnimationEnd;
                _actionDefaultOnAnimationCancel = actionDefaultOnAnimationCancel;
            }

            public override void OnAnimationStart(Animator animation)
            {
                if (_listener != null && _listener.OnAnimationStart(_view))
                    return;
                DefaultOnAnimationStart(_view);
            }

            public override void OnAnimationEnd(Animator animation)
            {
                if (_listener != null && _listener.OnAnimationEnd(_view))
                    return;
                DefaultOnAnimationEnd(_view);
            }

            public override void OnAnimationCancel(Animator animation)
            {
                if (_listener != null && _listener.OnAnimationCancel(_view))
                    return;
                DefaultOnAnimationCancel(_view);
            }

            private void DefaultOnAnimationStart([NonNull] View view)
            {
                var onAnimationStart = _actionDefaultOnAnimationStart;
                if (onAnimationStart == null)
                    return;
                onAnimationStart(view);
            }

            private void DefaultOnAnimationEnd([NonNull] View view)
            {
                var defaultOnAnimationEnd = _actionDefaultOnAnimationEnd;
                if (defaultOnAnimationEnd == null)
                    return;
                defaultOnAnimationEnd(view);
            }

            private void DefaultOnAnimationCancel([NonNull] View view)
            {
                Action<View> onAnimationCancel = _actionDefaultOnAnimationCancel;
                if (onAnimationCancel == null)
                    return;
                onAnimationCancel(view);
            }
        }

    }

}