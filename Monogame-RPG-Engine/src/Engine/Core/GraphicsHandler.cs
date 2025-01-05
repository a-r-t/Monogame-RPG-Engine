using Monogame_RPG_Engine.Engine.Extensions;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Monogame_RPG_Engine.Engine.Core
{
    // TODO: Look at this answer: https://stackoverflow.com/a/13905075
    public class GraphicsHandler
    {
        public GraphicsDevice GraphicsDevice { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }
        private Rectangle currentViewportScissorRectangle;
        private RenderTarget2D currentRenderTarget;
        private Stack<RenderTargetBinding> previousRenderTargets;

        public GraphicsHandler(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            GraphicsDevice = graphicsDevice;
            SpriteBatch = spriteBatch;
            currentViewportScissorRectangle = SpriteBatch.GraphicsDevice.ScissorRectangle;
            previousRenderTargets = new Stack<RenderTargetBinding>();
        }

        public void DrawRectangle(int x, int y, int width, int height, Color color, int borderThickness = 1)
        {
            DrawRectangle(new Rectangle(x, y, width, height), color, borderThickness);
        }

        public void DrawRectangle(Rectangle rectangle, Color color, int borderThickness = 1)
        {
            Texture2D rectangleTexture = new Texture2D(GraphicsDevice, 1, 1);
            rectangleTexture.SetData(new[] { Color.White });

            // vertical left
            SpriteBatch.Draw(rectangleTexture, new Rectangle(rectangle.X, rectangle.Y, borderThickness, rectangle.Height), color);

            // horizontal top
            SpriteBatch.Draw(rectangleTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + borderThickness, borderThickness), color);

            // vertical right
            SpriteBatch.Draw(rectangleTexture, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, borderThickness, rectangle.Height - borderThickness), color);

            // horizontal bottom
            SpriteBatch.Draw(rectangleTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - borderThickness, rectangle.Width + borderThickness, borderThickness), color);
        }

        public void DrawFilledRectangle(Rectangle rectangle, Color color)
        {
            Texture2D rectangleTexture = new Texture2D(GraphicsDevice, 1, 1);
            rectangleTexture.SetData(new[] { Color.White });

            SpriteBatch.Draw(rectangleTexture, rectangle, color);
        }

        public void DrawFilledRectangle(int x, int y, int width, int height, Color color)
        {
            Texture2D rectangleTexture = new Texture2D(GraphicsDevice, 1, 1);
            rectangleTexture.SetData(new[] { Color.White });

            SpriteBatch.Draw(rectangleTexture, new Rectangle(x, y, width, height), color);
        }

        public void DrawFilledRectangleWithBorder(Rectangle rectangle, Color color, Color? borderColor = null, int borderThickness = 1)
        {
            if (borderColor == null)
            {
                borderColor = color;
            }

            Texture2D rectangleTexture = new Texture2D(GraphicsDevice, 1, 1);
            rectangleTexture.SetData(new[] { Color.White });

            SpriteBatch.Draw(rectangleTexture, rectangle, color);

            // vertical left
            SpriteBatch.Draw(rectangleTexture, new Rectangle(rectangle.X, rectangle.Y, borderThickness, rectangle.Height), (Color)borderColor);

            // horizontal top
            SpriteBatch.Draw(rectangleTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + borderThickness, borderThickness), (Color)borderColor);

            // vertical right
            SpriteBatch.Draw(rectangleTexture, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, borderThickness, rectangle.Height - borderThickness), (Color)borderColor);

            // horizontal bottom
            SpriteBatch.Draw(rectangleTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - borderThickness, rectangle.Width + borderThickness, borderThickness), (Color)borderColor);
        }

        public void DrawLine(Vector2 begin, Vector2 end, Color color, int width = 1)
        {
            Texture2D rectangleTexture = new Texture2D(GraphicsDevice, 1, 1);
            rectangleTexture.SetData(new[] { Color.White });
            Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length(), width);
            Vector2 v = Vector2.Normalize(begin - end);
            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if (begin.Y > end.Y) angle = MathHelper.TwoPi - angle;
            SpriteBatch.Draw(rectangleTexture, r, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }

        public void DrawImage(Texture2D texture, Vector2 position, Rectangle? sourceRectangle = null, Color? color = null, float rotation = 0.0f, Vector2? origin = null, Vector2? scale = null, SpriteEffects spriteEffects = SpriteEffects.None, float layerDepth = 0f)
        {
            if (sourceRectangle == null)
            {
                sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            }
            if (color == null)
            {
                color = Color.White;
            }
            if (origin == null)
            {
                origin = Vector2.Zero;
            }
            if (scale == null)
            {
                scale = Vector2.One;
            }
            position = new Vector2(position.X.Round(), position.Y.Round());

            SpriteBatch.Draw(texture, position, sourceRectangle, (Color)color, rotation, (Vector2)origin, (Vector2)scale, spriteEffects, layerDepth);
        }

        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color? color = null, float rotation = 0.0f, Vector2? origin = null, Vector2? scale = null, SpriteEffects spriteEffects = SpriteEffects.None, float layerDepth = 0f)
        {
            if (color == null)
            {
                color = Color.White;
            }
            if (origin == null)
            {
                origin = Vector2.Zero;
            }
            if (scale == null)
            {
                scale = Vector2.One;
            }

            SpriteBatch.DrawString(spriteFont, text, position, (Color)color, rotation, (Vector2)origin, (Vector2)scale, spriteEffects, layerDepth);
        }

        public void DrawString(BitmapFont bitmapFont, string text, Vector2 position, Color? color = null, float rotation = 0.0f, Vector2? origin = null, Vector2? scale = null, SpriteEffects spriteEffects = SpriteEffects.None, float layerDepth = 0f)
        {
            if (color == null)
            {
                color = Color.White;
            }
            if (origin == null)
            {
                origin = Vector2.Zero;
            }
            if (scale == null)
            {
                scale = Vector2.One;
            }

            SpriteBatch.DrawString(bitmapFont, text, position, (Color)color, rotation, (Vector2)origin, (Vector2)scale, spriteEffects, layerDepth);
        }

        public void DrawString(DynamicSpriteFont dynamicSpriteFont, string text, Vector2 position, Color? color = null, float rotation = 0.0f, Vector2? origin = null, Vector2? scale = null, SpriteEffects spriteEffects = SpriteEffects.None, float layerDepth = 0f)
        {
            if (color == null)
            {
                color = Color.White;
            }
            if (origin == null)
            {
                origin = Vector2.Zero;
            }
            if (scale == null)
            {
                scale = Vector2.One;
            }
            SpriteBatch.DrawString(dynamicSpriteFont, text, position, (Color)color, rotation: rotation, origin: (Vector2)origin, scale: (Vector2)scale);
        }

        public void DrawStringWithOutline(SpriteFont spriteFont, string text, Vector2 position, Color? color = null, Color? outlineColor = null, int outlineThickness = 1, float rotation = 0.0f, Vector2? origin = null, Vector2? scale = null, SpriteEffects spriteEffects = SpriteEffects.None, float layerDepth = 0f)
        {
            if (color == null)
            {
                color = Color.White;
            }
            if (origin == null)
            {
                origin = Vector2.Zero;
            }
            if (scale == null)
            {
                scale = Vector2.One;
            }
            if (outlineColor == null)
            {
                outlineColor = color;
            }

            for (int x = -outlineThickness; x <= outlineThickness; x++)
            {
                for (int y = -outlineThickness; y <= outlineThickness; y++)
                {
                    SpriteBatch.DrawString(spriteFont, text, position + new Vector2(x, y), outlineColor.Value, rotation, origin.Value, scale.Value, spriteEffects, layerDepth);
                }
            }

            SpriteBatch.DrawString(spriteFont, text, position, (Color)color, rotation, (Vector2)origin, (Vector2)scale, spriteEffects, layerDepth);
        }

        public void DrawStringWithOutline(DynamicSpriteFont dynamicSpriteFont, string text, Vector2 position, Color? color = null, Color? outlineColor = null, int outlineThickness = 1, float rotation = 0.0f, Vector2? origin = null, Vector2? scale = null, SpriteEffects spriteEffects = SpriteEffects.None, float layerDepth = 0f)
        {
            if (color == null)
            {
                color = Color.White;
            }
            if (origin == null)
            {
                origin = Vector2.Zero;
            }
            if (scale == null)
            {
                scale = Vector2.One;
            }
            if (outlineColor == null)
            {
                outlineColor = color;
            }

            for (int x = -outlineThickness; x <= outlineThickness; x++)
            {
                for (int y = -outlineThickness; y <= outlineThickness; y++)
                {
                    SpriteBatch.DrawString(dynamicSpriteFont, text, position + new Vector2(x, y), (Color)outlineColor, rotation: rotation, origin: (Vector2)origin, scale: (Vector2)scale);
                }
            }

            SpriteBatch.DrawString(dynamicSpriteFont, text, position, (Color)color, rotation: rotation, origin: (Vector2)origin, scale: (Vector2)scale);
        }

        public void SetScissorRectangle(Rectangle scissorRectangle)
        {
            SpriteBatch.End();
            SpriteBatch.GraphicsDevice.ScissorRectangle = scissorRectangle;
            RasterizerState rasterizerState = new RasterizerState() { ScissorTestEnable = true, CullMode = CullMode.CullCounterClockwiseFace };
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, rasterizerState);
        }

        public void RemoveScissorRectangle()
        {
            SpriteBatch.End();
            SpriteBatch.GraphicsDevice.ScissorRectangle = currentViewportScissorRectangle;
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
        }

        public void SetRenderTarget(int width, int height)
        {
            SpriteBatch.End();
            RenderTarget2D renderTarget = new RenderTarget2D(GraphicsDevice, width, height);
            SetRenderTarget(renderTarget);
        }

        // creates a render target that all future content will be drawn to until DrawRenderTarget method is eventually called
        // when DrawRenderTarget method is called, the entire render target will be drawn to the previous render target that was in use prior to this method being called
        public void SetRenderTarget(RenderTarget2D renderTarget)
        {
            SpriteBatch.End();

            // set currentRenderTarget reference to the new render target
            currentRenderTarget = renderTarget;

            // save existing render target if exists
            if (GraphicsDevice.GetRenderTargets().Length > 0 && GraphicsDevice.GetRenderTargets()[0].RenderTarget != null)
            {
                previousRenderTargets.Push(GraphicsDevice.GetRenderTargets()[0]);

            }

            // set new render target
            SpriteBatch.GraphicsDevice.SetRenderTarget(renderTarget);
            SpriteBatch.GraphicsDevice.Clear(Color.CornflowerBlue);

            // start new spritebatch with new render target
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
        }

        public void DrawRenderTarget(int x, int y, Color? color = null)
        {
            if (color == null)
            {
                color = Color.White;
            }
            SpriteBatch.End();

            // if there is a previous render target that was in use prior to SetRenderTarget being called, restore it
            // this will ensure the new render target is drawn on top of whatever was there prior
            if (previousRenderTargets.Count > 0 && previousRenderTargets.Peek().RenderTarget != null)
            {
                SpriteBatch.GraphicsDevice.SetRenderTarget(previousRenderTargets.Peek().RenderTarget as RenderTarget2D);
            }
            else
            {
                // If no previous render target, reset to the default back buffer
                SpriteBatch.GraphicsDevice.SetRenderTarget(null);
            }

            // draw render target on top of previous render target
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            SpriteBatch.Draw(currentRenderTarget, new Vector2(x, y), color.Value);

            // reset currentRenderTarget reference to the previous render target (if exists)
            // this will ensure that if this was a situation where a render target was being drawn on another render target, that previous render target can now "continue" on like normal
            if (previousRenderTargets.Count > 0 && previousRenderTargets.Peek().RenderTarget != null)
            {
                currentRenderTarget = previousRenderTargets.Pop().RenderTarget as RenderTarget2D;
            }
            else
            {
                currentRenderTarget = null;
            }
        }
    }


}

