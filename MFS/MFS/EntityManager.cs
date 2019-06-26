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
            foreach (Entity ent in entities.Values)
            {
                ent.Draw(spriteBatch);
            }
        }

        public void Clear()
        {
            entities.Clear();
        }
    }
}
