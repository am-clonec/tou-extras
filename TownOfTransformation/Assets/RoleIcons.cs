using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace TownOfTransformation.Assets;

public static class RoleIcons
{
    // THIS FILE SHOULD ONLY HOLD ROLE ICONS

    private const string ShortPath = "TownOfTransformation.Resources.RoleIcons";

    // Neutrals
    public static LoadableAsset<Sprite> Sentinel { get; } = new LoadableResourceAsset($"{ShortPath}.Sentinel.png", 200);
    public static LoadableAsset<Sprite> SkibidiToilet { get; } = new LoadableResourceAsset($"{ShortPath}.SkibidiToilet.png", 200);
}