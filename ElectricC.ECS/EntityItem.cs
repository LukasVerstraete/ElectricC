namespace ElectricC.ECS
{
    public struct EntityItem
    {
        public static readonly EntityItem Empty = new EntityItem(-1, null);

        public bool IsAlive;
        public int EntityIndex;
        public Component Component;
        public bool IsComponent;

        public EntityItem(int entityIndex) : this(entityIndex, null) { }
        public EntityItem(Component component) : this(-1, component) { }
        public EntityItem(int entityIndex, Component component)
        {
            EntityIndex = entityIndex;
            Component = component;
            IsComponent = component != null;
            IsAlive = !IsComponent;
        }
    }
}
