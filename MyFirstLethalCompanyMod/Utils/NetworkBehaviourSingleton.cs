using System;
using Unity.Netcode;
using UnityEngine;

namespace MyFirstLethalCompanyMod.Utils
{
    public abstract class NetworkBehaviourSingleton<T> : NetworkBehaviour where T : NetworkBehaviourSingleton<T>
    {
        public static event Action<NetworkBehaviourSingleton<T>>? OnInstanceSet;
        private static T? instance;
        private static bool isSpawning = false;

        public static T? Instance
        {
            get
            {
                if (instance == null && !isSpawning)
                {
                    isSpawning = true;
                    CreateInstance();
                    isSpawning = false;
                }
                return instance;
            }
            protected set
            {
                if (instance == value)
                    return;
                instance = value;
                OnInstanceSet?.Invoke(instance!);
            }
        }

        private static void CreateInstance()
        {
            instance = FindObjectOfType<T>();

            if (instance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name);
                instance = obj.AddComponent<T>();
                DontDestroyOnLoad(obj);
            }

            NetworkObject netObj = instance.GetComponent<NetworkObject>();
            if (netObj == null)
                netObj = instance.gameObject.AddComponent<NetworkObject>();

            netObj.DontDestroyWithOwner = true;

            if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer && !netObj.IsSpawned)
            {
                netObj.Spawn();
            }
        }

        public static bool HasInstance => instance != null;

        protected virtual void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            if (instance == null)
                instance = (T)this;

            OnInstanceSet?.Invoke(this);

            NetworkObject netObj = GetComponent<NetworkObject>();
            if (netObj == null)
                netObj = gameObject.AddComponent<NetworkObject>();

            netObj.DontDestroyWithOwner = true;
        }

        protected virtual void Start()
        {
            if (NetworkManager.Singleton != null && IsServer && NetworkObject != null && !NetworkObject.IsSpawned)
            {
                NetworkObject.Spawn();
            }
        }

        protected virtual void OnEnable()
        {
            if (NetworkManager.Singleton != null && NetworkObject != null && !NetworkObject.IsSpawned && IsServer)
            {
                NetworkObject.Spawn();
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (instance == this)
                instance = null;
        }
    }
}