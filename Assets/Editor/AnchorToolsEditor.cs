using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class AnchorToolsEditor : EditorWindow
{

    [MenuItem("Custom Tools/Anchor Tools/AnchorToCorner &1", priority = 3)]
    public static void AnchorToCorner()
    {

        Undo.RecordObjects(Selection.gameObjects.Select(
            (go) =>
            {
                return go.GetComponent<RectTransform>();
            }
            ).ToArray(), "Anchoring To Corner ");
        foreach (var go in Selection.gameObjects)
        {
            UpdateAnchors(go, AnchorType.AnchorsToCorners);
        }
        EditorSceneManager.MarkAllScenesDirty();
    }

    [MenuItem("Custom Tools/Anchor Tools/AnchorToPivot &3", priority = 4)]
    public static void AnchorToPivot()
    {
        Undo.RecordObjects(Selection.gameObjects.Select(
            (go) =>
            {
                return go.GetComponent<RectTransform>();
            }
            ).ToArray(), "Anchoring To Pivot ");
        foreach (var go in Selection.gameObjects)
        {
            UpdateAnchors(go, AnchorType.AnchorsToPivot);
        }
        EditorSceneManager.MarkAllScenesDirty();
    }


    static public Rect anchorRect;
    static public Vector2 anchorVector;
    static private Rect anchorRectOld;
    static private Vector2 anchorVectorOld;
    static private RectTransform currentRectTransform;
    static private RectTransform parentRectTransform;
    static private Vector2 pivotOld;
    static private Vector2 offsetMinOld;
    static private Vector2 offsetMaxOld;
    static private AnchorType previousType;

    static public void UpdateAnchors(GameObject go, AnchorType type)
    {
        if (TryToGetRectTransform(go) && ShouldStick(type))
        {
            Stick(type);
        }
    }

    static private bool ShouldStick(AnchorType type)
    {
        return (
            currentRectTransform.offsetMin != offsetMinOld ||
            currentRectTransform.offsetMax != offsetMaxOld ||
            currentRectTransform.pivot != pivotOld ||
            anchorVector != anchorVectorOld ||
            anchorRect != anchorRectOld ||
            previousType != type
            );
    }

    static private void Stick(AnchorType type)
    {
        CalculateCurrentWH();
        CalculateCurrentXY();
        pivotOld = currentRectTransform.pivot;
        anchorVectorOld = anchorVector;
        switch (type)
        {
            case AnchorType.AnchorsToCorners:
                AnchorsToCorners();
                break;
            case AnchorType.AnchorsToPivot:
                AnchorsToPivot();
                break;
        }
        anchorRectOld = anchorRect;
        UnityEditor.EditorUtility.SetDirty(currentRectTransform.gameObject);
        previousType = type;
    }
    
    static private bool TryToGetRectTransform(GameObject go)
    {
        if (go != null)
        {
            currentRectTransform = go.GetComponent<RectTransform>();
            if (currentRectTransform != null && currentRectTransform.parent != null)
            {
                parentRectTransform = currentRectTransform.parent.gameObject.GetComponent<RectTransform>();
                return true;
            }
        }
        return false;
    }

    static private void CalculateCurrentXY()
    {
        float pivotX = anchorRect.width * currentRectTransform.pivot.x;
        float pivotY = anchorRect.height * (1 - currentRectTransform.pivot.y);
        Vector2 newXY = new Vector2(currentRectTransform.anchorMin.x * parentRectTransform.rect.width + currentRectTransform.offsetMin.x + pivotX - parentRectTransform.rect.width * anchorVector.x,
                                  -(1 - currentRectTransform.anchorMax.y) * parentRectTransform.rect.height + currentRectTransform.offsetMax.y - pivotY + parentRectTransform.rect.height * (1 - anchorVector.y));
        anchorRect.x = newXY.x;
        anchorRect.y = newXY.y;
        anchorRectOld = anchorRect;
    }

    static private void CalculateCurrentWH()
    {
        anchorRect.width = currentRectTransform.rect.width;
        anchorRect.height = currentRectTransform.rect.height;
        anchorRectOld = anchorRect;
    }

    static private void AnchorsToCorners()
    {
        float pivotX = anchorRect.width * currentRectTransform.pivot.x;
        float pivotY = anchorRect.height * (1 - currentRectTransform.pivot.y);
        currentRectTransform.anchorMin = new Vector2(0f, 1f);
        currentRectTransform.anchorMax = new Vector2(0f, 1f);
        currentRectTransform.offsetMin = new Vector2(anchorRect.x / currentRectTransform.localScale.x, anchorRect.y / currentRectTransform.localScale.y - anchorRect.height);
        currentRectTransform.offsetMax = new Vector2(anchorRect.x / currentRectTransform.localScale.x + anchorRect.width, anchorRect.y / currentRectTransform.localScale.y);
        currentRectTransform.anchorMin = new Vector2(currentRectTransform.anchorMin.x + anchorVector.x + (currentRectTransform.offsetMin.x - pivotX) / parentRectTransform.rect.width * currentRectTransform.localScale.x,
                                                 currentRectTransform.anchorMin.y - (1 - anchorVector.y) + (currentRectTransform.offsetMin.y + pivotY) / parentRectTransform.rect.height * currentRectTransform.localScale.y);
        currentRectTransform.anchorMax = new Vector2(currentRectTransform.anchorMax.x + anchorVector.x + (currentRectTransform.offsetMax.x - pivotX) / parentRectTransform.rect.width * currentRectTransform.localScale.x,
                                                 currentRectTransform.anchorMax.y - (1 - anchorVector.y) + (currentRectTransform.offsetMax.y + pivotY) / parentRectTransform.rect.height * currentRectTransform.localScale.y);
        currentRectTransform.offsetMin = new Vector2((0 - currentRectTransform.pivot.x) * anchorRect.width * (1 - currentRectTransform.localScale.x), (0 - currentRectTransform.pivot.y) * anchorRect.height * (1 - currentRectTransform.localScale.y));
        currentRectTransform.offsetMax = new Vector2((1 - currentRectTransform.pivot.x) * anchorRect.width * (1 - currentRectTransform.localScale.x), (1 - currentRectTransform.pivot.y) * anchorRect.height * (1 - currentRectTransform.localScale.y));

        offsetMinOld = currentRectTransform.offsetMin;
        offsetMaxOld = currentRectTransform.offsetMax;
    }

    static private void AnchorsToPivot()
    {
        currentRectTransform.anchorMin = new Vector2(anchorRect.x / parentRectTransform.rect.width, anchorRect.y / parentRectTransform.rect.height);
        currentRectTransform.anchorMax = new Vector2(anchorRect.x / parentRectTransform.rect.width, anchorRect.y / parentRectTransform.rect.height);
        currentRectTransform.offsetMin = new Vector2((0 - currentRectTransform.pivot.x) * anchorRect.width, (0 - currentRectTransform.pivot.y) * anchorRect.height);
        currentRectTransform.offsetMax = new Vector2((1 - currentRectTransform.pivot.x) * anchorRect.width, (1 - currentRectTransform.pivot.y) * anchorRect.height);
        offsetMinOld = currentRectTransform.offsetMin;
        offsetMaxOld = currentRectTransform.offsetMax;
    }

    public enum AnchorType
    {
        AnchorsToCorners,
        AnchorsToPivot
    }
}