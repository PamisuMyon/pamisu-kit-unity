using Game.Framework;
using UnityEditor;

namespace Game.Farm.Models
{
    public class PlotData : ISerializee
    {
        public string Id;
        public bool IsWatered;
        public CropData Crop;
        public bool HasCrop => Crop != null;
        
        public void PreSerialize()
        {
        }

        public void PostDeserialize()
        {
            Crop?.PostDeserialize();
        }
    }
}