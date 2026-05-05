using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace TouExtras.Assets;

public static class ExampleAssets
{
    private const string ShortPath = "TouExtras.Resources";
    public static LoadableAsset<Sprite> Banner { get; } = new LoadableResourceAsset($"{ShortPath}.ExampleBanner.png");
}
