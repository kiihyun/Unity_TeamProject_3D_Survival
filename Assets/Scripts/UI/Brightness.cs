using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class Brightness : MonoBehaviour
{
    public Slider birghtnessSlider;

    public PostProcessProfile brightness;
    public PostProcessLayer layer;

    AutoExposure exposure;

    // Start is called before the first frame update
    void Start()
    {
        brightness.TryGetSettings(out exposure);
        AdjustBrightness(birghtnessSlider.value);
    }

    public void AdjustBrightness(float value)
    {
        if (exposure != null)
        {
            if(value != 0)
            {
                exposure.keyValue.value = value;
            }
            else
            {
                exposure.keyValue.value = 0.01f; // Prevents exposure from being set to zero
            }
        }
        else
        {
            Debug.LogWarning("AutoExposure settings not found in Brightness script.");
        }
    }
}
