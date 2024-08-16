using Game.Events;
using Game.Framework;
using PamisuKit.Common;

namespace Game.Combat
{
    public static class DamageHelper
    {
        // TODO
        // public static Damage GetDamage(Character instigator, Character target)
        // {
        //     
        //     return new Damage();
        // }

        public static void ApplyDamage(Damage damage, Character target)
        {
            target.AttrComp.ChangeHealth(damage);
            var damageTextPos = target.Trans.position;
            damageTextPos.y += target.Model.VisualHeight;
            EventBus.Emit(new RequestShowDamageText 
            {
                WorldPos = damageTextPos,
                Damage = damage,
            });
        }
    }
}