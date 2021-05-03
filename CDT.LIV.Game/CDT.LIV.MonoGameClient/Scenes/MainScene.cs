﻿using System;
using System.Threading;
using System.Threading.Tasks;

using CDT.LIV.MonoGameClient.ViewModels.MainScene;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Zilon.Core.Client;
using Zilon.Core.Client.Sector;
using Zilon.Core.Commands;
using Zilon.Core.Players;
using Zilon.Core.Tactics.Behaviour;

namespace CDT.LIV.MonoGameClient.Scenes
{
    class MainScene : GameSceneBase
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly ISectorUiState _uiState;
        private SectorViewModel? _sectorViewModel;
        private Camera? _camera;

        public MainScene(Game game, SpriteBatch spriteBatch) : base(game)
        {
            _spriteBatch = spriteBatch;

            var serviceScope = ((LivGame)Game).ServiceProvider;
            _uiState = serviceScope.GetRequiredService<ISectorUiState>();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_sectorViewModel is null)
            {
                _camera = new Camera();

                _sectorViewModel = new SectorViewModel(Game, _camera, _spriteBatch);

                Components.Add(_sectorViewModel);

                Init();
            }

            if (_camera != null && _uiState.ActiveActor != null)
            {
                _camera.Follow(_uiState.ActiveActor, Game);
            }
        }

        private void Init()
        {
            var serviceScope = ((LivGame)Game).ServiceProvider;

            var globeLoopUpdater = serviceScope.GetRequiredService<IGlobeLoopUpdater>();

            using var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            globeLoopUpdater.ErrorOccured += (s, e) => 
            { 
                Console.WriteLine(e.Exception.ToString()); 
            };

            var commandLoop = serviceScope.GetRequiredService<ICommandLoopUpdater>();

            commandLoop.ErrorOccured += (s, e) => 
            { 
                Console.WriteLine(e.Exception.ToString());
            };
            commandLoop.CommandAutoExecuted += (s, e) => { Console.WriteLine("Auto execute last command"); };
            var playerState = serviceScope.GetRequiredService<ISectorUiState>();
            var inventoryState = serviceScope.GetRequiredService<IInventoryState>();
            commandLoop.CommandProcessed += (s, e) =>
            {
                inventoryState.SelectedProp = null;
                playerState.SelectedViewModel = null;
            };
            var commandLoopTask = commandLoop.StartAsync(cancellationToken);
            commandLoopTask.ContinueWith(task => Console.WriteLine(task.Exception),
                TaskContinuationOptions.OnlyOnFaulted);
            commandLoopTask.ContinueWith(task => Console.WriteLine("Game loop stopped."),
                TaskContinuationOptions.OnlyOnCanceled);
        }
    }

    public class Camera
    {
        private const int UNIT_SIZE = 32;

        public Matrix Transform { get; private set; }

        public void Follow(IActorViewModel target, Game game)
        {
            var playerActorWorldCoords = HexHelper.ConvertToWorld(((HexNode)(target.Actor.Node)).OffsetCoords);

            var position = Matrix.CreateTranslation(
              -playerActorWorldCoords[0] * UNIT_SIZE,
              -playerActorWorldCoords[1] * UNIT_SIZE / 2,
              0);

            var offset = Matrix.CreateTranslation(
                game.GraphicsDevice.Viewport.Width / 2,
                game.GraphicsDevice.Viewport.Height / 2,
                0);

            Transform = position * offset;
        }
    }
}
