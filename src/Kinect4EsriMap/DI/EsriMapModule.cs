using Autofac;
using Kinect4EsriMap.Gestures;
using Kinect4EsriMap.MapUtils;
using Kinect4Map.Gestures;
using Kinect4Map.MapUtils;

namespace Kinect4EsriMap.DI
{
    public class EsriMapModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
             base.Load(builder);
             builder.Register(c => new EsriMapHandler()).As<IMapHandler>(); 
             builder.Register(c => new MapClickGestureHandler()).As<IMapClickGestureHandler>();
             builder.Register(c => new MapPanGestureHandler()).As<IMapPanGestureHandler>();
             builder.Register(c => new MapZoomGestureHandler()).As<IMapZoomGestureHandler>();          
        }
    }
}
