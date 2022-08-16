using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Features;
using Exiled.API.Interfaces;

namespace FlashLightProjector
{
    public sealed class Config : IConfig
    {
        [Description("Enable")]
        public bool IsEnabled { get; set; } = true;

        [Description("Save objects on networkserver")]
        public bool NetworkServerObjectSaver { get; set; } = true;

        [Description("Light Intensity")] 
        public float LightIntensity { get; set; } = 0.5f;

        [Description("Count of light sources")]
        public int LightSourceCount { get; set; } = 25;

        [Description("Distance between two light sources")]
        public float LightSourceDistance { get; set; } = 2;

        [Description("Light range")] 
        public float LightRange { get; set; } = 100;
        
        [Description("Math func expanse")]
        public float MathFuncExpanseValue { get; set; } = 1;
    }
}
