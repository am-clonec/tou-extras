using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Events;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.LocalSettings;
using MiraAPI.Modifiers;
using MiraAPI.Networking;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using TouExtras.Assets;
using TouExtras.Buttons.Impostor;
using TouExtras.Modules;
using TouExtras.Options.Roles.Impostor;
using TownOfUs;
using TownOfUs.Assets;
using TownOfUs.Buttons.Crewmate;
using TownOfUs.Buttons.Impostor;
using TownOfUs.Events.Crewmate;
using TownOfUs.Events.TouEvents;
using TownOfUs.Extensions;
using TownOfUs.Interfaces;
using TownOfUs.Modifiers;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modifiers.Game.Universal;
using TownOfUs.Modifiers.Impostor;
using TownOfUs.Modifiers.Neutral;
using TownOfUs.Modules;
using TownOfUs.Modules.Localization;
using TownOfUs.Modules.Wiki;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Roles;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Impostor;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;


namespace TouExtras.Roles.Impostor;

public sealed class BakerRole(IntPtr cppPtr) : ImpostorRole(cppPtr), ITownOfUsRole, IWikiDiscoverable
{


    [HideFromIl2Cpp] public Muffin? Muffie { get; set; }
    public string LocaleKey => "Baker";
    public string RoleName => TouLocale.Get($"TouRole{LocaleKey}");
    public string RoleDescription => TouLocale.GetParsed($"TouRole{LocaleKey}IntroBlurb");
    public string RoleLongDescription => TouLocale.GetParsed($"TouRole{LocaleKey}TabDescription");

    public string GetAdvancedDescription()
    {
        return
            TouLocale.GetParsed($"TouRole{LocaleKey}WikiDescription") +
            MiscUtils.AppendOptionsText(GetType());
    }

    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new(TouLocale.GetParsed($"TouRole{LocaleKey}BakeWiki", "Bake"),
                    TouLocale.GetParsed($"TouRole{LocaleKey}BakeWikiDescription"),
                    TouAssets.SwapActive)
            };
        }
    }

    public Color RoleColor => TownOfUsColors.Swapper;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorPower;
    

    public CustomRoleConfiguration Configuration => new(this)
    {
        
        Icon = TouRoleIcons.Swapper,
        OptionsScreenshot = TouBanners.ImpostorRoleBanner,
        MaxRoleCount = 1,
        IntroSound = TouAudio.TimeLordIntroSound
    };



    public override void Initialize(PlayerControl player)
    {
        RoleBehaviourStubs.Initialize(this, player);
        
        var options = OptionGroupSingleton<BakerOptions>.Instance;

    }



    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleBehaviourStubs.Deinitialize(this, targetPlayer);

        
    }

[MethodRpc((uint)TownOfUsRpc.PlantBomb)]
    public static void RpcPlaceMuffin(PlayerControl player, Vector2 position, PlayerControl target)
    {
        if (LobbyBehaviour.Instance)
        {
            MiscUtils.RunAnticheatWarning(player);
            return;
        }
        if (player.Data.Role is not BakerRole role)
        {
            Error("RpcPlaceMuffin - Invalid baker");
            return;
        }


        if (player.AmOwner)
        {
            role.Muffie = Muffin.CreateMuffin(player, position);
        }
        
        Coroutines.Start(Muffin.MuffinShowTarget(target, position));
        role.Muffie?.Detonate(target);
        
        
    }
    

    
}