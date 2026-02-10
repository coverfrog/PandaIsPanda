using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PandaIsPandaMvp;
using UnityEngine;

namespace PandaIsPanda
{
    public class DataManager : MonoBehaviour
    {
        public Dictionary<ulong, InventoryData> InventoryData { get; } = new Dictionary<ulong, InventoryData>();
        
        public IReadOnlyDictionary<ulong, RoundConstant> RoundConstants { get; private set; }
        
        public IReadOnlyDictionary<ulong, SpawnEventConstant> SpawnEventConstants { get; private set; }
        
        public IReadOnlyDictionary<ulong, UnitConstant> UnitConstants { get; private set; }
        
        public IReadOnlyDictionary<ulong, LocalizationTextConstant> LocalizationTextConstants { get; private set; }
        
        public IReadOnlyDictionary<ulong, ItemConstant> ItemConstants { get; private set; }
        
        public async UniTask LoadAllAsync()
        {
            await UniTask.WhenAll
            (
                AddressableUtil.LoadAsync<RoundConstantTable>("constanttable/round")
                    .ContinueWith(d => RoundConstants = d.Data),

                AddressableUtil.LoadAsync<SpawnEventConstantTable>("constanttable/spawnevent")
                    .ContinueWith(d => SpawnEventConstants = d.Data),
            
                AddressableUtil.LoadAsync<UnitConstantTable>("constanttable/unit")
                    .ContinueWith(d => UnitConstants = d.Data),
                
                AddressableUtil.LoadAsync<LocalizationTextConstantTable>("constanttable/localizationtext")
                    .ContinueWith(d => LocalizationTextConstants = d.Data),
            
                AddressableUtil.LoadAsync<ItemConstantTable>("constanttable/item")
                    .ContinueWith(d => ItemConstants = d.Data)
                );
        }
        
        public static DataManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}