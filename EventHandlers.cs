using System;
using System.Collections.Generic;
using System.Text;
using AdminToys;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs;
using Mirror;
using UnityEngine;
using Object = UnityEngine.Object;
    
namespace FlashLightProjector
{
    internal class EventHandlers
    {
        private readonly Dictionary<Player, LightSourceToy[]> _playerLights = new Dictionary<Player, LightSourceToy[]>(); 
        internal void OnFlashLight(TogglingFlashlightEventArgs ev)
        {
            try
            {
                if (ev.NewState)
                {
                    if (!_playerLights.ContainsKey(ev.Player) ||
                        (_playerLights.ContainsKey(ev.Player) && _playerLights[ev.Player][0] == null))
                    {
                        CreateLine(ev.Player);
                    }
                    else
                    {
                        for (int i = 0; i < Plugin.Instance.Config.LightSourceCount; i++)
                            _playerLights[ev.Player][i].NetworkLightRange = FlashlightMathFunction(i + 1);
                    }
                }
                else
                    TryTurnOffFlashlight(ev.Player);
            }
            catch (Exception error)
            {
                ///Log.Debug("SCP:SL go bruh");
            }
        }

        internal void OnWaitingForPlayers()
        {
            if (Plugin.Instance.Config.NetworkServerObjectSaver)
                foreach (var pair in _playerLights)
                    foreach (var primitive in pair.Value)
                        NetworkServer.UnSpawn(primitive.gameObject);
            _playerLights.Clear();
        }

        private void CreateLine(Player owner)
        {
            if(!_playerLights.ContainsKey(owner))
                _playerLights.Add(owner, new LightSourceToy[Plugin.Instance.Config.LightSourceCount]);
            
            for (int i = 0; i < Plugin.Instance.Config.LightSourceCount; i++)
            {
                var toy = CreateLightSource(Vector3.zero, Vector3.one, Vector3.zero);
                toy.NetworkLightRange = FlashlightMathFunction(i + 1);
                toy.NetworkLightIntensity = Plugin.Instance.Config.LightIntensity;
                toy.NetworkLightShadows = Plugin.Instance.Config.LightShadows;
                toy.gameObject.transform.parent = owner.CameraTransform;
                toy.transform.localPosition = Vector3.forward * i * Plugin.Instance.Config.LightSourceDistance;
                NetworkServer.Spawn(toy.gameObject);
                _playerLights[owner][i] = toy;
            }
        }
        private LightSourceToy CreateLightSource(Vector3 position, Vector3 scale, Vector3 eular)
        {
            bool success = NetworkClient.GetPrefab(Guid.Parse("6996edbf-2adf-a5b4-e8ce-e089cf9710ae"), out var gm);
            if (!success) throw new Exception("Light source prefab cannot find in NetworkClient::GetPrefab()");
            LightSourceToy adminToyBase = Object.Instantiate<AdminToys.LightSourceToy>(gm.GetComponent<LightSourceToy>());
            return adminToyBase;
        }

        private float FlashlightMathFunction(int x) => (Plugin.Instance.Config.MathFuncExpanseValue / x) * Plugin.Instance.Config.LightRange;

        private void TryTurnOffFlashlight(Player target)
        {
            if (_playerLights.ContainsKey(target) && _playerLights[target][0] != null)
                if (!Plugin.Instance.Config.NetworkServerObjectSaver)
                {
                    foreach (var obj in _playerLights[target])
                    {
                        NetworkServer.UnSpawn(obj.gameObject);
                        _playerLights[target] = new LightSourceToy[Plugin.Instance.Config.LightSourceCount];
                    }
                }
                else
                    foreach (var light in _playerLights[target])
                        light.NetworkLightRange = 0;
        }

        internal void OnLeft(LeftEventArgs ev)
        {
            if(_playerLights.ContainsKey(ev.Player))
                TryTurnOffFlashlight(ev.Player);
        }

        public void OnDied(DiedEventArgs ev)
        {
            if(_playerLights.ContainsKey(ev.Target))
                TryTurnOffFlashlight(ev.Target);
        }

        public void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if(ev.Item.Type == ItemType.Flashlight && _playerLights.ContainsKey(ev.Player))
                TryTurnOffFlashlight(ev.Player);
        }

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if(_playerLights.ContainsKey(ev.Player))
                TryTurnOffFlashlight(ev.Player);
        }
    }
}
