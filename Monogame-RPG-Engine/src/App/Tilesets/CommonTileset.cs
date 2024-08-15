using App.Resources;
using Engine.Builders;
using Engine.Core;
using Engine.Entity;
using Engine.Scene;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Tilesets
{
    public class CommonTileset : Tileset
    {
        public CommonTileset(ContentLoader contentLoader)
            : base(contentLoader.LoadTexture(GraphicsHelper.COMMON_TILESET), 16, 16, 3)
        {
        }

        public override List<MapTileBuilder> DefineTiles()
        {
            List<MapTileBuilder> mapTiles = new List<MapTileBuilder>();

            // grass
            Frame grassFrame = new FrameBuilder(GetSubImage(0, 0))
                    .WithScale(TileScale)
                    .Build();

            MapTileBuilder grassTile = new MapTileBuilder(grassFrame);

            mapTiles.Add(grassTile);

            // sign
            Frame signFrame = new FrameBuilder(GetSubImage(3, 0))
                    .WithScale(TileScale)
                    .WithBounds(1, 2, 14, 14)
                    .Build();

            MapTileBuilder signTile = new MapTileBuilder(signFrame)
                    .WithTileType(TileType.NOT_PASSABLE);

            mapTiles.Add(signTile);

            // sand
            Frame sandFrame = new FrameBuilder(GetSubImage(0, 1))
                    .WithScale(TileScale)
                    .Build();

            MapTileBuilder sandTile = new MapTileBuilder(sandFrame);

            mapTiles.Add(sandTile);

            // rock
            Frame rockFrame = new FrameBuilder(GetSubImage(3, 1))
                    .WithScale(TileScale)
                    .Build();

            MapTileBuilder rockTile = new MapTileBuilder(rockFrame)
                    .WithTileType(TileType.NOT_PASSABLE);

            mapTiles.Add(rockTile);

            // tree trunk With full hole
            Frame treeTrunkWithFullHoleFrame = new FrameBuilder(GetSubImage(2, 2))
                    .WithScale(TileScale)
                    .Build();

            MapTileBuilder treeTrunkWithFullHoleTile = new MapTileBuilder(grassFrame)
                    .WithTopLayer(treeTrunkWithFullHoleFrame)
                    .WithTileType(TileType.PASSABLE);

            mapTiles.Add(treeTrunkWithFullHoleTile);

            // left end branch
            Frame leftEndBranchFrame = new FrameBuilder(GetSubImage(2, 4))
                    .WithScale(TileScale)
                    .WithBounds(0, 6, 16, 4)
                    .Build();

            MapTileBuilder leftEndBranchTile = new MapTileBuilder(grassFrame)
                    .WithTopLayer(leftEndBranchFrame)
                    .WithTileType(TileType.PASSABLE);

            mapTiles.Add(leftEndBranchTile);

            // right end branch
            Frame rightEndBranchFrame = new FrameBuilder(GetSubImage(2, 4))
                    .WithScale(TileScale)
                    .WithBounds(0, 6, 16, 4)
                    .WithSpriteEffect(SpriteEffects.FlipHorizontally)
                    .Build();

            MapTileBuilder rightEndBranchTile = new MapTileBuilder(grassFrame)
                    .WithTopLayer(rightEndBranchFrame)
                    .WithTileType(TileType.PASSABLE);

            mapTiles.Add(rightEndBranchTile);

            // tree trunk
            Frame treeTrunkFrame = new FrameBuilder(GetSubImage(1, 0))
                    .WithScale(TileScale)
                    .Build();

            MapTileBuilder treeTrunkTile = new MapTileBuilder(grassFrame)
                    .WithTopLayer(treeTrunkFrame)
                    .WithTileType(TileType.PASSABLE);

            mapTiles.Add(treeTrunkTile);

            // tree top leaves
            Frame treeTopLeavesFrame = new FrameBuilder(GetSubImage(1, 1))
                    .WithScale(TileScale)
                    .Build();

            MapTileBuilder treeTopLeavesTile = new MapTileBuilder(grassFrame)
                    .WithTopLayer(treeTopLeavesFrame)
                    .WithTileType(TileType.PASSABLE);

            mapTiles.Add(treeTopLeavesTile);

            // yellow flower
            Frame[] yellowFlowerFrames = new Frame[] {
                new FrameBuilder(GetSubImage(1, 2), 33)
                        .WithScale(TileScale)
                        .Build(),
                new FrameBuilder(GetSubImage(1, 3), 33)
                        .WithScale(TileScale)
                        .Build(),
                new FrameBuilder(GetSubImage(1, 2), 33)
                        .WithScale(TileScale)
                        .Build(),
                new FrameBuilder(GetSubImage(1, 4), 33)
                        .WithScale(TileScale)
                        .Build()
            };

            MapTileBuilder yellowFlowerTile = new MapTileBuilder(yellowFlowerFrames);

            mapTiles.Add(yellowFlowerTile);

            // purple flower
            Frame[] purpleFlowerFrames = new Frame[] {
                new FrameBuilder(GetSubImage(0, 2), 33)
                        .WithScale(TileScale)
                        .Build(),
                new FrameBuilder(GetSubImage(0, 3), 33)
                        .WithScale(TileScale)
                        .Build(),
                new FrameBuilder(GetSubImage(0, 2), 33)
                        .WithScale(TileScale)
                        .Build(),
                new FrameBuilder(GetSubImage(0, 4), 33)
                        .WithScale(TileScale)
                        .Build()
            };

            MapTileBuilder purpleFlowerTile = new MapTileBuilder(purpleFlowerFrames);

            mapTiles.Add(purpleFlowerTile);

            // middle branch
            Frame middleBranchFrame = new FrameBuilder(GetSubImage(2, 3))
                    .WithScale(TileScale)
                    .WithBounds(0, 6, 16, 4)
                    .Build();

            MapTileBuilder middleBranchTile = new MapTileBuilder(grassFrame)
                    .WithTopLayer(middleBranchFrame)
                    .WithTileType(TileType.PASSABLE);

            mapTiles.Add(middleBranchTile);

            // tree trunk bottom
            Frame treeTrunkBottomFrame = new FrameBuilder(GetSubImage(2, 0))
                    .WithScale(TileScale)
                    .Build();

            MapTileBuilder treeTrunkBottomTile = new MapTileBuilder(treeTrunkBottomFrame)
                    .WithTileType(TileType.NOT_PASSABLE);

            mapTiles.Add(treeTrunkBottomTile);

            // mushrooms
            Frame mushroomFrame = new FrameBuilder(GetSubImage(2, 1))
                    .WithScale(TileScale)
                    .Build();

            MapTileBuilder mushroomTile = new MapTileBuilder(mushroomFrame)
                    .WithTileType(TileType.PASSABLE);

            mapTiles.Add(mushroomTile);


            // grey rock
            Frame greyRockFrame = new FrameBuilder(GetSubImage(3, 2))
                    .WithScale(TileScale)
                    .Build();

            MapTileBuilder greyRockTile = new MapTileBuilder(greyRockFrame)
                    .WithTileType(TileType.PASSABLE);

            mapTiles.Add(greyRockTile);

            // bush
            Frame bushFrame = new FrameBuilder(GetSubImage(3, 3))
                    .WithScale(TileScale)
                    .Build();

            MapTileBuilder bushTile = new MapTileBuilder(bushFrame)
                    .WithTileType(TileType.NOT_PASSABLE);

            mapTiles.Add(bushTile);

            // house body
            Frame houseBodyFrame = new FrameBuilder(GetSubImage(3, 4))
                    .WithScale(TileScale)
                    .Build();

            MapTileBuilder houseBodyTile = new MapTileBuilder(houseBodyFrame)
                    .WithTileType(TileType.NOT_PASSABLE);

            mapTiles.Add(houseBodyTile);

            // house roof body
            Frame houseRoofBodyFrame = new FrameBuilder(GetSubImage(4, 0))
                    .WithScale(TileScale)
                    .Build();

            MapTileBuilder houseRoofBodyTile = new MapTileBuilder(grassFrame)
                    .WithTopLayer(houseRoofBodyFrame)
                    .WithTileType(TileType.PASSABLE);

            mapTiles.Add(houseRoofBodyTile);

            // left house roof
            Frame leftHouseRoofFrame = new FrameBuilder(GetSubImage(4, 1))
                    .WithScale(TileScale)
                    .Build();

            MapTileBuilder leftHouseRoofTile = new MapTileBuilder(grassFrame)
                    .WithTopLayer(leftHouseRoofFrame)
                    .WithTileType(TileType.PASSABLE);

            mapTiles.Add(leftHouseRoofTile);

            // right house roof
            Frame rightHouseRoofFrame = new FrameBuilder(GetSubImage(4, 1))
                    .WithScale(TileScale)
                    .WithSpriteEffect(SpriteEffects.FlipHorizontally)
                    .Build();

            MapTileBuilder rightHouseRoofTile = new MapTileBuilder(grassFrame)
                    .WithTopLayer(rightHouseRoofFrame)
                    .WithTileType(TileType.PASSABLE);

            mapTiles.Add(rightHouseRoofTile);

            // left window
            Frame leftWindowFrame = new FrameBuilder(GetSubImage(4, 2))
                    .WithScale(TileScale)
                    .Build();

            MapTileBuilder leftWindowTile = new MapTileBuilder(leftWindowFrame)
                    .WithTileType(TileType.NOT_PASSABLE);

            mapTiles.Add(leftWindowTile);

            // right window
            Frame rightWindowFrame = new FrameBuilder(GetSubImage(4, 2))
                    .WithScale(TileScale)
                    .WithSpriteEffect(SpriteEffects.FlipHorizontally)
                    .Build();

            MapTileBuilder rightWindowTile = new MapTileBuilder(rightWindowFrame)
                    .WithTileType(TileType.NOT_PASSABLE);

            mapTiles.Add(rightWindowTile);

            // door
            Frame doorFrame = new FrameBuilder(GetSubImage(4, 3))
                    .WithScale(TileScale)
                    .Build();

            MapTileBuilder doorTile = new MapTileBuilder(doorFrame)
                    .WithTileType(TileType.NOT_PASSABLE);

            mapTiles.Add(doorTile);

            // top water
            Frame[] topWaterFrames = new Frame[] {
                new FrameBuilder(GetSubImage(5, 0), 33)
                        .WithScale(TileScale)
                        .Build(),
                new FrameBuilder(GetSubImage(5, 1), 33)
                        .WithScale(TileScale)
                        .Build(),
                new FrameBuilder(GetSubImage(5, 2), 33)
                        .WithScale(TileScale)
                        .Build(),
                new FrameBuilder(GetSubImage(5, 1), 33)
                        .WithScale(TileScale)
                        .Build(),
                new FrameBuilder(GetSubImage(5, 0), 33)
                        .WithScale(TileScale)
                        .Build(),
                new FrameBuilder(GetSubImage(5, 3), 33)
                        .WithScale(TileScale)
                        .Build(),
                new FrameBuilder(GetSubImage(5, 4), 33)
                        .WithScale(TileScale)
                        .Build(),
                new FrameBuilder(GetSubImage(5, 3), 33)
                        .WithScale(TileScale)
                        .Build()
            };

            MapTileBuilder topWaterTile = new MapTileBuilder(topWaterFrames)
                    .WithTileType(TileType.NOT_PASSABLE);

            mapTiles.Add(topWaterTile);


            return mapTiles;
        }
    }
}
