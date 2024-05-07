using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Logger = InternalAssets.Codebase.Library.Logging.Logger;

namespace InternalAssets.Codebase.Dialogs.GridInventoryDialog
{
    public class Grid : MonoBehaviour, IDisposable
    {
        [SerializeField] private Transform _draggableParent;
        [SerializeField] private GridDraggableItem _draggablePrefab;
        
        [field: SerializeField] public List<GridInventorySlot> Slots { get; private set; }
        [field: SerializeField] public List<GridPlacedItem> PlacedItems { get; private set; }

        private int _length = -1;
        private int _width = -1;

        public void Initialize(int length, int width)
        {
            _length = length;
            _width = width;
            
            int slotIndex = 0;

            for (int i = 0; i < _length; i++)
            for (int j = 0; j < _width; j++)
            {
                int currentI = i;
                int currentJ = j;

                Slots[slotIndex].Initialize(currentI, currentJ);
                
                Slots[slotIndex].PointerEnter += OnSlotPointerEnter;
                Slots[slotIndex].PointerExit += OnSlotPointerExit;

                slotIndex++;
            }
            
            PlacedItems.ForEach(pi => pi.Initialize(100, 100, 25));
        }

        public void Dispose()
        {
            foreach (GridInventorySlot slot in Slots)
            {
                slot.PointerEnter -= OnSlotPointerEnter;
                slot.PointerExit -= OnSlotPointerExit;
                
                slot.Dispose();
            }
        }
        
        private void OnSlotPointerEnter(GridInventorySlot slot, GridDraggableItem item)
        {
            Logger.Log("Interact");
            
            if (item != null)
            {
                TryFindSlots(item, slot, out List<GridInventorySlot> busySlots);
            }
        }
        
        private void OnSlotPointerExit(GridInventorySlot slot, GridDraggableItem item)
        {
            if (item != null)
            {
                foreach (GridInventorySlot inventorySlot in Slots)
                {
                    if(inventorySlot.IsAvailable)
                        inventorySlot.EnableDefaultColor();
                }
            }
        }

        private bool TryFindSlots(GridDraggableItem item, GridInventorySlot slot, out List<GridInventorySlot> busySlots, bool highlightSlots = true)
        {
            busySlots = new();
            
            bool canBePlaced = true;

            if(slot.IsAvailable == false)
                return false;
            
            for (int i = slot.IndexI; i <= slot.IndexI + (item.Length - 1); i++)
            {
                for (int j = slot.IndexJ; j <= slot.IndexJ + (item.Width - 1); j++)
                {
                    GridInventorySlot currentSlot = Slots.FirstOrDefault(el => el.IndexI == i && el.IndexJ == j);
                        
                    if (i >= _length || j >= _width)
                    {
                        canBePlaced = false;
                        continue;
                    }
                        
                    if (currentSlot == default)
                    {
                        canBePlaced = false;
                        continue;
                    }
                        
                    if (currentSlot.IsAvailable == false)
                    {
                        canBePlaced = false;
                        continue;
                    }
                        
                    if(busySlots.Contains(currentSlot) == false)
                        busySlots.Add(currentSlot);
                }
            }
                
            if(highlightSlots)
                busySlots.ForEach(bs => bs.EnableAvailabilityColor(canBePlaced));
            
            return canBePlaced;
        }
        
#if UNITY_EDITOR
        [Button]
        private void DebugInsertWithSize(int length, int width)
        {
            List<GridInventorySlot> busySlots = new();
            
            foreach (GridInventorySlot gridInventorySlot in Slots)
            {
                if(gridInventorySlot.IsAvailable == false) continue;

                busySlots.Clear();
                
                bool canBePlaced = true;
                
                for (int i = gridInventorySlot.IndexI; i <= gridInventorySlot.IndexI + (length - 1); i++)
                {
                    for (int j = gridInventorySlot.IndexJ; j <= gridInventorySlot.IndexJ + (width - 1); j++)
                    {
                        GridInventorySlot currentSlot = Slots.FirstOrDefault(el => el.IndexI == i && el.IndexJ == j);
                        
                        if (i >= _length || j >= _width || currentSlot == default || currentSlot.IsAvailable == false)
                        {
                            canBePlaced = false;
                            continue;
                        }
                        
                        if(busySlots.Contains(currentSlot) == false)
                            busySlots.Add(currentSlot);
                    }
                }

                if (canBePlaced)
                {
                    GridPlacedItem availableItem = PlacedItems.First(ai => ai.IsAvailable);
                    GridDraggableItem draggableItem = Instantiate(_draggablePrefab, availableItem.transform);
                    
                    draggableItem.Initialize(_draggableParent);
                    draggableItem.Length = length;
                    draggableItem.Width = width;
                    draggableItem.transform.localPosition = Vector3.zero;
                    
                    availableItem.BindItem(draggableItem, busySlots);
                    
                    break;
                }
            }
        }
#endif
    }
}