using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Setter for TextMeshPro Text of WeightSigns.
/// </summary>
/// <author> Jannick Mitsch </author>
/// <date>07.01.2022</date>
public class TextFieldTextSetter : MonoBehaviour
{
    public void SetText(int value)
    {
        this.GetComponent<TextMeshProUGUI>().SetText(value.ToString());
    }
}
