
using Engine.Core;
using Engine.Entity;
using Engine.Extensions;
using Engine.Utils;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

/*
    This class is for defining a map that is used for a specific level
    The map class handles/manages a lot of different things, including:
    1. tile map -- the map tiles that make up the map
    2. entities in the map -- this includes enemies, enhanced map tiles, and npcs
    3. the map's camera, which does a lot of work itself in the Camera class
    4. adjusting camera location based off of player location
    5. calculating which tile a game object is currently on based on its x and y location
*/
namespace Engine.Scene
{
    public abstract class Map
    {
        // the tile map (map tiles that make up the entire map image)
        public MapTile[] MapTiles { get; private set; }

        // width and height of the map in terms of the number of tiles width-wise and height-wise
        public int Width { get; private set; }
        public int Height { get; private set; }

        // the tileset this map uses for its map tiles
        public Tileset Tileset { get; private set; }

        // camera class that handles the viewable part of the map that is seen by the player of a game during a level
        public Camera Camera { get; private set; }

        // tile player should start on when this map is first loaded
        public Point PlayerStartTile { get; set; }

        // location player should start on when this map is first loaded
        public Point PlayerStartPosition { get; set; }

        // the location of the "mid point" of the screen
        // this is what tells the game that the player has reached the center of the screen, therefore the camera should move instead of the player
        // this goes into creating that "map scrolling" effect
        protected int xMidPoint;
        protected int yMidPoint;

        // in pixels, this basically creates a rectangle defining how big the map is
        // startX and Y will always be 0, endX and Y is the number of tiles multiplied by the number of pixels each tile takes up
        protected int startBoundX;
        protected int startBoundY;
        public int EndBoundX { get; private set; }
        public int EndBoundY { get; private set; }

        // the name of the map text file that has the tile map information
        public string MapFileName { get; private set; }

        // lists to hold map entities that are a part of the map
        public List<EnhancedMapTile> EnhancedMapTiles { get; private set; }
        public List<NPC> NPCs { get; private set; }
        public List<Trigger> Triggers { get; private set; }

        // returns all active enhanced map tiles (enhanced map tiles that are a part of the current update cycle) -- this changes every frame by the Camera class
        public List<EnhancedMapTile> ActiveEnhancedMapTiles
        {
            get
            {
                return Camera.ActiveEnhancedMapTiles;
            }
        }

        // returns all active npcs (npcs that are a part of the current update cycle) -- this changes every frame by the Camera class
        public List<NPC> ActiveNPCs
        {
            get
            {
                return Camera.ActiveNPCs;
            }
        }

        // returns all active triggers (triggers that are a part of the current update cycle) -- this changes every frame by the Camera class
        public List<Trigger> ActiveTriggers
        {
            get
            {
                return Camera.ActiveTriggers;
            }
        }

        // current script that is being executed (if any)
        private Script activeScript;
        public Script ActiveScript
        {
            get
            {
                return activeScript;
            }
            set
            {
                activeScript = value;
                // script is set to null if there is no active script running, or if a script has completed running
                // if script is null, do not set it to be active
                // otherwise, set script to active, which will begin its execution
                if (value != null)
                {

                    // if the script was not previously preloaded (preloadScripts method) beforehand, this will load the script dynamically
                    // preloading is recommended, but both are supported
                    if (value.ScriptActions == null)
                    {
                        activeScript.SetMap(this);
                        activeScript.SetPlayer(Player);
                        activeScript.Initialize();
                    }
                    activeScript.IsActive = true;
                }
            }
        }

        // if set to false, camera will not move as player moves
        public bool AdjustCamera { get; set; } = true;

        // map tiles in map that are animated
        public List<MapTile> AnimatedMapTiles { get; private set; }

        // flag manager instance to keep track of flags set while map is loaded
        public FlagManager FlagManager { get; set; }

        // map's textbox instance
        public Textbox Textbox { get; private set; }

        // reference to current player
        public Player Player { get; set; }

        public int WidthPixels
        {
            get
            {
                return Width * Tileset.SpriteWidthScaled;
            }
        }

        public int HeightPixels
        {
            get
            {
                return Height * Tileset.SpriteHeightScaled;
            }
        }

        // instance of content loader to allow maps to load their own content
        public ContentLoader ContentLoader { get; private set; }

