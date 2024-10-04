namespace Game.Combat
{
    public interface ITurnBased
    {
        // void OnCombatBegin();
        //
        // void OnCombatEnd();

        void OnTurnBegin();
        
        void OnTurnEnd();
        
    }
    
}