using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.LocalSettings;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Utilities;
using TownOfTransformation.Assets;
using TownOfTransformation.Buttons.Neutral;
using TownOfTransformation.Options.Roles.Neutral;
using TownOfTransformation.Buttons.Impostor;
using TownOfTransformation.Options.Roles.Impostor;
using TownOfTransformation.Roles.Impostor;
using TownOfUs;
using TownOfUs.Assets;
using TownOfUs.Extensions;
using TownOfUs.Modules.Localization;
using TownOfUs.Modules.Wiki;
using TownOfUs.Roles;
using TownOfUs.Roles.Crewmate;
using TownOfUs.Roles.Neutral;
using TownOfUs.Utilities;
using UnityEngine;


using MiraAPI.Events;

using MiraAPI.Modifiers;
using MiraAPI.Networking;

using Reactor.Networking.Attributes;

using TownOfUs.Buttons.Crewmate;
using TownOfUs.Buttons.Impostor;
using TownOfUs.Events.Crewmate;
using TownOfUs.Events.TouEvents;
using TownOfUs.Interfaces;
using TownOfUs.Modifiers;
using TownOfUs.Modifiers.Crewmate;
using TownOfUs.Modifiers.Game.Universal;
using TownOfUs.Modifiers.Impostor;
using TownOfUs.Modifiers.Neutral;
using TownOfUs.Modules;
using TownOfUs.Utilities.Appearances;


namespace TownOfTransformation.Roles.Neutral;

