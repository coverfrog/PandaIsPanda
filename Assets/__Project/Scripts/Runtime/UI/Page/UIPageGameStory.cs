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
        
        public delegate void SelectUnitHandler(Unit unit);
        
        public event GachaRequestHandler OnGachaRequest;
        
        public event SelectUnitHandler OnSelectUnit;

        #endregion
        
        [Header("# References")]
        [SerializeField] private RTLTextMeshPro m_txtRound;
        [SerializeField] private RTLTextMeshPro m_txtTimer;
        [SerializeField] private RTLTextMeshPro m_txtAliasCount;
        [SerializeField] private RTLTextMeshPro m_txtEnemyCount;
        [SerializeField] private RTLTextMeshPro m_txtGold;
        [SerializeField] private RTLTextMeshPro m_txtBamboo;
        [SerializeField] private RTLTextMeshPro m_txtUnitName;
        [SerializeField] private Button m_btnGachaNormal;
        [SerializeField] private Button m_btnGachaUnique;

        private GameStoryData m_data;
        private Camera m_cam;
        
        private void Awake()
        {
            m_cam = Camera.main;
        }

        #region # Open

        public void Open
        (
            GameStoryData data,
            GachaRequestHandler onGachaRequest,
            SelectUnitHandler onSelectUnit
        )
        {
            m_data = data;
            
            InputManager.Instance.OnPointerClick -= OnPointerClick;
            InputManager.Instance.OnPointerClick += OnPointerClick;
            
            OnGachaRequest -= onGachaRequest;
            OnGachaRequest += onGachaRequest;
            
            OnSelectUnit -= onSelectUnit;
            OnSelectUnit += onSelectUnit;
            
            //
            
            OnSelectUnitChanged(null);

            data.SelectedUnit.OnValueChanged -= OnSelectUnitChanged;
            data.SelectedUnit.OnValueChanged += OnSelectUnitChanged;
            
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
        
        #endregion
        
        #region # Reactive

        private void OnPointerClick(bool isClick, Vector2 screenPos)
        {
            if (isClick)
            {
                Ray ray = m_cam.ScreenPointToRay(screenPos);
                
                if (!Physics.Raycast(ray, out RaycastHit hit)) 
                    return;
                
                if (hit.collider &&
                    hit.collider.transform.parent.TryGetComponent(out Unit unit))
                {
                    Invoke_SelectUnit(unit);
                }
            }

            else
            {
                
            }
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

        private void OnSelectUnitChanged(Unit unit)
        {
            if (!gameObject.activeInHierarchy ||
                !m_txtUnitName)
                return;

            if (unit)
            {
                m_txtUnitName.text = unit.UnitData.Constant.NameId.ToLocalizationText();
            }
            else
            {
                m_txtUnitName.text = "";
            }
        }
        
        #endregion
        
        #region # Invoke

        private void Invoke_SelectUnit(Unit unit)
        {
            OnSelectUnit?.Invoke(unit);
        }
        
        public void Invoke_GachaRequestNormal()
        {
            OnGachaRequest?.Invoke(GachaCostKey.k_roundNormal);
        }

        public void Invoke_GachaRequestUnique()
        {
            OnGachaRequest?.Invoke(GachaCostKey.k_roundUnique);
        }
        
        #endregion
        
    }
}