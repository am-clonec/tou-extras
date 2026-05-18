using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace TownOfTransformation.Assets;

public static class NeutAssets
{
    // THIS FILE SHOULD ONLY HOLD BUTTONS AND ROLE BANNERS, EVERYTHING ELSE BELONGS IN Assets.cs
    private const string ShortPath = "TownOfTransformation.Resources.NeutButtons";
    public static LoadableAsset<Sprite> SentinelVentSprite { get; } = new LoadableResourceAsset($"{ShortPath}.SentinelVentButton.png");
    public static LoadableAsset<Sprite> SentinelExplodeSprite { get; } = new LoadableResourceAsset($"{ShortPath}.SentinelExplodeButton.png");
    public static LoadableAsset<Sprite> SentinelKillSprite { get; } = new LoadableResourceAsset($"{ShortPath}.SentinelKillButton.png");
}