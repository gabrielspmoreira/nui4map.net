using Autofac;
using Leap4Map.Drawing;
using Leap4Map.Handler;
using NUI4Map.Drawing;
using NUI4Map.Handler;

namespace Leap4Map.DI
{
    public class LeapMapModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
             base.Load(builder);
             builder.Register(c => new ControllerDrawer()).As<IControllerDrawer>();
             builder.Register(c => new LeapHandler()).As<INUIHandler>().SingleInstance();
        }
    }
}
