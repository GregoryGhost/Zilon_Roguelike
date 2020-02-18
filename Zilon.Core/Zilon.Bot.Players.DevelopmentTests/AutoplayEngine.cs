﻿using System;
using Zilon.Bot.Sdk;
using Zilon.Core.Tactics;
using Zilon.Emulation.Common;

namespace Zilon.Bot.Players.DevelopmentTests
{
    class AutoplayEngine<T> : AutoplayEngineBase<T> where T : IPluggableActorTaskSource
    {
        private readonly Startup _startup;

        public AutoplayEngine(Startup startup, BotSettings botSettings): base(botSettings)
        {
            _startup = startup;
        }

        protected override void ConfigBotAux()
        {
            _startup.ConfigureAux(ServiceScope.ServiceProvider);
        }

        protected override void CatchActorTaskExecutionException(ActorTaskExecutionException exception)
        {
            throw exception;
        }

        protected override void CatchException(Exception exception)
        {
            throw exception;
        }

        protected override void ProcessEnd()
        {
        }
    }
}
