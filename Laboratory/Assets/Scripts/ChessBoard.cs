using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ChessBoard : MonoBehaviour
{
    public readonly int widthCount = 8; // 가로의 칸 수
    public readonly int heightCount = 8; // 세로의 칸 수
    private readonly float squareWidth = 0.06f; // 각 칸의 가로 길이
    private readonly float squareHeight = 0.06f; // 각 칸의 세로 길이
    private readonly float boardHeight = 0.01f; // 보드의 높이

    [SerializeField] private Vector3 triggerOffset = Vector3.zero;
    [SerializeField] private Vector3 pieceOffset = Vector3.zero;
    [SerializeField] private GameObject squareTrigger;
    
    private BoxCollider[,] squareColliders;
    
    private void Start()
    {
        squareColliders = new BoxCollider[widthCount, heightCount];
        
        for (int x = 0; x < widthCount; x++)
        {
            for (int z = 0; z < heightCount; z++)
            {
                GameObject spawnedSquareTrigger = Instantiate(squareTrigger, transform.position, Quaternion.identity);
                spawnedSquareTrigger.transform.parent = transform;
                spawnedSquareTrigger.transform.localPosition
                    = (2 * x + 1 - widthCount) / 2.0f * squareWidth * Vector3.right
                      + (2 * z + 1 - heightCount) / 2.0f * squareHeight * Vector3.forward
                      + triggerOffset;
                spawnedSquareTrigger.transform.localRotation = Quaternion.identity;
                spawnedSquareTrigger.transform.localScale = Vector3.one;

                BoxCollider collider = spawnedSquareTrigger.GetComponent<BoxCollider>();
                collider.size = new Vector3(squareWidth, boardHeight, squareHeight);
                squareColliders[x, z] = collider;
            }
        }
    }

    public Vector3 GetSquareWorldPosition(int x, int z)
    {
        return squareColliders[x, z].transform.position + pieceOffset;
    }

    public Vector2Int GetSquarePositionFromCollider(Collider collider)
    {
        for (int x = 0; x < widthCount; x++)
        {
            for (int z = 0; z < heightCount; z++)
            {
                if (collider == squareColliders[x, z])
                {
                    return new Vector2Int(x, z);
                }
            }
        }
        
        return new Vector2Int(-1, -1);
    }
}
