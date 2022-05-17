using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenMove : MonoBehaviour
{
    [SerializeField] private Ease _ease;
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _position;
    private Vector3 _originalPosition;

    private void Start() {
        _originalPosition = this.GetComponent<RectTransform>().anchoredPosition;
    }

    public void MoveToPosition()
    {
        this.GetComponent<RectTransform>().DOAnchorPos(_position, _speed).SetEase(_ease);
    }

    public void TeleportToPosition()
    {
        this.GetComponent<RectTransform>().anchoredPosition = _position;
    }

    public void TeleportBackToPosition()
    {
        this.GetComponent<RectTransform>().anchoredPosition = _originalPosition;
    }

    public void MoveBackToPosition()
    {
        this.GetComponent<RectTransform>().DOAnchorPos(_originalPosition, _speed).SetEase(_ease);
    }
}
