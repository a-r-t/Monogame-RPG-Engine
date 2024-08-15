using Engine.Core;
using Engine.Extensions;
using Engine.Entity;
using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

// This class represents a Map's "Camera", aka a piece of the map that is currently included in a level's update/draw logic based on what should be shown on screen.
// A majority of its job is just determining which map tiles, enemies, npcs, and enhanced map tiles are "active" each frame (active = included in update/draw cycle)
namespace Engine.Scene
{
    public class Camera : Rectangle
    {
        // the current map this camera is attached to
        private Map map;

        // width and height of each tile in the map (the map's tileset has this info)
        private int tileWidth, tileHeight;

        // if the screen is covered in full length tiles, often there will be some extra room that doesn't quite have enough space for another entire tile
        // this leftover space keeps track of that "extra" space, which is needed to calculate the camera's current "end" position on the screen (in map coordinates, not screen coordinates)
        private int leftoverSpaceX, leftoverSpaceY;

        // current map entities that are to be included in this frame's update/draw cycle
        public List<EnhancedMapTile> ActiveEnhancedMapTiles { get; private set; }
        public List<NPC> ActiveNPCs { get; private set; }
        public List<Trigger> ActiveTriggers { get; private set; }

        // determines how many tiles off screen an entity can be before it will be deemed inactive and not included in the update/draw cycles until it comes back in range
        private const int UPDATE_OFF_SCREEN_RANGE = 4;

        // gets end bound X position of the camera (start position is always 0)
        public float EndBoundX
        {
            get
            {
                return X + (Width * tileWidth) + leftoverSpaceX;
            }
        }

        // gets end bound Y position of the camera (start position is always 0)
        public float EndBoundY
        {
            get
            {
                return Y + (Height * tileHeight) + leftoverSpaceY;
            }
        }

        public Camera(int startX, int startY, int tileWidth, int tileHeight, Map map)
            : base(startX, startY, ScreenManager.ScreenWidth / tileWidth, ScreenManager.ScreenHeight / tileHeight)
        {
            this.map = map;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.leftoverSpaceX = ScreenManager.ScreenWidth % this.tileWidth;
            this.leftoverSpaceY = ScreenManager.ScreenHeight % this.tileHeight;
            ActiveEnhancedMapTiles = new List<EnhancedMapTile>();
            ActiveNPCs = new List<NPC>();
            ActiveTriggers = new List<Trigger>();
        }

        // gets the tile index that the camera's x and y values are currently on (top left tile)
        // this is used to determine a starting place for the rectangle of area the camera currently contains on the map
        public Point GetTileIndexByCameraPosition()
        {
            int xIndex = (X / tileWidth).Round();
            int yIndex = (Y / tileHeight).Round();
            return new Point(xIndex, yIndex);
        }

        public void Update(Player player)
        {
            UpdateAnimatedMapTiles();
            UpdateMapEntities(player);
            UpdateScripts();
        }

        // 
        private void UpdateAnimatedMapTiles()
        {
            foreach (MapTile tile in map.AnimatedMapTiles)
            {
                // update each animated map tile in order to keep animations consistent
                tile.Update();
            }
        }

        // update map entities currently a part of the update/draw cycle
        // active entities are calculated each frame using the loadActiveEntity methods below
        public void UpdateMapEntities(Player player)
        {
            ActiveEnhancedMapTiles = LoadActiveEnhancedMapTiles();
            ActiveNPCs = LoadActiveNPCs();
            ActiveTriggers = LoadActiveTriggers();

            foreach (EnhancedMapTile enhancedMapTile in ActiveEnhancedMapTiles)
            {
                enhancedMapTile.Update(player);
            }

            foreach (NPC npc in ActiveNPCs)
            {
                npc.Update(player);
            }
        }


        // updates any currently running script
        // only one script should be able to be running (active) at a time
        private void UpdateScripts()
        {
            // if there is an active interact script, update the script
            if (map.ActiveScript != null)
            {
                map.ActiveScript.Update();
            }
        }

