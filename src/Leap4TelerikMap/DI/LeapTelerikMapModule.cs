using Autofac;
using Leap4TelerikMap.Gestures;
using NUI4Map.Gestures;
using NUI4Map.Handler;
using TelerikMapCommons.Handler;

namespace Leap4TelerikMap.DI
{

    public class LeapTelerikMapModule : Module
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

