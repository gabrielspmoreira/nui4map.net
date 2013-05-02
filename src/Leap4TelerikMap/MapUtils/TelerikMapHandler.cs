using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using MapUtils.Converter;
using MapUtils.Structs;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Map;
using NUI4Map.Handler;


namespace Leap4TelerikMap.MapUtils
{

    public class TelerikMapHandler : IMapHandler
    {

        private InformationLayer _informationLayer;
        private RadMap _radMap;

        public event Action MapInitialized;


        public void ClearFlags()
        {
            if (_informationLayer == null)
            {
                throw new NullReferenceException("Map not already created!");
            }
                
            _informationLayer.Items.Clear();
        }

        public UIElement CreateMap()
        {
            _radMap = new RadMap
                          {
                              ZoomLevel = 3,
                              MinZoomLevel = 3,
                              MaxZoomLevel = 6,
                              CommandBarVisibility = Visibility.Hidden,
                              NavigationVisibility = Visibility.Hidden,
                              ZoomBarVisibility = Visibility.Hidden,
                              ScaleVisibility = Visibility.Hidden,
                              MiniMapExpanderVisibility = Visibility.Hidden,
                              MouseLocationIndicatorVisibility = Visibility.Hidden,
                              UseSpringAnimations = true,
                              MouseClickMode = MouseBehavior.None
                          };

            _radMap.InitializeCompleted += MapInitializeCompleted;
            _radMap.Provider = GetMapProvider();
            var informationLayer = new InformationLayer {Visibility = Visibility.Visible};
            _informationLayer = informationLayer;
            _radMap.Items.Add(_informationLayer);
            return _radMap;
        }

        private void MapInitializeCompleted(object sender, EventArgs e)
        {

            if (MapInitialized != null)
            {
                MapInitialized();   
            }                
        }

        public void ShowClickedFlag(MapCoord coord, Uri imageUri)
        {
            ShowFlag(coord, imageUri, false);
        }

        private void ShowFlag(MapCoord coord, Uri imageUri, bool target)
        {
            if (_informationLayer == null)
            {
                throw new NullReferenceException("Map not already created!");
            }

            var hotSpot = new HotSpot();

            if (target)
            {
                hotSpot.X = 0.15;
                hotSpot.Y = 0.85;
            }
            else
            {
                hotSpot.X = 0.22;
                hotSpot.Y = 0.9;
            }

            var reprojCoord = coord.ToGeographic();
            var mapPinPoint = new MapPinPoint {ImageSource = new BitmapImage(imageUri)};
            MapLayer.SetHotSpot(mapPinPoint, hotSpot);
            MapLayer.SetLocation(mapPinPoint, new Location(reprojCoord.Latitude, reprojCoord.Longitude));
            _informationLayer.Items.Add(mapPinPoint);
        }

        public void ShowTargetFlag(MapCoord coord, Uri imageUri)
        {
            ShowFlag(coord, imageUri, true);
        }


        private MapProviderBase GetMapProvider()
        {
            var provider = new OpenStreetMapProvider();   
            /*provider.IsTileCachingEnabled = true;
            if (provider.IsTileCachingEnabled)
            {
                var cacheStorage = provider.CacheStorage as FileSystemCache;
                cacheStorage.CachePath = @"C:\Temp\TelerikMapCache\";
                cacheStorage.MaxExpirationTime = TimeSpan.FromDays(3650.0);
            }*/

            return provider;
        }
    }

}