        // determine which enhanced map tiles are active (exist and are within range of the camera)
        private List<EnhancedMapTile> LoadActiveEnhancedMapTiles()
        {
            List<EnhancedMapTile> activeEnhancedMapTiles = new List<EnhancedMapTile>();
            for (int i = map.EnhancedMapTiles.Count - 1; i >= 0; i--)
            {
                EnhancedMapTile enhancedMapTile = map.EnhancedMapTiles[i];

                if (IsMapEntityActive(enhancedMapTile))
                {
                    activeEnhancedMapTiles.Add(enhancedMapTile);
                    if (enhancedMapTile.MapEntityStatus == MapEntityStatus.INACTIVE)
                    {
                        enhancedMapTile.MapEntityStatus = MapEntityStatus.ACTIVE;
                    }
                }
                else if (enhancedMapTile.MapEntityStatus == MapEntityStatus.ACTIVE)
                {
                    enhancedMapTile.MapEntityStatus = MapEntityStatus.INACTIVE;
                }
                else if (enhancedMapTile.MapEntityStatus == MapEntityStatus.REMOVED)
                {
                    map.EnhancedMapTiles.RemoveAt(i);
                }
            }
            return activeEnhancedMapTiles;
        }

        // determine which npcs are active (exist and are within range of the camera)
        private List<NPC> LoadActiveNPCs()
        {
            List<NPC> activeNPCs = new List<NPC>();
            for (int i = map.NPCs.Count - 1; i >= 0; i--)
            {
                NPC npc = map.NPCs[i];

                if (IsMapEntityActive(npc))
                {
                    activeNPCs.Add(npc);
                    if (npc.MapEntityStatus == MapEntityStatus.INACTIVE)
                    {
                        npc.MapEntityStatus = MapEntityStatus.ACTIVE;
                    }
                }
                else if (npc.MapEntityStatus == MapEntityStatus.ACTIVE)
                {
                    npc.MapEntityStatus = MapEntityStatus.INACTIVE;
                }
                else if (npc.MapEntityStatus == MapEntityStatus.REMOVED)
                {
                    map.NPCs.RemoveAt(i);
                }
            }
            return activeNPCs;
        }

        // determine which triggers are active (exist and are within range of the camera)
        private List<Trigger> LoadActiveTriggers()
        {
            List<Trigger> activeTriggers = new List<Trigger>();
            for (int i = map.Triggers.Count - 1; i >= 0; i--)
            {
                Trigger trigger = map.Triggers[i];

                if (IsMapEntityActive(trigger))
                {
                    activeTriggers.Add(trigger);
                    if (trigger.MapEntityStatus == MapEntityStatus.INACTIVE)
                    {
                        trigger.MapEntityStatus = MapEntityStatus.ACTIVE;
                    }
                }
                else if (trigger.MapEntityStatus == MapEntityStatus.ACTIVE)
                {
                    trigger.MapEntityStatus = MapEntityStatus.INACTIVE;
                }
                else if (trigger.MapEntityStatus == MapEntityStatus.REMOVED)
                {
                    map.Triggers.RemoveAt(i);
                }
            }
            return activeTriggers;
        }

        /*
            determines if map entity (enemy, enhanced map tile, or npc) is active by the camera's standards
            1. if entity's status is REMOVED, it is not active, no questions asked
            2. if an entity is hidden, it is not active
            3. if entity's status is not REMOVED and the entity is not hidden, then there's additional checks that take place:
                1. if entity's isUpdateOffScreen attribute is true, it is active
                2. OR if the camera determines that it is in its boundary range, it is active
         */
        private bool IsMapEntityActive(MapEntity mapEntity)
        {
            return mapEntity.MapEntityStatus != MapEntityStatus.REMOVED && !mapEntity.IsHidden && mapEntity.Exists && (mapEntity.IsUpdateOffScreen || ContainsUpdate(mapEntity));
        }

        public override void Draw(GraphicsHandler graphicsHandler)
        {
            DrawMapTilesBottomLayer(graphicsHandler);
            DrawMapTilesTopLayer(graphicsHandler);
        }

        public void Draw(Player player, GraphicsHandler graphicsHandler)
        {
            DrawMapTilesBottomLayer(graphicsHandler);
            DrawMapEntities(player, graphicsHandler);
            DrawMapTilesTopLayer(graphicsHandler);
        }

