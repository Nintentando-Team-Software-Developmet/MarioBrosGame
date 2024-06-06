using MarioGame;
using MarioGame.Utils.DataStructures;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Entities
{
    public static class EnemyFactory
    {
        public static EnemyEntity CreateEnemy(EntitiesName name, EntityData entityData)
        {   if(entityData == null) throw new System.ArgumentNullException(nameof(entityData));
            Texture2D[] textures = null;
            switch (name)
            {
                case EntitiesName.GOOMBA:
                    textures = Animations.goombaTextures;
                    break;
            }

            return new EnemyEntity(textures, new Vector2(entityData.position.x, entityData.position.y));
        }
    }
}