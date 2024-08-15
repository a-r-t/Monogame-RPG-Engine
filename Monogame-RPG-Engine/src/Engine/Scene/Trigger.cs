using Engine.Core;
using Engine.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

// This class represents a trigger script that can be placed on a map
// upon the player colliding with the trigger, it will play out the attached script
namespace Engine.Scene
{
    public class Trigger : MapEntity
    {
        public Script TriggerScript { get; set; }

        public Trigger(float x, float y, int width, int height, Script triggerScript)
            : base(x, y)
        {
            TriggerScript = triggerScript;
            Width = width;
            Height = height;
            Bounds = new Entity.Rectangle(0, 0, Width, Height);
        }

        public Trigger(float x, float y, int width, int height, Script triggerScript, string existenceFlag)
            : base(x, y)
        {
            TriggerScript = triggerScript;
            Width = width;
            Height = height;
            Bounds = new Entity.Rectangle(0, 0, Width, Height);
            ExistenceFlag = existenceFlag;
        }

        protected Script LoadTriggerScript() { return null; }

        // only used for debugging purposes if seeing the trigger is necessary
        public override void Draw(GraphicsHandler graphicsHandler)
        {
            DrawBounds(graphicsHandler, Color.Red);
        }

        // only used for debugging purposes if seeing the trigger is necessary
        public void Draw(GraphicsHandler graphicsHandler, Color color)
        {
            Entity.Rectangle scaledCalibratedBounds = CalibratedBounds;
            scaledCalibratedBounds.Color = color;
            scaledCalibratedBounds.BorderColor = Color.Black;
            scaledCalibratedBounds.BorderThickness = 1;
            scaledCalibratedBounds.Draw(graphicsHandler);
        }
    }
}
