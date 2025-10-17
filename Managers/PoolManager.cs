// =================================
// Copyright (c) 2025 Pham Huu Gia Hai (aka chuozt)
// All rights reserved. 
// You may not copy, distribute, or publish these scripts 
// without explicit permission from the copyright holder.
// =================================

using System.Collections.Generic;
using UnityEngine;
using System;
using GameCore.DesignPatterns;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Chuozt.Template
{
    public class PoolManager : Singleton<PoolManager>
    {
        [SerializeField] private List<Pool> poolList = new List<Pool>();

        private Dictionary<PoolKey, ObjectPooling> poolDict = new Dictionary<PoolKey, ObjectPooling>();

#if ODIN_INSPECTOR
        [Button("Generate Pool Keys", ButtonSizes.Large)]
        private void GenerateEnumButton() => PoolKeyGenerator.GeneratePoolKeyEnum(this);
#endif

        protected override void OnAwake() => Initialize();

        public void Initialize()
        {
            poolDict.Clear();

            for (int i = 0; i < poolList.Count; i++)
            {
                if (poolList[i].prefabs == null || poolList[i].prefabs.Count == 0)
                    continue;

                CreatePool(ParsePoolKey(poolList[i].poolKey), poolList[i].prefabs, poolList[i].quantity);
            }
        }

        private void CreatePool(PoolKey key, List<GameObject> prefabs, int initialSize)
        {
            if (poolDict.ContainsKey(key))
                return;

            Transform poolParent = new GameObject(key.ToString() + "Group").transform;
            poolParent.SetParent(transform);

            var pool = new ObjectPooling(prefabs, initialSize, poolParent);
            poolDict.Add(key, pool);
        }

        public GameObject GetFromPool(PoolKey key, Vector3 pos = default, Quaternion rot = default, Transform parent = null, bool isRandom = true, int index = 0)
        {
            if (poolDict.TryGetValue(key, out var pool))
            {
                GameObject obj = pool.GetFromPool(isRandom, index);

                if (parent != null)
                    obj.transform.SetParent(parent);

                obj.transform.SetPositionAndRotation(pos, rot);
                return obj;
            }

            Debug.LogError($"No pool with key {key} exists!");
            return null;
        }

        public T GetFromPool<T>(PoolKey key, Vector3 pos = default, Quaternion rot = default, Transform parent = null, bool isRandom = true, int index = 0) where T : Component
        {
            if (poolDict.TryGetValue(key, out var pool))
            {
                GameObject obj = pool.GetFromPool(isRandom, index);

                if (parent != null)
                    obj.transform.SetParent(parent);

                obj.transform.SetPositionAndRotation(pos, rot);

                if (obj.TryGetComponent<T>(out var component))
                    return component;

                Debug.LogError($"Component of type {typeof(T)} not found on object from pool {key}.");
                return null;
            }

            Debug.LogError($"No pool with key {key} exists!");
            return null;
        }

        public void ReturnToPool(PoolKey key, GameObject obj)
        {
            if (poolDict.TryGetValue(key, out var pool))
                pool.ReturnToPool(obj);
            else
                Debug.LogError($"No pool with key {key} exists!");
        }

        PoolKey ParsePoolKey(string poolKeyString)
        {
            if (Enum.TryParse(poolKeyString, out PoolKey key))
                return key;

            return default;
        }
    }

    [Serializable]
    public struct Pool
    {
#if ODIN_INSPECTOR

        [HorizontalGroup("Row1", 0.5f), HideLabel]
        public string poolKey;

        [HorizontalGroup("Row1", 0.5f)]
        public int quantity;

        [HorizontalGroup("Row2", 0.5f, MarginLeft = 0.5f), ListDrawerSettings(ShowFoldout = true, DraggableItems = false, ShowPaging = false)]
        public List<GameObject> prefabs;

#else

    [SerializeField] public string poolKey;
    [SerializeField] public int quantity;
    [SerializeField] public List<GameObject> prefabs;

#endif
    }
}
