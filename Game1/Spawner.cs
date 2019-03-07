using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;
using System.Diagnostics;


namespace Game1
{
    class Spawner: Entity
    {
        public Vector2 xSpawnRange = new Vector2(-10, 10);
        public Vector2 ySpawnRange = new Vector2(-10,10);
        public int spawnTimer;
        public int spawnTime = 200;
        Random r = new Random();
        public Interactable currInteractable;
        public Drifter currDrifter;
        public bool canSpawnBoxes;

        public Spawner(World w, Vector2 Pos, Vector2 xRange, Vector2 yRange, bool canSpawnBoxes)
        {
            world = w;
            Position = Pos;
            xSpawnRange = xRange;
            ySpawnRange = yRange;
            this.canSpawnBoxes = canSpawnBoxes;
        }

        public Spawner(World w, Vector2 Pos, Vector2 xRange, Vector2 yRange, bool canSpawnBoxes, int spawnTime)
        {
            world = w;
            Position = Pos;
            xSpawnRange = xRange;
            ySpawnRange = yRange;
            this.spawnTime = spawnTime;
            this.canSpawnBoxes = canSpawnBoxes;
        }

        public override void Update()
        {
            spawnTimer++;
            if(spawnTimer > spawnTime)
            {
                spawnTimer = 0;
                if(EntityManager.TooManyEntitiesInExistence())
                {
                    return;
                }
                Vector2 spawnOrientation = new Vector2(r.Next((int)xSpawnRange.X, (int)xSpawnRange.Y), r.Next((int)ySpawnRange.X, (int)ySpawnRange.Y));
                if (canSpawnBoxes)
                {
                    currInteractable = new Interactable(world, Position + new Vector2(spawnOrientation.X));
                    EntityManager.Add(currInteractable);
                    return;
                }
                currDrifter = new Drifter(world, Position)
                {
                    driftDirection = spawnOrientation
                };
                EntityManager.Add(currDrifter);
            }
        }
    }
}
