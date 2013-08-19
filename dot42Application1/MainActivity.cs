﻿using System;
using Android.App;
using Android.Location;
using Android.Os;
using Android.Widget;
using Com.Google.Android.Gms.Maps;
using Com.Google.Android.Gms.Maps.Model;
using Dot42;
using Dot42.Manifest;

/*
 * Followed these steps.
 * https://developers.google.com/maps/documentation/android/start
 * https://developers.google.com/maps/documentation/android/map
 */

[assembly: Application("Google Maps!")]
[assembly: MetaData(Name = "com.google.android.maps.v2.API_KEY", Value = "YOUR_KEY_HERE")]
[assembly: UsesPermission("android.permission.INTERNET")]
[assembly: UsesPermission("android.permission.ACCESS_NETWORK_STATE")]
[assembly: UsesPermission("com.google.android.providers.gsf.permission.READ_GSERVICES")]
[assembly: UsesPermission("android.permission.WRITE_EXTERNAL_STORAGE")]
[assembly: UsesPermission("android.permission.ACCESS_COARSE_LOCATION")]
[assembly: UsesPermission("android.permission.ACCESS_FINE_LOCATION")]
namespace dot42Application1
{
    [Activity(Icon = "Icon")]
    public class MainActivity : Activity, ILocationListener
    {
        private GoogleMap mMap;
        private LocationManager service;
        private bool enabled;
        private string provider;
        //
        protected override void OnCreate(Bundle savedInstance)
        {
            base.OnCreate(savedInstance);
            SetContentView(R.Layouts.MainLayout);

            setUpMapIfNeeded();

            service = (LocationManager)GetSystemService(LOCATION_SERVICE);
            enabled = service.IsProviderEnabled(LocationManager.GPS_PROVIDER);
            if (!enabled)
            {
                return;
            }

            var criteria = new Criteria { Accuracy = Criteria.ACCURACY_FINE };
            provider = service.GetBestProvider(criteria, false);
            var location = service.GetLastKnownLocation(provider);
            if (null != location)
            {
                OnLocationChanged(location);
            }
        
        
        }

        private void setUpMapIfNeeded()
        {
            // Do a null check to confirm that we have not already instantiated the map.
            if (mMap == null)
            {
                mMap = ((MapFragment)GetFragmentManager().FindFragmentById(R.Ids.map)).GetMap();
                // Check if we were successful in obtaining the map.
                if (mMap != null)
                {
                    // The Map is verified. It is now safe to manipulate the map.
                    mMap.SetMapType(GoogleMap.MAP_TYPE_SATELLITE);

                    mMap.SetMyLocationEnabled(true);
                    // Zoom in the Google Map
                    mMap.AnimateCamera(CameraUpdateFactory.ZoomTo(20));

                }
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (enabled) service.RequestLocationUpdates(
                provider,
                2000, // minimum time interval between updates in ms
                1, // minimum distance between updates in m
                this);
        }

        protected override void OnPause()
        {
            base.OnPause();
            if (enabled) service.RemoveUpdates(this);
        }



        #region implement ILocationListener

        public void OnLocationChanged(Location location)
        {
            double latitude = location.Latitude;
            double longitude = location.Longitude;

            if (location != null)
            {
                if (mMap != null)
                {
                    // Create a LatLng object for the current location
                    LatLng latLng = new LatLng(latitude, longitude);

                    // Show the current location in Google Map        
                    mMap.MoveCamera(CameraUpdateFactory.NewLatLng(latLng));
                     
                    mMap.AnimateCamera(CameraUpdateFactory.ZoomTo(20));

                }
            }
        }

        public void OnStatusChanged(string provider, int status, Bundle bundle)
        {
        }

        public void OnProviderEnabled(string provider)
        {
            //txProvider.Text = "Enabled new provider " + provider;
        }

        public void OnProviderDisabled(string provider)
        {
            //txProvider.Text = "Disabled provider " + provider;
        }

        #endregion
    }
}
