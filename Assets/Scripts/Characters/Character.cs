using System;
using System.Collections.Generic;
using Turn_based_Combat;
using UnityEngine;
using UnityEngine.Serialization;
using Action = Turn_based_Combat.Action;

namespace Characters
{
    public class Character : MonoBehaviour
    {
        /// <summary>
        /// How much damage the character can take.
        /// Affected by Constitution core attribute.
        /// </summary>
        [Header("Player Combat Attributes")]
        [SerializeField] private int characterMaxHealthPoints;

        /// <summary>
        /// Resource for casting spells or using powers.
        /// Affected by Intelligence core attribute.
        /// </summary>
        [SerializeField] private int characterMaxMagicPoints;

        /// <summary>
        /// Resource for physical actions, sprinting, skills.
        /// Affected by Constitution core attribute.
        /// </summary>
        [FormerlySerializedAs("CharacterMaxStaminaPoints")] public int characterMaxStaminaPoints;

        /// <summary>
        /// Base damage dealt by physical attacks, weapons or physical skills add to this attribute.
        /// Affected by Strength core attribute.
        /// </summary>
        [SerializeField] private int attackPower;

        /// <summary>
        /// Base damage or effectiveness of magical abilities, magic weapons and spells add to this attribute.
        /// Affected by Intelligence core Attribute.
        /// </summary>
        [SerializeField] private int magicPower;

        /// <summary>
        /// Reduces incoming physical damage, armour and some skills can add to this attribute.
        /// Affected by Constitution core attribute.
        /// </summary>
        [SerializeField] private int defensePower;

        /// <summary>
        /// Reduces incoming magic damage.
        /// Affected by Wisdom core attribute.
        /// </summary>
        [SerializeField] private int magicDefensePower;

        /// <summary>
        /// Determines turn order
        /// </summary>
        [SerializeField] public int initiative;

        public int currentCharacterHealth = 50;
        private int _currentCharacterMagicPoints;
        public int currentCharacterStaminaPoints = 20;

        /// <summary>
        /// A reference to the current grid cell the player is standing on, this can be null.
        /// </summary>
        public GridCell currentGridCellCharacterIsStandingOn;

        public event Action<Character> OnFollowerDeath;
        public event Action<Character> OnEnemyDeath;
        public event Action<Character> OnPlayerDeath;

        public List<Action> actionsAvailableToCharacter;

        private void Start()
        {
            currentCharacterHealth = characterMaxHealthPoints;
            _currentCharacterMagicPoints = characterMaxMagicPoints;
            currentCharacterStaminaPoints = characterMaxStaminaPoints;
        }

        private void Update()
        {
            // TODO: This should be called when the player takes damage, and NOT every frame.
            // if (currentCharacterHealth <= 0)
            // {
            //     if (gameObject.CompareTag("Player"))
            //         // The player has died (game over)
            //         OnPlayerDeath?.Invoke(this);
            //     if (gameObject.CompareTag("Follower"))
            //         // A follower has died
            //         OnFollowerDeath?.Invoke(this);
            //     if (gameObject.CompareTag("Enemy"))
            //         OnEnemyDeath?.Invoke(this);
            // }
        }

        /// <summary>
        /// Calculates the total amount of damage this character receives.
        /// </summary>
        /// <param name="damage">The intended amount of damage</param>
        /// <param name="damageType">The type of damage</param>
        public void CalculateDamage(int damage, TbcController.DamageType damageType)
        {
            // TODO: Create some magic formula to work-out damage negation based on character stats.
            TakeDamage(damage);
        }
        
        /// <summary>
        /// Do <c>x</c> amount of damage to this character.
        /// </summary>
        /// <remarks>
        /// This method will NOT take into account of any defence stats.
        /// </remarks>
        /// <param name="damage">x</param>
        private void TakeDamage(int damage)
        {
            currentCharacterHealth -= damage;
            if (currentCharacterHealth <= 0)
            {
                switch (gameObject.tag)
                {
                    case "Player":
                        OnPlayerDeath?.Invoke(this);
                        break;
                    case "Follower":
                        OnFollowerDeath?.Invoke(this);
                        break;
                    case "Enemy":
                        OnEnemyDeath?.Invoke(this);
                        break;
                    default: 
                        Debug.LogError("Tried to kill an untagged character: " + name);
                        break;
                }
            }
        }
        
        public void PayMagicCost(int magicCost)
        {
            _currentCharacterMagicPoints -= magicCost;
        }
        
        public void PayStaminaCost(int staminaCost)
        {
            currentCharacterStaminaPoints -= staminaCost;
        }
    }
}