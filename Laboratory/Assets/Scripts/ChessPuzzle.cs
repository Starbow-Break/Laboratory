using UnityEngine;

public class ChessPuzzle : MonoBehaviour, IInteractable
{   
    [Header("Board")]
    [SerializeField] private ChessBoard chessBoard; // 사용하는 체스 보드

    [Header("Pieces")] 
    [SerializeField] private GameObject pawn;
    [SerializeField] private GameObject rook;
    [SerializeField] private GameObject knight;
    [SerializeField] private GameObject bishop;
    [SerializeField] private GameObject king;
    [SerializeField] private GameObject queen;
    
    [Header("Materials")]
    [SerializeField] private Material whiteMaterial;
    [SerializeField] private Material blackMaterial;
    
    [Tooltip("체스판의 초기 상태를 나타내는 문자열을 입력합니다. 왼쪽 아래부터 오른쪽으로 말이 채워집니다.\n" + 
             "폰은 p, 룩은 r, 나이트는 n, 비숍은 b, 킹은 k, 퀸은 q이며 흰색 기물은 대문자, 검정색 기물은 소문자로 적어주세요.")]
    [SerializeField] private string initializeState;
    
    private GameObject[,] pieces; // 각 체스 칸테 놓여진 기물
    private Vector2Int selectedBoardPosition; // 선택된 위치
    private PlayerController interactingPlayer;
    private BoxCollider boxCollider;

    void Start()
    {
        pieces = new GameObject[8, 8];
        selectedBoardPosition = new Vector2Int(-1, -1);
        boxCollider = GetComponent<BoxCollider>();
        CreatePuzzle();
    }
    
    void Update()
    {
        if (interactingPlayer == null)
        {
            return;
        }
        
        // 마우스 좌클릭시 커서가 오브젝트를 가리키고 있으면 해당 오브젝트 선택
        if (Input.GetMouseButtonDown(0))
        {   
            // 스크린에서 마우스 클릭 위치를 통과하는 광선 생성
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            
            // 광선이 오브젝트를 감지하면 해당 오브젝트 선택을 시도
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector2Int select = chessBoard.GetSquarePositionFromCollider(hit.collider);
                Debug.Log(select);
                if(select.x == -1 && select.y == -1)
                {
                    selectedBoardPosition = new Vector2Int(-1, -1);
                }
                else
                {
                    if (selectedBoardPosition.x == -1 && selectedBoardPosition.y == -1)
                    {
                        if (pieces[select.x, select.y] != null)
                        {
                            selectedBoardPosition = select;
                        }
                    }
                    else
                    {
                        if (pieces[select.x, select.y] == null)
                        {
                            GameObject piece = pieces[selectedBoardPosition.x, selectedBoardPosition.y];
                            piece.transform.position = chessBoard.GetSquareWorldPosition(select.x, select.y);
                            pieces[select.x, select.y] = piece;
                            pieces[selectedBoardPosition.x, selectedBoardPosition.y] = null;
                            selectedBoardPosition = new Vector2Int(-1, -1);
                        }
                        else
                        {
                            selectedBoardPosition = select;
                        }
                    }
                }
            }
        }
        // 우클릭 시 상호작용 해제
        else if (Input.GetMouseButtonDown(1)) 
        {
            EndInteraction(interactingPlayer);
        }
    }
    
    // player랑 상호작용 시작
    public void StartInteraction(PlayerController player)
    {
        CameraSwitcher.instance.SwitchCamera("Interaction Cam");
        CursorLocker.instance.UnlockCursor();
        player.enabled = false;
        interactingPlayer = player;
        boxCollider.enabled = false;
    }
    
    // player랑 상호작용 끝
    public void EndInteraction(PlayerController player)
    {
        CameraSwitcher.instance.SwitchCamera("Player Sight Cam");
        CursorLocker.instance.LockCursor();
        player.enabled = true;
        interactingPlayer = null;
        boxCollider.enabled = true;
    }
    
    // initializeState에 맞춰서 기물 배치
    private void CreatePuzzle()
    {
        for (int h = 0; h < chessBoard.heightCount; h++)
        {
            for (int w = 0; w < chessBoard.widthCount; w++)
            {
                int index = h * chessBoard.widthCount + w;
                if (initializeState.Length <= index)
                {
                    continue;
                }

                SpawnPiece(initializeState[index], w, h);
            }
        }
    }
    
    // 기물 스폰
    private void SpawnPiece(char pieceCode, int x, int y)
    {
        Vector3 spawnPosition = chessBoard.GetSquareWorldPosition(x, y);
        GameObject spawnedPiece = null;
        
        switch (pieceCode)
        {
            case 'P':
            case 'p':
                spawnedPiece = Instantiate(pawn, spawnPosition, chessBoard.transform.rotation);
                break;
            case 'R':
            case 'r':
                spawnedPiece = Instantiate(rook, spawnPosition, chessBoard.transform.rotation);
                break;
            case 'N':
            case 'n':
                spawnedPiece = Instantiate(knight, spawnPosition, chessBoard.transform.rotation);
                break;
            case 'B':
            case 'b':
                spawnedPiece = Instantiate(bishop, spawnPosition, chessBoard.transform.rotation);
                break;
            case 'Q':
            case 'q':
                spawnedPiece = Instantiate(queen, spawnPosition, chessBoard.transform.rotation);
                break;
            case 'K':
            case 'k':
                spawnedPiece = Instantiate(king, spawnPosition, chessBoard.transform.rotation);
                break;
        }
        
        if (spawnedPiece != null)
        {
            Renderer renderer = spawnedPiece.GetComponent<Renderer>();
            if ('A' <= pieceCode && pieceCode <= 'Z')
            {
                renderer.material = whiteMaterial;
            }
            else
            {
                renderer.material = blackMaterial;
            }
        }
        
        pieces[x, y] = spawnedPiece;
    }
}
