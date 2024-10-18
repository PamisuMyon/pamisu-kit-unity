using System.Collections.Generic;
using Game.Framework;

namespace Game.Farm.Models
{
    public class FarmData : ISerializee
    {
        public List<PatchData> Patches;
        
        public void PreSerialize()
        {
        }

        public void PostDeserialize()
        {
            Patches ??= new List<PatchData>();
            
            for (int i = 0; i < Patches.Count; i++)
            {
                Patches[i].PostDeserialize();
            }
        }
    }
    
}