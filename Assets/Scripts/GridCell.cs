using UnityEngine;

/// <summary>
/// Represents a single cell in a grid, including its position and occupancy state.
/// </summary>
/// <remarks>A <see cref="GridCell"/> contains information about its position in both grid coordinates and world
/// space, as well as its current occupancy status, which can indicate whether it is occupied by a player character, an
/// enemy character, an obstacle, or nothing.</remarks>
public class GridCell : MonoBehaviour
{
    public enum OccupiedBy
    {
        /// Is occupied by nothing
        None,
        // Is occupied by a player character
        PlayerCharacter,
        // Is occupied by an enemy character
        EnemyCharacter,
        // Is occupied by an obstacle
        Obstacle
    }

    public Vector2Int GridCellPosition;
    public Vector3 CellWorldPosition;
    public OccupiedBy CellIsOccupiedBy;

    private void Start()
    {
        // Check to see if this Grid Cell overlaps with any Obstacles
        Collider col = GetComponent<Collider>();
        Vector3 center = col.bounds.center;
        Vector3 halfExtents = col.bounds.extents * 0.5f;
        
        // Check for overlapping coliders
        Collider[] overlaps = new Collider[10]; 
        var size = Physics.OverlapBoxNonAlloc(center, halfExtents, overlaps, Quaternion.identity);

        for (int i = 0; i < size; i++)
        {
            if (overlaps[i] != col)
            {
                // Found an overlap
                if (overlaps[i].gameObject.CompareTag("Obstacle"))
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
