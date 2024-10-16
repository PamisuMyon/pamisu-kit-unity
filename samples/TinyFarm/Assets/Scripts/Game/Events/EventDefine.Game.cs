using Game.Framework;
using Game.Inventory.Models;
using Game.Worker.Models;

namespace Game.Events
{

    public struct PlayerControlStateChanged
    {
        public PlayerControlState OldState;
        public PlayerControlState NewState;
    }
    
    public struct ReqPlayerControlStateReset { }
    
    public struct ReqChangePlayerControlState
    {
        public PlayerControlState NewState;
        public Item Item;
    }

    public struct ReqAddWorkerTask
    {
        public WorkerTaskType Type;
        public Unit Target;
    }
    
}