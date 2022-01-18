using ElectricC.Input;
using System.Collections.Generic;

namespace ElectricC.ECS
{
    public abstract class ComponentSystem
    {
        public EntityManager EntityManager { get; set; }

        protected List<int> validEntities;

        public ComponentSystem()
        {
            validEntities = new List<int>();
        }

        internal void EntityModified(int entityId)
        {
            bool wasValid = validEntities.Contains(entityId);
            bool isValid = IsEntityValid(entityId);

            if (wasValid && !isValid) { validEntities.Remove(entityId); }
            else if (!wasValid && isValid) { validEntities.Add(entityId); }
        }

        public virtual void OnLoad() { }
        public virtual void OnResize(float width, float height) { }
        public virtual void Input(InputManager input) { }
        public virtual void Update(float deltaTime) { }
        public virtual void Render() { }

        protected abstract bool IsEntityValid(int entityId);
    }
}
