using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace TouExtras.Assets;

public static class ModifAssets
{
    private const string ShortPath = "TouExtras.Resources.ModifAssets";
    public static LoadableAsset<Sprite> BowTie { get; } = new LoadableResourceAsset($"{ShortPath}.BowTie.png");
    public static LoadableAsset<Sprite> Astronaut { get; } = new LoadableResourceAsset($"{ShortPath}.Astronaut.png");
    public static LoadableAsset<Sprite> Pirate { get; } = new LoadableResourceAsset($"{ShortPath}.Pirate.png");
    public static LoadableAsset<Sprite> Party { get; } = new LoadableResourceAsset($"{ShortPath}.Party.png");
}