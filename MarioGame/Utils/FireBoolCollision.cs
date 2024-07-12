using System.Collections.Generic;
using System.Linq;

using MarioGame;

using nkast.Aether.Physics2D.Dynamics;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Events;
using SuperMarioBros.Utils.DataStructures;

using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;

namespace SuperMarioBros.Utils;

public static class FireBoolCollision
{
    public static void Restitution(Body body, float forwardSpeedFire)
    {
        float minBounceVelocity = 4.0f;

        if (body != null && body.LinearVelocity.Y > 0)
        {
            body.LinearVelocity = new AetherVector2(forwardSpeedFire, -minBounceVelocity);
        }
    }
    public static void MoveFireballAfterWait(Entity fireballEntity)
    {
        if (fireballEntity != null)
        {
            var colliderComponent = fireballEntity.GetComponent<ColliderComponent>();
            var fireAnimation = fireballEntity.GetComponent<AnimationComponent>();

            if (colliderComponent != null && fireAnimation != null)
            {
                colliderComponent.collider.Position = new AetherVector2(-100, 750);
                colliderComponent.collider.BodyType = BodyType.Static;
                fireAnimation.animations = new AnimationComponent(Animations.entityTextures[EntitiesName.FIRE], 34, 34)
                    .animations;
                colliderComponent.Enabled(true);
            }
        }
    }
    public static void DisableCollider(Entity entity)
    {
        if (entity != null)
        {
            var colliderComponent = entity.GetComponent<ColliderComponent>();
            if (colliderComponent != null)
            {
                colliderComponent.RemoveCollider();
            }
        }
    }
    public static Entity GetOtherEntityByCollider(IEnumerable<Entity> entities, Fixture fixtureA, Fixture fixtureB,
        Entity fireballEntity)
    {
        return entities.FirstOrDefault(e =>
            e.GetComponent<ColliderComponent>()?.collider ==
            (fixtureA.Body == fireballEntity.GetComponent<ColliderComponent>()?.collider
                ? fixtureB.Body
                : fixtureA.Body));
    }
    public static Entity GetEntityByCollider(IEnumerable<Entity> entities, Body body)
    {
        return entities.FirstOrDefault(e => e.GetComponent<ColliderComponent>()?.collider == body);
    }
    public static void MoveFireball(Entity fireballEntity, Dictionary<Entity, float> fireballTimers, float waitTime)
    {
        if (fireballEntity != null)
        {
            var colliderComponent = fireballEntity.GetComponent<ColliderComponent>();
            var fireAnimation = fireballEntity.GetComponent<AnimationComponent>();

            if (colliderComponent != null && fireAnimation != null)
            {
                var positionExprotion = colliderComponent.collider.Position;

                colliderComponent.collider.Position = positionExprotion;
                fireAnimation.animations =
                    new AnimationComponent(Animations.entityTextures[EntitiesName.FIREEXPLOSION], 34, 34).animations;
                colliderComponent.Enabled(false);
                EventDispatcher.Instance.Dispatch(new SoundEffectEvent(SoundEffectType.PlayerFireballCollided));

                if (fireballTimers != null) fireballTimers[fireballEntity] = waitTime;
            }
        }
    }
}