        public Map(string mapFileName, Tileset tileset, ContentLoader contentLoader)
        {
            MapFileName = mapFileName;
            Tileset = tileset;
            ContentLoader = contentLoader;
            SetupMap();
            this.startBoundX = 0;
            this.startBoundY = 0;
            EndBoundX = Width * tileset.SpriteWidthScaled;
            EndBoundY = Height * tileset.SpriteHeightScaled;
            this.xMidPoint = ScreenManager.ScreenWidth / 2;
            this.yMidPoint = ScreenManager.ScreenHeight / 2;
            PlayerStartTile = new Point(0, 0);
        }

        // sets up map by reading in the map file to create the tile map
        // loads in enemies, enhanced map tiles, and npcs
        // and instantiates a Camera
        public void SetupMap()
        {
            AnimatedMapTiles = new List<MapTile>();

            LoadMapFile();


            EnhancedMapTiles = LoadEnhancedMapTiles();
            foreach (EnhancedMapTile enhancedMapTile in EnhancedMapTiles)
            {
                enhancedMapTile.SetMap(this);
            }

            NPCs = LoadNPCs();
            foreach (NPC npc in NPCs)
            {
                npc.SetMap(this);
            }

            Triggers = LoadTriggers();
            foreach (Trigger trigger in Triggers)
            {
                trigger.SetMap(this);
            }

            this.LoadScripts();

            Camera = new Camera(0, 0, Tileset.SpriteWidthScaled, Tileset.SpriteHeightScaled, this);
            Textbox = new Textbox(this);
            Textbox.LoadContent();
        }

        // reads in a map file to create the map's tilemap
        private void LoadMapFile()
        {
            StreamReader fileInput;
            try
            {
                // open map file that is located in the MAP_FILES_PATH directory
                fileInput = new StreamReader(Config.MAP_FILES_PATH + MapFileName);
            }
            catch (FileNotFoundException)
            {
                // if map file does not exist, create a new one for this map (the map editor uses this)
                Debug.WriteLine("Map file " + Config.MAP_FILES_PATH + MapFileName + " not found! Creating empty map file...");

                try
                {
                    CreateEmptyMapFile();
                    fileInput = new StreamReader(Config.MAP_FILES_PATH + MapFileName);
                }
                catch (IOException ex2)
                {
                    Debug.WriteLine(ex2.Message);
                    Debug.WriteLine("Failed to create an empty map file!");
                    throw new IOException();
                }
            }

            // read in map width and height from the first line of map file
            string[] dimensions = fileInput.ReadLine().Split(" ");
            Width = Convert.ToInt32(dimensions[0]);
            Height = Convert.ToInt32(dimensions[1]);

            // define array size for map tiles, which is width * height (this is a standard array, NOT a 2D array)
            MapTiles = new MapTile[Height * Width];

            // read in all tile indexes in map file into an array
            int[] tileIndexes = fileInput.ReadToEnd()
                .Split(new string[] { " ", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(tileIndexStr => Convert.ToInt32(tileIndexStr))
                .ToArray();

            // iterates over each tile index from the map file
            // uses the defined tileset to get the associated MapTile to that tileset, and place it in the array
            int currentTileIndex = 0;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    int tileIndex = tileIndexes[currentTileIndex];
                    int xLocation = j * Tileset.SpriteWidthScaled;
                    int yLocation = i * Tileset.SpriteHeightScaled;
                    MapTile tile = Tileset.GetTile(tileIndex).Build(xLocation, yLocation);
                    tile.SetMap(this);
                    SetMapTile(j, i, tile);

                    if (tile.IsAnimated)
                    {
                        AnimatedMapTiles.Add(tile);
                    }

                    currentTileIndex++;
                }
            }

            fileInput.Close();
        }

        // creates an empty map file for this map if one does not exist
        // defaults the map dimensions to 0x0
        private void CreateEmptyMapFile() //throws IOException
        {
            StreamWriter fileWriter = new StreamWriter(Config.MAP_FILES_PATH + MapFileName);
            fileWriter.Write("0 0\n");
            fileWriter.Close();
        }

        // get position on the map based on a specfic tile index
        public Point GetPositionByTileIndex(int xIndex, int yIndex)
        {
            MapTile tile = GetMapTile(xIndex, yIndex);
            return new Point(tile.X, tile.Y);
        }

