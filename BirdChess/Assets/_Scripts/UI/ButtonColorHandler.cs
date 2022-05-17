using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;


public class ButtonColorHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool _isToggled;
    private GameObject _button;
    private TextMeshProUGUI _text;
    [SerializeField] private Color _originalColor;
    [SerializeField] private Color _pressedColor;


    private void Update()
    {
        //Debug.Log(EventSystem.current.currentSelectedGameObject.name);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //if (EventSystem.current.currentSelectedGameObject == null) return;
        Debug.Log(1);

        Debug.Log(EventSystem.current.currentSelectedGameObject.name);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // throw new System.NotImplementedException();
    }

    private void Awake()
    {
        // button = GetComponent<Button>();
        //text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }




    void ToggleColor()
    {
        _isToggled = !_isToggled;

        _button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = _isToggled ? _originalColor : _pressedColor;

        _button = GameObject.Find(EventSystem.current.currentSelectedGameObject.name);

        _button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = _isToggled ? _originalColor : _pressedColor;
    }
}
