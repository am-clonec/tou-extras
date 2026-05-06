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
using TownOfUs.Options.Roles.Crewmate;
using TownOfUs.Roles.Crewmate;

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