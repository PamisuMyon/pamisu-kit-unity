using PamisuKit.Common.Util;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CustomUpdate
{
    public class CustomUpdateTitle : MonoBehaviour
    {
        [SerializeField]
        private Button _customButton;
        
        [SerializeField]
        private Button _nativeButton;

        private void Awake()
        {
            _customButton.SetOnClickListener(() => SceneManager.LoadScene("CustomUpdate"));
            _nativeButton.SetOnClickListener(() => SceneManager.LoadScene("NativeUpdate"));
        }
    }
}