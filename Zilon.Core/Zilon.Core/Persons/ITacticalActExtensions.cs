﻿using Zilon.Core.Tactics.Spatial;

namespace Zilon.Core.Persons
{
    public static class TacticalActExtensions
    {
        /// <summary>
        /// Проверяет, допустимая ли дистанция для совершения действия.
        /// </summary>
        /// <param name="act"> Проверяемое действие. </param>
        /// <param name="currentCubePos"> Узел, из которого совершается действие. </param>
        /// <param name="targetCubePos"> Целевой узел. </param>
        /// <returns>Возвращает true, если дистанция допустима.</returns>
        public static bool CheckDistance(this ITacticalAct act,
            CubeCoords currentCubePos,
            CubeCoords targetCubePos)
        {
            var range = act.Stats.Range;
            var distance = currentCubePos.DistanceTo(targetCubePos);
            var isInDistance = range.Contains(distance);
            return isInDistance;
        }

        /// <summary>
        /// Проверяет, допустимая ли дистанция для совершения действия.
        /// </summary>
        /// <param name="act"> Проверяемое действие. </param>
        /// <param name="currentNode"> Узел, из которого совершается действие. </param>
        /// <param name="targetNode"> Целевой узел. </param>
        /// <returns>Возвращает true, если дистанция допустима.</returns>
        public static bool CheckDistance(this ITacticalAct act,
            IMapNode currentNode,
            IMapNode targetNode,
            ISectorMap map)
        {
            var range = act.Stats.Range;
            var distance = map.DistanceBetween(currentNode, targetNode);
            var isInDistance = range.Contains(distance);
            return isInDistance;
        }
    }
}
