using ElectricC.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricC.World.Systems
{
    public class WorldSystem : ComponentSystem
    {
        protected override bool IsEntityValid(int entityId)
        {
            throw new NotImplementedException();
        }
    }
}
