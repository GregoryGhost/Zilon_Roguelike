﻿using System.Linq;

using FluentAssertions;

using Moq;

using NUnit.Framework;

using Zilon.Core.Persons;
using Zilon.Core.Persons.Auxiliary;

namespace Zilon.Core.Tests.Persons.Auxiliary
{
    [TestFixture]
    public class PersonEffectHelperTests
    {
        /// <summary>
        /// Тест проверяет, что если значение увеличилось выше, чем ключевая точка,
        /// то эффект изымается.
        /// </summary>
        [Test]
        public void UpdateSurvivalEffect_HasLesserEffectAndValueMoreThatKetValue_HasNoEffect()
        {
            //ARRANGE

            var survivalRandomSource = CreateMaxRollsRandomSource();
            var currentEffects = new EffectCollection();

            var testedEffect = new SurvivalStatHazardEffect(SurvivalStatType.Satiety,
                SurvivalStatHazardLevel.Lesser,
                survivalRandomSource);

            currentEffects.Add(testedEffect);

            var stat = new SurvivalStat(1, -10, 10)
            {
                Type = SurvivalStatType.Satiety,
                KeyPoints = new[] {
                    new SurvivalStatKeyPoint(SurvivalStatHazardLevel.Lesser, 0)
                }
            };



            // ACT
            PersonEffectHelper.UpdateSurvivalEffect(currentEffects,
                stat, 
                new[] { stat.KeyPoints[0] },
                survivalRandomSource);



            // ASSERT
            var factEffect = currentEffects.Items.OfType<SurvivalStatHazardEffect>()
                .SingleOrDefault(x => x.Type == SurvivalStatType.Satiety);
            factEffect.Should().BeNull();
        }

        /// <summary>
        /// Тест проверяет, что если значение увеличилось выше, чем ключевая точка эффекта выше уровнем,
        /// то эффект снижает уровень.
        /// </summary>
        [Test]
        public void UpdateSurvivalEffect_HasStrongEffectAndValueMoreThatKeyValue_HasLesserEffect()
        {
            //ARRANGE

            const SurvivalStatType expectedSurvivalHazardType = SurvivalStatType.Satiety;

            var survivalRandomSource = CreateMaxRollsRandomSource();

            var currentEffects = new EffectCollection();

            var testedEffect = new SurvivalStatHazardEffect(expectedSurvivalHazardType,
                SurvivalStatHazardLevel.Lesser,
                survivalRandomSource);

            currentEffects.Add(testedEffect);

            var stat = new SurvivalStat(-5, -10, 10)
            {
                Type = expectedSurvivalHazardType,
                KeyPoints = new[] {
                    new SurvivalStatKeyPoint(SurvivalStatHazardLevel.Lesser, 0),
                    new SurvivalStatKeyPoint(SurvivalStatHazardLevel.Strong, -10)
                }
            };



            // ACT
            PersonEffectHelper.UpdateSurvivalEffect(currentEffects,
                stat,
                new[] { stat.KeyPoints[1] },
                survivalRandomSource);



            // ASSERT
            var factEffect = currentEffects.Items
                .OfType<SurvivalStatHazardEffect>()
                .Single(x => x.Type == expectedSurvivalHazardType);

            factEffect.Level.Should().Be(SurvivalStatHazardLevel.Lesser);
        }

        /// <summary>
        /// Тест проверяет, что если значение увеличилось выше, чем ключевая точка эффекта выше уровнем,
        /// то эффект снижает уровень.
        /// </summary>
        [Test]
        public void UpdateSurvivalEffect_HasMaxEffectAndValueMoreThatKeyValue_HasStrongEffect()
        {
            //ARRANGE

            const SurvivalStatType expectedSurvivalHazardType = SurvivalStatType.Satiety;

            var survivalRandomSource = CreateMaxRollsRandomSource();

            var currentEffects = new EffectCollection();

            var testedEffect = new SurvivalStatHazardEffect(expectedSurvivalHazardType,
                SurvivalStatHazardLevel.Strong,
                survivalRandomSource);

            currentEffects.Add(testedEffect);

            var stat = new SurvivalStat(-5, -10, 10)
            {
                Type = expectedSurvivalHazardType,
                KeyPoints = new[] {
                    new SurvivalStatKeyPoint(SurvivalStatHazardLevel.Lesser, 5),
                    new SurvivalStatKeyPoint(SurvivalStatHazardLevel.Strong, 0),
                    new SurvivalStatKeyPoint(SurvivalStatHazardLevel.Max, -10)
                }
            };



            // ACT
            PersonEffectHelper.UpdateSurvivalEffect(currentEffects,
                stat,
                new[] { stat.KeyPoints[2] },
                survivalRandomSource);



            // ASSERT
            var factEffect = currentEffects.Items
                .OfType<SurvivalStatHazardEffect>()
                .Single(x => x.Type == expectedSurvivalHazardType);

            factEffect.Level.Should().Be(SurvivalStatHazardLevel.Strong);
        }


        private static ISurvivalRandomSource CreateMaxRollsRandomSource()
        {
            var survivalRandomSourceMock = new Mock<ISurvivalRandomSource>();
            survivalRandomSourceMock.Setup(x => x.RollMaxHazardDamage()).Returns(6);
            var survivalRandomSource = survivalRandomSourceMock.Object;

            return survivalRandomSource;
        }
    }
}