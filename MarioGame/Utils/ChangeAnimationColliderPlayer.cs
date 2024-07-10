
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

    public static void TransformTogetherWithPlayerStatus(PlayerComponent playerComponent, AnimationComponent playerAnimation, ColliderComponent playerCollider)
    {
        if (playerComponent == null || playerAnimation == null)
        {
            return;
        }
        switch (playerComponent.statusMario)
        {
            case StatusMario.BigMario:
                if (IsBending(playerAnimation))
                {
                    TransformToBigBendMario(playerAnimation, playerCollider);
                }
                else
                {
                    TransformToBigMario(playerAnimation, playerCollider);
                }
                break;
            case StatusMario.FireMario:
                if (IsBending(playerAnimation))
                {
                    TransformToBigBendMario(playerAnimation, playerCollider);
                }
                else
                {
                    TransformToFireMario(playerAnimation, playerCollider);
                }
                break;
            case StatusMario.StarMarioBig:
                if (IsBending(playerAnimation))
                {
                    TransformToBigBendMario(playerAnimation, playerCollider);
                }
                else
                {
                    TransformToBigMarioStar(playerAnimation, playerCollider);
                }
                break;
            case StatusMario.SmallMario:
                TransformToSmallMario(playerAnimation, playerCollider);
                break;
            case StatusMario.StarMarioSmall:
                TransformToSmallMarioStar(playerAnimation, playerCollider);
                break;
            default:
                break;
        }
    }

    private static bool IsBending(AnimationComponent playerAnimation)
    {
        return playerAnimation.currentState == AnimationState.BENDRIGHT || playerAnimation.currentState == AnimationState.BENDLEFT;
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

    public static void CheckEnemyProximity(ColliderComponent playerCollider, AnimationComponent playerAnimation, IEnumerable<Entity> enemyEntities, GameTime gameTime, double invulnerabilityEndTime)
    {
        if (playerCollider != null)
        {
            var playerPosition = playerCollider.collider.Position;
            if (enemyEntities != null)
            {
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
            convertAnimationMario(gameTime, playerCollider, playerAnimation);
        }
    }

    private static void convertAnimationMario( GameTime gameTime,ColliderComponent playerCollider, AnimationComponent playerAnimation)
    {
        if (gameTime != null)
        {
            double currentTime = gameTime.TotalGameTime.TotalSeconds;
            double oscillationPeriod = 0.2;
            double elapsedTime = currentTime % oscillationPeriod;
            float newColliderHeight = (elapsedTime < oscillationPeriod / 2) ? 90f : 60f;
            int newAnimationHeight = (elapsedTime < oscillationPeriod / 2) ? 90 : 60;
            if (playerCollider.collider.FixtureList.Count > 0)
            {
                var colliderShape = playerCollider.collider.FixtureList[0].Shape;
                if (colliderShape is PolygonShape polygonShape)
                {
                    var halfWidth = polygonShape.Vertices.GetAABB().Width / 2;
                    var halfHeight = newColliderHeight / GameConstants.pixelPerMeter / 2;
                    polygonShape.Vertices = PolygonTools.CreateRectangle(halfWidth, halfHeight);
                }
            }
            if (playerAnimation != null) playerAnimation.UpdateAnimationSize(playerAnimation.width, newAnimationHeight);
        }
    }

}
