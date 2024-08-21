using Game.Configs;
using Game.Framework;

namespace Game.Effects
{
    public static class EffectFactory
    {
        public static Effect Create(EffectConfig config)
        {
            return config.Type switch
            {
                _ => null
            };
        }
    }
}