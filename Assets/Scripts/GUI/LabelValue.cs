using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelValue : MonoBehaviour
{
    public Text labelComponent;
    public Text valueComponent;

    public void SetLabel(string label)
    {
        labelComponent.text = label;
    }

    public void SetValue(float value)
    {
        valueComponent.text = value.ToString("F2");
    }

    public void SetValue(int value)
    {
        valueComponent.text = value.ToString();
    }
}
