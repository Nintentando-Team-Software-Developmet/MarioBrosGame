
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;

using MarioGame;

using Microsoft.Xna.Framework.Graphics;

using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Common;
using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Utils;

public static class ChangeAnimationColliderPlayer
{

    public static void TransformTogetherWithPlayerStatus(PlayerComponent playerComponent,AnimationComponent playerAnimation, ColliderComponent playerCollider)
    {
        if (playerComponent != null && playerComponent.statusMario == StatusMario.BigMario)
        {
            if (playerAnimation != null && playerAnimation.currentState == AnimationState.BENDRIGHT ||
                playerAnimation != null && playerAnimation.currentState == AnimationState.BENDLEFT)
            {
                TransformToBigBendMario(playerAnimation, playerCollider);
            }
            else
            {
                TransformToBigMario(playerAnimation,playerCollider);
            }

        }else if (playerComponent != null && playerComponent.statusMario == StatusMario.FireMario)
        {
            if (playerAnimation != null && playerAnimation.currentState == AnimationState.BENDRIGHT ||
                playerAnimation != null && playerAnimation.currentState == AnimationState.BENDLEFT)
            {
                TransformToBigBendMario(playerAnimation, playerCollider);
            }
            else
            {
                TransformToFireMario(playerAnimation,playerCollider);
            }
        }
        else if (playerComponent != null && playerComponent.statusMario == StatusMario.StarMarioBig)
        {
            if (playerAnimation != null && playerAnimation.currentState == AnimationState.BENDRIGHT ||
                playerAnimation != null && playerAnimation.currentState == AnimationState.BENDLEFT)
            {
                TransformToBigBendMario(playerAnimation, playerCollider);
            }
            else
            {
                TransformToBigMarioStar(playerAnimation,playerCollider);
            }
        }else if (playerComponent != null && playerComponent.statusMario == StatusMario.SmallMario)
        {
            TransformToSmallMario(playerAnimation,playerCollider);
        }
        else if (playerComponent != null && playerComponent.statusMario == StatusMario.StarMarioSmall)
        {
            TransformToSmallMarioStar(playerAnimation,playerCollider);
        }

    }
    public static void TransformToBigMarioStar(AnimationComponent playerAnimation, ColliderComponent playerCollider)
    {
        if (playerAnimation != null)
            if (playerCollider != null)
                TransformMario(playerAnimation, Animations.entityTextures[EntitiesName.STARBIGMARIO], 64, 101,playerCollider, 40f, 51f);
    }
    public static void TransformToBigMario(AnimationComponent playerAnimation, ColliderComponent playerCollider)
    {
        if (playerAnimation != null)
            if (playerCollider != null)
                TransformMario(playerAnimation, Animations.entityTextures[EntitiesName.BIGMARIO], 64, 101,playerCollider, 40f, 51f);
    }
    public static void TransformToFireMario(AnimationComponent playerAnimation, ColliderComponent playerCollider)
    {
        if (playerAnimation != null)
            if (playerCollider != null)
                TransformMario(playerAnimation, Animations.entityTextures[EntitiesName.FIREMARIO], 64, 101,playerCollider, 40f, 51f);

    }
    public static void TransformToSmallMario(AnimationComponent playerAnimation, ColliderComponent playerCollider)
    {

        if (playerAnimation != null)
            if (playerCollider != null)
                TransformMario(playerAnimation, Animations.entityTextures[EntitiesName.MARIO], 64, 64,playerCollider, 30f, 32f);

    }
    public static void TransformToSmallMarioStar(AnimationComponent playerAnimation, ColliderComponent playerCollider)
    {

        if (playerAnimation != null)
            if (playerCollider != null)
                TransformMario(playerAnimation, Animations.entityTextures[EntitiesName.STARSMALLMARIO], 64, 64,playerCollider, 30f, 32f);

    }
    private static void TransformMario(AnimationComponent playerAnimation, Dictionary<AnimationState, Texture2D[]> animations, int animationWidth, int animationHeight, ColliderComponent playerCollider, float colliderWidth, float colliderHeight)
    {
        playerAnimation.animations = new AnimationComponent(animations, animationWidth, animationHeight, 0.09f).animations;
        playerAnimation.UpdateAnimationSize(animationWidth, animationHeight);

        if (playerCollider.collider.FixtureList.Count > 0)
        {
            var colliderShape = playerCollider.collider.FixtureList[0].Shape;
            if (colliderShape is PolygonShape polygonShape)
            {
                var halfWidth = colliderWidth / GameConstants.pixelPerMeter;
                var halfHeight = colliderHeight / GameConstants.pixelPerMeter;
                polygonShape.Vertices = PolygonTools.CreateRectangle(halfWidth, halfHeight);
            }
        }
    }

    private static void TransformMario(AnimationComponent playerAnimation,int animationWidth,
        int animationHeight,ColliderComponent playerCollider,float colliderWidth, float colliderHeight)
    {
        playerAnimation.UpdateAnimationSize(animationWidth, animationHeight);

        var colliderShape = playerCollider.collider.FixtureList[0].Shape;
        if (colliderShape is PolygonShape polygonShape)
        {
            var halfWidth = colliderWidth / GameConstants.pixelPerMeter;
            var halfHeight = colliderHeight / GameConstants.pixelPerMeter;
            polygonShape.Vertices = PolygonTools.CreateRectangle(halfWidth, halfHeight);
        }
    }

    public static void TransformToBigBendMario(AnimationComponent playerAnimation, ColliderComponent playerCollider)
    {
        if (playerAnimation != null)
            if (playerCollider != null)
                TransformMario(playerAnimation, 64, 90, playerCollider, 40f, 47f);
    }

    public static void CheckEnemyProximity(ColliderComponent playerCollider, IEnumerable<Entity> enemyEntities, GameTime gameTime,double invulnerabilityEndTime)
    {
        if (playerCollider != null)
        {
            var playerPosition = playerCollider.collider.Position;

            if (enemyEntities != null)
                foreach (var enemyEntity in enemyEntities)
                {
                    var enemyCollider = enemyEntity.GetComponent<ColliderComponent>();
                    if (enemyCollider != null)
                    {
                        var enemyPosition = enemyCollider.collider.Position;
                        float distance;
                        AetherVector2.Distance(ref playerPosition, ref enemyPosition, out distance);

                        if (distance < 2f)
                        {
                            enemyCollider.Enabled(false);
                        }

                        if (gameTime != null && gameTime.TotalGameTime.TotalSeconds > invulnerabilityEndTime)
                        {
                            enemyCollider.Enabled(true);
                        }
                        else
                        {
                            enemyCollider.Enabled(true);
                        }
                    }
                }
        }
    }

}
