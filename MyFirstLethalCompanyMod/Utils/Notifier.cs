// TODO: FIX OR REMOVE
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace MyFirstLethalCompanyMod.Utils
{
    public class Notifier : NetworkBehaviourSingleton<Notifier>
    {
        private bool isInitialized = false;

        private void EnsureInitialized()
        {
            if (isInitialized) return;
            if (NetworkManager.Singleton != null && IsServer && NetworkObject != null && !NetworkObject.IsSpawned)
            {
                Plugin.Logger?.LogDebug("Spawning Notifier network object");
                NetworkObject.Spawn();
            }

            isInitialized = true;
        }

        public void SendNotification(string str)
        {
            if (string.IsNullOrEmpty(str))
                return;

            EnsureInitialized();

            if (!NetworkObject.IsSpawned && IsServer)
            {
                Plugin.Logger?.LogDebug("Waiting for network spawn, scheduling notification");
                StartCoroutine(DelayedNotification(str));
                return;
            }

            SendNotificationImmediate(str);
        }

        private void SendNotificationImmediate(string str)
        {
            if (IsServer || IsHost)
            {
                Plugin.Logger?.LogDebug($"Server broadcasting: {str}");
                if (NetworkObject != null && NetworkObject.IsSpawned)
                {
                    SendNotificationClientRpc(str);
                }
                HUDManager.Instance?.DisplayGlobalNotification(str);
            }
            else if (IsClient)
            {
                Plugin.Logger?.LogDebug($"Client requesting broadcast: {str}");
                if (NetworkObject != null && NetworkObject.IsSpawned)
                {
                    RequestSendNotificationServerRpc(str);
                }
                else
                {
                    Plugin.Logger?.LogWarning("NetworkObject not spawned, showing locally only");
                    HUDManager.Instance?.DisplayGlobalNotification(str);
                }
            }
        }

        private IEnumerator DelayedNotification(string str)
        {
            // Wait for network to initialize
            yield return new WaitForSeconds(0.1f);

            if (NetworkObject != null && !NetworkObject.IsSpawned && IsServer)
            {
                NetworkObject.Spawn();
                yield return new WaitForSeconds(0.1f);
            }

            SendNotificationImmediate(str);
        }

        [ClientRpc]
        private void SendNotificationClientRpc(string str)
        {
            Plugin.Logger?.LogDebug($"Client received notification: {str}");
            HUDManager.Instance?.DisplayGlobalNotification(str);
        }

        [ServerRpc(RequireOwnership = false)]
        private void RequestSendNotificationServerRpc(string str)
        {
            Plugin.Logger?.LogDebug($"Server received client request: {str}");
            SendNotificationClientRpc(str);
            HUDManager.Instance?.DisplayGlobalNotification(str);
        }
    }
}