using UnityEngine;

namespace Characters
{
    /// <summary>
    /// The core attributes for this character, this can either be a player character or an enemy NPC.
    /// All characters in the game will derive their combat attributes from this.
    /// See <see cref="PlayerCombatController"/>
    /// </summary>
    public class CharacterCoreAttributes : MonoBehaviour
    {
        /// <summary>
        /// Affects the character's physical attributes.
        /// </summary>
        /// <remarks>
        /// Combat attributes include:
        /// <c>attackPower</c>,
        /// <c>maxCarryWeight</c>
        /// </remarks>
        [Header("This characters core attributes")]
        public int strength = 1;
        
        /// <summary>
        /// Affects the character's maximum health points, stamina, and physical defensive attributes.
        /// </summary>
        /// <remarks>
        /// Combat attributes include:
        /// <c>playerMaxHealthPoints</c>,
        /// <c>maxPlayerStaminaPoints</c>,
        /// <c>defensePower</c>
        /// </remarks>
        public int constitution = 1;
        
        /// <summary>
        /// Affects the character's magic effectiveness (magic power and magic points).
        /// </summary>
        /// <remarks>
        /// Combat attributes include:
        /// <c>maxPlayerMagicPoints</c>,
        /// <c>magicPower</c>,
        /// </remarks>
        public int intelligence = 1;
        
        /// <summary>
        /// Affects the character's magic defences.
        /// </summary>
        /// <remarks>
        /// Combat attributes include:
        /// <c>magicDefensePower</c>,
        /// </remarks>
        public int wisdom = 1;
        
        /// <summary>
        /// Affects the character's turn speed along with the chance of dodging an attack.
        /// </summary>
        /// <remarks>
        /// Combat attributes include:
        /// <c>initiative</c>
        /// </remarks>
        public int dexterity = 1;
    }
}
