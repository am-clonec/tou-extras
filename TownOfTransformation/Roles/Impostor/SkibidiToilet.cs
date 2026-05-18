using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using Il2CppSystem.Runtime.InteropServices;
using JetBrains.Annotations;
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
using TownOfTransformation.Assets;
using TownOfTransformation.Buttons.Impostor;
using TownOfTransformation.Modules;
using TownOfTransformation.Options.Roles.Impostor;
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
using MiraAPI.Utilities.Assets;
using TownOfUs.Events;
using TownOfUs.Options.Modifiers.Universal;
using MiraAPI.Patches;
using MiraAPI.Keybinds;
using TownOfTransformation.Roles.Impostor;
using TownOfTransformation.Roles.Neutral;
using TownOfUs.Buttons;
using TownOfUs.Options.Modifiers.Alliance;
using TownOfTransformation.Modifiers;
using Reactor.Networking;
using HarmonyLib;
using Rewired;
using TMPro;
using TownOfUs.Utilities.Appearances;
using System.Security;

namespace TownOfTransformation.Roles.Impostor;
public sealed class SkibidiToiletRole(IntPtr cppPtr) : ImpostorRole(cppPtr), ITownOfUsRole, IWikiDiscoverable, IVisualAppearance
{
    public string LocaleKey => "SkibidiToilet";
    public string RoleName => TouLocale.Get($"TouRole{LocaleKey}");
    public string RoleDescription => TouLocale.GetParsed($"TouRole{LocaleKey}IntroBlurb");
    public string RoleLongDescription => TouLocale.GetParsed($"TouRole{LocaleKey}TabDescription");
    public bool Transformed { get; set; }
    private AnimationClip ogRun;
    
    private AnimationClip ogIdle;
    private Vector3 ogSize;
    private bool layer;
    private string ogColorblindName;
    private string ogName;
    

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
                    TouNeutAssets.ChefCookSprite)
            };
        }
    } 

    public Color RoleColor => TownOfUsColors.Impostor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public RoleAlignment RoleAlignment => RoleAlignment.ImpostorPower;
    

        public VisualAppearance GetVisualAppearance()
    {
        var name = Player.GetDefaultModifiedAppearance().PlayerName;
        var pet = Player.GetDefaultModifiedAppearance().PetId;
        if(Transformed)
        {
            name = "Skibidi";
            pet = "pet_EmptyPet";
        }
        return new VisualAppearance(Player.GetDefaultModifiedAppearance(), TownOfUsAppearances.Swooper)
        {
            PlayerName = name,
            PetId = pet,
        };
    }

    public void Transform()
    {
        ogIdle = Player.MyPhysics.Animations.group.IdleAnim;
        ogRun = Player.MyPhysics.Animations.group.RunAnim;
        layer = Player.cosmetics.gameObject.active;
        Player.MyPhysics.Animations.group.RunAnim = NormalAssets.SkibidiWalkAnimation.LoadAsset();
        Player.MyPhysics.Animations.group.IdleAnim = NormalAssets.SkibidiIdleAnimation.LoadAsset();
        Player.MyPhysics.Animations.PlayIdleAnimation();
        Player.cosmetics.gameObject.SetActive(false);
        Transformed = true;
        Player.MyPhysics.Animations.group.SpriteAnimator.GetComponent<SpriteRenderer>().material =
            new(Shader.Find("Sprites/Default"));
        /*ogSize = Player.Collider.transform.localScale;
        Player.Collider.transform.localScale = new Vector3(1.5f, 1.5f, 1f);*/
        
        Transform Names = Player.gameObject.transform.Find("Names");
        Transform ColorblindName = Names.transform.Find("ColorblindName_TMP");
        
        ogColorblindName = ColorblindName.GetComponent<TextMeshPro>().text;
       
        ColorblindName.GetComponent<TextMeshPro>().text = "Skibidi";
        
    
    }
    public void Untransform()
    {
        Player.MyPhysics.Animations.group.RunAnim = ogRun;
        Player.MyPhysics.Animations.group.IdleAnim = ogIdle;
        Player.MyPhysics.Animations.PlayIdleAnimation();
        Player.cosmetics.gameObject.SetActive(layer);
        Transformed = false;
        SpriteRenderer rend = Player.MyPhysics.Animations.group.SpriteAnimator.GetComponent<SpriteRenderer>();
        rend.material = new Material(Shader.Find("Unlit/PlayerShader"));
        PlayerMaterial.SetColors(Player.cosmetics.ColorId, rend);
        /*Player.Collider.transform.localScale = ogSize;*/
        
    }
    public CustomRoleConfiguration Configuration => new(this)
    {
        
        Icon = RoleIcons.SkibidiToilet,
        OptionsScreenshot = TouBanners.ImpostorRoleBanner,
        MaxRoleCount = 1,
        IntroSound = TouAudio.TimeLordIntroSound
    };
}