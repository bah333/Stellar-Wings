using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// purely used for this "pseudo" global variables
public class GlobalVariables : MonoBehaviour
{
    public double lowMaterialAmount;
    public double mediumMaterialAmount;
    public double highMaterialAmount;
    public double shipScrapsAmount;

    void Awake()
    {
        lowMaterialAmount = 0;
        mediumMaterialAmount = 0;
        highMaterialAmount = 0;
        shipScrapsAmount = 0;
    }

}
