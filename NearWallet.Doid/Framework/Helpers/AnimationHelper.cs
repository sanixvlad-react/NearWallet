using NearWallet.Doid.Fragments;
using FragmentX = AndroidX.Fragment.App.Fragment;

namespace NearWallet.Doid.Framework.Helpers
{
    public class AnimationHelper
    {
        public static int GetEnterAnimation(FragmentX fragment, string header)
        {
            if (fragment == null)
                return Resource.Animation.abc_fade_in;

            if (fragment is CoinFragment && header == fragment.Resources.GetString(Resource.String.collectibles))
                return Resource.Animation.side_in_right;

            if (fragment is NftFragment && header == fragment.Resources.GetString(Resource.String.balances))
                return Resource.Animation.side_in_left;


            return -1;
        }

        public static int GetExitAnimation(FragmentX fragment, string header)
        {
            if (fragment == null)
                return Resource.Animation.abc_fade_out;

            if (fragment is CoinFragment && header == fragment.Resources.GetString(Resource.String.collectibles))
                return Resource.Animation.side_out_left;

            if (fragment is NftFragment && header == fragment.Resources.GetString(Resource.String.balances))
                return Resource.Animation.side_out_right;


            return -1;
        }

    }
}
