using System.Collections.Generic;

using Android.App;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.UI;
using Android.Gms.Maps.Utils.Clustering;
using Android.Gms.Maps.Utils.Clustering.View;

using Java.Lang;

namespace NearWallet.Doid.Views.MapCluster
{
    public class ClusterRenderer : DefaultClusterRenderer
    {
        private IconGenerator iconGeneratorForMarkerGroup;
        private ImageView imageviewForMarkerGroup;
        public Dictionary<string, ClusterItem> dicAllMarkerOnMap;

        public ClusterRenderer(Activity context, GoogleMap map, ClusterManager clusterManager, Dictionary<string, ClusterItem> dicAllMarkerOnMap)
            : base(context, map, clusterManager)
        {
            this.dicAllMarkerOnMap = dicAllMarkerOnMap;
            InitViewForMarkerGroup(context);
        }

        private void InitViewForMarkerGroup(Activity context)
        {
            //Retrieve views from AXML to display groups of markers (clustering)
            View viewMarkerClusterGrouped = context.LayoutInflater.Inflate(Resource.Layout.marker_cluster_grouped, null);
            imageviewForMarkerGroup = viewMarkerClusterGrouped.FindViewById<ImageView>(Resource.Id.marker_cluster_grouped_imageview);

            //Configure the groups of markers icon generator with the view. The icon generator will be used to display the marker's picture with a text
            iconGeneratorForMarkerGroup = new IconGenerator(context);
            iconGeneratorForMarkerGroup.SetContentView(viewMarkerClusterGrouped);
            iconGeneratorForMarkerGroup.SetBackground(null);
        }

        //Draw a single marker
        protected override void OnBeforeClusterItemRendered(Java.Lang.Object p0, MarkerOptions markerOptions)
        {
            ClusterItem clusterItem = (ClusterItem)p0;

            //Icon for single marker
            markerOptions.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.ic_marker));


            //Text for Info Window
            markerOptions.SetTitle("Grenoble");
            markerOptions.SetSnippet(markerOptions.Position.Latitude.ToString() + ", " + markerOptions.Position.Longitude.ToString());
        }

        //Draw a grouped marker
        protected override void OnBeforeClusterRendered(ICluster p0, MarkerOptions markerOptions)
        {
            //imageviewForMarkerGroup.SetImageResource(Resource.Drawable.marker_cluster_grouped);
            Bitmap icon = iconGeneratorForMarkerGroup.MakeIcon(p0.Size.ToString());
            markerOptions.SetIcon(BitmapDescriptorFactory.FromBitmap(icon));
        }

        //After a cluster item have been rendered to a marker
        protected override void OnClusterItemRendered(Object item, Marker marker)
        {
            base.OnClusterItemRendered(item, marker);

            ClusterItem clusterItem = (ClusterItem)item;

            if (!dicAllMarkerOnMap.ContainsKey(marker.Id))
                dicAllMarkerOnMap.Add(marker.Id, clusterItem);
        }
    }
}
