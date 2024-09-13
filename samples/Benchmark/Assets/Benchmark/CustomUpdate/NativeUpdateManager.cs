using Cysharp.Threading.Tasks;
using PamisuKit.Common.Util;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CustomUpdate
{
    public class NativeUpdateManager : MonoBehaviour
    {
        [SerializeField]
        private Button _returnButton;
        
        [SerializeField]
        private TMP_InputField _numberInput;

        [SerializeField]
        private Button _startButton;

        [SerializeField]
        private Button _resetButton;

        [SerializeField]
        private TMP_Text _infoText;

        [Space]
        [SerializeField]
        private CustomUpdateDebugger _debugger;

        [SerializeField]
        private GameObject _prefab;

        private void Awake()
        {
            _returnButton.SetOnClickListener(() => SceneManager.LoadScene("CustomUpdateTitle"));
            _startButton.SetOnClickListener(() =>
            {
                if (int.TryParse(_numberInput.text, out var number))
                {
                    Generate(number).Forget();
                }
            });
            _resetButton.SetOnClickListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            if (_debugger.IsEnabled)
            {
                _infoText.text = $"Last: {_debugger.LastTime:0.00}ms\nAverate: {_debugger.AverageTime:0.00}ms\nFPS: {1f / Time.unscaledDeltaTime:0}";
            }
        }

        public async UniTaskVoid Generate(int num)
        {
            _startButton.enabled = false;
            while (num > 0)
            {
                for (int i = 0; i < 100 && num > 0; i++, num--)
                {
                    Instantiate(_prefab); 
                }
                await UniTask.Yield();
            }
            _debugger.IsEnabled = true;
        }
        
    }
}