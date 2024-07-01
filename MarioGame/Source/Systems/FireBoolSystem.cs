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
        public static readonly float waitTime = 0.5f;

        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            if (gameTime != null)
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

                IEnumerable<Entity> playerEntities = entities.WithComponents(
                    typeof(PlayerComponent), typeof(AnimationComponent), typeof(ColliderComponent),
                    typeof(MovementComponent), typeof(CameraComponent));
                IEnumerable<Entity> fireEntities = entities.WithComponents(
                    typeof(FireBoolComponent), typeof(AnimationComponent), typeof(ColliderComponent));

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
                            HandleShooting(animation, collider, entities, pendingActions, camera);
                            canShoot = false;
                        }
                    }
                    else
                    {
                        canShoot = true;
                    }

                    CheckFireballBounds(entities);
                }

                ExecutePendingActions();

                UpdateFireballTimers(deltaTime);
            }
        }

        private static void HandleShooting(AnimationComponent animation, ColliderComponent collider,
            IEnumerable<Entity> entities, List<Action> pendingActions, CameraComponent camera)
        {
            float offsetX =
                (animation.currentState == AnimationState.WALKLEFT ||
                 animation.currentState == AnimationState.JUMPLEFT || animation.currentState == AnimationState.STOPLEFT)
                    ? -0.8f
                    : 0.8f;
            float directionSpeed = (offsetX < 0) ? -forwardSpeed : forwardSpeed;

            if (animation.currentState == AnimationState.STOP ||
                animation.currentState == AnimationState.WALKRIGHT ||
                animation.currentState == AnimationState.JUMPRIGHT ||
                animation.currentState == AnimationState.STOPLEFT ||
                animation.currentState == AnimationState.WALKLEFT ||
                animation.currentState == AnimationState.JUMPLEFT)
            {
                createFire(entities, collider.collider.Position.X + offsetX, collider.collider.Position.Y,
                    pendingActions, camera, directionSpeed);
            }
        }

        private static void CheckFireballBounds(IEnumerable<Entity> entities)
        {
            IEnumerable<Entity> fireballEntities =
                entities.WithComponents(typeof(FireBoolComponent), typeof(ColliderComponent));
            foreach (var fireball in fireballEntities)
            {
                var fireballCollider = fireball.GetComponent<ColliderComponent>().collider;
                if (fireballCollider.BodyType == BodyType.Dynamic && fireballCollider.Position.Y > 8.5f)
                {
                    pendingActions.Add(() => MoveFireballAfterWait(fireball));
                }
            }
        }

        private static void ExecutePendingActions()
        {
            foreach (var action in pendingActions)
            {
                action();
            }

            pendingActions.Clear();
        }

        private static void UpdateFireballTimers(float deltaTime)
        {
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

        private static void createFire(IEnumerable<Entity> entities, float positionX, float positionY,
            List<Action> pendingActions, CameraComponent cameraComponent, float forwardSpeedFire)
        {
            var fireball = entities
                .WithComponents(typeof(FireBoolComponent), typeof(AnimationComponent), typeof(ColliderComponent))
                .FirstOrDefault(entity =>
                    entity.GetComponent<ColliderComponent>().collider.BodyType == BodyType.Static);

            if (fireball != null)
            {
                InitializeFireball(fireball, positionX, positionY, entities, pendingActions, cameraComponent,
                    forwardSpeedFire);
            }
        }

        private static void InitializeFireball(Entity fireball, float positionX, float positionY,
            IEnumerable<Entity> entities, List<Action> pendingActions, CameraComponent cameraComponent,
            float forwardSpeedFire)
        {
            var collider = fireball.GetComponent<ColliderComponent>();
            var fireAnimation = fireball.GetComponent<AnimationComponent>();
            var fireballComponent = fireball.GetComponent<FireBoolComponent>();

            collider.collider.Position = new AetherVector2(positionX, positionY);
            collider.collider.BodyType = BodyType.Dynamic;
            collider.collider.OnCollision += (fixtureA, fixtureB, contact) => HandleCollision(fixtureA, fixtureB,
                contact, entities, pendingActions, cameraComponent, forwardSpeedFire);
            collider.collider.LinearVelocity = new AetherVector2(forwardSpeedFire, 0);
            fireAnimation.animations =
                new AnimationComponent(Animations.entityTextures[EntitiesName.FIRE], 34, 34).animations;
            fireballComponent.InitialDirection = Math.Sign(forwardSpeedFire);
        }

        private static bool HandleCollision(Fixture fixtureA, Fixture fixtureB, Contact contact,
            IEnumerable<Entity> entities, List<Action> pendingActions, CameraComponent cameraComponent,
            float forwardSpeedFire)
        {
            var fireballEntity = GetEntityByCollider(entities, fixtureA.Body) ??
                                 GetEntityByCollider(entities, fixtureB.Body);

            if (fireballEntity == null || fireballEntity.GetComponent<FireBoolComponent>() == null) return true;

            var fireballComponent = fireballEntity.GetComponent<FireBoolComponent>();
            forwardSpeedFire = fireballComponent.InitialDirection * Math.Abs(forwardSpeedFire);

            var otherEntity = GetOtherEntityByCollider(entities, fixtureA, fixtureB, fireballEntity);

            if (otherEntity != null)
            {
                var animationComponent = otherEntity.GetComponent<AnimationComponent>();
                if (animationComponent != null)
                {
                    var otherEntityName = Animations.entityTextures
                        .FirstOrDefault(x => x.Value == animationComponent.animations).Key;
                    HandleEntityCollision(otherEntityName, otherEntity, fireballEntity, fireballComponent,
                        pendingActions);
                }
            }
            else
            {
                HandleDuctExtensionCollision(entities, fireballEntity, pendingActions);
            }

            var fireballBody = fireballEntity.GetComponent<ColliderComponent>()?.collider;
            if (fireballBody != null)
            {
                Restitution(fireballBody, forwardSpeedFire);
            }

            CheckCameraPosition(cameraComponent, fireballEntity, pendingActions);

            return true;
        }

        private static Entity GetEntityByCollider(IEnumerable<Entity> entities, Body body)
        {
            return entities.FirstOrDefault(e => e.GetComponent<ColliderComponent>()?.collider == body);
        }

        private static Entity GetOtherEntityByCollider(IEnumerable<Entity> entities, Fixture fixtureA, Fixture fixtureB,
            Entity fireballEntity)
        {
            return entities.FirstOrDefault(e =>
                e.GetComponent<ColliderComponent>()?.collider ==
                (fixtureA.Body == fireballEntity.GetComponent<ColliderComponent>()?.collider
                    ? fixtureB.Body
                    : fixtureA.Body));
        }

        private static void HandleEntityCollision(EntitiesName otherEntityName, Entity otherEntity,
            Entity fireballEntity, FireBoolComponent fireballComponent, List<Action> pendingActions)
        {
            switch (otherEntityName)
            {
                case EntitiesName.GOOMBA:
                case EntitiesName.KOOPA:
                    if (fireballComponent != null)
                    {
                        fireballComponent.collidedWithGoomba = true;
                    }

                    pendingActions.Add(() => DisableCollider(otherEntity));
                    pendingActions.Add(() => MoveFireball(fireballEntity));
                    break;

                case EntitiesName.DUCT:
                    HandleDuctCollision(otherEntity, fireballEntity, pendingActions);
                    break;

                case EntitiesName.BLOCK:
                case EntitiesName.DUCTEXTENSION:
                    pendingActions.Add(() => MoveFireball(fireballEntity));
                    break;

                case EntitiesName.COINBLOCK:
                case EntitiesName.QUESTIONBLOCK:
                    HandleBlockCollision(otherEntity, fireballEntity, pendingActions);
                    break;

                default:
                    break;
            }
        }

        private static void HandleDuctCollision(Entity otherEntity, Entity fireballEntity, List<Action> pendingActions)
        {
            var positionDuctComponent = otherEntity.GetComponent<ColliderComponent>();
            float bottomPart1 = positionDuctComponent.Position.Y / 120 + 1;
            float fireballY = fireballEntity.GetComponent<ColliderComponent>().collider.Position.Y;
            if (fireballY > bottomPart1)
            {
                pendingActions.Add(() => MoveFireball(fireballEntity));
            }
        }

        private static void HandleBlockCollision(Entity otherEntity, Entity fireballEntity, List<Action> pendingActions)
        {
            var positionBlockComponent = otherEntity.GetComponent<ColliderComponent>();
            float bottomPart2 = positionBlockComponent.Position.Y / 110;
            float fireballY2 = fireballEntity.GetComponent<ColliderComponent>().collider.Position.Y;
            if (fireballY2 > bottomPart2)
            {
                pendingActions.Add(() => MoveFireball(fireballEntity));
            }
        }

        private static void HandleDuctExtensionCollision(IEnumerable<Entity> entities, Entity fireballEntity,
            List<Action> pendingActions)
        {
            var entitiesDuct = entities.WithComponents(typeof(DuctExtensionComponent), typeof(AnimationComponent),
                typeof(ColliderComponent));
            foreach (var player in entitiesDuct)
            {
                var collider = player.GetComponent<ColliderComponent>();
                float fireballY = fireballEntity.GetComponent<ColliderComponent>().collider.Position.Y;
                float fireballX = fireballEntity.GetComponent<ColliderComponent>().collider.Position.X;
                if ((fireballY > collider.collider.Position.Y && fireballX > 28.5f && fireballX < 30.5f) ||
                    (fireballY > collider.collider.Position.Y && fireballX > 35.5f && fireballX < 37.5f))
                {
                    pendingActions.Add(() => MoveFireball(fireballEntity));
                }
            }
        }

        private static void CheckCameraPosition(CameraComponent cameraComponent, Entity fireballEntity,
            List<Action> pendingActions)
        {
            if (cameraComponent.Position.X / 100 > fireballEntity.GetComponent<ColliderComponent>().collider.Position.X)
            {
                pendingActions.Add(() => MoveFireballAfterWait(fireballEntity));
            }
            else if (cameraComponent.Position.X / 77 <
                     fireballEntity.GetComponent<ColliderComponent>().collider.Position.X &&
                     fireballEntity.GetComponent<ColliderComponent>().collider.Position.X > 40f)
            {
                pendingActions.Add(() => MoveFireballAfterWait(fireballEntity));
            }
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
                fireAnimation.animations =
                    new AnimationComponent(Animations.entityTextures[EntitiesName.FIREEXPLOSION], 34, 34).animations;
                colliderComponent.Enabled(false);

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
                fireAnimation.animations = new AnimationComponent(Animations.entityTextures[EntitiesName.FIRE], 34, 34)
                    .animations;
                colliderComponent.Enabled(true);
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
