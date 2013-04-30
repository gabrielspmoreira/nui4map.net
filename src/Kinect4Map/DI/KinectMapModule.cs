using Autofac;
using Kinect4Map.Drawing;

namespace Kinect4Map.DI
{
    public class KinectMapModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
             base.Load(builder);
             builder.Register(c => new HandsDrawer()).As<IHandsDrawer>();     
        }
    }
}
