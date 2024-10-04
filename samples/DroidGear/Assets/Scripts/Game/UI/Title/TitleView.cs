using PamisuKit.Common.Util;
using PamisuKit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Title
{
    public class TitleView : MonoEntity
    {

        [SerializeField]
        private Button _startButton;

        [SerializeField]
        private Button _quitButton;

        protected override void OnCreate()
        {
            base.OnCreate();
            _startButton.SetOnClickListener(() =>
            {
                GetDirector<TitleDirector>().StartGame();
            });
            _quitButton.SetOnClickListener(() =>
            {
                App.Instance.Quit();
            });
        }
    }
}