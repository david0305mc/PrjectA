using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public static class TestUtil
{
    


    public static Vector2 WorldToCanvasPoint(this RectTransform This, Vector3 WorldPosition, [NotNull] Camera WorldCamera, out bool Success, Camera CanvasCamera = null)
    {
        Success = RectTransformUtility.ScreenPointToLocalPointInRectangle(This, WorldCamera.WorldToScreenPoint(WorldPosition), CanvasCamera, out var CanvasPoint);

        return CanvasPoint;
    }
}
