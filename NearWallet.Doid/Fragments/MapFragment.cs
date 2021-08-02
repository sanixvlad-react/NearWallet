
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Android.OS;
using Android.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Runtime;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.Clustering;

using AndroidX.AppCompat.App;

using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

using NearWallet.Doid.Views.MapCluster;

using NearWallet.Core;

namespace NearWallet.Doid.Fragments
{
    public class MapFragment : BaseFragment, IOnMapReadyCallback
    {
        private GoogleMap map;
        public GoogleMap Map
        {
            get => map;
            set
            {
                map = value;
                InitializeUiSettingsOnMap();
            }
        }
        private MapView mapView;

        private ClusterManager clusterManager;
        private ClusterRenderer clusterRenderer;

        public Dictionary<string, ClusterItem> m_dicAllMarkerOnMap = new Dictionary<string, ClusterItem>();

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle bundle)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_map, container, false);

            ((MainActivity)Activity).IsEnabledRefresh = false;

            mapView = view.FindViewById<MapView>(Resource.Id.map_view);
            mapView.OnCreate(bundle);
            mapView.GetMapAsync(this);

            return view;
        }

        #region Map
        private void InitializeUiSettingsOnMap()
        {
            try
            {
                Map.UiSettings.MyLocationButtonEnabled = true;
                Map.MyLocationEnabled = true;
                Map.UiSettings.CompassEnabled = true;
                Map.UiSettings.RotateGesturesEnabled = false;
                Map.MapType = GoogleMap.MapTypeNormal;

                //Map.CameraChange += Map_CameraChange;
                //Map.InfoWindowClick += Map_InfoWindowClick;

                ConfigCluster();

                SetMyPositionOnMap();
            }
            catch
            {

            }
        }

        private void ConfigCluster()
        {
            //Initialize cluster manager.
            clusterManager = new ClusterManager(Activity, map);

            map.SetOnCameraIdleListener(clusterManager);

            SetupMap();

            //Initialize cluster renderer, and keep a reference that will be usefull for the InfoWindowsAdapter
            //clusterRenderer = new ClusterRenderer(Activity, map, clusterManager, m_dicAllMarkerOnMap);
            //clusterManager.Renderer = clusterRenderer;

            //Custom info window : single markers only (a click on a cluster marker should not show info window)
            //clusterManager.MarkerCollection.SetOnInfoWindowAdapter(m_InfoWindowAdapter);

            //Handle Info Window's click event
            //clusterManager.SetOnClusterItemInfoWindowClickListener(Activity);
        }

        private void SetupMap()
        {
            List<ClusterItem> lsMarkers = new List<ClusterItem>();
            foreach(var item in Data.PlaceModels)
                lsMarkers.Add(new ClusterItem(item.Latitude, item.Longitude, item.Title));

            clusterManager.AddItems(lsMarkers);
        }

        /// <summary>
        /// установить местоположение пользователя
        /// </summary>
        public async void SetMyPositionOnMap()
        {
            try
            {
                Position position = null;

                if (CrossGeolocator.Current.IsGeolocationAvailable)
                {
                    if (CrossGeolocator.Current.IsGeolocationEnabled)
                    {
                        position = await CrossGeolocator.Current.GetPositionAsync();
                    }
                    else
                    {

                    }
                }

                if (position == null) return;

                LatLng latlng = new LatLng(position.Latitude, position.Longitude);

                CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 13);
                Map.MoveCamera(camera);
            }
            catch
            {

            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            Map = googleMap;
        }
        #endregion

        #region Override Methodts
        public override void OnRefresh()
        {
            
        }

        public override void OnPause()
        {
            base.OnPause();
            mapView.OnPause();
        }

        public override void OnResume()
        {
            base.OnResume();
            mapView.OnResume();
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            mapView.OnSaveInstanceState(outState);
        }
        #endregion
    }
}
