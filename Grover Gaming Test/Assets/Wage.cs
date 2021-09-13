using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Wage : MonoBehaviour
{
    public GameObject upArrow;
    public GameObject downArrow;
    public TMP_Text denominationText;
    float currentDenomination;
    int count = 0;
    public float[] denomination = {0.25f, 0.50f,1.00f,5.00f}; 

    // Start is called before the first frame update
    void Start()
    {
        denominationText.text = denomination[0].ToString();
        currentDenomination = denomination[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RaiseDenomination()
    {
        if(count == denomination.Length-1)
        {
            count = 0;
        }
        else
        {
            count++;
        }
        currentDenomination = denomination[count];
        denominationText.text = denomination[count].ToString();
    }

    public void LowerDenomination()
    {
        if (count == 0)
        {
            count = denomination.Length;
        }
        else
        {
            count--;
        }
        currentDenomination = denomination[count];
        denominationText.text = denomination[count].ToString();
    }
}
