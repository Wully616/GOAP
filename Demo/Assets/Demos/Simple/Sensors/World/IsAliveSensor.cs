﻿using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Sensors;

namespace Demos.Simple.Sensors.World
{
    public class IsAliveSensor : LocalWorldSensorBase
    {
        public override void Update()
        {
            
        }

        public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
        {
            return true;
        }
    }
}