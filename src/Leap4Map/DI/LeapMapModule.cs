using Autofac;
using Leap4Map.Drawing;

namespace Leap4Map.DI
{
    public class LeapMapModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
             base.Load(builder);
             builder.Register(c => new HandsDrawer()).As<IHandsDrawer>();     
        }
    }
}
