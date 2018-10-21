﻿using System;

using JetBrains.Annotations;

using Newtonsoft.Json;

using Zilon.Core.Common;

namespace Zilon.Core.Schemes
{
    public class TacticalActStatsSubScheme : SubSchemeBase, ITacticalActStatsSubScheme
    {
        [UsedImplicitly]
        [JsonConstructor]
        public TacticalActStatsSubScheme(TacticalActOffenceSubScheme offence,
            TacticalActEffectType effect,
            Roll efficient,
            Range<int> range,
            int hitCount,
            bool isMelee)
        {
            Offense = offence ?? throw new ArgumentNullException(nameof(offence));
            Effect = effect;
            Efficient = efficient ?? throw new ArgumentNullException(nameof(efficient));
            Range = range ?? throw new ArgumentNullException(nameof(range));
            HitCount = hitCount;
            IsMelee = isMelee;
        }

        /// <summary>
        /// Характеристики атакующей способности действия.
        /// </summary>
        public ITacticalActOffenceSubScheme Offense { get; set; }

        /// <summary>
        /// Эффект, который оказывает действие.
        /// </summary>
        public TacticalActEffectType Effect { get; }

        /// <summary>
        /// Эффективность действия.
        /// </summary>
        public Roll Efficient { get; }

        /// <summary>
        /// Дистанция, в котором возможно использования действия.
        /// </summary>
        public Range<int> Range { get; set; }

        /// <summary>
        /// Количество ударов при совершении действия.
        /// </summary>
        public int HitCount { get; }

        /// <summary>
        /// Является ли действие рукопашным.
        /// </summary>
        /// <remarks>
        /// Рукопашные действия переводят актёра в режим рукопашного боя.
        /// Во время рукопашного режима можно использовать только рукопашные действия.
        /// </remarks>
        public bool IsMelee { get; }
    }
}
