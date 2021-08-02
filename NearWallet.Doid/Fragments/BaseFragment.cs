using Android.Content;
using AndroidX.SwipeRefreshLayout.Widget;

namespace NearWallet.Doid.Fragments
{
    public abstract class BaseFragment : AndroidX.Fragment.App.Fragment, SwipeRefreshLayout.IOnRefreshListener
    {
        public abstract void OnRefresh();

        
    }

}
