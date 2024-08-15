using Engine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Scene
{
    public class LayeredMapTile
    {
        private MapTile BottomLayer { get; set; }
        private MapTile TopLayer { get; set; }

        public LayeredMapTile(MapTile bottomLayer)
        {
            BottomLayer = bottomLayer;
        }

        public LayeredMapTile(MapTile bottomLayer, MapTile topLayer)
        {
            BottomLayer = bottomLayer;
            TopLayer = topLayer;
        }

        public void Update()
        {
            BottomLayer.Update();
            if (TopLayer != null)
            {
                TopLayer.Update();
            }
        }

        public void Draw(GraphicsHandler graphicsHandler)
        {
            BottomLayer.Draw(graphicsHandler);
            if (TopLayer != null)
            {
                TopLayer.Draw(graphicsHandler);
            }
        }
    }
}
