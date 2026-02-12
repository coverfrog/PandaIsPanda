using System;
using System.Collections.Generic;
using System.Linq;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PandaIsPanda
{
    public class UIPageGameStory : UIPage
    {
        #region # Event

        public delegate void GachaRequestHandler(ulong costId);
        
        public event GachaRequestHandler OnGachaRequest;

        #endregion
        
        [Header("# References")]
        [SerializeField] private RTLTextMeshPro m_txtRound;
        [SerializeField] private RTLTextMeshPro m_txtTimer;
        [SerializeField] private RTLTextMeshPro m_txtAliasCount;
        [SerializeField] private RTLTextMeshPro m_txtEnemyCount;
        [SerializeField] private RTLTextMeshPro m_txtGold;
        [SerializeField] private RTLTextMeshPro m_txtBamboo;
        [SerializeField] private Button m_btnGachaNormal;
        [SerializeField] private Button m_btnGachaUnique;

        private GameStoryData m_data;
        
        public void Open
        (
            GameStoryData data,
            GachaRequestHandler onGachaRequest
        )
        {
            m_data = data;
            
            OnGachaRequest -= onGachaRequest;
            OnGachaRequest += onGachaRequest;
            
            OnAliasCountChanged(data.AliasCount.Value);

            data.AliasCount.OnValueChanged -= OnAliasCountChanged;
            data.AliasCount.OnValueChanged += OnAliasCountChanged;
            
            OnEnemyCountChanged(data.EnemyCount.Value);
            
            data.EnemyCount.OnValueChanged -= OnEnemyCountChanged;
            data.EnemyCount.OnValueChanged += OnEnemyCountChanged;
            
            OnRoundChanged(data.Round.Value);
            
            data.Round.OnValueChanged -= OnRoundChanged;
            data.Round.OnValueChanged += OnRoundChanged;

            OnTimerChanged(data.Timer.Value);
            
            data.Timer.OnValueChanged -= OnTimerChanged;
            data.Timer.OnValueChanged += OnTimerChanged;
            
            OnItemsUpdate(null);
            
            data.InventoryData.OnItemUpdate -= OnItemsUpdate;
            data.InventoryData.OnItemUpdate += OnItemsUpdate;

        }

        private void OnItemsUpdate(List<ItemData> inventoryItems)
        {
            int goldCount = 0;
            int bambooCount = 0;

            if (inventoryItems != null)
            {
                ItemData goldItem = inventoryItems.Find(i => i.Constant.Id == ItemKey.k_gold);
                ItemData bambooItem = inventoryItems.Find(i => i.Constant.Id == ItemKey.k_bamboo);
                
                goldCount = goldItem != null ? goldItem.Count : 0;
                bambooCount = bambooItem != null ? bambooItem.Count : 0;
            }
            
            if (m_txtGold)
                m_txtGold.text = $"Gold : {goldCount}";
            
            if (m_txtBamboo)
                m_txtBamboo.text = $"Bamboo : {bambooCount}";

            EnableGachaBtn(m_btnGachaNormal, GachaCostKey.k_roundNormal);
            EnableGachaBtn(m_btnGachaUnique, GachaCostKey.k_roundUnique);
        }

        private void EnableGachaBtn(Button button, ulong costId)
        {
            var enable = true;
            
            if (m_data.AliasCount.Value >= m_data.AliasMaxCount.Value)
            {
                enable = false;
            }

            else
            {
                var inventoryItem = m_data.InventoryData.Items;
                
                if (inventoryItem != null)
                {
                    var costItems = DataManager.Instance.GachaCostConstants[costId].CostItems;
            
                    foreach (CountValue<ulong> cv in costItems)
                    {
                        var haveItems = inventoryItem.Where(item => item.Constant.Id == cv.value).ToList();
                        if (!haveItems.Any())
                        {
                            enable = false;
                            break;
                        }
                
                        var haveCount = haveItems.Sum(item => item.Count);
                        if (cv.count > haveCount)
                        {
                            enable = false;
                            break;
                        }
                    }
                }
                else
                {
                    enable = false;
                }
            }
            
            button.interactable = enable;
        }

        private void OnTimerChanged(float value)
        {
            if (!gameObject.activeInHierarchy ||
                !m_txtTimer)
                return;
            
            m_txtTimer.text = $"Timer : {value:00.0}";
        }

        private void OnRoundChanged(ulong value)
        {
            if (!gameObject.activeInHierarchy ||
                !m_txtRound)
                return;

            m_txtRound.text = $"Round : {value}";
        }

        private void OnEnemyCountChanged(int value)
        {
            if (!gameObject.activeInHierarchy ||
                !m_txtEnemyCount)
                return;

            m_txtEnemyCount.text = $"Enemy Count: {value}";
        }

        private void OnAliasCountChanged(int value)
        {
            if  (!gameObject.activeInHierarchy ||
                !m_txtAliasCount)
                return;
            
            m_txtAliasCount.text = $"Alias Count: {value}";
            
            EnableGachaBtn(m_btnGachaNormal, GachaCostKey.k_roundNormal);
            EnableGachaBtn(m_btnGachaUnique, GachaCostKey.k_roundUnique);
        }
        
        public void Invoke_GachaRequestNormal()
        {
            OnGachaRequest?.Invoke(GachaCostKey.k_roundNormal);
        }

        public void Invoke_GachaRequestUnique()
        {
            OnGachaRequest?.Invoke(GachaCostKey.k_roundUnique);
        }
    }
}