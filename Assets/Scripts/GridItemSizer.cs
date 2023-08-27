using UnityEngine;  
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(GridLayoutGroup))]
public class GridItemSizer : MonoBehaviour
{
    private RectTransform rectTransform;
    private GridLayoutGroup gridLayoutGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
    }

    private void UpdateCellSize()
    {
        float size = rectTransform.rect.size.x / 3.1f;
        gridLayoutGroup.cellSize = Vector2.one * size;
    }

    private void OnGUI()
    {
        if(rectTransform == null)
            Awake();

        UpdateCellSize();
    }
}
