using TMPro;
using UnityEngine;
public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text fpsText;

    [SerializeField] float pollingTime = 1f;
    private float time;
    private int frameCount;

    private void Update()
    {
        time += Time.unscaledDeltaTime;
        frameCount++;

        if (time >= pollingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            fpsText.text = frameRate + "";

            time -= pollingTime;
            frameCount = 0;
        }
    }
}
