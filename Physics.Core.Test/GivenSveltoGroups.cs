using Svelto.DataStructures;
using Svelto.ECS;
using Xunit;

namespace Physics.Core.Test
{
    public class GivenSveltoGroups
    {
        public readonly struct ComponentA : IEntityComponent {}
        public readonly struct ComponentB : IEntityComponent {}
        public readonly struct ComponentC : IEntityComponent {}

        private class DescriptorA : GenericEntityDescriptor<ComponentA, EGIDComponent> {}
        
        private class DescriptorB : ExtendibleEntityDescriptor<DescriptorA>
        {
            public DescriptorB() : base(new IComponentBuilder[]
            {
                new ComponentBuilder<ComponentB>() 
            })
            {}
        }
        
        private class DescriptorC : ExtendibleEntityDescriptor<DescriptorB>
        {
            public DescriptorC() : base(new IComponentBuilder[]
            {
                new ComponentBuilder<ComponentC>() 
            })
            {}
        }
        
        private static class GameGroups
        {
            public static readonly ExclusiveGroupStruct GroupA = GroupATag.BuildGroup;
            public static readonly ExclusiveGroupStruct GroupB = GroupCompound<GroupBTag, GroupATag>.BuildGroup;
            public static readonly ExclusiveGroupStruct ExclusiveGroupB = GroupBTag.BuildGroup;
            public static readonly ExclusiveGroupStruct GroupC = GroupCompound<GroupCTag, GroupBTag, GroupATag>.BuildGroup;
            public static readonly ExclusiveGroupStruct GroupBAndC = GroupCompound<GroupBTag, GroupCTag>.BuildGroup;
            public static readonly ExclusiveGroupStruct ExclusiveGroupC = GroupCTag.BuildGroup;

            public static readonly FasterReadOnlyList<ExclusiveGroupStruct> GroupATags = GroupATag.Groups;
            public static readonly FasterReadOnlyList<ExclusiveGroupStruct> GroupBTags = GroupBTag.Groups;
            public static readonly FasterReadOnlyList<ExclusiveGroupStruct> GroupCTags = GroupCTag.Groups;
    
            private class GroupATag : GroupTag<GroupATag> {}
            private class GroupBTag : GroupTag<GroupBTag> {}
            private class GroupCTag : GroupTag<GroupCTag> {}
        }

        private class TestEngine : IQueryingEntitiesEngine
        {
            public EntitiesDB entitiesDB { get; set; }

            public bool IsReady;
            public void Ready()
            {
                IsReady = true;
            }
        }

        private readonly TestEngine _engine;
        
        private uint _egid;
        private uint Egid => _egid++;

        public GivenSveltoGroups()
        {
            var simpleSubmissionEntityViewScheduler = new SimpleEntitiesSubmissionScheduler();
            var enginesRoot = new EnginesRoot(simpleSubmissionEntityViewScheduler);
            var entityFactory = enginesRoot.GenerateEntityFactory();

            _engine = new TestEngine();
            enginesRoot.AddEngine(_engine);

            // A
            var aInit1 = entityFactory.BuildEntity<DescriptorA>(Egid, GameGroups.GroupA);
            aInit1.Init(new ComponentA());
            
            var aInit2 = entityFactory.BuildEntity<DescriptorA>(Egid, GameGroups.GroupA);
            aInit2.Init(new ComponentA());
            
            // B
            var bInit1 = entityFactory.BuildEntity<DescriptorB>(Egid, GameGroups.GroupB);
            bInit1.Init(new ComponentA());
            bInit1.Init(new ComponentB());
            
            var bInit2 = entityFactory.BuildEntity<DescriptorB>(Egid, GameGroups.GroupB);
            bInit2.Init(new ComponentA());
            bInit2.Init(new ComponentB());
            
            // C
            var cInit1 = entityFactory.BuildEntity<DescriptorC>(Egid, GameGroups.GroupC);
            cInit1.Init(new ComponentA());
            cInit1.Init(new ComponentB());
            cInit1.Init(new ComponentC());
            
            var cInit2 = entityFactory.BuildEntity<DescriptorC>(Egid, GameGroups.GroupC);
            cInit2.Init(new ComponentA());
            cInit2.Init(new ComponentB());
            cInit2.Init(new ComponentC());

            simpleSubmissionEntityViewScheduler.SubmitEntities();
        }

        [Fact]
        public void ShouldHave2InGroupA()
        {
            Assert.True(_engine.IsReady);

            Assert.Equal((uint)2, _engine.entitiesDB.QueryEntities<ComponentA>(GameGroups.GroupA).count);
            Assert.Equal((uint)0, _engine.entitiesDB.QueryEntities<ComponentB>(GameGroups.GroupA).count);
            Assert.Equal((uint)0, _engine.entitiesDB.QueryEntities<ComponentC>(GameGroups.GroupA).count);
        }
        
