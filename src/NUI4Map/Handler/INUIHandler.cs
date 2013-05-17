using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUI4Map.Structs;

namespace NUI4Map.Handler
{
    public interface INUIHandler
    {
        SensorType SensorType { get; }
        event Action<object> OnFrame;
        void Start();
        void Stop();

    }
}
