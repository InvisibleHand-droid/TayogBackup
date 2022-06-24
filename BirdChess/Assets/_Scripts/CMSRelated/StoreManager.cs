using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    // Called from the IAP Buttons that come along after enabling in-app purchasing
    /* docs.unity3d.com/Manual/UnityIAPGoogleConfiguration.html */

    public void OnBattlePassPurchaseComplete()
    {
        // enable battlepass on CMS side
        // the string in this case refers to the ID of the product we created in the Google Play console
        CmsManager.instance.EnablePlayerBoolean("battlepass");
    }
}