public sealed class FortegreenRole(IntPtr cppPtr)
    : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, ICrewVariant, IVisualAppearance
{
    public RoleBehaviour CrewVariant => RoleManager.Instance.GetRole((RoleTypes)RoleId.Get<TrapperRole>());
    
    public string LocaleKey => "Fortegreen";
    public string RoleName => TouLocale.Get($"ExampleRole{LocaleKey}");
    public string RoleDescription => TouLocale.GetParsed($"ExampleRole{LocaleKey}IntroBlurb");
    public string RoleLongDescription => TouLocale.GetParsed($"ExampleRole{LocaleKey}TabDescription");
    public bool Transformed { get; set; } = false;
    public string GetAdvancedDescription()
    {
        return
            TouLocale.GetParsed($"ExampleRole{LocaleKey}WikiDescription") +
            MiscUtils.AppendOptionsText(GetType());
    }

        public VisualAppearance GetVisualAppearance()
    {
        var nameColor = new Color(1f, 0.8392156863f, 0.9254901961f, 1f);
        var hatId = "hat_bsb2_bowPink";
        var skinId = "skin_None";
        var visorId = "visor_Blush";
        var colorId = 13;
        var name = "Rose";
        var material1 = new Color(0.1490196078f, 0.6509803922f, 0.3843137255f, 1f);
        var material2 = new Color(0.07058823529f, 0.631372549f, 0.2823529412f, 1f);
        var material3 = new Color(0.01176470588f, 0.05490196078f, 0.01960784314f, 1f);
        
        if (Transformed)
        {
            nameColor = new Color(1f, 0.5725490196f, 0.7019607843f, 1f);
            hatId = "hat_None";
            skinId = "skin_None";
            visorId = "visor_None";
            colorId = 9483643;
            name = "Fortegreen";
            material1 = new Color(0.1490196078f, 0.6509803922f, 0.3843137255f, 1f);
            material2 = new Color(0.07058823529f, 0.631372549f, 0.2823529412f, 1f);
            material3 = new Color(0.01176470588f, 0.05490196078f, 0.01960784314f, 1f);
        } else
        {
            nameColor = Player.GetDefaultModifiedAppearance().NameColor ?? Color.white;
            hatId = Player.GetDefaultModifiedAppearance().HatId ?? "hat_None";
            skinId = Player.GetDefaultModifiedAppearance().SkinId ?? "skin_None";
            visorId = Player.GetDefaultModifiedAppearance().VisorId ?? "visor_None";
            colorId = Player.GetDefaultModifiedAppearance().ColorId;
            name = Player.GetDefaultModifiedAppearance().PlayerName ?? PlayerControl.LocalPlayer.name;
            material1 = Player.GetDefaultModifiedAppearance().PlayerMaterialColor ?? Color.white;
            material2 = Player.GetDefaultModifiedAppearance().PlayerMaterialBackColor ?? Color.white;
            material3 = Player.GetDefaultModifiedAppearance().RendererColor;
        }
        
        
            
        


        return new VisualAppearance(Player.GetDefaultModifiedAppearance(), TownOfUsAppearances.Swooper)
        {
            HatId = hatId,
            SkinId = skinId,
            VisorId = visorId,
            PlayerName = name,
            PetId = "pet_EmptyPet",
            ColorId = colorId,
            NameColor = nameColor,
            ColorBlindTextColor = Color.clear,

            /*PlayerMaterialColor = material1,
            PlayerMaterialBackColor = material2,
            /*RendererColor = material3,*/
        };
    }
    [HideFromIl2Cpp]
    public List<CustomButtonWikiDescription> Abilities
    {
        get
        {
            return new List<CustomButtonWikiDescription>
            {
                new(TouLocale.GetParsed($"ExampleRole{LocaleKey}Explode", "Explode"),
                    TouLocale.GetParsed($"ExampleRole{LocaleKey}ExplodeWikiDescription"),
                    NeutAssets.SentinelExplodeSprite),
            };
        }
    }

    public Color RoleColor => TouExampleColors.Fortegreen;
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralKilling;
    public float Level { get; set; } = 0f;

    public CustomRoleConfiguration Configuration => new(this)
    {
        CanUseVent = OptionGroupSingleton<FortegreenOptions>.Instance.CanVentAtLvl <= Level,
        IntroSound = TouAudio.GlitchSound,
        Icon = RoleIcons.Sentinel,
        GhostRole = (RoleTypes)RoleId.Get<NeutralGhostRole>()
    };

    public void SetVisApp()
    {
        Player.RawSetAppearance(this);
    }
    public void ResetVisApp()
    {
        Player.ResetAppearance();
    }

    public void Reload()
    {
        Initialize(Player);
    }

    public bool HasImpostorVision => OptionGroupSingleton<FortegreenOptions>.Instance.ImpostorVision <= Level;

    public bool WinConditionMet()
    {
        var glitchCount = CustomRoleUtils.GetActiveRolesOfType<FortegreenRole>().Count(x => !x.Player.HasDied());

        if (MiscUtils.KillersAliveCount > glitchCount)
        {
            return false;
        }

        return glitchCount >= Helpers.GetAlivePlayers().Count - glitchCount;
    }

    public void OffsetButtons()
    {
        var canVent = OptionGroupSingleton<FortegreenOptions>.Instance.CanVentAtLvl <= Level || LocalSettingsTabSingleton<TownOfUsLocalSettings>.Instance.OffsetButtonsToggle.Value;
        var transform = CustomButtonSingleton<FortegreenTransformButton>.Instance;
        var ignite = CustomButtonSingleton<FortegreenKillButton>.Instance;
        Coroutines.Start(MiscUtils.CoMoveButtonIndex(transform, !canVent));
        Coroutines.Start(MiscUtils.CoMoveButtonIndex(ignite, !canVent));
    }

    public override void Initialize(PlayerControl player)
    {
        RoleBehaviourStubs.Initialize(this, player);
        if (Player.AmOwner)
        {
            OffsetButtons();
            HudManager.Instance.ImpostorVentButton.graphic.sprite = NeutAssets.SentinelVentSprite.LoadAsset();
            HudManager.Instance.ImpostorVentButton.buttonLabelText.SetOutlineColor(TouExampleColors.Fortegreen);
        }
    }

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleBehaviourStubs.Deinitialize(this, targetPlayer);
        if (Player.AmOwner)
        {
            HudManager.Instance.ImpostorVentButton.graphic.sprite = TouAssets.VentSprite.LoadAsset();
            HudManager.Instance.ImpostorVentButton.buttonLabelText.SetOutlineColor(TownOfUsColors.Impostor);

        }
    }

    public override bool CanUse(IUsable usable)
    {
        if (!GameManager.Instance.LogicUsables.CanUse(usable, Player))
        {
            return false;
        }

        var console = usable.TryCast<Console>()!;
        return console == null || console.AllowImpostor;
    }

    public override bool DidWin(GameOverReason gameOverReason)
    {
        return WinConditionMet();
    }
}
