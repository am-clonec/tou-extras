using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace TownOfTransformation.Assets;

public static class ImpAssets
{
    // THIS FILE SHOULD ONLY HOLD BUTTONS AND ROLE BANNERS, EVERYTHING ELSE BELONGS IN Assets.cs
    private const string ShortPath = "TownOfTransformation.Resources.ImpButtons";
    public static LoadableAsset<Sprite> SkibidiToiletTransformSprite { get; } = new LoadableResourceAsset($"{ShortPath}.SkibidiToiletTransform.png");
    public static LoadableAsset<Sprite> SkibidiToiletPoopSprite { get; } = new LoadableResourceAsset($"{ShortPath}.SkibidiToiletPoop.png");
}