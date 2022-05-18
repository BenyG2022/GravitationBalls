using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBallsCounter : MonoBehaviour
{

    [SerializeField] private TMPro.TextMeshProUGUI _textCounter = null;

    public void assignValueToTextCounter(int value) => _textCounter.text = value.ToString();


}
