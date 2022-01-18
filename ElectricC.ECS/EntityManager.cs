using ElectricC.Input;
using System;
using System.Collections.Generic;

namespace ElectricC.ECS
{
    public class EntityManager
    {
        private const int COMPONENTS_PER_ENTITY = 10;
        private const int ENTITY_POOL_STARTING_SIZE = 20;

        private List<ComponentSystem> Systems { get; set; }

        private List<EntityItem> entities;
        private Queue<int> recyclableIds;
        private int idCounter;

        public EntityManager(): this(ENTITY_POOL_STARTING_SIZE)
        {}

        public EntityManager(int entityCount)
        {
            Systems = new List<ComponentSystem>();
            entities = new List<EntityItem>(entityCount * COMPONENTS_PER_ENTITY);
            recyclableIds = new Queue<int>(entityCount);
            idCounter = 0;
        }

        public void OnLoad()
        {
            Systems.ForEach(system => system.OnLoad());
        }

        public void OnResize(float width, float height)
        {
            Systems.ForEach(system => system.OnResize(width, height));
        }

        public void Input(InputManager input)
        {
            Systems.ForEach(system => system.Input(input));
        }

        public void Update(float deltaTime)
        {
            Systems.ForEach(system => system.Update(deltaTime));
        }

        public void Render()
        {
            Systems.ForEach(system => system.Render());
        }

        public void AddSystem(ComponentSystem system)
        {
            system.EntityManager = this;
            Systems.Add(system);
            IterateEntityPointers(0, (entityId, entity) => system.EntityModified(entityId));
        }

        public void NotifySystemsEntityUpdated(int entityId)
        {
            Systems.ForEach(system => system.EntityModified(entityId));
        }

        public int CreateEntity()
        {
            if (recyclableIds.Count != 0)
            {
                int id = recyclableIds.Dequeue();
                EntityItem entity = entities[id];
                entity.IsAlive = true;
                return id;
            }
            ShiftEntityItems(0, 1);
            entities.Insert(idCounter, new EntityItem(entities.Count + 1));
            return idCounter++;
        }

        public int BuildEntity(params Component[] components)
        {
            int entityId = CreateEntity();
            AddComponents(entityId, components);
            return entityId;
        }

        public void DestroyEntity(int entityId)
        {
            EntityItem entityItem;

            CheckEntityExistsOrIsAlive(entityId, out entityItem);

            int lastIndex = entities.Count;
            if (IsEntityPointer(entityId + 1, out var nextEntityItem))
            { lastIndex = nextEntityItem.EntityIndex; }
            int componentCount = lastIndex - entityItem.EntityIndex;

            ShiftEntityItems(entityId + 1, -componentCount);

            entities.RemoveRange(entityItem.EntityIndex, componentCount);
            entityItem.IsAlive = false;
            entities[entityId] = entityItem;
            recyclableIds.Enqueue(entityId);

            NotifySystemsEntityUpdated(entityId);
        }

        public void AddComponent(int entityId, Component component)
        {
            AddComponents(entityId, new Component[] { component });
        }

        public void AddComponents(int entityId, params Component[] components)
        {
            EntityItem entityItem;

            CheckEntityExistsOrIsAlive(entityId, out entityItem);

            ShiftEntityItems(entityId + 1, components.Length);

            foreach (Component component in components)
            {
                component.OwnerId = entityId;
                entities.Insert(entityItem.EntityIndex, new EntityItem(component));
            }

            NotifySystemsEntityUpdated(entityId);
        }

        public void RemoveComponent(int entityId, Component component)
        {
            EntityItem entity;
            CheckEntityExistsOrIsAlive(entityId, out entity);

            int componentIndex = -1;
            IterateEntityComponents(entity, entityId, (index, c) =>
            {
                if (c.Component == component)
                {
                    c.Component.OwnerId = -1;
                    componentIndex = index;
                    return true;
                }
                return false;
            });

            if (componentIndex == -1)
            { throw new InvalidOperationException("The specified entity does not contain that component."); }

            ShiftEntityItems(entityId + 1, -1);
            entities.RemoveAt(componentIndex);
            NotifySystemsEntityUpdated(entityId);
        }

        public void RemoveComponent<T>(int entityId) where T : Component
        {
            RemoveComponent<T>(entityId, (component) => true);
        }

