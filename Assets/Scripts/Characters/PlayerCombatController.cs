using System;
using Turn_based_Combat;
using UnityEngine;

namespace Characters
{
    /// <summary>
    /// The combat system for controllable characters. This will store and manage the player's stats like skills,
    /// health
    /// </summary>
    [RequireComponent(typeof(CharacterCoreAttributes), typeof(Character))]
    public class PlayerCombatController : MonoBehaviour
    {
        private Character _character;
        public event Action<GameObject, GameObject> OnEnterCombatState;
        public event Action<GameObject, GameObject> OnExitCombatState;

        private void Start()
        {
            _character = GetComponent<Character>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Combat Area"))
            {
                TbcController.Instance.isPlayerInCombat = true;
                OnEnterCombatState?.Invoke(gameObject, other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Combat Area"))
            {
                TbcController.Instance.isPlayerInCombat = false;
                OnExitCombatState?.Invoke(gameObject, other.gameObject);
            }
        }
    }
}
