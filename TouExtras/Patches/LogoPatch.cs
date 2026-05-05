using HarmonyLib;
using MiraAPI.Utilities.Assets;
using TouExtras.Assets;
using TownOfUs.Assets;
using UnityEngine;

namespace TouExtras.Patches;

[HarmonyPatch]
public static class LogoPatch
{
    [HarmonyPatch(typeof(TouAssets), nameof(TouAssets.Banner), MethodType.Getter)]
    [HarmonyPrefix]
#pragma warning disable CA1707 // Harmony prefix uses an underscore-prefixed __result parameter by convention
    public static bool Prefix(ref LoadableAsset<Sprite> __result)
#pragma warning restore CA1707
    {
        __result = ExampleAssets.Banner;
        return false;
    }
}