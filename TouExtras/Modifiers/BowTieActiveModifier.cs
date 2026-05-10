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
using MiraAPI.LocalSettings;
using MiraAPI.Patches.Stubs;
using MiraAPI.Roles;
using Reactor.Networking.Attributes;
using TouExtras.Buttons.Impostor;
using TownOfUs;
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
using TownOfUs.Roles;
using TownOfUs.Roles.Impostor;
using TownOfUs.Roles.Neutral;
using HarmonyLib;
using MiraAPI.Modifiers.Types;
using TownOfUs.Options.Modifiers;
using TownOfUs.Options.Modifiers.Universal;
using TownOfUs.Options.Roles.Neutral;
using TownOfUs.Events;
using JetBrains.Annotations;
using Il2CppMono.Security.Authenticode;
using System.Collections;
using TownOfUs.Utilities.Appearances;
using Reactor.Utilities.Extensions;
using TownOfUs.Networking;
using TownOfUs.Options.Roles.Impostor;
using TownOfUs.Options;
using TownOfUs.Patches;
using Epic.OnlineServices.RTC;
using TouExtras.Options.Modifiers.NeutImp;

namespace TouExtras.Modifiers;

public sealed class BowTieActiveModifier() : BaseModifier, IVisualAppearance
{
    public override string ModifierName => ModifName();
    public override bool HideOnUi => true;
    public bool VisualPriority => true;

    public VisualAppearance GetVisualAppearance()
    {
        var nameColor = new Color(1f, 0.8392156863f, 0.9254901961f, 1f);
        var hatId = "hat_bsb2_bowPink";
        var skinId = "skin_None";
        var visorId = "visor_Blush";
        var colorId = 13;
        var name = "Rose";
        
        if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.PinkTie)
        {
            nameColor = new Color(0.8705882353f, 0.5725490196f, 0.7019607843f, 1f);
            hatId = "hat_bsb2_bowPink";
            skinId = "skin_None";
            visorId = "visor_Blush";
            colorId = 13;
            name = "Bow Tie Lover";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Astronaut)
        {
            nameColor = new Color(0.7058823529f, 0.2431372549f, 0.08235294118f, 1f);
            hatId = "hat_astronaut";
            skinId = "skin_Astro";
            visorId = "visor_None";
            colorId = 4;
            name = "Cool Astronaut";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Pirate)
        {
            nameColor = new Color(0.2901960784f, 0.337254902f, 0.368627451f, 1f);
            hatId = "hat_pkHW01_Pirate";
            skinId = "skin_None";
            visorId = "visor_None";
            colorId = 6;
            name = "Scary Pirate";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Party)
        {
            nameColor = new Color(0.6745098039f, 0.168627451f, 0.6823529412f, 1f);
            hatId = "hat_partyhat";
            skinId = "skin_None";
            visorId = "visor_shuttershadesBlue";
            colorId = 3;
            name = "Party Person";
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

            /*PlayerMaterialColor = new Color(1f, 0.8392156863f, 0.9254901961f, 1f),
            PlayerMaterialBackColor = new Color(0.8705882353f, 0.5725490196f, 0.7019607843f, 1f), */
            
            
            
        };
    }

    public static string ModifName()
    {
        if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.PinkTie)
        {
            return "Bow Tied";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Astronaut)
        {
            return "In Space";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Pirate)
        {
            return "Aboard Ship";
        } else if (OptionGroupSingleton<BowTieErOptions>.Instance.BowType.Value == (int)BType.Party)
        {
            return "At Party";
        } else
        {
            return "pls tell clonec in the All Of Us discord that this thing broke and that he should fix it ty";
        }
    }
    public override void OnActivate()
    {
        Player.RawSetAppearance(this);
    }

    public override void OnDeactivate()
    {
        Player.ResetAppearance();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (VanillaSystemCheckPatches.ShroomSabotageSystem && VanillaSystemCheckPatches.ShroomSabotageSystem.IsActive)
        {
            Player.RawSetAppearance(this);

        }
    }
}