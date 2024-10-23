using System.Collections.Generic;
using System.Text;
using Game.Inventory;
using Game.Inventory.Models;
using Game.Worker.Models;
using UnityEngine;
using OdinSerializer;

namespace Playground
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
            // var list = new List<Item>();
            // for (int i = 0; i < _initItems.Length; i++)
            // {
            //     var item = Item.Create(_initItems[i].Config, _initItems[i].Amount);
            //     list.Add(item);
            //     Debug.Log(item.Id);
            // }
            //
            // var json = SerializationUtility.SerializeValue(list, DataFormat.JSON);
            // Debug.Log(Encoding.UTF8.GetString(json));
            //
            // var newList = SerializationUtility.DeserializeValue<List<Item>>(json, DataFormat.JSON);
            // foreach (var item in newList)
            // {
            //     item.PostDeserialize();
            // }
            // Debug.Log(newList);

            // var dict = new Dictionary<WorkerTaskType, Queue<WorkerTask>>();
            // dict[WorkerTaskType.Harvesting] = new Queue<WorkerTask>();
            // dict[WorkerTaskType.Harvesting].Enqueue(new WorkerTask
            // {
            //     TargetId = "1213",
            // });
            // var json = SerializationUtility.SerializeValue(dict, DataFormat.JSON);
            // Debug.Log(Encoding.UTF8.GetString(json));
        }
    }
}