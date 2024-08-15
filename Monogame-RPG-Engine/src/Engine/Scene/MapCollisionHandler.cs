using Engine.Extensions;
using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Text;

// This class has methods to check if a game object has collided with a map tile
// it is used by the game object class to determine if a collision occurred
namespace Engine.Scene
{
    public class MapCollisionHandler
    {
        // x axis collision logic
        // determines if a collision occurred with another entity on the map, and calculates where gameobject should be placed to resolve the collision
        public static MapCollisionCheckResult GetAdjustedPositionAfterCollisionCheckX(GameObject gameObject, Map map, Direction direction)
        {
            int numberOfTilesToCheck = Math.Max(gameObject.Bounds.Height / map.Tileset.SpriteHeightScaled, 1);
            float edgeBoundX = direction == Direction.LEFT ? gameObject.Bounds.X1 : gameObject.Bounds.X2;
            Point tileIndex = map.GetTileIndexByPosition(edgeBoundX, gameObject.Bounds.Y1);
            GameObject entityCollidedWith = null;
            for (int i = -1; i <= numberOfTilesToCheck + 1; i++)
            {
                MapTile mapTile = map.GetMapTile(tileIndex.X.Round(), tileIndex.Y.Round() + i);
                if (mapTile != null && HasCollidedWithMapEntity(gameObject, mapTile, direction))
                {
                    entityCollidedWith = mapTile;
                    float adjustedPositionX = gameObject.X;
                    if (direction == Direction.RIGHT)
                    {
                        float boundsDifference = gameObject.X2 - gameObject.Bounds.X2;
                        adjustedPositionX = mapTile.Bounds.X1 - gameObject.Width + boundsDifference;
                    }
                    else if (direction == Direction.LEFT)
                    {
                        float boundsDifference = gameObject.Bounds.X1 - gameObject.X;
                        adjustedPositionX = (mapTile.Bounds.X2 + 1) - boundsDifference;
                    }
                    return new MapCollisionCheckResult(new Point(adjustedPositionX, gameObject.Y), entityCollidedWith);
                }
            }

            // check active enhanced map tiles for potential collision
            foreach (EnhancedMapTile enhancedMapTile in map.ActiveEnhancedMapTiles)
            {
                if (!gameObject.Equals(enhancedMapTile) && !enhancedMapTile.IsUncollidable && HasCollidedWithMapEntity(gameObject, enhancedMapTile, direction))
                {
                    entityCollidedWith = enhancedMapTile;
                    float adjustedPositionX = gameObject.X;
                    if (direction == Direction.RIGHT)
                    {
                        float boundsDifference = gameObject.X2 - gameObject.Bounds.X2;
                        adjustedPositionX = enhancedMapTile.Bounds.X1 - gameObject.Width + boundsDifference;
                    }
                    else if (direction == Direction.LEFT)
                    {
                        float boundsDifference = gameObject.Bounds.X1 - gameObject.X;
                        adjustedPositionX = (enhancedMapTile.Bounds.X2 + 1) - boundsDifference;
                    }
                    return new MapCollisionCheckResult(new Point(adjustedPositionX, gameObject.Y), entityCollidedWith);
                }
            }

            // check active npcs for potential collision
            foreach (NPC npc in map.ActiveNPCs)
            {
                if (!gameObject.Equals(npc) && !npc.IsUncollidable && HasCollidedWithMapEntity(gameObject, npc, direction))
                {
                    entityCollidedWith = npc;
                    float adjustedPositionX = gameObject.X;
                    if (direction == Direction.RIGHT)
                    {
                        float boundsDifference = gameObject.X2 - gameObject.Bounds.X2;
                        adjustedPositionX = npc.Bounds.X1 - gameObject.Width + boundsDifference;
                    }
                    else if (direction == Direction.LEFT)
                    {
                        float boundsDifference = gameObject.Bounds.X1 - gameObject.X;
                        adjustedPositionX = (npc.Bounds.X2 + 1) - boundsDifference;
                    }
                    return new MapCollisionCheckResult(new Point(adjustedPositionX, gameObject.Y), entityCollidedWith);
                }
            }

            // check active triggers for potential collision
            if (gameObject.IsAffectedByTriggers)
            {
                foreach (Trigger trigger in map.ActiveTriggers)
                {
                    if (!gameObject.Equals(trigger) && !trigger.IsUncollidable && trigger.Exists && HasCollidedWithMapEntity(gameObject, trigger, direction))
                    {
                        entityCollidedWith = trigger;
                        float adjustedPositionX = gameObject.X;
                        if (direction == Direction.RIGHT)
                        {
                            float boundsDifference = gameObject.X2 - gameObject.Bounds.X2;
                            adjustedPositionX = trigger.Bounds.X1 - gameObject.Width + boundsDifference;
                        }
                        else if (direction == Direction.LEFT)
                        {
                            float boundsDifference = gameObject.Bounds.X1 - gameObject.X;
                            adjustedPositionX = (trigger.Bounds.X2 + 1) - boundsDifference;
                        }
                        return new MapCollisionCheckResult(new Point(adjustedPositionX, gameObject.Y), entityCollidedWith);
                    }
                }
            }

            // check for collision with player
            // this is to allow non-player entities to collision check against the player
            Player player = map.Player;
            if (player != null && !gameObject.Equals(player) && (gameObject is not MapEntity || !((MapEntity)(gameObject)).IsUncollidable))
            {
                {
                    if (HasCollidedWithMapEntity(gameObject, player, direction))
                    {
                        entityCollidedWith = player;
                        float adjustedPositionX = gameObject.X;
                        if (direction == Direction.RIGHT)
                        {
                            float boundsDifference = gameObject.X2 - gameObject.Bounds.X2;
                            adjustedPositionX = player.Bounds.X1 - gameObject.Width + boundsDifference;
                        }
                        else if (direction == Direction.LEFT)
                        {
                            float boundsDifference = gameObject.Bounds.X1 - gameObject.X;
                            adjustedPositionX = (player.Bounds.X2 + 1) - boundsDifference;
                        }
                        return new MapCollisionCheckResult(new Point(adjustedPositionX, gameObject.Y), entityCollidedWith);
                    }
                }
            }

            // no collision occurred
            return new MapCollisionCheckResult(null, null);
        }

