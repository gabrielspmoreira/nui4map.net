using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NUI4Map.Handler
{
    public interface INUIHandler
    {
        event Action<object> OnFrame;
        void Start();
        void Stop();

    }
}
