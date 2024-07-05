
using System.Collections.Generic;

using Microsoft.Xna.Framework;


using MarioGame;

using Microsoft.Xna.Framework.Graphics;

using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Common;
using SuperMarioBros.Source.Components;

using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Utils;

public static class ChangeAnimationColliderPlayer
{
    public static void TransformToBigMario(AnimationComponent playerAnimation, ColliderComponent playerCollider)
    {
        if (playerAnimation != null)
            if (playerCollider != null)
                TransformMario(playerAnimation, Animations.entityTextures[EntitiesName.BIGMARIO], 64, 100,playerCollider, 40f, 50f);
    }
    public static void TransformToFireMario(AnimationComponent playerAnimation, ColliderComponent playerCollider)
    {
        if (playerAnimation != null)
            if (playerCollider != null)
                TransformMario(playerAnimation, Animations.entityTextures[EntitiesName.FIREMARIO], 64, 100,playerCollider, 40f, 50f);

    }
    public static void TransformToSmallMario(AnimationComponent playerAnimation, ColliderComponent playerCollider)
    {

        if (playerAnimation != null)
            if (playerCollider != null)
                TransformMario(playerAnimation, Animations.entityTextures[EntitiesName.MARIO], 64, 64,playerCollider, 30f, 32f);

    }
    private static void TransformMario(AnimationComponent playerAnimation,Dictionary<AnimationState, Texture2D[]> animations,int animationWidth,
        int animationHeight,ColliderComponent playerCollider,float colliderWidth, float colliderHeight)
    {
        playerAnimation.animations = new AnimationComponent(animations, animationWidth, animationHeight, 0.09f).animations;
        playerAnimation.UpdateAnimationSize(animationWidth, animationHeight);

        var colliderShape = playerCollider.collider.FixtureList[0].Shape;
        if (colliderShape is PolygonShape polygonShape)
        {
            var halfWidth = colliderWidth / GameConstants.pixelPerMeter;
            var halfHeight = colliderHeight / GameConstants.pixelPerMeter;
            polygonShape.Vertices = PolygonTools.CreateRectangle(halfWidth, halfHeight);
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
                TransformMario(playerAnimation, 64, 85, playerCollider, 40f, 45f);
    }
    public static void TransformToBigNormalMario(AnimationComponent playerAnimation, ColliderComponent playerCollider)
    {
        if (playerAnimation != null)
            if (playerCollider != null)
                TransformMario(playerAnimation, 64, 100, playerCollider, 40f, 50f);
    }



}
