using UnityEngine;

// 체스 기물
public class ChessPiece: MonoBehaviour
{
    // 기물 종류
    public enum PieceType {
        King,
        Queen,
        Knight,
        Bishop,
        Pawn,
        Rook
    }
    
    // 기물 색
    public enum PieceColor {
        Black,
        White
    }
    
    PieceType pieceType;
    PieceColor pieceColor;
}
