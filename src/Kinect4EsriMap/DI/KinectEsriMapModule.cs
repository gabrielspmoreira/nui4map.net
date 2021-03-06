﻿using Autofac;
using EsriMapCommons.Handler;
using Kinect4EsriMap.Gestures;
using NUI4Map.Gestures;
using NUI4Map.Handler;

namespace Kinect4EsriMap.DI
{
    public class KinectEsriMapModule : Module
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
