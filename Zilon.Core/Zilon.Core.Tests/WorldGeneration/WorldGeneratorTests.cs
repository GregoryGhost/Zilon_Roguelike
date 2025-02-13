﻿using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

using NUnit.Framework;

using Zilon.Core.CommonServices.Dices;
using Zilon.Core.Schemes;
using Zilon.Core.WorldGeneration;

namespace Zilon.Core.Tests.WorldGeneration
{
    [Ignore("Эти тесты для ручной проверки. Нужно их привести к нормальным модульным тестам.")]
    [TestFixture()]
    public class WorldGeneratorTests
    {
        [Test]
        public async Task GenerateAsyncTest()
        {
            var dice = new Dice();
            var schemeService = CreateSchemeService();
            var generator = new WorldGenerator(dice, schemeService);

            var globe = await generator.GenerateGlobeAsync();
            globe.Save(@"c:\worldgen");
        }

        [Test()]
        public async Task GenerateRegionAsyncTest()
        {
            var dice = new Dice();
            var schemeService = CreateSchemeService();
            var generator = new WorldGenerator(dice, schemeService);

            var globe = await generator.GenerateGlobeAsync();

            var region = generator.GenerateRegionAsync(globe, globe.Localities.First().Cell);
        }

        private ISchemeService CreateSchemeService()
        {
            var schemePath = ConfigurationManager.AppSettings["SchemeCatalog"];

            var schemeLocator = new FileSchemeLocator(schemePath);

            var schemeHandlerFactory = new SchemeServiceHandlerFactory(schemeLocator);

            return new SchemeService(schemeHandlerFactory);
        }
    }
}