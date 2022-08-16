using Exiled.API.Enums;
using Exiled.API.Features;
using HarmonyLib;

namespace FlashLightProjector
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance { get; private set; }
        internal EventHandlers EventHandlers = new EventHandlers();
        public override PluginPriority Priority { get; } = PluginPriority.Last;
        public Plugin() => Instance = this;
        public string PluginName => typeof(Plugin).Namespace;
        private Harmony _harmony = new Harmony("FlashLightProjector-patcher");
        public override void OnEnabled()
        {
            RegisterEvents(); 
            _harmony.PatchAll();
            Log.Info($"Plugin {PluginName} started");
        }
        public override void OnDisabled() => UnregisterEvents();
        private void RegisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.TogglingFlashlight += EventHandlers.OnFlashLight;
            Exiled.Events.Handlers.Player.Left += EventHandlers.OnLeft;
            Exiled.Events.Handlers.Player.Died += EventHandlers.OnDied;
            Exiled.Events.Handlers.Player.DroppingItem += EventHandlers.OnDroppingItem;
            Exiled.Events.Handlers.Player.ChangingRole += EventHandlers.OnChangingRole;
        }
        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.TogglingFlashlight -= EventHandlers.OnFlashLight;
            Exiled.Events.Handlers.Player.Left -= EventHandlers.OnLeft;
            Exiled.Events.Handlers.Player.Died -= EventHandlers.OnDied;
            Exiled.Events.Handlers.Player.DroppingItem -= EventHandlers.OnDroppingItem;
            Exiled.Events.Handlers.Player.ChangingRole -= EventHandlers.OnChangingRole;
        }
    }
}
