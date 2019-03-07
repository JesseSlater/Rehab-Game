using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D.Dynamics;


namespace Game1
{
    static class EntityManager
    {
        static List<Entity> entities = new List<Entity>();

        static bool isUpdating;
        static List<Entity> addedEntities = new List<Entity>();

        public static int Count { get { return entities.Count; } }
        private static World thisWorld;
        private static int maxEntities = 35;

        public static void Add(Entity entity)
        {
            if (!isUpdating)
            {
                entities.Add(entity);
            }
            else
            {
                addedEntities.Add(entity);
            }
        }


        public static void Add(Entity entity, World w)
        {
            if(thisWorld == null)
            {
                thisWorld = w;
            }

            if (!isUpdating)
            {
                entity.SetWorld(w);
                entities.Add(entity);
            }
            else
            {
                entity.SetWorld(w);
                addedEntities.Add(entity);
            }
        }

        public static void Update()
        {
            isUpdating = true;

            foreach (var entity in entities)
                entity.Update();

            isUpdating = false;

            foreach (var entity in addedEntities)
                entities.Add(entity);

            addedEntities.Clear();

            // remove any expired entities.
            entities = entities.Where(x => !x.IsExpired).ToList();
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var entity in entities)
                entity.Draw(spriteBatch);
        }

        public static void ClearLevel()
        {
            GameRoot.Player1 = null;
            GameRoot.canXCameraMove = false;
            GameRoot.canYCameraMove = false;
            foreach (var entity in entities)
            {
                entity.DestroyBody();
                entity.IsExpired = true;
                GameRoot.EnemiesInLevel = 0;
            }
        }

        public static bool TooManyEntitiesInExistence()
        {
            if(entities.Count > maxEntities)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        




    }
}
