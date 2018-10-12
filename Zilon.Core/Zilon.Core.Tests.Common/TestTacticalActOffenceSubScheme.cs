﻿using Zilon.Core.Components;
using Zilon.Core.Schemes;

namespace Zilon.Core.Tests.Common
{
    public class TestTacticalActOffenceSubScheme : SubSchemeBase, ITacticalActOffenceSubScheme
    {
        public OffenseType Type { get; set; }
        public ImpactType Impact { get; set; }
        public int ApRank { get; set; }
    }
}
