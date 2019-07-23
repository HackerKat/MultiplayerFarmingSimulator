using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MFS
{
    public class Prop : Entity
    {
        public delegate void CollisionEvent();

        public Prop (Vector2 position, ushort spriteID)
            : base (position, spriteID)
        {
            Sprite propSprite = SpriteManager.Instance.GetSprite(spriteID);
        }
        
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            Sprite propSprite = SpriteManager.Instance.GetSprite(spriteID);
            //TODO: bounds check
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > clientBounds.Width - CollisionRect.Width)
                position.X = clientBounds.Width - CollisionRect.Width;
            if (position.Y > clientBounds.Height - CollisionRect.Height)
                position.Y = clientBounds.Height - CollisionRect.Height;

            propSprite.Animate(gameTime);

            //TODO:  correct?
            collisionRect.X = (int)position.X;
            collisionRect.Y = (int)position.Y;
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite propSprite = SpriteManager.Instance.GetSprite(spriteID);

            propSprite.Draw(spriteBatch, position);
        }

        public override string GetEntityType()
        {
            return "prop";
        }

        public void CollisionDetected(Entity entity, CollisionEvent func)
        {
            if (CollisionRect.Intersects(entity.CollisionRect))
            {
                func();
            }
        }
    }
}