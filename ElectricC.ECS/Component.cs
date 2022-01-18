namespace ElectricC.ECS
{
    public abstract class Component
    {
        public string Name { get; private set; }
        public int OwnerId { get; internal set; }

        protected Component()
        {
            Name = GetType().Name;
            OwnerId = -1;
        }
    }
}