        public void RemoveComponent<T>(int entityId, Predicate<T> check) where T : Component
        {
            EntityItem entity;
            CheckEntityExistsOrIsAlive(entityId, out entity);

            int componentIndex = -1;
            IterateEntityComponents(entity, entityId, (index, component) =>
            {
                if (component.Component is T && check((T)component.Component))
                {
                    component.Component.OwnerId = -1;
                    componentIndex = index;
                    return true;
                }
                return false;
            });

            if (componentIndex == -1)
            { throw new InvalidOperationException("The specified entity does not contain a component of that type."); }

            ShiftEntityItems(entityId + 1, -1);
            entities.RemoveAt(componentIndex);
            NotifySystemsEntityUpdated(entityId);
        }

        public void RemoveComponents(int entityId, Predicate<Component> check)
        {
            EntityItem entity;
            CheckEntityExistsOrIsAlive(entityId, out entity);

            List<int> componentIndices = new List<int>();
            IterateEntityComponents(entity, entityId, (index, component) =>
            {
                if (check(component.Component))
                {
                    component.Component.OwnerId = -1;
                    componentIndices.Add(index);
                }
                return false;
            });

            if (componentIndices.Count > 0)
            {
                ShiftEntityItems(entityId + 1, componentIndices.Count);
                foreach (int componentIndex in componentIndices)
                { entities.RemoveAt(componentIndex); }

                NotifySystemsEntityUpdated(entityId);
            }
        }

        public bool HasComponent<T>(int entityId) where T : Component
        {
            return HasComponent<T>(entityId, (component) => true);
        }

        public bool HasComponent<T>(int entityId, Predicate<T> check) where T : Component
        {
            EntityItem entity;
            CheckEntityExistsOrIsAlive(entityId, out entity);

            bool foundComponent = false;

            IterateEntityComponents(entity, entityId, (index, component) =>
            {
                if (component.Component is T && check((T)component.Component))
                {
                    foundComponent = true;
                    return true;
                }
                return false;
            });

            return foundComponent;
        }

        public T GetComponent<T>(int entityId) where T : Component
        {
            return GetComponent<T>(entityId, (component) => true);
        }

        public T GetComponent<T>(int entityId, Predicate<T> check) where T : Component
        {
            EntityItem entity;
            CheckEntityExistsOrIsAlive(entityId, out entity);

            T resultComponent = null;
            IterateEntityComponents(entity, entityId, (index, component) =>
            {
                if (component.Component is T && check((T)component.Component))
                {
                    resultComponent = (T)component.Component;
                    return true;
                }
                return false;
            });

            if (resultComponent == null)
            { throw new InvalidOperationException($"The specified entity does not contain a component of type {typeof(T)}."); }

            return resultComponent;
        }

        public IEnumerable<Component> GetComponents(int entityId)
        {
            return GetComponents(entityId, (component) => true);
        }

        public IEnumerable<Component> GetComponents(int entityId, Predicate<Component> check)
        {
            EntityItem entity;
            CheckEntityExistsOrIsAlive(entityId, out entity);

            int lastIndex = entities.Count;
            if (IsEntityPointer(entityId + 1, out EntityItem nextEntity))
            {
                lastIndex = nextEntity.EntityIndex;
            }
            for (int index = entity.EntityIndex; index < lastIndex; index++)
            {
                if (check(entities[index].Component))
                { yield return entities[index].Component; }
            }
        }

        private bool IsEntityPointer(int entityId, out EntityItem entityItem)
        {
            entityItem = EntityItem.Empty;
            if (entityId < 0 || entityId >= entities.Count || entities[entityId].IsComponent)
            {
                return false;
            }
            entityItem = entities[entityId];
            return true;
        }

        private void IterateEntityPointers(int startingId, Action<int, EntityItem> action)
        {
            for (
                int index = startingId;
                index < entities.Count && IsEntityPointer(index, out EntityItem entity);
                index++
            )
            { action(index, entity); }
        }

        private void IterateEntityComponents(EntityItem entity, int entityId, Func<int, EntityItem, bool> action)
        {
            int lastIndex = entities.Count;
            if (IsEntityPointer(entityId + 1, out EntityItem nextEntity))
            { lastIndex = nextEntity.EntityIndex; }

            for (int index = entity.EntityIndex; index < lastIndex; index++)
            {
                if (action(index, entities[index])) { break; }
            }
        }

        private bool CheckEntityExistsOrIsAlive(int entityId, out EntityItem entity)
        {
            if (!IsEntityPointer(entityId, out entity) || !entity.IsAlive)
            { throw new InvalidOperationException("The provided entityId does not exist as an entityType"); }
            return true;
        }

        private void ShiftEntityItems(int fromIndex, int amount)
        {
            IterateEntityPointers(fromIndex, (index, entityItem) =>
            {
                entityItem.EntityIndex += amount;
                entities[index] = entityItem;
            });
        }
    }
}
