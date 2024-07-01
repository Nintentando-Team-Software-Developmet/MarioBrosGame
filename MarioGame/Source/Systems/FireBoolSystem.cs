using System;
using System.Collections.Generic;
using System.Linq;
using MarioGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;
using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Utils.DataStructures;
using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;

namespace SuperMarioBros.Source.Systems
{
public class FireBoolSystem : BaseSystem
{

    private static float forwardSpeed = 4f;
    private bool canShoot = true;
    private static readonly List<Action> pendingActions = new List<Action>();
    public static AetherVector2 positionExprotion { get; set; }
    private static readonly Dictionary<Entity, float> fireballTimers = new Dictionary<Entity, float>();

    public static readonly float waitTime = 0.3f;


        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
    {
        if (gameTime != null)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;


            IEnumerable<Entity> playerEntities = entities.WithComponents(typeof(PlayerComponent), typeof(AnimationComponent), typeof(ColliderComponent), typeof(MovementComponent), typeof(CameraComponent));
            foreach (var player in playerEntities)
            {
                var keyboardState = Keyboard.GetState();
                var collider = player.GetComponent<ColliderComponent>();
                var animation = player.GetComponent<AnimationComponent>();
                var camera = player.GetComponent<CameraComponent>();

                if (keyboardState.IsKeyDown(Keys.A))
                {
                    if (canShoot)
                    {
                        if (animation.currentState == AnimationState.STOP ||
                            animation.currentState == AnimationState.WALKRIGHT || animation.currentState == AnimationState.JUMPRIGHT)
                        {
                            createFire(entities, collider.collider.Position.X + 0.8f, collider.collider.Position.Y, pendingActions,camera, forwardSpeed);
                            canShoot = false;
                        }
                        else if (animation.currentState == AnimationState.STOPLEFT ||
                                 animation.currentState == AnimationState.WALKLEFT || animation.currentState == AnimationState.JUMPLEFT)
                        {
                            createFire(entities, collider.collider.Position.X - 0.8f, collider.collider.Position.Y, pendingActions,camera, -forwardSpeed);
                            canShoot = false;
                        }
                    }
                }
                else
                {
                    canShoot = true;
                }

                IEnumerable<Entity> fireballEntities = entities.WithComponents(typeof(FireBoolComponent), typeof(ColliderComponent));
                foreach (var fireball in fireballEntities)
                {
                    var fireballCollider = fireball.GetComponent<ColliderComponent>().collider;
                    if (fireballCollider.BodyType == BodyType.Dynamic && fireballCollider.Position.Y > 8.5f)
                    {
                        pendingActions.Add(() => MoveFireball(fireball));
                    }
                }
            }

            foreach (var action in pendingActions)
            {
                action();
            }

            pendingActions.Clear();
            var fireballsToMove = new List<Entity>();
            foreach (var fireball in fireballTimers.Keys.ToList())
            {
                fireballTimers[fireball] -= deltaTime;
                if (fireballTimers[fireball] <= 0)
                {
                    fireballsToMove.Add(fireball);
                }
            }

            foreach (var fireball in fireballsToMove)
            {
                MoveFireballAfterWait(fireball);
                fireballTimers.Remove(fireball);
            }
        }
    }

