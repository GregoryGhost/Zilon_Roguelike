﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CDT.LAST.MonoGameClient.Engine
{
    internal class IconButton
    {
        private readonly Texture2D _icon;
        private readonly Rectangle _rect;
        private UiButtonState _buttonState;

        public IconButton(Texture2D texture, Texture2D icon, Rectangle rect)
        {
            Texture = texture;
            _icon = icon;
            _rect = rect;
            _buttonState = UiButtonState.OutOfButton;
        }

        public Texture2D Texture { get; }

        public void Draw(SpriteBatch spriteBatch)
        {
            var color = Color.White;
            if (_buttonState == UiButtonState.OutOfButton)
            {
            }
            else if (_buttonState == UiButtonState.Hover)
            {
                color = Color.Lerp(color, Color.Wheat, 0.25f);
            }
            else if (_buttonState == UiButtonState.Pressed)
            {
                color = Color.Lerp(color, Color.Wheat, 0.75f);
            }
            else
            {
                color = Color.Red;
            }

            spriteBatch.Draw(Texture, _rect, color);
            spriteBatch.Draw(_icon, _rect, color);
        }

        public void Update()
        {
            var mouseState = Mouse.GetState();
            if (CheckMouseOver())
            {
                if (_buttonState == UiButtonState.Hover && mouseState.LeftButton == ButtonState.Pressed)
                {
                    _buttonState = UiButtonState.Pressed;
                }
                else if (mouseState.LeftButton == ButtonState.Released && _buttonState == UiButtonState.Pressed)
                {
                    _buttonState = UiButtonState.Hover;
                    OnClick?.Invoke(this, EventArgs.Empty);
                }
                else if (mouseState.LeftButton == ButtonState.Released)
                {
                    _buttonState = UiButtonState.Hover;
                }
            }
            else
            {
                _buttonState = UiButtonState.OutOfButton;
            }
        }

        private bool CheckMouseOver()
        {
            var mouseState = Mouse.GetState();
            var mousePosition = mouseState.Position;

            var mouseRect = new Rectangle(mousePosition.X, mousePosition.Y, 1, 1);

            return _rect.Intersects(mouseRect);
        }

        public event EventHandler? OnClick;
    }

    internal class EquipmentButton
    {
        private readonly Texture2D[] _iconLayers;
        private readonly Rectangle _rect;
        private readonly Rectangle _sourceRect;
        private UiButtonState _buttonState;

        public EquipmentButton(Texture2D texture, Texture2D[] iconLayers, Rectangle rect, Rectangle sourceRect)
        {
            Texture = texture;
            _iconLayers = iconLayers;
            _rect = rect;
            _sourceRect = sourceRect;
            _buttonState = UiButtonState.OutOfButton;
        }

        public Texture2D Texture { get; }

        public void Draw(SpriteBatch spriteBatch)
        {
            var color = Color.White;
            if (_buttonState == UiButtonState.OutOfButton)
            {
            }
            else if (_buttonState == UiButtonState.Hover)
            {
                color = Color.Lerp(color, Color.Wheat, 0.25f);
            }
            else if (_buttonState == UiButtonState.Pressed)
            {
                color = Color.Lerp(color, Color.Wheat, 0.75f);
            }
            else
            {
                color = Color.Red;
            }

            spriteBatch.Draw(Texture, _rect, color);

            foreach (var iconLayer in _iconLayers)
            {
                spriteBatch.Draw(iconLayer, _rect, _sourceRect, color);
            }
        }

        public void Update()
        {
            var mouseState = Mouse.GetState();
            if (CheckMouseOver())
            {
                if (_buttonState == UiButtonState.Hover && mouseState.LeftButton == ButtonState.Pressed)
                {
                    _buttonState = UiButtonState.Pressed;
                }
                else if (mouseState.LeftButton == ButtonState.Released && _buttonState == UiButtonState.Pressed)
                {
                    _buttonState = UiButtonState.Hover;
                    OnClick?.Invoke(this, EventArgs.Empty);
                }
                else if (mouseState.LeftButton == ButtonState.Released)
                {
                    _buttonState = UiButtonState.Hover;
                }
            }
            else
            {
                _buttonState = UiButtonState.OutOfButton;
            }
        }

        private bool CheckMouseOver()
        {
            var mouseState = Mouse.GetState();
            var mousePosition = mouseState.Position;

            var mouseRect = new Rectangle(mousePosition.X, mousePosition.Y, 1, 1);

            return _rect.Intersects(mouseRect);
        }

        public event EventHandler? OnClick;
    }

    internal class Button
    {
        private readonly SpriteFont _font;
        private readonly Rectangle _rect;
        private UiButtonState _buttonState;

        public Button(string title, Texture2D texture, SpriteFont font, Rectangle rect)
        {
            Title = title;
            Texture = texture;
            _font = font;
            _rect = rect;
            _buttonState = UiButtonState.OutOfButton;
        }

        public Texture2D Texture { get; }

        public string Title { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            var color = Color.White;
            if (_buttonState == UiButtonState.OutOfButton)
            {
            }
            else if (_buttonState == UiButtonState.Hover)
            {
                color = Color.Lerp(color, Color.Wheat, 0.25f);
            }
            else if (_buttonState == UiButtonState.Pressed)
            {
                color = Color.Lerp(color, Color.Wheat, 0.75f);
            }
            else
            {
                color = Color.Red;
            }

            spriteBatch.Draw(Texture, _rect, color);
            spriteBatch.DrawString(_font, Title, new Vector2(_rect.Left, _rect.Top), Color.Gray);
        }

        public void Update()
        {
            var mouseState = Mouse.GetState();
            if (CheckMouseOver())
            {
                if (_buttonState == UiButtonState.Hover && mouseState.LeftButton == ButtonState.Pressed)
                {
                    _buttonState = UiButtonState.Pressed;
                }
                else if (mouseState.LeftButton == ButtonState.Released && _buttonState == UiButtonState.Pressed)
                {
                    _buttonState = UiButtonState.Hover;
                    OnClick?.Invoke(this, EventArgs.Empty);
                }
                else if (mouseState.LeftButton == ButtonState.Released)
                {
                    _buttonState = UiButtonState.Hover;
                }
            }
            else
            {
                _buttonState = UiButtonState.OutOfButton;
            }
        }

        private bool CheckMouseOver()
        {
            var mouseState = Mouse.GetState();
            var mousePosition = mouseState.Position;

            var mouseRect = new Rectangle(mousePosition.X, mousePosition.Y, 1, 1);

            return _rect.Intersects(mouseRect);
        }

        public event EventHandler? OnClick;
    }
}