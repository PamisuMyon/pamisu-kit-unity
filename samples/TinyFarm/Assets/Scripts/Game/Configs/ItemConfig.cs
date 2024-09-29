using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "Item_", menuName = "Configs/ItemConfig", order = 0)]
    public class ItemConfig : ScriptableObject
    {
        public string Name;
        public string Description;
        public AssetReferenceSprite IconRef;
        public float BuyPrice;
        public float SellPrice;
    }

}