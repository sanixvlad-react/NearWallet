using System;

using Android.OS;
using Android.Text;
using Android.Runtime;

using Java.Lang;

namespace MaterialSearchView
{
    internal sealed class MyTextWatcher : Java.Lang.Object, ITextWatcher, INoCopySpan, IJavaObject, IDisposable
    {
        private readonly Handler _handler = new Handler(Looper.MainLooper);
        private readonly Action<ICharSequence, int, int, int> _beforeTextChangedAction;
        private readonly Action<ICharSequence, int, int, int> _onTextChangedAction;
        private readonly Action<IEditable> _afterTextChangedAction;
        private Action _beforeTextChangedWorker;
        private Action _onTextChangedWorker;

        public MyTextWatcher(
          Action<ICharSequence, int, int, int> beforeTextChangedAction,
          Action<ICharSequence, int, int, int> onTextChangedAction,
          Action<IEditable> afterTextChangedAction)
        {
            _beforeTextChangedAction = beforeTextChangedAction;
            _onTextChangedAction = onTextChangedAction;
            _afterTextChangedAction = afterTextChangedAction;
        }

        public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {
            if (_beforeTextChangedWorker != null)
                _handler.RemoveCallbacks(_beforeTextChangedWorker);
            _beforeTextChangedWorker = (() =>
            {
                Action<ICharSequence, int, int, int> textChangedAction = _beforeTextChangedAction;
                if (textChangedAction == null)
                    return;
                textChangedAction(s, start, count, after);
            });
            _handler.PostDelayed(_beforeTextChangedWorker, 500L);
        }

        public void OnTextChanged(ICharSequence s, int start, int before, int count)
        {
            if (_onTextChangedWorker != null)
                _handler.RemoveCallbacks(_onTextChangedWorker);
            _onTextChangedWorker = (() =>
            {
                Action<ICharSequence, int, int, int> textChangedAction = _onTextChangedAction;
                if (textChangedAction == null)
                    return;
                textChangedAction(s, start, before, count);
            });
            _handler.PostDelayed(_onTextChangedWorker, 500L);
        }

        public void AfterTextChanged(IEditable s)
        {
            Action<IEditable> textChangedAction = _afterTextChangedAction;
            if (textChangedAction == null)
                return;
            textChangedAction(s);
        }
    }
}