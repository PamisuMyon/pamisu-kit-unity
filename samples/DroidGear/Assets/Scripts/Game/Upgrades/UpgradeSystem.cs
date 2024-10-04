using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Characters;
using Game.Combat;
using Game.Configs;
using Game.Configs.Upgrades;
using Game.Events;
using PamisuKit.Common.Util;
using PamisuKit.Framework;
using UnityEngine;

namespace Game.Upgrades
{
    public class UpgradeSystem : MonoSystem
    {

        [SerializeField]
        private UpgradeCategory[] _categories;

        private readonly List<UpgradeCategory> _shuffledCategories = new();
        private readonly List<UpgradeItem> _upgradeItems = new();
        private int _queuedUpgradeTimes;

        protected override void OnCreate()
        {
            base.OnCreate();
            _shuffledCategories.AddRange(_categories);
            
            On<PlayerExpChanged>(OnPlayerExpChanged);
            On<ReqSelectUpgradeItem>(OnReqSelectUpgradeItem);
        }

        private void OnPlayerExpChanged(PlayerExpChanged e)
        {
            if (e.LevelUpDelta == 0)
                return;
            _queuedUpgradeTimes = e.LevelUpDelta;
            ShowUpgrades();
        }
        
        private void OnReqSelectUpgradeItem(ReqSelectUpgradeItem e)
        {
            ApplyUpgradeItem(e.Item).Forget();
        }

        private List<UpgradeItem> Roll(int number = 3)
        {
            _shuffledCategories.Shuffle();
            number = Mathf.Min(number, _shuffledCategories.Count);
            _upgradeItems.Clear();
            var droids = GetSystem<CombatSystem>().Bb.Droids;
            for (int i = 0; i < number; i++)
            {
                var item = new UpgradeItem();
                var category = _shuffledCategories[i];
                if (category.Chara is DroneConfig || DroidExists(droids, category.Chara))
                {
                    item.Upgrade = category.Upgrades.RandomItem();
                    item.Chara = category.Chara;
                }
                else
                {
                    item.IsUnlockCharacter = true;
                    item.Chara = category.Chara;
                }
                
                _upgradeItems.Add(item);
            }
            return _upgradeItems;
        }

        private bool DroidExists(List<DroidController> droids, CharacterConfig config)
        {
            for (int i = 0; i < droids.Count; i++)
            {
                if (droids[i].Config == config)
                    return true;
            }
            return false;
        }

        private void ShowUpgrades()
        {
            _queuedUpgradeTimes--;
            var items = Roll();
            GetDirector<GameDirector>().PauseCombat();
            Emit(new ReqShowUpgradeItems { Items = items });
        }

        private async UniTask ApplyUpgradeItem(UpgradeItem item)
        {
            var combatSystem = GetSystem<CombatSystem>();
            if (item.IsUnlockCharacter)
            {
                await combatSystem.AddDroid(item.Chara);
            }
            else
            {
                if (item.Chara is DroneConfig)
                {
                    var drone = combatSystem.Bb.Player.Drone;
                    var upgrade = item.Upgrade.CreateUpgrade();
                    drone.Chara.UpgradeComp.ApplyUpgrade(upgrade);
                    Emit(new GearUpgraded
                    {
                        Config = drone.Chara.Config, 
                        Level = drone.Chara.UpgradeComp.Upgrades.Count
                    });
                }
                else
                {
                    var droids = combatSystem.Bb.Droids;
                    for (int i = 0; i < droids.Count; i++)
                    {
                        if (droids[i].Config != item.Chara)
                            continue;
                        var upgrade = item.Upgrade.CreateUpgrade();
                        droids[i].Chara.UpgradeComp.ApplyUpgrade(upgrade);
                        Emit(new GearUpgraded
                        {
                            Config = droids[i].Chara.Config, 
                            Level = droids[i].Chara.UpgradeComp.Upgrades.Count
                        });
                    }
                }
            }
            
            if (_queuedUpgradeTimes > 0)
                ShowUpgrades();
            else
                GetDirector<GameDirector>().ResumeCombat();
        }

    }

    [Serializable]
    public class UpgradeCategory
    {
        public CharacterConfig Chara;
        public UpgradeConfig[] Upgrades;
    }

    public struct UpgradeItem
    {
        public bool IsUnlockCharacter;
        public CharacterConfig Chara;
        public UpgradeConfig Upgrade;
    }

}
