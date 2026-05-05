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

public sealed class EvilSwapperRole(IntPtr cppPtr) : ImpostorRole(cppPtr), ITownOfUsRole, IWikiDiscoverable
{

    private MeetingMenu meetingMenu;

    [HideFromIl2Cpp] public PlayerVoteArea? Swap1 { get; set; }
    [HideFromIl2Cpp] public PlayerVoteArea? Swap2 { get; set; }
    public string LocaleKey => "EvilSwapper";
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
                new(TouLocale.GetParsed($"TouRole{LocaleKey}SwapWiki", "Swap (Meeting)"),
                    TouLocale.GetParsed($"TouRole{LocaleKey}SwapWikiDescription"),
                    TouAssets.SwapActive)
            };
        }
    }

    public Color RoleColor => TownOfUsColors.Swapper;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorPower;
    

    public CustomRoleConfiguration Configuration => new(this)
    {
        CanUseVent = OptionGroupSingleton<EvilSwapperOptions>.Instance.CanVent,
        Icon = TouRoleIcons.Swapper,
        OptionsScreenshot = TouBanners.ImpostorRoleBanner,
        MaxRoleCount = 1,
        IntroSound = TouAudio.TimeLordIntroSound
    };



    public override void Initialize(PlayerControl player)
    {
        RoleBehaviourStubs.Initialize(this, player);
        
        var options = OptionGroupSingleton<EvilSwapperOptions>.Instance;
        player.RemainingEmergencies = options.CanButton ? 1 : 0;

        if (Player.AmOwner)
        {
            meetingMenu = new MeetingMenu(this, SetActive, MeetingAbilityType.Toggle, TouAssets.SwapActive,
                TouAssets.SwapInactive, IsExempt)
            {
                Position = new Vector3(-0.40f, 0f, -3f)
            };
        }
    }

    public override void OnMeetingStart()
    {
        RoleBehaviourStubs.OnMeetingStart(this);

        if (Player.AmOwner)
        {
            meetingMenu.GenButtons(MeetingHud.Instance,
                Player.AmOwner && !Player.HasDied() && !Player.HasModifier<JailedModifier>());
        }
    }

    public override void OnVotingComplete()
    {
        RoleBehaviourStubs.OnVotingComplete(this);

        if (Player.AmOwner)
        {
            meetingMenu.HideButtons();
        }
    }

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleBehaviourStubs.Deinitialize(this, targetPlayer);

        if (Player.AmOwner)
        {
            meetingMenu?.Dispose();
            meetingMenu = null!;
        }
    }

    private static bool IsExempt(PlayerVoteArea voteArea)
    {
        var player = GameData.Instance.GetPlayerById(voteArea.TargetPlayerId)?.Object;

        return !player || !player?.Data || player!.Data.Disconnected || player.Data.IsDead ||
               player.HasModifier<JailedModifier>();
    }

    private void SetActive(PlayerVoteArea voteArea, MeetingHud __instance)
    {
        if (__instance.state == MeetingHud.VoteStates.Discussion || IsExempt(voteArea))
        {
            return;
        }

        if (!Swap1)
        {
            Swap1 = voteArea;
            meetingMenu.Actives[voteArea.TargetPlayerId] = true;
        }
        else if (!Swap2)
        {
            Swap2 = voteArea;
            meetingMenu.Actives[voteArea.TargetPlayerId] = true;
        }
        else if (Swap1 == voteArea)
        {
            meetingMenu.Actives[Swap1!.TargetPlayerId] = false;
            Swap1 = null;
        }
        else if (Swap2 == voteArea)
        {
            meetingMenu.Actives[Swap2!.TargetPlayerId] = false;
            Swap2 = null;
        }
        else
        {
            meetingMenu.Actives[Swap1!.TargetPlayerId] = false;
            Swap1 = Swap2;
            Swap2 = voteArea;
            meetingMenu.Actives[voteArea.TargetPlayerId] = !meetingMenu.Actives[voteArea.TargetPlayerId];
        }

        RpcSyncSwaps(Player, Swap1?.TargetPlayerId ?? 255, Swap2?.TargetPlayerId ?? 255);
    }

    [MethodRpc((uint)TownOfUsRpc.SetSwaps)]
    public static void RpcSyncSwaps(PlayerControl swapper, byte swap1, byte swap2)
    {
        var swapperRole = swapper.Data?.Role as EvilSwapperRole;
        var areas = MeetingHud.Instance.playerStates.ToList();
        swapperRole!.Swap1 = areas.Find(x => x.TargetPlayerId == swap1);
        swapperRole.Swap2 = areas.Find(x => x.TargetPlayerId == swap2);
    }
}