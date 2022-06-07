using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenScale : MonoBehaviour
{
    [SerializeField] private Ease _ease;
    [SerializeField] private Vector3 _originalScale;

    [SerializeField] private float _scaleSpeed;

    public void ScaleInGameObject()
    {
        this.transform.DOScale(_originalScale, _scaleSpeed).SetEase(_ease);
    }

    private void OnDisable() {
        //this.transform.localScale = new Vector3(0,0,0);
    }

    public void ScaleOutGameObject()
    {
        this.transform.DOScale(0, _scaleSpeed).SetEase(_ease).OnComplete(() => this.gameObject.SetActive(false));
    }
}
