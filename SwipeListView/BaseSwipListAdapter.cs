
using Android.Widget;

namespace SwipeListView
{
    public abstract class BaseSwipListAdapter : BaseAdapter
    {
        public bool GetSwipEnableByPosition(int position)
        {
            return true;
        }
    }
}
