using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TayogPieceSpriteScript : MonoBehaviour
{
    private Camera _cameraTarget;
    // Start is called before the first frame update
    private void Awake()
    {
        _cameraTarget = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.rotation = Quaternion.Euler(_cameraTarget.transform.eulerAngles.x, _cameraTarget.transform.eulerAngles.y, _cameraTarget.transform.eulerAngles.z);
    }
}
