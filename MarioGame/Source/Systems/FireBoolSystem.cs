using System;
using System.Collections.Generic;
using System.Linq;
using MarioGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using nkast.Aether.Physics2D.Dynamics;
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
        private static readonly Dictionary<Entity, float> fireballTimers = new Dictionary<Entity, float>();
        public static readonly float waitTime = 0.5f;

        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            if (gameTime != null)
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                IEnumerable<Entity> playerEntities = entities.WithComponents(typeof(PlayerComponent), typeof(AnimationComponent), typeof(ColliderComponent),typeof(MovementComponent), typeof(CameraComponent));
                foreach (var player in playerEntities)
                {
                    var keyboardState = Keyboard.GetState();
                    if (keyboardState.IsKeyDown(Keys.A))
                    {
                        if (canShoot)
                        {
                            HandleShooting(player.GetComponent<AnimationComponent>(), player.GetComponent<ColliderComponent>(), entities, pendingActions, player.GetComponent<CameraComponent>());
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

        private static void HandleShooting(AnimationComponent animation, ColliderComponent collider,IEnumerable<Entity> entities, List<Action> pendingActions,CameraComponent camera)
        {
            float offsetX = (animation.currentState == AnimationState.WALKLEFT || animation.currentState == AnimationState.JUMPLEFT || animation.currentState == AnimationState.STOPLEFT)
                    ? -0.8f
                    : 0.8f;
            float directionSpeed = (offsetX < 0) ? -forwardSpeed : forwardSpeed;
            if (animation.currentState == AnimationState.STOP || animation.currentState == AnimationState.WALKRIGHT ||
                animation.currentState == AnimationState.JUMPRIGHT || animation.currentState == AnimationState.STOPLEFT ||
                animation.currentState == AnimationState.WALKLEFT || animation.currentState == AnimationState.JUMPLEFT)
            {
                createFire(entities, collider.collider.Position.X + offsetX, collider.collider.Position.Y,pendingActions, camera, directionSpeed);
            }
        }

        private static void CheckFireballBounds(IEnumerable<Entity> entities)
        {
            IEnumerable<Entity> fireballEntities = entities.WithComponents(typeof(FireBoolComponent), typeof(ColliderComponent));
            foreach (var fireball in fireballEntities)
            {
                var fireballCollider = fireball.GetComponent<ColliderComponent>().collider;
                if (fireballCollider.BodyType == BodyType.Dynamic && fireballCollider.Position.Y > 8.5f)
                {
                    pendingActions.Add(() => FireBoolCollisionSystem.MoveFireballAfterWait(fireball));
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
                FireBoolCollisionSystem.MoveFireballAfterWait(fireball);
                fireballTimers.Remove(fireball);
            }
        }

        private static void createFire(IEnumerable<Entity> entities, float positionX, float positionY,List<Action> pendingActions,CameraComponent cameraComponent,float forwardSpeedFire)
        {
            var fireball = entities
                .WithComponents(typeof(FireBoolComponent), typeof(AnimationComponent), typeof(ColliderComponent))
                .FirstOrDefault(entity =>
                    entity.GetComponent<ColliderComponent>().collider.BodyType == BodyType.Static);
            if (fireball != null)
            {
                InitializeFireball(fireball, positionX, positionY, entities, pendingActions, cameraComponent, forwardSpeedFire);
            }
        }

        private static void InitializeFireball(Entity fireball, float positionX, float positionY,IEnumerable<Entity> entities,List<Action> pendingActions,CameraComponent cameraComponent,float forwardSpeedFire)
        {
            fireball.GetComponent<ColliderComponent>().collider.Position = new AetherVector2(positionX, positionY);
            fireball.GetComponent<ColliderComponent>().collider.BodyType = BodyType.Dynamic;
            fireball.GetComponent<ColliderComponent>().collider.OnCollision += (fixtureA, fixtureB, contact) => HandleCollision(fixtureA, fixtureB,
                 entities, pendingActions, cameraComponent, forwardSpeedFire);
            fireball.GetComponent<ColliderComponent>().collider.LinearVelocity = new AetherVector2(forwardSpeedFire, 0);
            fireball.GetComponent<AnimationComponent>().animations = new AnimationComponent(Animations.entityTextures[EntitiesName.FIRE], 34, 34).animations;
            fireball.GetComponent<FireBoolComponent>().InitialDirection = Math.Sign(forwardSpeedFire);
        }

        private static bool HandleCollision(Fixture fixtureA, Fixture fixtureB, IEnumerable<Entity> entities,List<Action> pendingActions,CameraComponent cameraComponent,float forwardSpeedFire)
        {
            var fireballEntity = FireBoolCollisionSystem.GetEntityByCollider(entities, fixtureA.Body) ??
                                 FireBoolCollisionSystem.GetEntityByCollider(entities, fixtureB.Body);
            if (fireballEntity == null || fireballEntity.GetComponent<FireBoolComponent>() == null) return true;
            forwardSpeedFire = fireballEntity.GetComponent<FireBoolComponent>().InitialDirection * Math.Abs(forwardSpeedFire);
            var otherEntity = FireBoolCollisionSystem.GetOtherEntityByCollider(entities, fixtureA, fixtureB, fireballEntity);
            if (otherEntity != null)
            {
                var animationComponent = otherEntity.GetComponent<AnimationComponent>();
                if (animationComponent != null)
                {
                    var otherEntityName = Animations.entityTextures.FirstOrDefault(x => x.Value == animationComponent.animations).Key;
                    HandleEntityCollision(otherEntityName, otherEntity, fireballEntity, fireballEntity.GetComponent<FireBoolComponent>(),pendingActions);
                }
            }
            else
            {
                HandleDuctExtensionCollision(entities, fireballEntity, pendingActions);
            }
            var fireballBody = fireballEntity.GetComponent<ColliderComponent>()?.collider;
            if (fireballBody != null)
            {
                FireBoolCollisionSystem.Restitution(fireballBody, forwardSpeedFire);
            }
            CheckCameraPosition(cameraComponent, fireballEntity, pendingActions);
            return true;
        }

        private static void HandleEntityCollision(EntitiesName otherEntityName, Entity otherEntity,Entity fireballEntity,FireBoolComponent fireballComponent,List<Action> pendingActions)
        {
            switch (otherEntityName)
            {
                case EntitiesName.GOOMBA: case EntitiesName.KOOPA:
                    if (fireballComponent != null)
                    {
                        fireballComponent.collidedWithGoomba = true;
                    }
                    pendingActions.Add(() => FireBoolCollisionSystem.DisableCollider(otherEntity));
                    pendingActions.Add(() => FireBoolCollisionSystem.MoveFireball(fireballEntity,fireballTimers,waitTime));
                    break;
                case EntitiesName.DUCT:
                    HandleDuctCollision(otherEntity, fireballEntity, pendingActions);
                    break;
                case EntitiesName.BLOCK: case EntitiesName.DUCTEXTENSION:
                    pendingActions.Add(() => FireBoolCollisionSystem.MoveFireball(fireballEntity,fireballTimers,waitTime));
                    break;
                case EntitiesName.COINBLOCK: case EntitiesName.QUESTIONBLOCK:
                    HandleBlockCollision(otherEntity, fireballEntity, pendingActions);
                    break;
                default:
                    break;
            }
        }

        private static void HandleDuctCollision(Entity otherEntity, Entity fireballEntity, List<Action> pendingActions)
        {
            if (fireballEntity.GetComponent<ColliderComponent>().collider.Position.Y > otherEntity.GetComponent<ColliderComponent>().Position.Y / 120 + 1)
            {
                pendingActions.Add(() => FireBoolCollisionSystem.MoveFireball(fireballEntity,fireballTimers,waitTime));
            }
        }

        private static void HandleBlockCollision(Entity otherEntity, Entity fireballEntity, List<Action> pendingActions)
        {
            float bottomPart2 = otherEntity.GetComponent<ColliderComponent>().Position.Y / 110;
            float fireballY2 = fireballEntity.GetComponent<ColliderComponent>().collider.Position.Y;
            if (fireballY2 > bottomPart2)
            {
                pendingActions.Add(() => FireBoolCollisionSystem.MoveFireball(fireballEntity,fireballTimers,waitTime));
            }
        }

        private static void HandleDuctExtensionCollision(IEnumerable<Entity> entities, Entity fireballEntity,List<Action> pendingActions)
        {
            var entitiesDuct = entities.WithComponents(typeof(DuctExtensionComponent), typeof(AnimationComponent),typeof(ColliderComponent));
            foreach (var player in entitiesDuct)
            {
                if ((fireballEntity.GetComponent<ColliderComponent>().collider.Position.Y > player.GetComponent<ColliderComponent>().collider.Position.Y &&
                     fireballEntity.GetComponent<ColliderComponent>().collider.Position.X > 28.5f && fireballEntity.GetComponent<ColliderComponent>().collider.Position.X < 30.5f) ||
                    (fireballEntity.GetComponent<ColliderComponent>().collider.Position.Y > player.GetComponent<ColliderComponent>().collider.Position.Y &&
                     fireballEntity.GetComponent<ColliderComponent>().collider.Position.X > 35.5f && fireballEntity.GetComponent<ColliderComponent>().collider.Position.X < 37.5f))
                {
                    pendingActions.Add(() => FireBoolCollisionSystem.MoveFireball(fireballEntity,fireballTimers,waitTime));
                }
            }
        }

        private static void CheckCameraPosition(CameraComponent cameraComponent, Entity fireballEntity,List<Action> pendingActions)
        {
            if (cameraComponent.Position.X / 100 > fireballEntity.GetComponent<ColliderComponent>().collider.Position.X)
            {
                pendingActions.Add(() => FireBoolCollisionSystem.MoveFireballAfterWait(fireballEntity));
            }
            else if (cameraComponent.Position.X / 77 < fireballEntity.GetComponent<ColliderComponent>().collider.Position.X && fireballEntity.GetComponent<ColliderComponent>().collider.Position.X > 40f)
            {
                pendingActions.Add(() => FireBoolCollisionSystem.MoveFireballAfterWait(fireballEntity));
            }
        }
    }
}
