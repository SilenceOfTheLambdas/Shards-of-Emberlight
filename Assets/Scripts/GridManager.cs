using Characters;
using Turn_based_Combat;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridManager : MonoBehaviour
{
    private GridCell[,] _gridCellList;
    private Transform _gridCellSpawnPointOrigin;
    private Vector3 _gridCellGraphicSize;

    [SerializeField] private Transform gridMapPlane;
    [SerializeField] private GameObject gridCellPrefab;
    [SerializeField] private Transform gridCellParentTransform;
    public LayerMask gridCellMask;
    
    /// <summary>
    /// A list of enemy characters in this combat area.
    /// </summary>
    public Character[] enemyCharacters;

    private void Awake()
    {
        // Work out the size of the plane / size of the grid cells
        var planeRenderer = gridMapPlane.GetComponent<Renderer>();
        Vector3 planeSize;
        if (planeRenderer != null)
        {
            planeSize = planeRenderer.bounds.size;
        }
        else planeSize = Vector3.zero;

        // Get the size of the grid cell
        Renderer gridCellGraphicRenderer = gridCellPrefab.GetComponentInChildren<Renderer>();
        if (gridCellGraphicRenderer != null)
        {
            _gridCellGraphicSize = gridCellGraphicRenderer.bounds.size;
        }
        else
        {
            Debug.LogError("Grid cell prefab does not have a valid renderer. Please assign a renderer to the prefab.");
            return;
        }

        // After getting gridCellGraphicSize
        float cellSize = Mathf.Max(_gridCellGraphicSize.x, _gridCellGraphicSize.z);
        _gridCellGraphicSize = new Vector3(cellSize, 0, cellSize);

        _gridCellList = new GridCell[(int)planeSize.x / (int)_gridCellGraphicSize.x, (int)planeSize.z / (int)_gridCellGraphicSize.z];

        GenerateGridMap();
    }

    private void GenerateGridMap()
    {
        // Spawn the grid cells
        for (var x = 0; x < _gridCellList.GetLength(0); x++)
        {
            for (var z = 0; z < _gridCellList.GetLength(1); z++)
            {
                // Calculate the world position for the cell
                Vector3 cellWorldPosition = new Vector3(x * _gridCellGraphicSize.x, 0.2f, z * _gridCellGraphicSize.z) + transform.position;
                
                // NOTE: *transform* gameObject scale needs to be the same for both x and z
                GridCell gridCell = Instantiate(gridCellPrefab, cellWorldPosition, Quaternion.identity, gridCellParentTransform).GetComponentInChildren<GridCell>();
                
                // Set the cell's properties
                gridCell.GridCellPosition = new Vector2Int(x, z);
                gridCell.CellWorldPosition = cellWorldPosition;
                gridCell.CellIsOccupiedBy = GridCell.OccupiedBy.None;
                // Store the cell in the list
                _gridCellList[x, z] = gridCell;
            }
        }
    }

    public GridCell GetGridCellPlayerClickedOn()
    {
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Ray ray = TbcController.Instance.playerCamera.ScreenPointToRay(screenPosition);
        Physics.Raycast(ray, out RaycastHit hit, 100f, gridCellMask);
        
        if (hit.collider.GetComponent<GridCell>() != null)
        {
            return hit.collider.GetComponent<GridCell>();
        }
        Debug.LogError("No grid cell was clicked on");
        return null;
    }
}
