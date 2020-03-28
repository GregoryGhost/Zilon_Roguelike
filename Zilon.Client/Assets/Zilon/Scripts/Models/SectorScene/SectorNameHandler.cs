﻿using Assets.Zilon.Scripts.Services;

using JetBrains.Annotations;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

using Zilon.Core.Players;

public class SectorNameHandler : MonoBehaviour
{
    public Text SectorNameText;

    [Inject] [NotNull] private readonly HumanPlayer _humanPlayer;
    [Inject] [NotNull] private readonly UiSettingService _uiSettingService;

    public void FixedUpdate()
    {
        var locationScheme = _humanPlayer.SectorNode.Biome.LocationScheme;
        var scheme = _humanPlayer.SectorNode.SectorScheme;

        var currentLanguage = _uiSettingService.CurrentLanguage;

        var locationName = LocalizationHelper.GetValueOrDefaultNoname(currentLanguage, locationScheme.Name);
        var sectorLevelName = LocalizationHelper.GetValueOrDefaultNoname(currentLanguage, scheme.Name);

        SectorNameText.text = $"{locationName} {sectorLevelName}";

        Destroy(this);
    }
}
