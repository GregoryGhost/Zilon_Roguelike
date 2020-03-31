﻿using System.Collections.Generic;

using Zilon.Core.Diseases;

namespace Zilon.Core.Persons
{
    /// <summary>
    /// Данные персонажа о заболеваемости.
    /// </summary>
    public interface IDiseaseData
    {
        /// <summary>
        /// Текущие болезни персонажа.
        /// </summary>
        IEnumerable<IDiseaseProcess> Diseases { get; }

        /// <summary>
        /// Инфицирование персонажа указанной болезнью.
        /// </summary>
        /// <param name="disease"> Болезнь, которой будет инфицирован персонаж. </param>
        void Infect(IDisease disease);

        /// <summary>
        /// Процесс, обратный инфецированию. Удаляет болезнь из персонажа.
        /// </summary>
        /// <param name="disease"> Целевая болезнь. </param>
        void RemoveDisease(IDisease disease);
    }
}