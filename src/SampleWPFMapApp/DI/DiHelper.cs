using Autofac;
using Kinect4EsriMap.DI;
using Kinect4Map.DI;
using Kinect4TelerikMap.DI;
using Leap4Map.DI;
using Leap4TelerikMap.DI;
using NUI4Map.Structs;

namespace NUI4Map.SampleWPFMapApp.DI
{
    static class DiHelper
    {
        #region Sample Settings
        public static MapControlType MapControlType = MapControlType.TelerikRadControl;
        public static SensorType SensorType = SensorType.Leap;
        #endregion

        private static IContainer _container;
        public static IContainer GetContainer()
        {
            if (_container == null)
            {
                var builder = new ContainerBuilder();

                if (SensorType == SensorType.Leap)
                {
                    builder.RegisterModule(new LeapMapModule());
                    if (MapControlType == MapControlType.TelerikRadControl)
                    {
                        builder.RegisterModule(new LeapTelerikMapModule());
                    }
                }
                else if (SensorType == SensorType.Kinect)
                {
                    builder.RegisterModule(new KinectMapModule());
                    if (MapControlType == MapControlType.TelerikRadControl)
                    {
                        builder.RegisterModule(new KinectTelerikMapModule());
                    }
                    else if (MapControlType == MapControlType.EsriArcGISRuntime)
                    {
                        builder.RegisterModule(new KinectEsriMapModule());
                    }
                    
                }



                //// SWITCH BETWEEN ARCGIS RUNTIME FOR WPF OR TELERIK MAP CONTROL
                //builder.RegisterModule(new KinectEsriMapModule());
                

                _container = builder.Build();
            }
            
            return _container;
        }
    }
}
