using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    [SerializeField] GameObject loadingScreen, Board;

    Transform[,] GameBoard = new Transform[9, 9];

    private void Awake()
    {
        loadingScreen.SetActive(true);
        StartCoroutine("ButtonSetup");
    }

    IEnumerator ButtonSetup()
    {
        yield return null;

        Transform Childgrid, Cell;

        for (int i = 0; i < Board.transform.childCount; i++)
        {
            Childgrid = Board.transform.GetChild(i);

            for (int j = 0; j < Childgrid.childCount; j++)
            {
                Cell = Childgrid.GetChild(j);

                GameBoard[i, j] = Cell;

                for (int k = 0; k < Cell.childCount; k++)
                {
                    int row = i, column = j, number = k;

                    Transform cellPosibility = Cell.GetChild(k);

                    cellPosibility.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        cellClicked(row, column, number, cellPosibility);
                    });
                }
                yield return null;
            }
            yield return null;
        }
        yield return null;

        loadingScreen.SetActive(false);
    }

    void cellClicked(int i, int j, int k, Transform cell)
    {
        bool iscollapsed = cell.parent.GetComponent<CellCollapser>().isCollapsed;

        if (iscollapsed)
        {
            removefromRowColumnGrid(i, j, k);
            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    CellCollapser obj = GameBoard[row, column].GetComponent<CellCollapser>();
                    if (obj.isCollapsed)
                        continue;
                    if (obj.cells.Count == 1)
                        obj.cells.First().GetComponent<Button>().onClick.Invoke();
                }
            }
        }
        else
        {
            addtoRowColumnGrid(i, j, k);
        }
    }

    int findRow(int N, int K)
    {
        int rem = N % K;

        if (rem == 0)
            return N;
        else
            return N - rem;
    }

    void removefromRowColumnGrid(int i, int j, int k)
    {
        for (int row = 0; row < 9; row++)
        {
            if (row == j)
                continue;
            CellCollapser obj = GameBoard[i, row].GetComponent<CellCollapser>();
            if (obj == null) continue;
            obj.RemoveFromCell((k + 1).ToString());
        }

        int rowStart = findRow(i, 3);
        for (int row = rowStart; row < rowStart + 3; row++)
        {
            if (row == i)
                continue;
            int actrowStart = findRow(j, 3);
            for (int actrow = actrowStart; actrow < actrowStart + 3; actrow++)
            {
                CellCollapser obj = GameBoard[row, actrow].GetComponent<CellCollapser>();
                if (obj == null) continue;
                obj.RemoveFromCell((k + 1).ToString());
            }
        }

        for (int column = i % 3; column < 9; column += 3)
        {
            if (column == i)
                continue;
            for (int actColumn = j % 3; actColumn < 9; actColumn += 3)
            {
                CellCollapser obj = GameBoard[column, actColumn].GetComponent<CellCollapser>();
                if (obj == null) continue;
                obj.RemoveFromCell((k + 1).ToString());
            }
        }
    }

    void addtoRowColumnGrid(int i, int j, int k)
    {
        for (int row = 0; row < 9; row++)
        {
            if (row == j)
                continue;
            CellCollapser obj = GameBoard[i, row].GetComponent<CellCollapser>();
            if (obj == null) continue;
            obj.AddToCell((k + 1).ToString());
        }

        int rowStart = findRow(i, 3);
        for (int row = rowStart; row < rowStart + 3; row++)
        {
            if (row == i)
                continue;
            int actrowStart = findRow(j, 3);
            for (int actrow = actrowStart; actrow < actrowStart + 3; actrow++)
            {
                CellCollapser obj = GameBoard[row, actrow].GetComponent<CellCollapser>();
                if (obj == null) continue;
                obj.AddToCell((k + 1).ToString());
            }
        }

        for (int column = i % 3; column < 9; column += 3)
        {
            if (column == i)
                continue;
            for (int actColumn = j % 3; actColumn < 9; actColumn += 3)
            {
                CellCollapser obj = GameBoard[column, actColumn].GetComponent<CellCollapser>();
                if (obj == null) continue;
                obj.AddToCell((k + 1).ToString());
            }
        }

        for (int row = 0; row < 9; row++)
        {
            for (int column = 0; column < 9; column++)
            {
                CellCollapser obj = GameBoard[row, column].GetComponent<CellCollapser>();
                if (obj.isCollapsed)
                {
                    if (obj.collapsingValue == (k + 1).ToString())
                    {
                        removefromRowColumnGrid(row, column, k);
                    }
                }
            }
        }
    }
}
