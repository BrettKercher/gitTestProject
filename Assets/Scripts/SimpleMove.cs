using UnityEditor;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    public CharacterController Controller;
    public float Speed = 13;

    // Update is called once per frame
    void Update()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        Controller.SimpleMove(new Vector3(h * Speed, 0, v * Speed));
    }

    private ushort[] Grid() {
        var board = new ushort[7, 7];
        var moves = Ring(board, 0);
        return moves;
    }

    private ushort[] Ring(ushort[,] board, ushort origin) {
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);
        
        // Convert linear index to 2D coordinates
        int originRow = origin / cols;
        int originCol = origin % cols;
        
        // Find the center of the board
        int centerRow = rows / 2;
        int centerCol = cols / 2;
        
        // Determine which ring the origin is in (distance from center using Chebyshev distance)
        int ringDistance = Mathf.Max(Mathf.Abs(originRow - centerRow), Mathf.Abs(originCol - centerCol));
        
        // Get all positions in this ring in clockwise order
        var result = new System.Collections.Generic.List<ushort>();
        
        // Define the bounds of the current ring
        int minRow = centerRow - ringDistance;
        int maxRow = centerRow + ringDistance;
        int minCol = centerCol - ringDistance;
        int maxCol = centerCol + ringDistance;
        
        // Collect ring positions in clockwise order: top → right → bottom → left
        var ringPositions = new System.Collections.Generic.List<ushort>();
        
        // Top edge (left to right)
        if (minRow >= 0 && minRow < rows) {
            for (int c = minCol; c <= maxCol; c++) {
                if (c >= 0 && c < cols) {
                    ringPositions.Add((ushort)(minRow * cols + c));
                }
            }
        }
        
        // Right edge (top to bottom, excluding top corner)
        if (maxCol >= 0 && maxCol < cols) {
            for (int r = minRow + 1; r <= maxRow; r++) {
                if (r >= 0 && r < rows) {
                    ringPositions.Add((ushort)(r * cols + maxCol));
                }
            }
        }
        
        // Bottom edge (right to left, excluding right corner)
        if (maxRow >= 0 && maxRow < rows && maxRow != minRow) {
            for (int c = maxCol - 1; c >= minCol; c--) {
                if (c >= 0 && c < cols) {
                    ringPositions.Add((ushort)(maxRow * cols + c));
                }
            }
        }
        
        // Left edge (bottom to top, excluding both corners)
        if (minCol >= 0 && minCol < cols && minCol != maxCol) {
            for (int r = maxRow - 1; r > minRow; r--) {
                if (r >= 0 && r < rows) {
                    ringPositions.Add((ushort)(r * cols + minCol));
                }
            }
        }
        
        // Find the origin in the ring and start from the next position
        int originIndex = ringPositions.IndexOf(origin);
        if (originIndex == -1) return new ushort[0];
        
        // Return positions starting from the next position after origin, going clockwise
        for (int i = 1; i < ringPositions.Count; i++) {
            int index = (originIndex + i) % ringPositions.Count;
            result.Add(ringPositions[index]);
        }
        
        return result.ToArray();
    }
}
