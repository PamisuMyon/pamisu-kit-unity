using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Title
{
    public class TitleDirector : Director
    {

        [SerializeField]
        private AssetReference _combatSceneRef;
        
        public void StartGame()
        {
            Addressables.LoadSceneAsync(_combatSceneRef);
        }
        
    }
}