        // draws the bottom layer of visible map tiles to the screen
        // this is different than "active" map tiles as determined in the update method -- there is no reason to actually draw to screen anything that can't be seen
        // so this does not include the extra range granted by the UPDATE_OFF_SCREEN_RANGE value
        public void DrawMapTilesBottomLayer(GraphicsHandler graphicsHandler)
        {
            Point tileIndex = GetTileIndexByCameraPosition();
            for (int i = tileIndex.Y.Round() - 1; i <= tileIndex.Y + Height + 1; i++)
            {
                for (int j = tileIndex.X.Round() - 1; j <= tileIndex.X + Width + 1; j++)
                {
                    MapTile tile = map.GetMapTile(j, i);
                    if (tile != null)
                    {
                        tile.DrawBottomLayer(graphicsHandler);
                    }
                }
            }

            foreach (EnhancedMapTile enhancedMapTile in ActiveEnhancedMapTiles)
            {
                if (ContainsDraw(enhancedMapTile))
                {
                    enhancedMapTile.DrawBottomLayer(graphicsHandler);
                }
            }
        }

        // draws the top layer of visible map tiles to the screen where applicable
        public void DrawMapTilesTopLayer(GraphicsHandler graphicsHandler)
        {
            Point tileIndex = GetTileIndexByCameraPosition();
            for (int i = tileIndex.Y.Round() - 1; i <= tileIndex.Y + Height + 1; i++)
            {
                for (int j = tileIndex.X.Round() - 1; j <= tileIndex.X + Width + 1; j++)
                {
                    MapTile tile = map.GetMapTile(j, i);
                    if (tile != null && tile.TopLayer != null)
                    {
                        tile.DrawTopLayer(graphicsHandler);
                    }
                }
            }

            foreach (EnhancedMapTile enhancedMapTile in ActiveEnhancedMapTiles)
            {
                if (ContainsDraw(enhancedMapTile) && enhancedMapTile.TopLayer != null)
                {
                    enhancedMapTile.DrawTopLayer(graphicsHandler);
                }
            }
        }



        // draws active map entities to the screen
        public void DrawMapEntities(Player player, GraphicsHandler graphicsHandler)
        {
            List<NPC> drawNpcsAfterPlayer = new List<NPC>();

            // goes through each active npc and determines if it should be drawn at this time based on their location relative to the player
            // if drawn here, npc will later be "overlapped" by player
            // if drawn later, npc will "cover" player
            foreach (NPC npc in ActiveNPCs)
            {
                if (npc.Bounds.Y1 < player.Bounds.Y1 + (player.Bounds.Height / 2f))
                {
                    npc.Draw(graphicsHandler);
                }
                else
                {
                    drawNpcsAfterPlayer.Add(npc);
                }
            }

            // player is drawn to screen
            player.Draw(graphicsHandler);

            // npcs determined to be drawn after player from the above step are drawn here
            foreach (NPC npc in drawNpcsAfterPlayer)
            {
                npc.Draw(graphicsHandler);
            }

            // Uncomment this to see triggers drawn on screen
            // helps for placing them in the correct spot/debugging
            /*
            foreach (Trigger trigger in activeTriggers) 
            {
                if (ContainsDraw(trigger)) 
                {
                    trigger.Draw(graphicsHandler);
                }
            }
            */
        }

        // checks if a game object's position falls within the camera's current radius
        // TODO: replace x1 + width with x2, etc?
        public bool ContainsUpdate(GameObject gameObject)
        {
            return X1 - (tileWidth * UPDATE_OFF_SCREEN_RANGE) < gameObject.X1 + gameObject.Width &&
                    EndBoundX + (tileWidth * UPDATE_OFF_SCREEN_RANGE) > gameObject.X1 &&
                    Y1 - (tileHeight * UPDATE_OFF_SCREEN_RANGE) < gameObject.Y1 + gameObject.Height &&
                    EndBoundY + (tileHeight * UPDATE_OFF_SCREEN_RANGE) > gameObject.Y;
        }

        // checks if a game object's position falls within the camera's current radius
        // this does not include the extra range granted by the UDPATE_OFF_SCREEN_RANGE value, because there is no point to drawing graphics that can't be seen
        public bool ContainsDraw(GameObject gameObject)
        {
            return X1 - tileWidth < gameObject.X1 + gameObject.Width && EndBoundX + tileWidth > gameObject.X1 &&
                    Y1 - tileHeight < gameObject.Y1 + gameObject.Height && EndBoundY + tileHeight > gameObject.Y1;
        }

        public bool IsAtTopOfMap()
        {
            return Y1 <= 0;
        }

        public bool IsAtBottomOfMap()
        {
            return EndBoundY >= map.EndBoundY;
        }
    }
}
