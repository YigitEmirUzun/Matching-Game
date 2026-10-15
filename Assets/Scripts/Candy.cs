using UnityEngine;

public class Candy : MonoBehaviour
{
    public enum CandyType
    {
        Red,
        Blue,
        Green,
        Yellow,
        Pink
    }

    public CandyType type;
    private BoardManager boardManager;

    public int row;
    public int col;

}

