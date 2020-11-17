using Svelto.DataStructures;
using Svelto.ECS;

namespace Physics.Core
{
    public static class GameGroups
    {
        public static readonly ExclusiveGroupStruct Transform = TransformGroupTag.BuildGroup;
        public static readonly ExclusiveGroupStruct RigidBody = GroupCompound<TransformGroupTag, RigidBodyGroupTag>.BuildGroup;
        public static readonly ExclusiveGroupStruct RigidBodyWithBoxCollider = GroupCompound<TransformGroupTag, RigidBodyGroupTag, RigidBodyWithBoxColliderGroupTag>.BuildGroup;

        public static readonly FasterReadOnlyList<ExclusiveGroupStruct> TransformGroups = TransformGroupTag.Groups;
        public static readonly FasterReadOnlyList<ExclusiveGroupStruct> RigidBodyGroups = RigidBodyGroupTag.Groups;
        public static readonly FasterReadOnlyList<ExclusiveGroupStruct> RigidBodyWithBoxColliderGroups = RigidBodyWithBoxColliderGroupTag.Groups;
        
        // ReSharper disable ClassNeverInstantiated.Local
        private class TransformGroupTag : GroupTag<TransformGroupTag> {}
        private class RigidBodyGroupTag : GroupTag<RigidBodyGroupTag> {}
        private class RigidBodyWithBoxColliderGroupTag : GroupTag<RigidBodyWithBoxColliderGroupTag> {}
    }
}