using System.Collections.Generic;
using System.Text;
using Game.Inventory;
using Game.Inventory.Models;
using UnityEngine;
using OdinSerializer;

namespace Game
{
    public class SerializeTest : MonoBehaviour
    {
        [SerializeField]
        private CollectionInitItem[] _initItems;

        private void Start()
        {
            Test();
        }

        private void Test()
        {
            var list = new List<Item>();
            for (int i = 0; i < _initItems.Length; i++)
            {
                var item = Item.Create(_initItems[i].Config, _initItems[i].Amount);
                list.Add(item);
                Debug.Log(item.Id);
            }
            
            var json = SerializationUtility.SerializeValue(list, DataFormat.JSON);
            Debug.Log(Encoding.UTF8.GetString(json));

            var newList = SerializationUtility.DeserializeValue<List<Item>>(json, DataFormat.JSON);
            foreach (var item in newList)
            {
                item.PostDeserialize();
            }
            Debug.Log(newList);
        }
    }
}