using Autofac;
using Kinect4Map.Gestures;
using Kinect4TelerikMap.Gestures;
using Kinect4TelerikMap.Handler;
using Kinect4TelerikMap.Handler;
using NUI4Map.Gestures;
using NUI4Map.Handler;

namespace Kinect4TelerikMap.DI
{

    public class TelerikMapModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {
             base.Load(builder);
             builder.Register(c => new TelerikMapHandler()).As<IMapHandler>(); 
             builder.Register(c => new MapClickGestureHandler()).As<IMapClickGestureHandler>();
             builder.Register(c => new MapPanGestureHandler()).As<IMapPanGestureHandler>();
             builder.Register(c => new MapZoomGestureHandler()).As<IMapZoomGestureHandler>();          
        }

    } 

}

