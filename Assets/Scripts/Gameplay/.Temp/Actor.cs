using UnityEngine;
using System;

namespace Xi.Gameplay.Character
{
    [DisallowMultipleComponent()]
    public class Actor : MonoBehaviour
    {
        public ActorType type;
        public string Name;

        //public ActorManager ActorManager { get; set; }
        public CharacterManager characterManager { get; set; }
        public IDamageable HealthSystem { get; set; }
        public Action<Actor, IDamageableGroup> OnKill;

        //public Inventory Inventory { get; private set; }

        /// <summary>
        /// returns true if the shooter has any component with the interface ICharacterController implemented
        /// </summary>
        public bool isLocallyMine
        {
            get
            {
                //return characterManager?.character != null;
                return true;
            }
        }

        private void Awake()
        {
            //ActorManager = GetComponentInParent<ActorManager>();
            //Inventory = transform.SearchFor<Inventory>();
            characterManager = GetComponent<CharacterManager>();
            HealthSystem = GetComponent<IDamageable>();
        }

        private void Start()
        {
            OnKill = ConfirmKill;
            //if (ActorManager) ActorManager.actor = this;

            //if (UIManager.Instance && isLocallyMine)
            //{
            //    UIManager.Instance?.HealthDisplay?.UpdateHealthNoLerp(HealthSystem.GetHealth());
            //    UIManager.Instance.HealthDisplay.slider.maxValue = HealthSystem.GetHealth();
            //    UIManager.Instance.HealthDisplay.backgroundSlider.maxValue = HealthSystem.GetHealth();
            //    UIManager.Instance.HealthDisplay.actorNameText.text = Name;
            //}
        }

        private void Update()
        {
            //if (UIManager.Instance && characterManager != null)
                //UIManager.Instance.HealthDisplay.UpdateHealth(HealthSystem.GetHealth());
        }

        public void ConfirmKill(Actor victim, IDamageableGroup group)
        {
            //if(ActorManager)
            //ActorManager.kills++;

            if (isLocallyMine)
            {
                //UIManager.Instance?.KillFeed?.Show(this, victim.Name, group.GetBone() == HumanBodyBones.Head ? true : false);
                //UIManager.Instance?.Hitmarker?.Enable(true, true, UIManager.Instance.Hitmarker.maxSize);
            }
        }

        public void ConfirmDeath()
        {
            //if(ActorManager)
            //ActorManager.deaths++;
        }
    }

    public enum ActorType
    {
        Human,
        Bot
    }

    public interface IDamageableGroup
    {

    }
    public interface IDamageable
    {

    }
}