        // get specific map tile from tile map
        public MapTile GetMapTile(int x, int y)
        {
            if (IsInBounds(x, y))
            {
                return MapTiles[GetConvertedIndex(x, y)];
            }
            else
            {
                return null;
            }
        }

        // set specific map tile from tile map to a new map tile
        public void SetMapTile(int x, int y, MapTile tile)
        {
            if (IsInBounds(x, y))
            {
                MapTile oldMapTile = GetMapTile(x, y);
                AnimatedMapTiles.Remove(oldMapTile);
                MapTiles[GetConvertedIndex(x, y)] = tile;
                tile.SetMap(this);
                if (tile.IsAnimated)
                {
                    AnimatedMapTiles.Add(tile);
                }
            }
        }

        // returns a tile based on a position in the map
        public MapTile GetTileByPosition(int xPosition, int yPosition)
        {
            Point tileIndex = GetTileIndexByPosition(xPosition, yPosition);
            if (IsInBounds(tileIndex.X.Round(), tileIndex.Y.Round()))
            {
                return GetMapTile(tileIndex.X.Round(), tileIndex.Y.Round());
            }
            else
            {
                return null;
            }
        }

        // returns the index of a tile (x index and y index) based on a position in the map
        public Point GetTileIndexByPosition(float xPosition, float yPosition)
        {
            int xIndex = xPosition.Round() / Tileset.SpriteWidthScaled;
            int yIndex = yPosition.Round() / Tileset.SpriteHeightScaled;
            return new Point(xIndex, yIndex);
        }

        // checks if map tile being requested is in bounds of the tile map array
        private bool IsInBounds(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }

        // since tile map array is a standard (1D) array and not a 2D,
        // instead of doing [y][x] to get a value, instead the same can be achieved with x + width * y
        private int GetConvertedIndex(int x, int y)
        {
            return x + Width * y;
        }

        // list of enhanced map tiles defined to be a part of the map, should be overridden in a subclass
        protected virtual List<EnhancedMapTile> LoadEnhancedMapTiles()
        {
            return new List<EnhancedMapTile>();
        }

        // list of npcs defined to be a part of the map, should be overridden in a subclass
        protected virtual List<NPC> LoadNPCs()
        {
            return new List<NPC>();
        }

        // list of enemies defined to be a part of the map, should be overridden in a subclass
        protected virtual List<Trigger> LoadTriggers()
        {
            return new List<Trigger>();
        }

        // list of scripts defined to be a part of the map, should be overridden in a subclass
        protected virtual void LoadScripts() { }

        // add an enhanced map tile to the map's list of enhanced map tiles
        public void AddEnhancedMapTile(EnhancedMapTile enhancedMapTile)
        {
            enhancedMapTile.SetMap(this);
            EnhancedMapTiles.Add(enhancedMapTile);
        }

        // add an npc to the map's list of npcs
        public void AddNPC(NPC npc)
        {
            npc.SetMap(this);
            NPCs.Add(npc);
        }

        // add an enemy to the map's list of trigger
        public void AddTrigger(Trigger trigger)
        {
            trigger.SetMap(this);
            Triggers.Add(trigger);
        }

        public void PreloadScripts()
        {
            // setup map scripts to have references to the map and player
            foreach (MapTile mapTile in MapTiles)
            {
                if (mapTile.InteractScript != null)
                {
                    mapTile.InteractScript.SetMap(this);
                    mapTile.InteractScript.SetPlayer(Player);
                    mapTile.InteractScript.Initialize();
                }
            }
            foreach (NPC npc in NPCs)
            {
                if (npc.InteractScript != null)
                {
                    npc.InteractScript.SetMap(this);
                    npc.InteractScript.SetPlayer(Player);
                    npc.InteractScript.Initialize();
                }
            }
            foreach (EnhancedMapTile enhancedMapTile in EnhancedMapTiles)
            {
                if (enhancedMapTile.InteractScript != null)
                {
                    enhancedMapTile.InteractScript.SetMap(this);
                    enhancedMapTile.InteractScript.SetPlayer(Player);
                    enhancedMapTile.InteractScript.Initialize();
                }
            }
            foreach (Trigger trigger in Triggers)
            {
                if (trigger.TriggerScript != null)
                {
                    trigger.TriggerScript.SetMap(this);
                    trigger.TriggerScript.SetPlayer(Player);
                    trigger.TriggerScript.Initialize();
                }
            }
        }

