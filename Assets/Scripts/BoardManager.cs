using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;


public class BoardManager : MonoBehaviour
{
    public int width;
    public int height;
    
    public GameObject[,] grid;

    public List<GameObject> candys;

    public List<Vector2Int> matchedCandys;

    private void Awake()
    {
        grid = new GameObject[height, width];
        matchedCandys = new List<Vector2Int>();
    }

    private void Start()
    {
        CreateBoard();
        MatchControl();
        DestroyMatchedCandys();
        Gravity();
    }

    public void CreateBoard()
    {
        for (int row = 0;  row < height; row++)
        {
            for(int col = 0; col < width; col++)
            {
                GameObject candy = candys[Random.Range(0,candys.Count)];

                GameObject newCandy = Instantiate(candy, new Vector2(col, row * -1), Quaternion.identity);

                newCandy.GetComponent<Candy>().row = row;
                newCandy.GetComponent<Candy>().col = col;
                grid[row, col] = newCandy;
            }
        }
    }

    public void MatchControl()
    {
        matchedCandys.Clear();
        for (int row = 0; row < height; row++)
        {
            for(int col = 0; col < width; col++)
            {
                GameObject candy = grid[row, col];
                if(col == 0 || grid[row, col - 1].GetComponent<Candy>().type != candy.GetComponent<Candy>().type)
                {
                    int count = 1;

                    while(col + count < width && grid[row,col + count].GetComponent<Candy>().type == candy.GetComponent<Candy>().type)
                    {
                        count++;
                    }

                    if(count >= 3)
                    {
                        Debug.Log($"Yatay eþleþme var!, {row},{col} Renk : {candy.GetComponent<Candy>().type}");

                        for(int i = 0; i < count; i++)
                        {
                            if (!matchedCandys.Contains(new Vector2Int(row, col + i)))
                            {
                                matchedCandys.Add(new Vector2Int(row, col + i));
                            }
                        }
                    }
                }

                if (row == 0 || grid[row - 1, col].GetComponent<Candy>().type != candy.GetComponent<Candy>().type)
                {
                    int count = 1;

                    while (row + count < height && grid[row + count, col].GetComponent<Candy>().type == candy.GetComponent<Candy>().type)
                    {
                        count++;
                    }

                    if (count >= 3)
                    {
                        Debug.Log($"Dikey eþleþme var!, {row},{col} Renk : {candy.GetComponent<Candy>().type}");

                        for (int i = 0; i < count; i++)
                        {
                            if (!matchedCandys.Contains(new Vector2Int(row + i, col)))
                            {
                                matchedCandys.Add(new Vector2Int(row + i, col));
                            }
                        }
                    }
                }
            }
        }
    }

    public void DestroyMatchedCandys()
    {
        foreach(Vector2Int i in matchedCandys)
        {
            GameObject destroyObject = grid[i.x,i.y].gameObject;
            grid[i.x, i.y] = null;
            Destroy(destroyObject);  
        }

        matchedCandys.Clear();
    }

    public void Gravity()
    {
        for (int col = 0; col < width; col++)
        {
            for (int row = height - 1; row > 0; row--)
            {
                if (grid[row, col] == null)
                {
                    for (int aboveRow = row - 1; aboveRow >= 0; aboveRow--)
                    {
                        if (grid[aboveRow, col] != null)
                        {
                            GameObject aboveCandy = grid[aboveRow, col];

                            grid[row, col] = aboveCandy;
                            grid[aboveRow, col] = null;

                            Vector2.Lerp(aboveCandy.transform.position, new Vector2(col, row * -1), 3f * Time.deltaTime);

                            aboveCandy.transform.DOMove(new Vector3(col, row * -1 , 1f),0.2f).SetEase(Ease.OutQuad);

                            break;
                        }
                    }
                }
            }
        }
    }

}
