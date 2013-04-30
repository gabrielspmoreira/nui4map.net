using Autofac;
using Leap4Map.DI;
using Leap4TelerikMap.DI;

namespace SampleWPFMappApp.DI
{
    static class DiHelper
    {
        private static IContainer _container;
        public static IContainer GetContainer()
        {
            if (_container == null)
            {
                var builder = new ContainerBuilder();
                builder.RegisterModule(new LeapMapModule());

                //// SWITCH BETWEEN ARCGIS RUNTIME FOR WPF OR TELERIK MAP CONTROL
                //builder.RegisterModule(new EsriMapModule());
                builder.RegisterModule(new LeapTelerikMapModule());

                _container = builder.Build();
            }
            
            return _container;
        }
    }
}
