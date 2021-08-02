using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;

using AndroidX.Core.Content;

namespace SwipeListView
{
    public class SwipeMenuItem
    {

        public int Id { get; set; }
        public Context Context { get; internal set; }
        public string Title { get; set; }
        public Drawable Icon { get; set; }
        public Drawable Background { get; set; }
        public Color TitleTextColor { get; set; }
        public int TitleTextSize { get; set; }
        public int Width { get; set; }

        public int IconRes
        {
            set => Icon = ContextCompat.GetDrawable(Context, value);
        }
        public int TitleRes
        {
            set => Title = Context.GetString(value);
        }
        public int BackgroundRes
        {
            set => Background = ContextCompat.GetDrawable(Context, value);
        }

        public SwipeMenuItem(Context context)
        {
            Context = context;
        }

    }
}
