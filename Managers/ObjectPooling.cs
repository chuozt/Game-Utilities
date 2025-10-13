// =================================
// Copyright (c) 2025 Pham Huu Gia Hai (aka chuozt)
// All rights reserved. 
// You may not copy, distribute, or publish these scripts 
// without explicit permission from the copyright holder.
// =================================
    
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chuozt.Template
{
    public class ObjectPooling
    {
        private Queue<GameObject> pool;
        private List<GameObject> prefabs;
        private Transform parent;

        public ObjectPooling(List<GameObject> prefabs, int totalSize, Transform parent = null)
        {
            this.prefabs = prefabs;
            this.parent = parent;
            pool = new Queue<GameObject>();

            int prefabCount = prefabs.Count;
            int perPrefab = totalSize / prefabCount;
            int remainder = totalSize % prefabCount;

            for (int i = 0; i < prefabCount; i++)
            {
                int spawnCount = perPrefab + (i < remainder ? 1 : 0); // Distribute remaining
                for (int j = 0; j < spawnCount; j++)
                    AddGameObjectToPool(prefabs[i]);
            }
        }

        private void AddGameObjectToPool(GameObject prefab)
        {
            GameObject newObj = Object.Instantiate(prefab, parent);
            newObj.SetActive(false);
            pool.Enqueue(newObj);
        }

        public GameObject GetFromPool(bool isRandom = false, int index = 0)
        {
            if (pool.Count == 0)
            {
                // Pick a prefab to expand from (e.g., random or index-based)
                GameObject prefabToSpawn = isRandom
                    ? prefabs[Random.Range(0, prefabs.Count)]
                    : prefabs[Mathf.Clamp(index, 0, prefabs.Count - 1)];

                AddGameObjectToPool(prefabToSpawn); // Auto-expand
            }

            var list = pool.ToList();
            GameObject obj = isRandom ? list[Random.Range(0, list.Count)] : list[Mathf.Clamp(index, 0, list.Count - 1)];

            pool = new Queue<GameObject>(list.Except(new[] { obj }));
            obj.SetActive(true);
            return obj;
        }

        public void ReturnToPool(GameObject obj)
        {
            if (obj == null) return;
            obj.SetActive(false);
            obj.transform.SetParent(parent);
            pool.Enqueue(obj);
        }
    }
}
