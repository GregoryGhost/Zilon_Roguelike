﻿using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using NUnit.Framework;

using Zilon.Core.CommonServices.Dices;

namespace Zilon.Core.WorldGeneration.AgentCards.Tests
{
    [TestFixture]
    public class TransportHelperTests
    {
        /// <summary>
        /// Тест проверяет, что метод всегда находит случайный нас.пункт, если он есть.
        /// </summary>
        [Test]
        public void RollTargetLocality_AllInOneRealm_AlwaysFound()
        {
            var realm = new Realm();
            var localities = new List<Locality>(Enumerable.Range(1, 10).Select(index => new Locality { Name = $"{index}", Owner = realm }));
            var dice = new Dice(1);

            for (var i = 0; i < 100; i++)
            {
                var targetLocality = TransportHelper.RollTargetLocality(localities, dice, realm, localities.First());
                targetLocality.Should().NotBeNull();
            }
        }
    }
}