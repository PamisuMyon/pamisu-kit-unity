using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Configs;
using PamisuKit.Framework;

namespace Game
{
    public class GameDirector : Director
    {

        private readonly Queue<MonoEntity> _entitySetupQueue = new();
        
        public bool IsReady { get; private set; }
        
        protected override void OnCreate()
        {
            base.OnCreate();
            Init().Forget();
        }

        private async UniTaskVoid Init()
        {
            await CreateMonoSystem<ConfigSystem>().Init();
            IsReady = true;

            while (_entitySetupQueue.Count > 0)
            {
                _entitySetupQueue.Dequeue().Setup(Region);
            }
        }

        public override void SetupMonoEntity(MonoEntity entity)
        {
            if (!IsReady)
            {
                _entitySetupQueue.Enqueue(entity);
                return;
            }
            entity.Setup(Region);
        }

    }
}