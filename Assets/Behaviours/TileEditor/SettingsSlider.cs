using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Text readout;


    public int GetValue()
    {
        int val = Mathf.FloorToInt(slider.value);
        readout.text = val.ToString();

        return val;
    }


    void Awake()
    {
        GetValue();
    }

}
