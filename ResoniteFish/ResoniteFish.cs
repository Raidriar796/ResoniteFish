using System;
using Elements.Core;
using FrooxEngine;
using HarmonyLib;
using ResoniteModLoader;

namespace ResoniteFish;
public class ResoniteFish : ResoniteMod {
	internal const string VERSION_CONSTANT = "1.1.0";
	public override string Name => "Fish";
	public override string Author => "Delta & Raidriar";
	public override string Version => VERSION_CONSTANT;
	public override string Link => "https://github.com/XDelta/ResoniteFish";

	[AutoRegisterConfigKey]
	public static readonly ModConfigurationKey<bool> FishSound = new("FishSound", "Enable Sounds", () => false);

	public static ModConfiguration? Config;

	public override void OnEngineInit() {
		Harmony harmony = new("net.deltawolf.Fish");
		harmony.PatchAll();
		Config = GetConfiguration()!;
		Config!.Save(true);
	}

	[HarmonyPatch(typeof(InteractionHandler), "OpenContextMenu")]
	private class ContextMenuOpenMenuPatch {
		public static void Postfix(InteractionHandler __instance, InteractionHandler.MenuOptions options) {
			ContextMenu ctx = __instance.ContextMenu;
			if (options == InteractionHandler.MenuOptions.Default) {
				ContextMenuItem Button = ctx.AddItem("Fish", new Uri("resdb:///5a56714e4cc021888cb51162cc34b55a7a1333fe8bc162c8be906f9345206b3c.png"), colorX.Cyan);

				if (Config!.GetValue(FishSound) == true) {
					Slot MenuSlot = Button.Slot;
					var StaticAudio = MenuSlot.AttachAudioClip(new Uri("resdb:///1ed74a85e7d9a8e2456fadbd276cb9dd223ad96377ad230409693d408524606e.wav"));

					var ClipPlayer = MenuSlot.AttachComponent<RandomAudioClipPlayer>();
					ClipPlayer.Clips.Add();
					ClipPlayer.Clips.GetElement(0).Clip.Target = StaticAudio;

					var Trigger = MenuSlot.AttachComponent<ButtonActionTrigger>();
					Trigger.OnPressed.Target = ClipPlayer.Play;
				}
			}
		}
	}
}