        // y axis collision logic
        // determines if a collision occurred with another entity on the map, and calculates where gameobject should be placed to resolve the collision
        public static MapCollisionCheckResult GetAdjustedPositionAfterCollisionCheckY(GameObject gameObject, Map map, Direction direction)
        {
            int numberOfTilesToCheck = Math.Max(gameObject.Bounds.Width / map.Tileset.SpriteWidthScaled, 1);
            float edgeBoundY = direction == Direction.UP ? gameObject.Bounds.Y1 : gameObject.Bounds.Y2;
            Point tileIndex = map.GetTileIndexByPosition(gameObject.Bounds.X1, edgeBoundY);
            GameObject entityCollidedWith = null;
            for (int i = -1; i <= numberOfTilesToCheck + 1; i++)
            {
                MapTile mapTile = map.GetMapTile(tileIndex.X.Round() + i, tileIndex.Y.Round());
                if (mapTile != null && HasCollidedWithMapEntity(gameObject, mapTile, direction))
                {
                    entityCollidedWith = mapTile;
                    float adjustedPositionY = gameObject.Y;
                    if (direction == Direction.DOWN)
                    {
                        float boundsDifference = gameObject.Y2 - gameObject.Bounds.Y2;
                        adjustedPositionY = mapTile.Bounds.Y1 - gameObject.Height + boundsDifference;
                    }
                    else if (direction == Direction.UP)
                    {
                        float boundsDifference = gameObject.Bounds.Y1 - gameObject.Y;
                        adjustedPositionY = (mapTile.Bounds.Y2 + 1) - boundsDifference;
                    }
                    return new MapCollisionCheckResult(new Point(gameObject.X, adjustedPositionY), entityCollidedWith);
                }
            }

            // check active enhanced map tiles for potential collision
            foreach (EnhancedMapTile enhancedMapTile in map.ActiveEnhancedMapTiles)
            {
                if (!gameObject.Equals(enhancedMapTile) && !enhancedMapTile.IsUncollidable && HasCollidedWithMapEntity(gameObject, enhancedMapTile, direction))
                {
                    entityCollidedWith = enhancedMapTile;
                    float adjustedPositionY = gameObject.Y;
                    if (direction == Direction.DOWN)
                    {
                        float boundsDifference = gameObject.Y2 - gameObject.Bounds.Y2;
                        adjustedPositionY = enhancedMapTile.Bounds.Y1 - gameObject.Height + boundsDifference;
                    }
                    else if (direction == Direction.UP)
                    {
                        float boundsDifference = gameObject.Bounds.Y1 - gameObject.Y;
                        adjustedPositionY = (enhancedMapTile.Bounds.Y2 + 1) - boundsDifference;
                    }
                    return new MapCollisionCheckResult(new Point(gameObject.X, adjustedPositionY), entityCollidedWith);
                }
            }

            // check active npcs for potential collision
            foreach (NPC npc in map.ActiveNPCs)
            {
                if (!gameObject.Equals(npc) && !npc.IsUncollidable && HasCollidedWithMapEntity(gameObject, npc, direction))
                {
                    entityCollidedWith = npc;
                    float adjustedPositionY = gameObject.Y;
                    if (direction == Direction.DOWN)
                    {
                        float boundsDifference = gameObject.Y2 - gameObject.Bounds.Y2;
                        adjustedPositionY = npc.Bounds.Y1 - gameObject.Height + boundsDifference;
                    }
                    else if (direction == Direction.UP)
                    {
                        float boundsDifference = gameObject.Bounds.Y1 - gameObject.Y;
                        adjustedPositionY = (npc.Bounds.Y2 + 1) - boundsDifference;
                    }
                    return new MapCollisionCheckResult(new Point(gameObject.X, adjustedPositionY), entityCollidedWith);
                }
            }

            // check active triggers for potential collision
            if (gameObject.IsAffectedByTriggers)
            {
                foreach (Trigger trigger in map.ActiveTriggers)
                {
                    if (!gameObject.Equals(trigger) && !trigger.IsUncollidable && trigger.Exists && HasCollidedWithMapEntity(gameObject, trigger, direction))
                    {
                        entityCollidedWith = trigger;
                        float adjustedPositionY = gameObject.Y;
                        if (direction == Direction.DOWN)
                        {
                            float boundsDifference = gameObject.Y2 - gameObject.Bounds.Y2;
                            adjustedPositionY = trigger.Bounds.Y1 - gameObject.Height + boundsDifference;
                        }
                        else if (direction == Direction.UP)
                        {
                            float boundsDifference = gameObject.Bounds.Y1 - gameObject.Y;
                            adjustedPositionY = (trigger.Bounds.Y2 + 1) - boundsDifference;
                        }
                        return new MapCollisionCheckResult(new Point(gameObject.X, adjustedPositionY), entityCollidedWith);
                    }
                }
            }

            // check for collision with player
            // this is to allow non-player entities to collision check against the player
            Player player = map.Player;
            if (player != null && !gameObject.Equals(player) && (gameObject is not MapEntity || !((MapEntity)(gameObject)).IsUncollidable))
            {
                {
                    if (HasCollidedWithMapEntity(gameObject, player, direction))
                    {
                        entityCollidedWith = player;
                        float adjustedPositionY = gameObject.Y;
                        if (direction == Direction.DOWN)
                        {
                            float boundsDifference = gameObject.Y2 - gameObject.Bounds.Y2;
                            adjustedPositionY = player.Bounds.Y1 - gameObject.Height + boundsDifference;
                        }
                        else if (direction == Direction.UP)
                        {
                            float boundsDifference = gameObject.Bounds.Y1 - gameObject.Y;
                            adjustedPositionY = (player.Bounds.Y2 + 1) - boundsDifference;
                        }
                        return new MapCollisionCheckResult(new Point(gameObject.X, adjustedPositionY), entityCollidedWith);
                    }
                }
            }

            // no collision occurred
            return new MapCollisionCheckResult(null, null);
        }

        // based on tile type, perform logic to determine if a collision did occur with an intersecting tile or not
        private static bool HasCollidedWithMapEntity(GameObject entity, GameObject otherEntity, Direction direction)
        {
            // if entity that is being checked for collision against is a map tile
            // collision is determined based on tile type
            if (otherEntity is MapTile)
            {
                MapTile mapTile = (MapTile)otherEntity;
                switch (mapTile.TileType)
                {
                    case TileType.PASSABLE:
                        return false;
                    case TileType.NOT_PASSABLE:
                        return entity.Intersects(mapTile);
                    default:
                        return false;
                }
            }

            // for all other cases other than MapTile, let game object subclass (NPC, enhanced map tile, etc.) handle the intersection logic
            else
            {
                return entity.Intersects(otherEntity);
            }
        }
    }
}
