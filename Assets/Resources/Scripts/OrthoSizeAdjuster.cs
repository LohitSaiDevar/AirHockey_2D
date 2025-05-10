using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AspectOrthoSetting
{
    [Tooltip("Width part of the aspect ratio (e.g., 16 for 16:9)")]
    public int aspectX = 16;

    [Tooltip("Height part of the aspect ratio (e.g., 9 for 16:9)")]
    public int aspectY = 9;

    [Tooltip("Orthographic size for this aspect ratio")]
    public float orthoSize = 5f;

    // Convenience property to calculate the float ratio
    public float AspectRatio => (float)aspectX / aspectY;
}

[RequireComponent(typeof(Camera))]
public class OrthoSizeAdjuster : MonoBehaviour
{
    [Tooltip("List of aspect ratio settings")]
    public List<AspectOrthoSetting> aspectSettings = new List<AspectOrthoSetting>();

    [Tooltip("Default orthographic size if no match is found")]
    public float defaultOrthoSize = 5f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        if (!cam.orthographic)
        {
            Debug.LogWarning("Camera is not orthographic. This script only works with orthographic cameras.");
            return;
        }

        SetOrthoSizeByAspect();
    }

    void SetOrthoSizeByAspect()
    {
        float currentAspect = (float)Screen.width / Screen.height;
        float closestDifference = Mathf.Infinity;
        float selectedOrthoSize = defaultOrthoSize;
        AspectOrthoSetting closestSetting = null;

        foreach (var setting in aspectSettings)
        {
            float settingAspect = setting.AspectRatio;
            float diff = Mathf.Abs(currentAspect - settingAspect);
            if (diff < closestDifference)
            {
                closestDifference = diff;
                selectedOrthoSize = setting.orthoSize;
                closestSetting = setting;
            }
        }

        cam.orthographicSize = selectedOrthoSize;

        Debug.Log($"Set ortho size to: {selectedOrthoSize} (Current aspect: {currentAspect})");
        if (closestSetting != null)
        {
            Debug.Log($"Set ortho size to: {selectedOrthoSize} (Current aspect: {closestSetting.aspectX}:{closestSetting.aspectY})");
        }
        else
        {
            Debug.Log($"Set ortho size to: {selectedOrthoSize} (No matching aspect found)");
        }
    }
}
