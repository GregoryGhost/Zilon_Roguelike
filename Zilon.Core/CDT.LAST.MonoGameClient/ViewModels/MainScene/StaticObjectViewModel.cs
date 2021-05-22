﻿using System;

using CDT.LAST.MonoGameClient.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Zilon.Core.Client;
using Zilon.Core.Common;
using Zilon.Core.Graphs;
using Zilon.Core.Tactics;
using Zilon.Core.Tactics.Spatial;

namespace CDT.LAST.MonoGameClient.ViewModels.MainScene
{
    internal class StaticObjectViewModel : GameObjectBase, IContainerViewModel
    {
        private readonly Game _game;
        private readonly Texture2D _personHeadSprite;
        private readonly SpriteContainer _rootSprite;
        private readonly SpriteBatch _spriteBatch;

        public StaticObjectViewModel(Game game, IStaticObject staticObject, SpriteBatch spriteBatch)
        {
            _game = game;
            StaticObject = staticObject;
            _spriteBatch = spriteBatch;

            _personHeadSprite = _game.Content.Load<Texture2D>("Sprites/game-objects/environment/Grass");

            var worldCoords = HexHelper.ConvertToWorld(((HexNode)StaticObject.Node).OffsetCoords);

            var hexSize = MapMetrics.UnitSize / 2;
            var staticObjectPosition = new Vector2(
                (float)(worldCoords[0] * hexSize * Math.Sqrt(3)),
                worldCoords[1] * hexSize * 2 / 2
            );

            _rootSprite = new SpriteContainer
            {
                Position = staticObjectPosition
            };

            var shadowTexture = _game.Content.Load<Texture2D>("Sprites/game-objects/simple-object-shadow");
            _rootSprite.AddChild(new Sprite(shadowTexture)
            {
                Position = new Vector2(0, 0),
                Origin = new Vector2(0.5f, 0.5f),
                Color = new Color(Color.White, 0.5f)
            });

            var graphicsRoot = new SpriteContainer();
            _rootSprite.AddChild(graphicsRoot);

            graphicsRoot.AddChild(new Sprite(_personHeadSprite, origin: new Vector2(0.5f, 0.75f), color: Color.White));
        }

        public override bool HiddenByFow => false;

        public override Vector2 HitEffectPosition => Vector2.UnitY * -24;
        public override IGraphNode Node => StaticObject.Node;

        public override void Draw(GameTime gameTime, Matrix transform)
        {
            _spriteBatch.Begin(transformMatrix: transform);

            _rootSprite.Draw(_spriteBatch);

            _spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
        }

        public IStaticObject StaticObject { get; set; }
        public object Item => StaticObject;
    }
}