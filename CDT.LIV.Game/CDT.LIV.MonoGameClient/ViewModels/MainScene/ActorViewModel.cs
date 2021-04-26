﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Zilon.Core.Client;
using Zilon.Core.Common;
using Zilon.Core.Tactics;
using Zilon.Core.Tactics.Spatial;

namespace CDT.LIV.MonoGameClient.ViewModels.MainScene
{
    internal class ActorViewModel : GameObjectBase, IActorViewModel
    {
        private const int UNIT_SIZE = 50;

        private readonly Game _game;
        private readonly SpriteBatch _spriteBatch;

        public ActorViewModel(Game game, SpriteBatch spriteBatch)
        {
            _game = game;
            _spriteBatch = spriteBatch;
        }

        public IActor Actor { get; set; }
        public object Item => Actor;

        public override void Draw(GameTime gameTime)
        {
            var personHeadSprite = _game.Content.Load<Texture2D>("Sprites/Head");
            var personBodySprite = _game.Content.Load<Texture2D>("Sprites/Body");

            _spriteBatch.Begin();

            var playerActorWorldCoords = HexHelper.ConvertToWorld(((HexNode)Actor.Node).OffsetCoords);

            _spriteBatch.Draw(personBodySprite,
               new Rectangle(
                   (int)(playerActorWorldCoords[0] * UNIT_SIZE),
                   (int)(playerActorWorldCoords[1] * UNIT_SIZE / 2 - UNIT_SIZE * 0.45f),
                   UNIT_SIZE,
                   UNIT_SIZE),
               Color.White);

            _spriteBatch.Draw(personHeadSprite,
                new Rectangle(
                    (int)(playerActorWorldCoords[0] * UNIT_SIZE + UNIT_SIZE * 0.25f),
                    (int)(playerActorWorldCoords[1] * UNIT_SIZE / 2 - UNIT_SIZE * 0.5f),
                    (int)(UNIT_SIZE * 0.5),
                    (int)(UNIT_SIZE * 0.5)),
                Color.White);

            _spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
