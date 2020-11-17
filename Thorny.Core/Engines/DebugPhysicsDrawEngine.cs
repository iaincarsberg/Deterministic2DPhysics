using System;
using FixedMaths.Core;
using Graphics.Core;
using Physics.Core.EntityComponents;
using Physics.Core.Loggers;
using Physics.Core.Loggers.Data;
using Svelto.Common;
using Svelto.ECS;
using Thorny.Common;
using Thorny.Core.Extensions;
using Colour = Graphics.Core.Colour;
using PhysicsGameGroups = Physics.Core.GameGroups;

namespace Thorny.Core.Engines
{
    [Sequenced(nameof(ThornyEngineNames.DebugPhysicsDrawEngine))]
    public class DebugPhysicsDrawEngine : IQueryingEntitiesEngine, IScheduledGraphicsEngine
    {
        private readonly EngineScheduler _engineScheduler;
        private readonly IGraphics _graphics;

        public DebugPhysicsDrawEngine(EngineScheduler engineScheduler, IGraphics graphics)
        {
            _engineScheduler = engineScheduler;
            _graphics = graphics;
        }

        public void Ready()
        {
            _engineScheduler.RegisterScheduledGraphicsEngine(this);
        }
        
        public EntitiesDB entitiesDB { get; set; }

        public void Draw(FixedPoint delta, ulong physicsTick)
        {
            var (debugTransforms, debugCount) = entitiesDB.QueryEntities<TransformEntityComponent>(GameGroups.Debug);
            for (var i = 0; i < debugCount; i++)
            {
                ref var transformEntityComponent = ref debugTransforms[i];

                var (drawX, drawY) = transformEntityComponent.Interpolate(delta);

                _graphics.DrawCross(Colour.SlateGrey, (int) Math.Round(drawX), (int) Math.Round(drawY), 3);
            }
            
            foreach (var ((transforms, count), _) in entitiesDB.QueryEntities<TransformEntityComponent>(PhysicsGameGroups.TransformGroups))
            {
                for (var i = 0; i < count; i++)
                {
                    ref var transformEntityComponent = ref transforms[i];

                    var (drawX, drawY) = transformEntityComponent.Interpolate(delta);

                    _graphics.DrawPlus(Colour.Aqua, (int) Math.Round(drawX), (int) Math.Round(drawY), 3);
                }
            }
            
            foreach (var ((transforms, colliders, count), _) in entitiesDB.QueryEntities<TransformEntityComponent, BoxColliderEntityComponent>(PhysicsGameGroups.RigidBodyWithBoxColliderGroups))
            {
                for (var i = 0; i < count; i++)
                {
                    ref var transformEntityComponent = ref transforms[i];
                    ref var boxColliderEntityComponent = ref colliders[i];

                    var point = transformEntityComponent.Interpolate(delta);
                    var aabb = boxColliderEntityComponent.ToAABB(point);

                    var (minX, minY) = aabb.Min;
                    var (maxX, maxY) = aabb.Max;

                    _graphics.DrawBox(Colour.PaleVioletRed, (int) Math.Round(minX), (int) Math.Round(minY), (int) Math.Round(maxX), (int) Math.Round(maxY));
                }
            }

            foreach (var point in FixedPointVector2Logger.Instance.GetPoints(physicsTick))
            {
                var (drawX, drawY) = point.Point;

                switch (point.Shape)
                {
                    case Shape.Cross:
                        _graphics.DrawCross(point.Colour.ToGraphicsColour(), (int) Math.Round(drawX), (int) Math.Round(drawY), point.Radius ?? 3);
                        break;
                    case Shape.Plus:
                        _graphics.DrawPlus(point.Colour.ToGraphicsColour(), (int) Math.Round(drawX), (int) Math.Round(drawY), point.Radius ?? 3);
                        break;
                    case Shape.Circle:
                        _graphics.DrawCircle(point.Colour.ToGraphicsColour(), (int) Math.Round(drawX), (int) Math.Round(drawY), point.Radius ?? 3);
                        break;
                    case Shape.Box:
                        var (minX, minY) = point.BoxMin ?? FixedPointVector2.Zero;
                        var (maxX, maxY) = point.BoxMax ?? FixedPointVector2.Zero;
                        _graphics.DrawBox(point.Colour.ToGraphicsColour(), (int)Math.Round(drawX + minX), (int)Math.Round(drawY + minY), (int)Math.Round(drawX + maxX), (int)Math.Round(drawY + maxY));
                        break;
                    case Shape.Line:
                        var (fromX, fromY) = point.BoxMin ?? FixedPointVector2.Zero;
                        var (toX, toY) = point.BoxMax ?? FixedPointVector2.Zero;
                        _graphics.DrawLine(point.Colour.ToGraphicsColour(), (int)Math.Round(fromX), (int)Math.Round(fromY), (int)Math.Round(toX), (int)Math.Round(toY));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}