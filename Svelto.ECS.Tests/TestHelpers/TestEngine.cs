namespace Svelto.ECS.Tests
{
    class TestEngine : IQueryingEntitiesEngine
    {
        public EntitiesDB entitiesDB { get; set; }
        public void       Ready()    { }

        public bool HasEntity<T>(EGID ID) where T : struct, IEntityComponent { return entitiesDB.Exists<T>(ID); }

        public bool HasAnyEntityInGroup<T>(ExclusiveGroup groupID) where T : struct, IEntityComponent
        {
            return entitiesDB.QueryEntities<T>(groupID).count > 0;
        }

        public bool HasAnyEntityInGroupArray<T>(ExclusiveGroup groupID) where T : struct, IEntityComponent
        {
            return entitiesDB.QueryEntities<T>(groupID).count > 0;
        }
    }
}