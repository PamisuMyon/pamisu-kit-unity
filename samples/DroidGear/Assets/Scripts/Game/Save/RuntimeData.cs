namespace Game.Save
{
    public enum GameState
    {
        None,
        AppReady,
        Combat
    }

    public class RuntimeData
    {
        public GameState GameState { get; set; } = GameState.None;

        public bool IsAppReady => GameState >= GameState.AppReady;
        public bool IsInCombat => GameState >= GameState.Combat;
    }
}
