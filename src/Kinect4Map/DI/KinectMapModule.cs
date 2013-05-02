using Autofac;
using Kinect4Map.Handler;
using NJI4Map.Drawing;
using NUI4Map.Drawing;
using NUI4Map.Handler;

namespace Kinect4Map.DI
{
    public class KinectMapModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
             base.Load(builder);
             builder.Register(c => new ControllerDrawer()).As<IControllerDrawer>();
             builder.Register(c => new KinectHandler()).As<INUIHandler>().SingleInstance();
        }
    }
}