        public NPC GetNPCById(int id)
        {
            foreach (NPC npc in NPCs)
            {
                if (npc.Id == id)
                {
                    return npc;
                }
            }
            return null;
        }


        public List<MapEntity> GetSurroundingMapEntities(Player player)
        {
            List<MapEntity> surroundingMapEntities = new List<MapEntity>();

            // gets surrounding tiles
            Point playerCurrentTile = GetTileIndexByPosition((int)player.Bounds.X1, (int)player.Bounds.Y1);
            for (int i = (int)playerCurrentTile.Y - 1; i <= playerCurrentTile.Y + 1; i++)
            {
                for (int j = (int)playerCurrentTile.X - 1; j <= playerCurrentTile.X + 1; j++)
                {
                    MapTile mapTile = GetMapTile(j, i);
                    if (mapTile != null && mapTile.InteractScript != null)
                    {
                        surroundingMapEntities.Add(mapTile);
                    }
                }
            }
            // gets active surrounding npcs
            surroundingMapEntities.AddRange(ActiveNPCs);
            surroundingMapEntities.AddRange(ActiveEnhancedMapTiles);
            return surroundingMapEntities;
        }

        public void EntityInteract(Player player)
        {
            List<MapEntity> surroundingMapEntities = GetSurroundingMapEntities(player);
            List<MapEntity> playerTouchingMapEntities = new List<MapEntity>();
            foreach (MapEntity mapEntity in surroundingMapEntities)
            {
                if (mapEntity.InteractScript != null && mapEntity.Intersects(player.GetInteractionRange()))
                {
                    playerTouchingMapEntities.Add(mapEntity);
                }
            }
            MapEntity interactedEntity = null;
            if (playerTouchingMapEntities.Count == 1)
            {
                if (playerTouchingMapEntities[0].IsUncollidable || IsInteractedEntityValid(playerTouchingMapEntities[0], player))
                {
                    interactedEntity = playerTouchingMapEntities[0];
                }
            }
            else if (playerTouchingMapEntities.Count > 1)
            {
                MapEntity currentLargestAreaOverlappedEntity = null;
                float currentLargestAreaOverlapped = 0;
                foreach (MapEntity mapEntity in playerTouchingMapEntities)
                {
                    if (mapEntity.IsUncollidable || IsInteractedEntityValid(mapEntity, player))
                    {
                        float areaOverlapped = mapEntity.GetAreaOverlapped(player.GetInteractionRange());
                        if (areaOverlapped > currentLargestAreaOverlapped)
                        {
                            currentLargestAreaOverlappedEntity = mapEntity;
                            currentLargestAreaOverlapped = areaOverlapped;
                        }
                    }
                }
                interactedEntity = currentLargestAreaOverlappedEntity;
            }
            if (interactedEntity != null)
            {
                ActiveScript = interactedEntity.InteractScript;
            }
        }

        private bool IsInteractedEntityValid(MapEntity interactedEntity, Player player)
        {
            Rectangle playerBounds = player.Bounds;
            Rectangle entityBounds = interactedEntity.Bounds;

            // this does several checks to ensure the player's location releative to the entity's is valid for interaction
            // takes into account things like player's current location, entity's current location, player's facing direction, player's center point, etc.
            // this prevents things like being able to interact with an entity without facing it and other oddities like that

            // if player is facing left and entity is completely to the left of the player, location is valid
            if (player.FacingDirection == Direction.LEFT && entityBounds.X2 < playerBounds.X1)
            {
                return true;
            }
            // if player is facing right and entity is completely to the right of the player, location is valid
            else if (player.FacingDirection == Direction.RIGHT && entityBounds.X1 > playerBounds.X2)
            {
                return true;
            }

            bool isEntityOverOrUnderPlayer = entityBounds.Y2 < playerBounds.Y1 || entityBounds.Y1 > playerBounds.Y2;
            if (interactedEntity is NPC)
            {
                // if player is facing left and entity is either on top of or underneath player and player's center point is greater than entity's center point, location is valid
                if (player.FacingDirection == Direction.LEFT && isEntityOverOrUnderPlayer && playerBounds.X1 < entityBounds.X2)
                {
                    return true;
                }
                // if player is facing right and entity is either on top of or underneath player and player's center point is less than entity's center point, location is valid
                else if (player.FacingDirection == Direction.RIGHT && isEntityOverOrUnderPlayer && playerBounds.X2 > entityBounds.X1)
                {
                    return true;
                }
            }
            else
            {
                // if interacted with anything other than NPC, it doesn't matter which direction you're facing, so it's valid if above/below player
                if (isEntityOverOrUnderPlayer)
                {
                    return true;
                }
            }

            // if none of the location validity checks matched, location is not valid and the interaction will fail
            return false;
        }

