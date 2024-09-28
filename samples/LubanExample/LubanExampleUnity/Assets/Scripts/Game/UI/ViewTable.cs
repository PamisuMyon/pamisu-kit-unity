using System.Text;
using Game.Configs;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ViewTable : MonoEntity
    {

        [SerializeField]
        private Text _detail1;

        [SerializeField]
        private Text _detail2;
        
        protected override void OnCreate()
        {
            base.OnCreate();
            Debug.Log("111");

            var configSystem = GetSystem<ConfigSystem>();
            var sb = new StringBuilder("基础示例表中所有记录：\n");
            foreach (var it in configSystem.Tables.TbExampleBasic.DataList) 
            {
                sb.AppendLine($"{it.Id} {it.Name} {it.Type}");
            }

            sb.AppendLine("\n根据id获取基础示例表中单条记录：");
            var item = configSystem.Tables.TbExampleBasic.GetOrDefault(1001);
            sb.AppendLine($"{item.Id} {item.Name} {item.Type}");

            _detail1.text = sb.ToString();

            sb = new StringBuilder("列表表中所有记录：\n");
            foreach (var it in configSystem.Tables.TbExampleList.DataList) 
            {
                sb.AppendLine($"{it.Name} {it.Race} {it.Occupation} {it.Age}岁 {it.Origin}");
            }

            sb.AppendLine("\n根据下标获取列表表中指定记录：");
            var item1 = configSystem.Tables.TbExampleList.DataList[3];
            sb.AppendLine($"{item1.Name} {item1.Race} {item1.Occupation} {item1.Age}岁 {item1.Origin}");

            _detail2.text = sb.ToString();
        }
        
    }
}