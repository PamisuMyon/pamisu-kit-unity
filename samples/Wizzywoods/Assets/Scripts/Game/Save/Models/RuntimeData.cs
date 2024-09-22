namespace Game.Save.Models
{
    public class RuntimeData
    {
        public EGameState GameState = EGameState.None;
    }
    
    public enum EGameState
    {
        None,
        GlobalSystemsReady,
    }
    
}