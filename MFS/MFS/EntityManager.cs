using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS
{
    public class EntityManager
    {
        public static EntityManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntityManager();
                }
                return instance;
            }
        }
        private static EntityManager instance;
        private ushort lastID;

        private Dictionary<ushort, Entity> entities;

        public ushort PlayerID
        {
            get;
            set;
        }


        private EntityManager()
        {
            entities = new Dictionary<ushort, Entity>();
            lastID = 0;
        }

        public ushort AddEntity(Entity entity)
        {
            return AddEntity(entity, false);
        }

        //TODO: delete
        public ushort AddEntity(Entity entity, bool isPlayer)
        {
            ushort entityID = lastID;
            entities.Add(entityID, entity);
            
            lastID++;

            if (isPlayer)
            {
                PlayerID = entityID;
            }

            return entityID;
        }

        public ushort AddEntity(Entity entity, ushort id)
        {
            entities.Add(id, entity);
            return id;
        }


        public Entity GetEntity(ushort entityID)
        {
            if (entities.ContainsKey(entityID))
            {
                return entities[entityID];
            }
            return null;
        }

        public void RemoveEntity(ushort entityID)
        {
            if (entities.ContainsKey(entityID))
            {
                entities.Remove(entityID);
            }
        }

        public List<KeyValuePair<ushort, Entity>> GetAllEntities()
        {
            return entities.ToList();
        }

        public void Update(GameTime gameTime, Rectangle clientBounds)
        {
            foreach(Entity ent in entities.Values)
            {
                ent.Update(gameTime, clientBounds);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            List<Entity> entitiesToDraw = entities.Values.ToList();
            entitiesToDraw.Sort(CompareEntity);

            foreach (Entity ent in entitiesToDraw)
            {
                ent.Draw(spriteBatch);
            }
        }

        private int CompareEntity(Entity a, Entity b)
        {
            int heightA = a.Size.Y;
            int heightB = b.Size.Y;

            if (a.Position.Y + heightA < b.Position.Y + heightB)
            {
                return -1;
            }
            else if (a.Position.Y + heightA == b.Position.Y + heightB)
            {
                if (a.Position.X < b.Position.X)
                {
                    return -1;
                }
            }
            return 1;
        }

        public bool CollisionDetection(Entity a, Entity b, out Vector2 offset)
        {
            offset = Vector2.Zero;
            
            Rectangle aRect = a.CollisionRect;
            Rectangle bRect = b.CollisionRect;

            if (!aRect.Intersects(bRect))
            {
                return false;
            }
            Vector2 aHalfExtend = new Vector2(aRect.Width / 2f, aRect.Height / 2f);
            Vector2 bHalfExtend = new Vector2(bRect.Width / 2f, bRect.Height / 2f);

            Vector2 aCenter = new Vector2(aRect.X + aHalfExtend.X, aRect.Y + aHalfExtend.Y);
            Vector2 bCenter = new Vector2(bRect.X + bHalfExtend.X, bRect.Y + bHalfExtend.Y);

            Vector2 distance = bCenter - aCenter;

            if(Math.Abs(distance.X) > Math.Abs(distance.Y))
            {
                float length = 0f;

                if (distance.X < 0)
                {
                    length = (aCenter.X - aHalfExtend.X) - (bCenter.X + bHalfExtend.X);
                }

                else
                {
                    length = (aCenter.X + aHalfExtend.X) - (bCenter.X - bHalfExtend.X);
                }

                offset.X = length;
            }

            else
            {
                float length = 0f;

                if (distance.Y < 0)
                {
                    length = (aCenter.Y - aHalfExtend.Y) - (bCenter.Y + bHalfExtend.Y);
                }

                else
                {
                    length = (aCenter.Y + aHalfExtend.Y) - (bCenter.Y - bHalfExtend.Y);
                }

                offset.Y = length;

            }
            return true;
        }

        public void MoveEntity(ushort id, Vector2 moveVector)
        {
            Entity entity = GetEntity(id);
            entity.Position += moveVector;

            foreach (Entity ent in entities.Values)
            {
                if(ent != entity)
                {
                    Vector2 offset;
                    if (CollisionDetection(entity, ent, out offset))
                    {
                        entity.Position -= offset;
                        break;
                    }
                }
            }
        }

        public void Clear()
        {
            entities.Clear();
        }
    }
}
