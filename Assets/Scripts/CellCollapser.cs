using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CellCollapser : MonoBehaviour
{
    public List<Transform> cells = new List<Transform>();
    public bool isCollapsed = false;
    public string collapsingValue;
    Transform collapsedChild = null;
    GridLayoutGroup gridLayout;

    private void Awake()
    {
        gridLayout = GetComponent<GridLayoutGroup>();

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform cell = transform.GetChild(i);

            Button button = cell.GetComponent<Button>();
            TextMeshProUGUI text = cell.GetComponentInChildren<TextMeshProUGUI>();

            button.onClick.AddListener(() => CellCollapse(text.text));
            cells.Add(cell);
        }
    }

    public void CellCollapse(string text)
    {
        collapsingValue = isCollapsed ? "" : text;
        int k = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i < cells.Count)
                cells[i].gameObject.SetActive(isCollapsed);

            Transform cell = transform.GetChild(i);

            bool isTargetCell = cell.GetChild(0).GetComponent<TextMeshProUGUI>().text == text;

            if (isTargetCell)
            {
                collapsedChild = cell;
                if (!cells.Remove(cell))
                {
                    k = i - 1;
                    cells.Add(cell);
                }

            }
        }

        for (int i = k < 0 ? 0 : k; i < cells.Count; i++)
        {
            cells[i].gameObject.SetActive(isCollapsed);
        }

        collapsedChild.gameObject.SetActive(true);

        if (collapsedChild != null)
        {
            gridLayout.enabled = isCollapsed;

            RectTransform rectrans = collapsedChild.GetComponent<RectTransform>();
            ResetRectTransform(rectrans);
        }

        isCollapsed = !isCollapsed;
    }

    public void RemoveFromCell(string number)
    {
        Transform cell = cells.Find(c => c.GetChild(0).GetComponent<TextMeshProUGUI>().text == number);

        if (cell != null)
        {
            cell.gameObject.SetActive(false);
            cells.Remove(cell);
        }
    }

    public void AddToCell(string number)
    {
        Transform cell = GetCellByNumber(number);

        if (cell != null && !cells.Contains(cell))
        {
            cells.Add(cell);
        }

        if (!isCollapsed)
        {
            cell.gameObject.SetActive(true);
        }
    }

    private Transform GetCellByNumber(string number)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform cell = transform.GetChild(i);

            if (cell.GetChild(0).GetComponent<TextMeshProUGUI>().text == number)
            {
                return cell;
            }
        }
        return null;
    }

    private void ResetRectTransform(RectTransform rectTrans)
    {
        rectTrans.anchorMin = Vector2.zero;
        rectTrans.anchorMax = Vector2.one;
        rectTrans.pivot = new Vector2(0.5f, 0.5f);
        rectTrans.sizeDelta = Vector2.zero;
        rectTrans.anchoredPosition = Vector2.zero;
    }
}