        [Fact]
        public void ShouldHave2InGroupB()
        {
            Assert.True(_engine.IsReady);

            Assert.Equal((uint)2, _engine.entitiesDB.QueryEntities<ComponentA>(GameGroups.GroupB).count);
            Assert.Equal((uint)2, _engine.entitiesDB.QueryEntities<ComponentB>(GameGroups.GroupB).count);
            Assert.Equal((uint)2, _engine.entitiesDB.QueryEntities<ComponentA, ComponentB>(GameGroups.GroupB).count);
            Assert.Equal((uint)0, _engine.entitiesDB.QueryEntities<ComponentC>(GameGroups.GroupB).count);
        }
        
        [Fact]
        public void ShouldHave2InGroupC()
        {
            Assert.True(_engine.IsReady);

            Assert.Equal((uint)2, _engine.entitiesDB.QueryEntities<ComponentA>(GameGroups.GroupC).count);
            Assert.Equal((uint)2, _engine.entitiesDB.QueryEntities<ComponentB>(GameGroups.GroupC).count);
            Assert.Equal((uint)2, _engine.entitiesDB.QueryEntities<ComponentC>(GameGroups.GroupC).count);
            Assert.Equal((uint)2, _engine.entitiesDB.QueryEntities<ComponentA, ComponentB>(GameGroups.GroupC).count);
            Assert.Equal((uint)2, _engine.entitiesDB.QueryEntities<ComponentB, ComponentC>(GameGroups.GroupC).count);
            Assert.Equal((uint)2, _engine.entitiesDB.QueryEntities<ComponentA, ComponentC>(GameGroups.GroupC).count);
            Assert.Equal(2, _engine.entitiesDB.QueryEntities<ComponentA, ComponentB, ComponentC>(GameGroups.GroupC).ToBuffers().count);
        }
        
        [Fact]
        public void ShouldHave0InGroupBAndC()
        {
            Assert.True(_engine.IsReady);

            Assert.Equal((uint)0, _engine.entitiesDB.QueryEntities<ComponentA>(GameGroups.GroupBAndC).count);
        }
        
        [Fact]
        public void ShouldHave0InExclusiveGroupB()
        {
            Assert.True(_engine.IsReady);

            Assert.Equal((uint)0, _engine.entitiesDB.QueryEntities<ComponentA>(GameGroups.ExclusiveGroupB).count);
        }
        
        [Fact]
        public void ShouldHave0InExclusiveGroupC()
        {
            Assert.True(_engine.IsReady);

            Assert.Equal((uint)0, _engine.entitiesDB.QueryEntities<ComponentA>(GameGroups.ExclusiveGroupC).count);
        }

        [Fact]
        public void ShouldSumAllAs()
        {
            Assert.Equal(6, Sum(_engine.entitiesDB.QueryEntities<ComponentA>(GameGroups.GroupATags)));
            Assert.Equal(4, Sum(_engine.entitiesDB.QueryEntities<ComponentB>(GameGroups.GroupATags)));
            Assert.Equal(2, Sum(_engine.entitiesDB.QueryEntities<ComponentC>(GameGroups.GroupATags)));
        }
        
        [Fact]
        public void ShouldSumAllBs()
        {
            Assert.Equal(4, Sum(_engine.entitiesDB.QueryEntities<ComponentA>(GameGroups.GroupBTags)));
            Assert.Equal(4, Sum(_engine.entitiesDB.QueryEntities<ComponentB>(GameGroups.GroupBTags)));
            Assert.Equal(2, Sum(_engine.entitiesDB.QueryEntities<ComponentC>(GameGroups.GroupBTags)));
        }
        
        [Fact]
        public void ShouldSumAllCs()
        {
            Assert.Equal(2, Sum(_engine.entitiesDB.QueryEntities<ComponentA>(GameGroups.GroupCTags)));
            Assert.Equal(2, Sum(_engine.entitiesDB.QueryEntities<ComponentB>(GameGroups.GroupCTags)));
            Assert.Equal(2, Sum(_engine.entitiesDB.QueryEntities<ComponentC>(GameGroups.GroupCTags)));
        }
        
        private static int Sum<T1>(GroupsEnumerable<T1> groupsEnumerable) 
            where T1 : struct, IEntityComponent
        {
            var sum = 0;
            foreach (var entity in groupsEnumerable)
            {
                sum += (int)entity._buffers.count;
            }

            return sum;
        }
    }
}