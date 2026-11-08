using System;
using System.Collections.Generic;
using Comfort.Common;
using EFT;
using EFT.InventoryLogic;

namespace WeightLevelingRedux.Patches
{
    internal static class WeightRefresher
    {
        // Inventory.TotalWeight is a cached Deferred<float> that only the inventory
        // add/remove/refresh events dirty, so a config change must dirty it manually.
        internal static void Apply()
        {
            WeightPatch.Invalidate();

            try
            {
                var refreshed = new HashSet<Inventory>();

                Refresh(WeightPatch.GetSessionProfile()?.Inventory, refreshed);

                if (Singleton<GameWorld>.Instantiated)
                {
                    Refresh(Singleton<GameWorld>.Instance.MainPlayer?.InventoryController?.Inventory, refreshed);
                }
            }
            catch (Exception ex)
            {
                Plugin.LogSource.LogError($"Failed to refresh weight after config change: {ex}");
            }
        }

        private static void Refresh(Inventory inventory, HashSet<Inventory> refreshed)
        {
            if (inventory == null || !refreshed.Add(inventory))
            {
                return;
            }

            inventory.TotalWeight.SetDirty();
            inventory.TotalWeightEliteSkill.SetDirty();
            inventory._onWeightUpdated.Invoke();
        }
    }
}