    private static void createFire(IEnumerable<Entity> entities, float positionX, float positionY, List<Action> pendingActions, CameraComponent cameraComponent, float forwardSpeedFire)
    {
        IEnumerable<Entity> fireballEntities = entities
            .WithComponents(typeof(FireBoolComponent), typeof(AnimationComponent), typeof(ColliderComponent))
            .Where(entity => entity.GetComponent<ColliderComponent>().collider.BodyType == BodyType.Static);

        var firstFireball = fireballEntities.FirstOrDefault();

        if (firstFireball != null)
        {
            var collider = firstFireball.GetComponent<ColliderComponent>();
            var fireballComponent = firstFireball.GetComponent<FireBoolComponent>();

            collider.collider.Position = new AetherVector2(positionX, positionY);
            collider.collider.BodyType = BodyType.Dynamic;
            collider.collider.OnCollision += (fixtureA, fixtureB, contact) => HandleCollision(fixtureA, fixtureB, contact, entities, pendingActions, cameraComponent, forwardSpeedFire);
            collider.collider.LinearVelocity = new AetherVector2(forwardSpeedFire, 0);

            fireballComponent.InitialDirection = Math.Sign(forwardSpeedFire);  // Añadir esta línea
        }
    }


private static bool HandleCollision(Fixture fixtureA, Fixture fixtureB, Contact contact, IEnumerable<Entity> entities, List<Action> pendingActions,CameraComponent cameraComponent,float forwardSpeedFire)
{
    Entity fireballEntity = entities.FirstOrDefault(e =>
        e.GetComponent<ColliderComponent>()?.collider == fixtureA.Body ||
        e.GetComponent<ColliderComponent>()?.collider == fixtureB.Body);

    if (fireballEntity == null) return true;

    if (fireballEntity.GetComponent<FireBoolComponent>() == null)
    {
        return true;
    }
    var fireballComponent = fireballEntity.GetComponent<FireBoolComponent>();
    forwardSpeedFire = fireballComponent.InitialDirection * Math.Abs(forwardSpeedFire);

    Entity otherEntity = entities.FirstOrDefault(e =>
        e.GetComponent<ColliderComponent>()?.collider ==
        (fixtureA.Body == fireballEntity.GetComponent<ColliderComponent>()?.collider ? fixtureB.Body : fixtureA.Body));
    if (otherEntity != null)
    {
        var animationComponent = otherEntity.GetComponent<AnimationComponent>();
        if (animationComponent != null)
        {
            string otherEntityName = Animations.entityTextures
                .FirstOrDefault(x => x.Value == animationComponent.animations)
                .Key.ToString() ?? string.Empty;

            if (otherEntityName == "GOOMBA" || otherEntityName == "KOOPA")
            {
                if (fireballComponent != null)
                {
                    fireballComponent.collidedWithGoomba = true;

                }
                pendingActions.Add(() => DisableCollider(otherEntity));
                pendingActions.Add(() => MoveFireball(fireballEntity));

            }
            else if (otherEntityName == "DUCT")
            {
                var positionDuctComponent = otherEntity.GetComponent<ColliderComponent>();

                float bottomPart1 = positionDuctComponent.Position.Y / 120 +1;
                float fireballY = fireballEntity.GetComponent<ColliderComponent>().collider.Position.Y;
                if (fireballY > bottomPart1)
                {
                    pendingActions.Add(() => MoveFireball(fireballEntity));

                }
            }
            else if (otherEntityName == "BLOCK" || otherEntityName == "FIRE" || otherEntityName == "DUCTEXTENSION")
            {
                pendingActions.Add(() => MoveFireball(fireballEntity));

            }
            else if (otherEntityName == "COINBLOCK" || otherEntityName == "QUESTIONBLOCK")
            {
                var positionDuctComponent = otherEntity.GetComponent<ColliderComponent>();

                float bottomPart1 = positionDuctComponent.Position.Y / 110;
                float fireballY = fireballEntity.GetComponent<ColliderComponent>().collider.Position.Y;
                if (fireballY > bottomPart1 )
                {
                    pendingActions.Add(() => MoveFireball(fireballEntity));

                }
            }
            var fireball = fireballEntity.GetComponent<ColliderComponent>();
            positionExprotion = fireball.collider.Position;


        }
    }
    else
    {
        IEnumerable<Entity> entitiesDuct = entities.WithComponents(typeof(DuctExtensionComponent), typeof(AnimationComponent), typeof(ColliderComponent));
        foreach (var player in entitiesDuct)
        {
            var collider = player.GetComponent<ColliderComponent>();
            float fireballY = fireballEntity.GetComponent<ColliderComponent>().collider.Position.Y;
            float fireballX = fireballEntity.GetComponent<ColliderComponent>().collider.Position.X;
            if (fireballY > collider.collider.Position.Y && fireballX > 28.5f && fireballX < 30.5f || fireballY > collider.collider.Position.Y && fireballX > 35.5f && fireballX < 37.5f)
            {
                pendingActions.Add(() => MoveFireball(fireballEntity));
            }
        }
    }

    var fireballBody = fireballEntity.GetComponent<ColliderComponent>()?.collider;
    if (fireballBody != null)
    {
        Restitution(fireballBody,forwardSpeedFire);
    }

    if (cameraComponent.Position.X / 100 > fireballEntity.GetComponent<ColliderComponent>().collider.Position.X)
    {
        pendingActions.Add(() => MoveFireballAfterWait(fireballEntity));
    }
    else if (cameraComponent.Position.X/77 < fireballEntity.GetComponent<ColliderComponent>().collider.Position.X && fireballEntity.GetComponent<ColliderComponent>().collider.Position.X > 40f )
    {
        pendingActions.Add(() => MoveFireballAfterWait(fireballEntity));

    }
    return true;
}

    private static void DisableCollider(Entity entity)
    {
        var colliderComponent = entity.GetComponent<ColliderComponent>();
        if (colliderComponent != null)
        {
            colliderComponent.RemoveCollider();
        }
    }

    private static void MoveFireball(Entity fireballEntity)
    {
        var colliderComponent = fireballEntity.GetComponent<ColliderComponent>();
        var fireAnimation = fireballEntity.GetComponent<AnimationComponent>();

        if (colliderComponent != null && fireAnimation != null)
        {
            var positionExprotion = colliderComponent.collider.Position;

            colliderComponent.collider.Position = positionExprotion;
            fireAnimation.animations = new AnimationComponent(Animations.entityTextures[EntitiesName.FIREEXPROTION], 34, 34).animations;
            colliderComponent.collider.BodyType = BodyType.Static;

            fireballTimers[fireballEntity] = waitTime;
        }
    }

    private static void MoveFireballAfterWait(Entity fireballEntity)
    {
        var colliderComponent = fireballEntity.GetComponent<ColliderComponent>();
        var fireAnimation = fireballEntity.GetComponent<AnimationComponent>();

        if (colliderComponent != null && fireAnimation != null)
        {
            colliderComponent.collider.Position = new AetherVector2(-100, 750);
            colliderComponent.collider.BodyType = BodyType.Static;
            fireAnimation.animations = new AnimationComponent(Animations.entityTextures[EntitiesName.FIRE], 34, 34).animations;
        }
    }

    private static void Restitution(Body body, float forwardSpeedFire)
    {
        float minBounceVelocity = 4.0f;

        if (body.LinearVelocity.Y > 0)
        {
            body.LinearVelocity = new AetherVector2(forwardSpeedFire, -minBounceVelocity);
        }
    }

}

}
