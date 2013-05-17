using System;
using System.Windows;
using System.Windows.Media.Imaging;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Symbols;
using EsriMapCommons.Extensions;
using MapUtils.Structs;
using NUI4Map.Handler;

namespace EsriMapCommons.Handler
{
    public class EsriMapHandler : IMapHandler
    {
        private Map _map;
        private GraphicsLayer _flagsGraphicsLayer;

        public event Action MapInitialized;

        public UIElement CreateMap()
        {
            _map = new Map {UseAcceleratedDisplay = false, 
                            Extent = new Envelope(-2014711, 15, 156956, 12175318),
                            WrapAround = true};

            var baseMap = new ArcGISTiledMapServiceLayer
                              {
                                  ID = "BaseMap",                                 
                                  Url =
                                      "http://services.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer"
                              };
            baseMap.Initialized += BaseMapInitialized;
            _map.Layers.Add(baseMap);

            _flagsGraphicsLayer = new GraphicsLayer { ID = "FlagsGraphicsLayer"};
            _map.Layers.Add(_flagsGraphicsLayer);

            return _map;
        }

        public void ShowClickedFlag(MapCoord coord, Uri imageUri)
        {
            ShowFlag(coord, imageUri);           
        }

        public void ShowTargetFlag(MapCoord coord, Uri imageUri)
        {
            ShowFlag(coord, imageUri);
        }

        public void ClearFlags()
        {
            if (_flagsGraphicsLayer == null)
            {
                throw new NullReferenceException("Map not already created");
            }
            _flagsGraphicsLayer.ClearGraphics();
        }

        private void ShowFlag(MapCoord coord, Uri imageUri)
        {
            var mapPoint = coord.ToEsriWebMercatorMapPoint();
            if (_flagsGraphicsLayer == null || mapPoint == null)
            {
                throw new ArgumentNullException("coord", "Coordinates or Graphic layers is null");
            }

            var symbol = new PictureMarkerSymbol {Source = new BitmapImage(imageUri),
                                                  OffsetX=1,
                                                  OffsetY=30};


            // Ação quando o usuário responder o local
            var graphic = new Graphic
                              {
                                  Symbol = symbol,
                                  Geometry = mapPoint
                              };

            _flagsGraphicsLayer.Graphics.Add(graphic);
        }

        private void BaseMapInitialized(object sender, EventArgs e)
        {
            if (MapInitialized != null)
            {
                MapInitialized();
            }
        }
    }
}