        public void Update(KeyboardState keyboardState)
        {
            if (AdjustCamera)
            {
                AdjustMovementY(Player);
                AdjustMovementX(Player);
            }
            Camera.Update(Player);
            if (Textbox.IsActive)
            {
                Textbox.Update(keyboardState);
            }
        }

        // based on the player's current X position (which in a level can potentially be updated each frame),
        // adjust the player's and camera's positions accordingly in order to properly create the map "scrolling" effect
        private void AdjustMovementX(Player player)
        {
            // if player goes past center screen (on the right side) and there is more map to show on the right side, push player back to center and move camera forward
            if (player.CalibratedXLocation > xMidPoint && Camera.EndBoundX < EndBoundX)
            {
                float xMidPointDifference = xMidPoint - player.CalibratedXLocation;
                Camera.MoveX(-xMidPointDifference);

                // if camera moved past the right edge of the map as a result from the move above, move camera back and push player forward
                if (Camera.EndBoundX > EndBoundX)
                {
                    float cameraDifference = Camera.EndBoundX - EndBoundX;
                    Camera.MoveX(-cameraDifference);
                }
            }
            // if player goes past center screen (on the left side) and there is more map to show on the left side, push player back to center and move camera backwards
            else if (player.CalibratedXLocation < xMidPoint && Camera.X > startBoundX)
            {
                float xMidPointDifference = xMidPoint - player.CalibratedXLocation;
                Camera.MoveX(-xMidPointDifference);

                // if camera moved past the left edge of the map as a result from the move above, move camera back and push player backward
                if (Camera.X < startBoundX)
                {
                    float cameraDifference = startBoundX - Camera.X;
                    Camera.MoveX(cameraDifference);
                }
            }
        }

        // based on the player's current Y position (which in a level can potentially be updated each frame),
        // adjust the player's and camera's positions accordingly in order to properly create the map "scrolling" effect
        private void AdjustMovementY(Player player)
        {
            // if player goes past center screen (below) and there is more map to show below, push player back to center and move camera upward
            if (player.CalibratedYLocation > yMidPoint && Camera.EndBoundY < EndBoundY)
            {
                float yMidPointDifference = yMidPoint - player.CalibratedYLocation;
                Camera.MoveY(-yMidPointDifference);

                // if camera moved past the bottom of the map as a result from the move above, move camera upwards and push player downwards
                if (Camera.EndBoundY > EndBoundY)
                {
                    float cameraDifference = Camera.EndBoundY - EndBoundY;
                    Camera.MoveY(-cameraDifference);
                }
            }
            // if player goes past center screen (above) and there is more map to show above, push player back to center and move camera upwards
            else if (player.CalibratedYLocation < yMidPoint && Camera.Y > startBoundY)
            {
                float yMidPointDifference = yMidPoint - player.CalibratedYLocation;
                Camera.MoveY(-yMidPointDifference);

                // if camera moved past the top of the map as a result from the move above, move camera downwards and push player upwards
                if (Camera.Y < startBoundY)
                {
                    float cameraDifference = startBoundY - Camera.Y;
                    Camera.MoveY(cameraDifference);
                }
            }
        }

        public void Reset()
        {
            SetupMap();
        }

        public virtual void Draw(GraphicsHandler graphicsHandler)
        {
            Camera.Draw(graphicsHandler);
            if (Textbox.IsActive)
            {
                Textbox.Draw(graphicsHandler);
            }
        }

        public virtual void Draw(Player player, GraphicsHandler graphicsHandler)
        {
            Camera.Draw(Player, graphicsHandler);
            if (Textbox.IsActive)
            {
                Textbox.Draw(graphicsHandler);
            }
        }
    }
}
