using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Keybinds;
using MiraAPI.Networking;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TouExtras.Assets;
using TouExtras.Options.Roles.Impostor;
using TouExtras.Roles.Impostor;
using TouExtras.Roles.Neutral;
using TownOfUs.Assets;
using TownOfUs.Buttons;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfUs.Utilities;
using UnityEngine;
using MiraAPI.Utilities;
using MiraAPI.Modifiers;
using TouExtras.Modifiers;
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Roles.Crewmate;
using TouExtras.Modules;

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
using HarmonyLib;

namespace TouExtras.Buttons.Impostor;

public sealed class BakerBakeButton : TownOfUsRoleButton<BakerRole>
{
    public override string Name => "Bake";
    public override Color TextOutlineColor => new Color32(255, 69, 0, 255); // Red-orange

    public override float Cooldown =>
        Math.Clamp(OptionGroupSingleton<BakerOptions>.Instance.BakeCooldown, 5f, 120f);

    public override int MaxUses => 5;
    public override LoadableAsset<Sprite> Sprite => TouCrewAssets.Transport;

    public override void ClickHandler()
    {
        if (!CanClick())
        {
            return;
        }

        OnClick();
    }

    protected override void OnClick()
    {
        PlayerControl.LocalPlayer.NetTransform.Halt();

        if (Minigame.Instance)
        {
            return;
        }

        var player1Menu = CustomPlayerMenu.Create();
        player1Menu.transform.FindChild("PhoneUI").GetChild(0).GetComponent<SpriteRenderer>().material =
            PlayerControl.LocalPlayer.cosmetics.currentBodySprite.BodySprite.material;
        player1Menu.transform.FindChild("PhoneUI").GetChild(1).GetComponent<SpriteRenderer>().material =
            PlayerControl.LocalPlayer.cosmetics.currentBodySprite.BodySprite.material;

        player1Menu.Begin(
            plr => ((!plr.Data.Disconnected && !plr.Data.IsDead) || Helpers.GetBodyById(plr.PlayerId)) &&
                   (plr.moveable || plr.inVent),
            plr =>
            {
                player1Menu.Close();

                if (plr == null)
                {
                    return;
                }

                BakerRole.RpcPlaceMuffin(PlayerControl.LocalPlayer, PlayerControl.LocalPlayer.transform.position, plr);
                plr.RpcAddModifier<HangryModifier>();
                var hangryPlayers = ModifierUtils.GetPlayersWithModifier<HangryModifier>();
                hangryPlayers.Do(x =>
                {
                    var notif1 = Helpers.CreateAndShowNotification(
                        TouLocale.GetParsed("TouRoleBakerCravingNotif", "You really want a muffin right now..."),
                        Color.white, new Vector3(0f, 1f, -20f), spr: TouRoleIcons.Chef.LoadAsset());
                    notif1.AdjustNotification();
                });
                ExtrasGlobalVars.MuffinPos = PlayerControl.LocalPlayer.transform.position;
            }
        );
        foreach (var panel in player1Menu.potentialVictims)
        {
            panel.PlayerIcon.cosmetics.SetPhantomRoleAlpha(1f);
            if (panel.NameText.text != PlayerControl.LocalPlayer.Data.PlayerName)
            {
                panel.NameText.color = Color.white;
            }
        }
    }
}