using System;
using System.Collections.Generic;
using UnityEngine;

namespace PandaIsPanda
{
    [Serializable]
    public class GameStoryData
    {
        [SerializeField] private ReactiveProperty<Unit> m_selectedUnit = new(null);

        public ReactiveProperty<Unit> SelectedUnit => m_selectedUnit;
        
        [SerializeField] private ReactiveProperty<int> m_aliasMaxCount = new(3);
        
        public ReactiveProperty<int> AliasMaxCount => m_aliasMaxCount;
        
        [SerializeField] private ReactiveProperty<int> m_aliasCount = new(0);
        
        public ReactiveProperty<int> AliasCount => m_aliasCount;
        
        [SerializeField] private ReactiveProperty<int> m_enemyCount = new(0);

        public ReactiveProperty<int> EnemyCount => m_enemyCount;

        [SerializeField] private ReactiveProperty<ulong> m_round = new(1);
        
        public ReactiveProperty<ulong> Round => m_round;
        
        [SerializeField] private ReactiveProperty<float> m_timer = new ReactiveProperty<float>(0);
        
        public ReactiveProperty<float> Timer => m_timer;
        
        [SerializeField] private InventoryData m_inventoryData = new();
        
        public InventoryData InventoryData => m_inventoryData;
    }
}