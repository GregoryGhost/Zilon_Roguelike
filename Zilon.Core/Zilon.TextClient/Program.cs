﻿using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Zilon.Core.Commands;
using Zilon.Core.Players;
using Zilon.Core.World;

namespace Zilon.TextClient
{
    internal enum GameScreen
    {
        Undefinded,
        GlobeSelection,
        Main,
        Scores
    }

    internal class GameState
    {
        public GameScreen CurrentScreen { get; set; }
        public ServiceProvider ServiceProvider { get; internal set; }
        public IServiceScope ServiceScope { get; set; }
    }

    internal static class Program
    {
        private static async Task Main()
        {
            var serviceContainer = new ServiceCollection();
            var startUp = new StartUp();
            startUp.RegisterServices(serviceContainer);

            serviceContainer.AddSingleton<IGlobeInitializer, GlobeInitializer>();
            serviceContainer.AddSingleton<IGlobeExpander>(provider =>
                (BiomeInitializer)provider.GetRequiredService<IBiomeInitializer>());
            serviceContainer.AddSingleton<IGlobeTransitionHandler, GlobeTransitionHandler>();
            serviceContainer.AddSingleton<IPersonInitializer, HumanPersonInitializer>();
            serviceContainer.AddSingleton<IPlayer, HumanPlayer>();
            serviceContainer.AddScoped<MoveCommand>();
            serviceContainer.AddScoped<NextTurnCommand>();

            using var serviceProvider = serviceContainer.BuildServiceProvider();

            // Create globe

            using var scope = serviceProvider.CreateScope();

            var gameState = new GameState
            {
                CurrentScreen = GameScreen.GlobeSelection,
                ServiceScope = scope,
                ServiceProvider = serviceProvider
            };

            var mainScreenHandler = new MainScreenHandler();
            var globeSelectionScreenHandler = new GlobeSelectionScreenHandler();
            var scoresScreenHandler = new ScoresScreenHandler();
            IScreenHandler screenHandler = globeSelectionScreenHandler;
            do
            {
                var nextScreen = await screenHandler.StartProcessingAsync(gameState);

                switch (nextScreen)
                {
                    case GameScreen.GlobeSelection:
                        screenHandler = globeSelectionScreenHandler;
                        break;

                    case GameScreen.Main:
                        screenHandler = mainScreenHandler;
                        break;

                    case GameScreen.Scores:
                        screenHandler = scoresScreenHandler;
                        break;

                    default:
                        throw new Exception();
                }
            } while (true);
        }
    }
}