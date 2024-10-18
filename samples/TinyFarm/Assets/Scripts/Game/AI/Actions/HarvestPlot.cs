using Cysharp.Threading.Tasks;
using Game.Farm;
using Game.Worker.Models;
using NPBehave;
using UnityEngine;

namespace Game.AI.Actions
{
    public class HarvestPlot : Task
    {
        private WorkerController _worker;
        private string _targetKey;
        
        public HarvestPlot(WorkerController worker, string targetKey) : base("HarvestPlot")
        {
            _worker = worker;
            _targetKey = targetKey;
        }

        protected override void DoStart()
        {
            base.DoStart();
            
            var target = Blackboard[_targetKey] as Plot;
            if (target == null)
            {
                Debug.LogError("Harvest target is not Plot", _worker);
                Stopped(false);
                return;
            }
            if (!target.CanHarvest())
            {
                Debug.LogError("Harvest failed", _worker);
                Stopped(false);
                return;
            }
            DoHarvest(target).Forget();
        }

        private async UniTaskVoid DoHarvest(Plot plot)
        {
            var produce = await plot.Harvest();
            _worker.CarryProduce(produce);
            Stopped(true);
        }
    }
}