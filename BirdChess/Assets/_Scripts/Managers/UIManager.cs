using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class PlayerUI
{
    [Header("Player Texts")]
    public TextMeshProUGUI playerHeaderText;

    [Header("Player Piece Count")]
    public TextMeshProUGUI manokCountText;
    public TextMeshProUGUI bibeCountText;
    public TextMeshProUGUI lawinCountText;
    public TextMeshProUGUI agilaCountText;

    [Header("Piece Buttons")]
    public GameObject manokButton;
    public GameObject bibeButton;
    public GameObject lawinButton;
    public GameObject agilaButton;

    [Header("Panel")]
    public GameObject panel;
}


public class UIManager : Singleton<UIManager>
{
    [SerializeField] private PlayerUI[] _playerUI;
    [SerializeField] private TextMeshProUGUI _stateText;

    [SerializeField] private GameObject _victoryPanel;
    [SerializeField] private TextMeshProUGUI _victoryText;
    // Start is called before the first frame update

    public override void Awake()
    {
        base.Awake();
    }



    // Update is called once per frame
    void Update()
    {

    }

    public void SetUIDependencies()
    {
        for (int i = 0; i < GameManager.Instance.players.Count; i++)
        {
            _playerUI[i].manokButton.GetComponent<ButtonReserveTarget>().player = GameManager.Instance.players[i];
            _playerUI[i].bibeButton.GetComponent<ButtonReserveTarget>().player = GameManager.Instance.players[i];
            _playerUI[i].agilaButton.GetComponent<ButtonReserveTarget>().player = GameManager.Instance.players[i];
            _playerUI[i].lawinButton.GetComponent<ButtonReserveTarget>().player = GameManager.Instance.players[i];
        }
    }

    public void UpdatePlayerReserveText()
    {
        for (int i = 0; i < GameManager.Instance.players.Count; i++)
        {
            _playerUI[i].manokCountText.SetText(GameManager.Instance.players[i].GetReserveCountOfPieceType(PieceType.Manok).ToString());
            _playerUI[i].bibeCountText.SetText(GameManager.Instance.players[i].GetReserveCountOfPieceType(PieceType.Bibe).ToString());
            _playerUI[i].lawinCountText.SetText(GameManager.Instance.players[i].GetReserveCountOfPieceType(PieceType.Lawin).ToString());
            _playerUI[i].agilaCountText.SetText(GameManager.Instance.players[i].GetReserveCountOfPieceType(PieceType.Agila).ToString());
        }
    }

    public void UpdateStateText()
    {
        _stateText.SetText(TurnManager.Instance.GetCurrentPlayer().name);
    }

    public void SetPlayerHeaderTexts()
    {
        for (int i = 0; i < GameManager.Instance.players.Count; i++)
        {
            _playerUI[i].playerHeaderText.SetText(GameManager.Instance.players[i]._teamColor.ToString());
        }
    }

    public void EnableVictoryWindow(TeamColor teamColor, string action)
    {
        _victoryText.SetText($"{teamColor.ToString()} {action}");
        _victoryPanel.SetActive(true);
    }

    //placeholder
    public void EnableEverythingElse()
    {
        for (int i = 0; i < _playerUI.Length; i++)
        {
            _playerUI[i].bibeButton.SetActive(true);
            _playerUI[i].lawinButton.SetActive(true);
            _playerUI[i].agilaButton.SetActive(true);
        }
    }

}
