using System.Collections;
using AbilitySystem;
using AbilitySystem.Authoring;
using Pamisu.Commons.Pool;
using Pamisu.Gameplay.Platformer;
using Pamisu.GASExtension;
using UnityEngine;

namespace Pamisu.Platformer2D.Abilities
{
    [CreateAssetMenu(menuName = "Platformer 2D/Abilities/Dash Ability")]
    public class DashAbility : AbstractAbilityScriptableObject
    {

        public float DashSpeed = 20f;
        public float DashDuration = .2f;
        public string IsDashingAnimParam = "IsDashing";
        public GameObject DashFlashEffectPrefab;
        public GameObject DashWaveEffectPrefab;
        public GameObject DashSparkEffectPrefab;
        public GameObject DashShadowEffectPrefabLeft;
        public GameObject DashShadowEffectPrefabRight;
        
        public override AbstractAbilitySpec CreateSpec(AbilitySystemCharacter owner)
        {
            var spec = new DashAbilitySpec(this, owner);
            return spec;
        }
    }

    public class DashAbilitySpec : AbstractAbilitySpecEx
    {

        private DashAbility ability;
        private PlatformerCharacter character;
        private PlatformerMovement2D movement;

        private int animIdIsDashing;
        private Vector2 direction;
        
        public DashAbilitySpec(AbstractAbilityScriptableObject ability, AbilitySystemCharacter owner) : base(ability, owner)
        {
            this.ability = ability as DashAbility;
            Debug.Assert(this.ability != null);
            animIdIsDashing = Animator.StringToHash(this.ability.IsDashingAnimParam);
            character = owner as PlatformerCharacter;
            movement = owner.GetComponent<PlatformerMovement2D>();
        }

        public override void CancelAbility()
        {
        }

        public override bool CheckGameplayTags()
        {
            return true;
        }

        protected override IEnumerator ActivateAbility()
        {
            // Visual effects
            var position = character.Cosmetic.transform.position;
            var rotation = character.transform.rotation;
            // Flash
            var flashGo = GameObjectPooler.Spawn(ability.DashFlashEffectPrefab, position, rotation);
            var flash = flashGo.GetComponent<DashFlash>();
            flash.Play(direction);

            // Impact Wave
            var waveGo = GameObjectPooler.Spawn(ability.DashWaveEffectPrefab, position, rotation);
            var wave = waveGo.GetComponent<ImpactWave>();
            wave.Play();
            
            // Spark
            var sparkGo = GameObjectPooler.Spawn(ability.DashSparkEffectPrefab, position, rotation);
            sparkGo.transform.parent = character.Cosmetic.transform;
            sparkGo.transform.forward = direction;
            var sparkParticle = sparkGo.GetComponent<ParticleSystem>();
            var main = sparkParticle.main;
            main.duration = ability.DashDuration;
            sparkParticle.Play();
            
            // Shadow
            var shadowPrefab = character.Orientation == CharacterOrientation.Left ? ability.DashShadowEffectPrefabLeft : ability.DashShadowEffectPrefabRight;
            var shadowGo = GameObjectPooler.Spawn(shadowPrefab, position, rotation);
            shadowGo.transform.parent = character.Cosmetic.transform;

            // Camera shake
            CameraShaker.Instance.Shake(-direction * .3f);

            
            // Movement
            movement.Rigidbody.velocity = Vector2.zero;
            
            var duration = ability.DashDuration;
            while (duration > 0)
            {
                movement.GroundedCheck();
                movement.TargetVelocity = direction * ability.DashSpeed;
                movement.Rigidbody.velocity = movement.TargetVelocity;
                
                duration -= Time.deltaTime;
                yield return null;
            }

            movement.TargetVelocity.y *= .6f;
            movement.Rigidbody.velocity = movement.TargetVelocity;
            
            
            sparkGo.transform.parent = null;
            shadowGo.transform.parent = null;
        }

        public void SetDirection(Vector2 dir)
        {
            if (isActive) return;
            direction = dir;
            if (direction == Vector2.zero)
                direction = character.Orientation == CharacterOrientation.Left ? Vector2.left : Vector2.right;
        }
        
    }
}