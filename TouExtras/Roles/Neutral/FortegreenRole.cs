using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.LocalSettings;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Utilities;
using TouExtras.Assets;
using TouExtras.Buttons.Neutral;
using TouExtras.Options.Roles.Neutral;
using TouExtras.Buttons.Impostor;
using TouExtras.Options.Roles.Impostor;
using TouExtras.Roles.Impostor;
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


namespace TouExtras.Roles.Neutral;

public sealed class FortegreenRole(IntPtr cppPtr)
    : NeutralRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, ICrewVariant
{
    public RoleBehaviour CrewVariant => RoleManager.Instance.GetRole((RoleTypes)RoleId.Get<TrapperRole>());
    
    public string LocaleKey => "Fortegreen";
    public string RoleName => TouLocale.Get($"ExampleRole{LocaleKey}");
    public string RoleDescription => TouLocale.GetParsed($"ExampleRole{LocaleKey}IntroBlurb");
    public string RoleLongDescription => TouLocale.GetParsed($"ExampleRole{LocaleKey}TabDescription");
    public bool Transformed { get; set; }
    public string GetAdvancedDescription()
    {
        return
            TouLocale.GetParsed($"ExampleRole{LocaleKey}WikiDescription") +
            MiscUtils.AppendOptionsText(GetType());
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
                    ExampleNeutAssets.SentinelExplodeSprite),
            };
        }
    }

    public Color RoleColor => TouExampleColors.Fortegreen;
    public ModdedRoleTeams Team => ModdedRoleTeams.Custom;
    public RoleAlignment RoleAlignment => RoleAlignment.NeutralKilling;
    public float Level => 0f;

    public CustomRoleConfiguration Configuration => new(this)
    {
        CanUseVent = (OptionGroupSingleton<BakerOptions>.Instance.BakeCooldown <= Level),
        IntroSound = TouAudio.GlitchSound,
        Icon = ExampleRoleIcons.Sentinel,
        GhostRole = (RoleTypes)RoleId.Get<NeutralGhostRole>()
    };

    public bool HasImpostorVision => OptionGroupSingleton<SentinelOptions>.Instance.ImpostorVision;

    public bool WinConditionMet()
    {
        var glitchCount = CustomRoleUtils.GetActiveRolesOfType<SentinelRole>().Count(x => !x.Player.HasDied());

        if (MiscUtils.KillersAliveCount > glitchCount)
        {
            return false;
        }

        return glitchCount >= Helpers.GetAlivePlayers().Count - glitchCount;
    }

    public void OffsetButtons()
    {
        var canVent = OptionGroupSingleton<SentinelOptions>.Instance.CanVent || LocalSettingsTabSingleton<TownOfUsLocalSettings>.Instance.OffsetButtonsToggle.Value;
        var douse = CustomButtonSingleton<SentinelExplodeButton>.Instance;
        var ignite = CustomButtonSingleton<FortegreenKillButton>.Instance;
        Coroutines.Start(MiscUtils.CoMoveButtonIndex(douse, !canVent));
        Coroutines.Start(MiscUtils.CoMoveButtonIndex(ignite, !canVent));
    }

    public override void Initialize(PlayerControl player)
    {
        RoleBehaviourStubs.Initialize(this, player);
        if (Player.AmOwner)
        {
            OffsetButtons();
            HudManager.Instance.ImpostorVentButton.graphic.sprite = ExampleNeutAssets.SentinelVentSprite.LoadAsset();
            HudManager.Instance.ImpostorVentButton.buttonLabelText.SetOutlineColor(TouExampleColors.Sentinel);
        }
    }

    public override void Deinitialize(PlayerControl targetPlayer)
    {
        RoleBehaviourStubs.Deinitialize(this, targetPlayer);
        if (Player.AmOwner)
        {
            HudManager.Instance.ImpostorVentButton.graphic.sprite = TouAssets.VentSprite.LoadAsset();
            HudManager.Instance.ImpostorVentButton.buttonLabelText.SetOutlineColor(TownOfUsColors.Impostor);

            var button = CustomButtonSingleton<SentinelExplodeButton>.Instance;

            if (button.Explode != null)
            {
                button.Explode.Clear();
                button.Explode = null;
            }
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
        [MethodRpc((uint)TownOfUsRpc.Transport)]
    public static void RpcTransport(PlayerControl transporter, byte player1, byte player2)
    {
        if (LobbyBehaviour.Instance)
        {
            MiscUtils.RunAnticheatWarning(transporter);
            return;
        }
        if (transporter.Data.Role is not TransporterRole && transporter.Data.Role is not SentinelRole && transporter.Data.Role is not InfiltratorRole && transporter.Data.Role is not EvilSwapperRole)
        {
            Error("RpcTransport - Invalid Transporter");
            return;
        }

        var t1 = GetTarget(player1);
        var t2 = GetTarget(player2);

        //also check again incase they went on the usable while the transporter was picking but ignore vents
        if (t1 == null || t2 == null)
        {
            if (transporter.AmOwner)
            {
                Coroutines.Start(MiscUtils.CoFlash(Color.red));
            }

            return;
        }

        var play1 = MiscUtils.PlayerById(player1)!;
        var play2 = MiscUtils.PlayerById(player2)!;

        var warden = play1.GetModifier<WardenFortifiedModifier>()?.Warden.GetRole<WardenRole>();
        if (warden != null)
        {
            if (transporter.AmOwner)
            {
                WardenRole.RpcWardenNotify(warden.Player, transporter, play1);
            }

            return;
        }

        var warden2 = play2.GetModifier<WardenFortifiedModifier>()?.Warden.GetRole<WardenRole>();
        if (warden2 != null)
        {
            if (transporter.AmOwner)
            {
                WardenRole.RpcWardenNotify(warden2.Player, transporter, play2);
            }

            return;
        }

        var cleric = play1.GetModifier<ClericBarrierModifier>()?.Cleric.GetRole<ClericRole>();
        if (cleric != null)
        {
            if (transporter.AmOwner)
            {
                ClericRole.RpcClericBarrierAttacked(transporter, cleric.Player, play1);
            }

            return;
        }

        var cleric2 = play2.GetModifier<ClericBarrierModifier>()?.Cleric.GetRole<ClericRole>();
        if (cleric2 != null)
        {
            if (transporter.AmOwner)
            {
                ClericRole.RpcClericBarrierAttacked(transporter, cleric2.Player, play2);
            }

            return;
        }

        var infectedtrans = transporter.GetModifier<PlaguebearerInfectedModifier>();
        var infectedplayer1 = play1.GetModifier<PlaguebearerInfectedModifier>();
        var infectedplayer2 = play2.GetModifier<PlaguebearerInfectedModifier>();
        if (infectedtrans != null)
        {
            if (infectedplayer1 == null)
            {
                play1.AddModifier<PlaguebearerInfectedModifier>(infectedtrans.PlagueBearerId);
            }

            if (infectedplayer2 == null)
            {
                play2.AddModifier<PlaguebearerInfectedModifier>(infectedtrans.PlagueBearerId);
            }
        }
        else if (infectedtrans == null && infectedplayer1 != null)
        {
            transporter.AddModifier<PlaguebearerInfectedModifier>(infectedplayer1.PlagueBearerId);
        }
        else if (infectedtrans == null && infectedplayer2 != null)
        {
            transporter.AddModifier<PlaguebearerInfectedModifier>(infectedplayer2.PlagueBearerId);
        }

        LookoutEvents.CheckForLookoutWatched(transporter, play1);
        LookoutEvents.CheckForLookoutWatched(transporter, play2);

        var mercenary = PlayerControl.LocalPlayer.Data.Role as MercenaryRole;
        if ((play1.HasModifier<MercenaryGuardModifier>() || play2.HasModifier<MercenaryGuardModifier>()) && mercenary)
        {
            mercenary!.AddPayment();
        }

        if (play1.TryGetModifier<InvulnerabilityModifier>(out var invic) && invic.AttackAllInteractions)
        {
            if (transporter.AmOwner)
            {
                play1.RpcCustomMurder(transporter, MeetingCheck.OutsideMeeting);
            }

            return;
        }

        if (play2.TryGetModifier<InvulnerabilityModifier>(out var invic2) && invic2.AttackAllInteractions)
        {
            if (transporter.AmOwner)
            {
                play2.RpcCustomMurder(transporter, MeetingCheck.OutsideMeeting);
            }

            return;
        }

        if (play1.HasModifier<VeteranAlertModifier>())
        {
            if (transporter.AmOwner)
            {
                play1.RpcCustomMurder(transporter, MeetingCheck.OutsideMeeting);
            }

            return;
        }

        if (play2.HasModifier<VeteranAlertModifier>())
        {
            if (transporter.AmOwner)
            {
                play2.RpcCustomMurder(transporter, MeetingCheck.OutsideMeeting);
            }

            return;
        }

        if (play1.TryGetModifier<ShyModifier>(out var shy))
        {
            shy.OnRoundStart();
        }

        if (play2.TryGetModifier<ShyModifier>(out var shy2))
        {
            shy2.OnRoundStart();
        }

        if (t1.TryCast<DeadBody>())
        {
            PreCheckUndertaker(t1.TryCast<DeadBody>()!);
        }

        if (t2.TryCast<DeadBody>())
        {
            PreCheckUndertaker(t2.TryCast<DeadBody>()!);
        }

        var positions = GetAdjustedPositions(t1, t2);
        if (t1.TryCast<PlayerControl>() != null && t2.TryCast<DeadBody>() != null)
        {
            positions.Item1 = play1.Collider.bounds.center;
        }

        if (t2.TryCast<PlayerControl>() != null && t1.TryCast<DeadBody>() != null)
        {
            positions.Item2 = play2.Collider.bounds.center;
        }

        Transport(t1, positions.Item2);
        Transport(t2, positions.Item1);
        var touAbilityEvent = new TouAbilityEvent(AbilityType.TransporterTransport, transporter, t1, t2);
        MiraEventManager.InvokeEvent(touAbilityEvent);

        if (transporter.AmOwner)
        {
            if (transporter.Data.Role is TransporterRole)
            {
                var button = CustomButtonSingleton<TransporterTransportButton>.Instance;
                button.DecreaseUses();
                button.ResetCooldownAndOrEffect();

                TownOfUsColors.UseBasic = false;
                if (button.TextOutlineColor != Color.clear)
                {
                    button.SetTextOutline(button.TextOutlineColor);
                    if (button.Button != null)
                    {
                        button.Button.usesRemainingSprite.color = button.TextOutlineColor;
                    }
                }
            }
            else if (transporter.Data.Role is SentinelRole)
            {
                var button = CustomButtonSingleton<SentinelTransportButton>.Instance;
                button.DecreaseUses();
                button.ResetCooldownAndOrEffect();

                TownOfUsColors.UseBasic = false;
                if (button.TextOutlineColor != Color.clear)
                {
                    button.SetTextOutline(button.TextOutlineColor);
                    if (button.Button != null)
                    {
                        button.Button.usesRemainingSprite.color = button.TextOutlineColor;
                    }
                }
            }
            else if (transporter.Data.Role is InfiltratorRole)
            {
                var button = CustomButtonSingleton<InfiltratorTransportButton>.Instance;
                button.DecreaseUses();
                button.ResetCooldownAndOrEffect();

                TownOfUsColors.UseBasic = false;
                if (button.TextOutlineColor != Color.clear)
                {
                    button.SetTextOutline(button.TextOutlineColor);
                    if (button.Button != null)
                    {
                        button.Button.usesRemainingSprite.color = button.TextOutlineColor;
                    }
                }
            }

            TownOfUsColors.UseBasic = LocalSettingsTabSingleton<TownOfUsLocalRoleSettings>.Instance
                .UseCrewmateTeamColorToggle.Value;
        }

        if (play1.AmOwner && t1 is PlayerControl || play2.AmOwner && t2 is PlayerControl)
        {
            var notif1 = Helpers.CreateAndShowNotification(
                $"<b>{TownOfUsColors.Transporter.ToTextColor()}{TouLocale.GetParsed("TouRoleTransporterTransportNotif")}</color></b>", Color.white,
                new Vector3(0f, 1f, -20f), spr: TouRoleIcons.Transporter.LoadAsset());

            notif1.AdjustNotification();

            if (Minigame.Instance != null)
            {
                Minigame.Instance.Close();
                Minigame.Instance.Close();
            }
        }

        MonoBehaviour? GetTarget(byte id)
        {
            var data = GameData.Instance.GetPlayerById(id);
            if (!data)
            {
                return null;
            }

            var body = Helpers.GetBodyById(id);
            if (data.IsDead && body)
            {
                return body;
            }

            var pc = data.Object;
            if (!pc)
            {
                return null;
            }

            if (pc.HasModifier<NoTransportModifier>())
            {
                return null;
            }

            if (pc.GetModifiers<BaseModifier>().Any(x => x is IUntransportable))
            {
                return null;
            }

            if (pc.moveable || pc.inVent || (pc.TryGetModifier<DisabledModifier>(out var mod) &&
                                             (!mod.IsConsideredAlive || !mod.CanBeInteractedWith)))
            {
                if (pc.inVent)
                {
                    pc.MyPhysics.ExitAllVents();
                }

                return pc;
            }

            return null;
        }

        void PreCheckUndertaker(DeadBody body)
        {
            var mods = ModifierUtils.GetActiveModifiers<DragModifier>();

            foreach (var mod in mods)
            {
                if (mod.BodyId == body.ParentId)
                {
                    var dragMod = mod.Player.GetModifier<DragModifier>()!;
                    var dropPos = body.transform.position;
                    dropPos.z = dropPos.y / 1000f;
                    dragMod.DeadBody!.transform.position = dropPos;

                    var touAbilityEvent2 = new TouAbilityEvent(AbilityType.UndertakerDrop, mod.Player, dragMod.DeadBody);
                    MiraEventManager.InvokeEvent(touAbilityEvent2);

                    if (mod.Player.AmOwner)
                    {
                        CustomButtonSingleton<UndertakerDragDropButton>.Instance.SetDrag();
                    }

                    mod.Player.RemoveModifier(dragMod);
                }
            }
        }

        (Vector2, Vector2) GetAdjustedPositions(MonoBehaviour transportable, MonoBehaviour transportable2)
        {
            // assign dummy values so it doesnt error about returning unassigned variables
            Vector2 TP1Position = new(0, 0);
            Vector2 TP2Position = new(0, 0);

            if (transportable.TryCast<DeadBody>() == null && transportable2.TryCast<DeadBody>() == null)
            {
                Error($"type: {transportable.GetIl2CppType().Name}");
                var TP1 = transportable.TryCast<PlayerControl>()!;
                TP1Position = TP1.GetTruePosition();
                TP1Position = new Vector2(TP1Position.x, TP1Position.y + 0.3636f);

                var TP2 = transportable2.TryCast<PlayerControl>()!;
                TP2Position = TP2.GetTruePosition();
                TP2Position = new Vector2(TP2Position.x, TP2Position.y + 0.3636f);

                if (TP1.HasModifier<MiniModifier>())
                {
                    TP1Position = new Vector2(TP1Position.x, TP1Position.y + 0.2233912f * 0.75f);
                    TP2Position = new Vector2(TP2Position.x, TP2Position.y - 0.2233912f * 0.75f);
                }
                else if (TP2.HasModifier<MiniModifier>())
                {
                    TP1Position = new Vector2(TP1Position.x, TP1Position.y - 0.2233912f * 0.75f);
                    TP2Position = new Vector2(TP2Position.x, TP2Position.y + 0.2233912f * 0.75f);
                }
            }
            else if (transportable.TryCast<DeadBody>() != null && transportable2.TryCast<DeadBody>() == null)
            {
                var Player1Body = transportable.TryCast<DeadBody>()!;
                TP1Position = Player1Body.TruePosition;
                TP1Position = new Vector2(TP1Position.x, TP1Position.y + 0.3636f);

                var TP2 = transportable2.TryCast<PlayerControl>()!;
                TP2Position = TP2.GetTruePosition();
                TP2Position = new Vector2(TP2Position.x, TP2Position.y + 0.3636f);

                if (TP2.HasModifier<MiniModifier>())
                {
                    TP1Position = new Vector2(TP1Position.x, TP1Position.y - 0.2233912f * 0.75f);
                    TP2Position = new Vector2(TP2Position.x, TP2Position.y + 0.2233912f * 0.75f);
                }
            }
            else if (transportable.TryCast<DeadBody>() == null && transportable2.TryCast<DeadBody>() != null)
            {
                var TP1 = transportable.TryCast<PlayerControl>()!;
                TP1Position = TP1.GetTruePosition();
                TP1Position = new Vector2(TP1Position.x, TP1Position.y + 0.3636f);

                var Player2Body = transportable2.TryCast<DeadBody>()!;
                TP2Position = Player2Body.TruePosition;
                TP2Position = new Vector2(TP2Position.x, TP2Position.y + 0.3636f);
                if (TP1.HasModifier<MiniModifier>())
                {
                    TP1Position = new Vector2(TP1Position.x, TP1Position.y + 0.2233912f * 0.75f);
                    TP2Position = new Vector2(TP2Position.x, TP2Position.y - 0.2233912f * 0.75f);
                }
            }
            else if (transportable.TryCast<DeadBody>() != null && transportable2.TryCast<DeadBody>() != null)
            {
                TP1Position = transportable.TryCast<DeadBody>()!.TruePosition;
                TP2Position = transportable2.TryCast<DeadBody>()!.TruePosition;
            }

            return (TP1Position, TP2Position);
        }
    }

    public static void Transport(MonoBehaviour mono, Vector3 position)
    {
        var deadBody = mono.TryCast<DeadBody>();
        var player = mono.TryCast<PlayerControl>();
        if (player != null && player.HasModifier<ImmovableModifier>())
        {
            return;
        }

        if (deadBody != null &&
            MiscUtils.PlayerById(deadBody.ParentId)?.HasModifier<ImmovableModifier>() == true)
        {
            return;
        }

        if (player != null)
        {
            player.MyPhysics.ResetMoveState();
            player.transform.position = position;
            player.NetTransform.SnapTo(position);
        }

        mono.transform.position = position;
        Collider2D cd = mono.GetComponent<Collider2D>();
        if (cd != null && deadBody != null)
        {
            mono.transform.position += cd.bounds.center - position;
        }

        var cnt = mono.TryCast<CustomNetworkTransform>();
        if (cnt != null)
        {
            cnt.SnapTo(position, (ushort)(cnt.lastSequenceId + 1));

            if (cnt.AmOwner && ModCompatibility.IsSubmerged())
            {
                ModCompatibility.ChangeFloor(cnt.myPlayer.GetTruePosition().y > -7);
                ModCompatibility.CheckOutOfBoundsElevator(cnt.myPlayer);
            }
        }

        if (player != null && player.AmOwner)
        {
            // If the transported player is a Puppeteer/Parasite controlling someone, snap camera to the victim instead
            MonoBehaviour? cameraTarget = null;
            
            if (player.Data?.Role is ITransportTrigger triggerRole)
            {
                cameraTarget = triggerRole.OnTransport();
            }
            
            MiscUtils.SnapPlayerCamera(cameraTarget ?? PlayerControl.LocalPlayer);
        }
    }
}
