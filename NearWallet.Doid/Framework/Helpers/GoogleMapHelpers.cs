using System;
using System.Linq;
using System.Collections.Generic;

using Android;
using Android.Util;
using Android.Gms.Maps;
using Android.Content.PM;

using AndroidX.Core.App;
using AndroidX.Core.Content;
using AndroidX.AppCompat.App;

namespace NearWallet.Doid.Framework.Helpers
{
    public static class GoogleMapHelpers
    {
        static readonly string TAG = "GoogleMapHelpers";

        static readonly string[] PERMISSIONS_LOCATION =
        {
            Manifest.Permission.AccessFineLocation,
            Manifest.Permission.AccessCoarseLocation
        };

        public static SupportMapFragment AddMapFragmentToLayout(this AppCompatActivity activity, int resourceId,
                                        IOnMapReadyCallback onMapReadyCallback = null, string tag = "map_frag")
        {
            var options = new GoogleMapOptions();
            options.InvokeMapType(GoogleMap.MapTypeHybrid)
                   .InvokeCompassEnabled(true);

            var mapFrag = SupportMapFragment.NewInstance(options);

            activity.SupportFragmentManager.BeginTransaction()
                    .Add(resourceId, mapFrag, tag)
                    .Commit();

            if (onMapReadyCallback == null)
            {
                if (activity is IOnMapReadyCallback callback)
                {
                    mapFrag.GetMapAsync(callback);
                }
                else
                {
                    throw new
                        ArgumentException("If the onMapReadyCallback is null, then the activity must implement the interface IOnMapReadyCallback.",
                                          nameof(activity));
                }
            }
            else
            {
                mapFrag.GetMapAsync(onMapReadyCallback);
            }

            return mapFrag;
        }

        public static bool MustShowPermissionRationale(this AppCompatActivity activity)
        {
            var showShowRationale = false;

            foreach (var perm in PERMISSIONS_LOCATION)
            {
                if (ActivityCompat.ShouldShowRequestPermissionRationale(activity, perm))
                {
                    showShowRationale = true;
                    Log.Debug(TAG, $"Need to show permission rational for {perm}.");
                }
            }

            return showShowRationale;
        }

        public static bool HasLocationPermissions(this AppCompatActivity activity)
        {
            var hasPermissions = true;
            foreach (var p in PERMISSIONS_LOCATION)
            {
                if (ContextCompat.CheckSelfPermission(activity, p) != (int)Permission.Granted)
                {
                    Log.Warn(TAG, $"App was not granted the {p} permission.");
                    hasPermissions = false;
                }
            }

            return hasPermissions;
        }

        public static bool AllPermissionsGranted(this IEnumerable<Permission> grantResults)
        {
            return grantResults.All(result => result != Permission.Denied);
        }
    }
}
