using System.Collections;
using System.Linq;
using HarmonyLib;
using MiraAPI.Modifiers;
using PowerTools;
using TownOfTransformation.Modifiers;

namespace Novisor;

[HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.HandleAnimation))]
public class Patch
{
    public static bool Prefix(PlayerPhysics __instance)
    {
        if (PlayerControl.AllPlayerControls.ToArray().First(x => x.MyPhysics == __instance)
            .HasModifier<PoopedModifier>())
        {
            if (__instance.body.velocity.x < -0.01f)
            {
                __instance.FlipX = true;
            }
            else if (__instance.body.velocity.x > 0.01f)
            {
                __instance.FlipX = false;
            }
            
            return false;
        }
        else return true;
    }
}