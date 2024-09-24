using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class TitleDirector : Director
    {

        [SerializeField]
        private AssetReference _gameSceneRef;
        
        public void StartGame()
        {
            Addressables.LoadSceneAsync(_gameSceneRef);
        }
        
    }
}