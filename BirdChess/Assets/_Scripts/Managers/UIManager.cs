using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
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

    public void SetUIDependencies(int i, Player player)
    {
        _playerUI[i].manokButton.GetComponent<ButtonReserveTarget>().player = player;
        _playerUI[i].bibeButton.GetComponent<ButtonReserveTarget>().player = player;
        _playerUI[i].agilaButton.GetComponent<ButtonReserveTarget>().player = player;
        _playerUI[i].lawinButton.GetComponent<ButtonReserveTarget>().player = player;

        if (!player.photonView.IsMine)
        {
            _playerUI[i].manokButton.GetComponent<Button>().interactable = false;
            _playerUI[i].bibeButton.GetComponent<Button>().interactable = false;
            _playerUI[i].agilaButton.GetComponent<Button>().interactable = false;
            _playerUI[i].lawinButton.GetComponent<Button>().interactable = false;
        }
    }

    [PunRPC]
    public void RPCUpdatePlayerReserveText()
    {
        for (int i = 0; i < GameManager.Instance.players.Count; i++)
        {
            _playerUI[i].manokCountText.SetText(_playerUI[i].manokButton.GetComponent<ButtonReserveTarget>().player.GetReserveCountOfPieceType(PieceType.Manok).ToString());
            _playerUI[i].bibeCountText.SetText(_playerUI[i].manokButton.GetComponent<ButtonReserveTarget>().player.GetReserveCountOfPieceType(PieceType.Bibe).ToString());
            _playerUI[i].lawinCountText.SetText(_playerUI[i].manokButton.GetComponent<ButtonReserveTarget>().player.GetReserveCountOfPieceType(PieceType.Lawin).ToString());
            _playerUI[i].agilaCountText.SetText(_playerUI[i].manokButton.GetComponent<ButtonReserveTarget>().player.GetReserveCountOfPieceType(PieceType.Agila).ToString());
        }
    }

    [PunRPC]
    public void RPCUpdateStateText()
    {
        _stateText.SetText(TurnManager.Instance.GetCurrentPlayer().name);
    }

    public void SetPlayerHeaderTexts(int i, Player player)
    {
        _playerUI[i].playerHeaderText.SetText(player.teamColor.ToString());
    }

    [PunRPC]
    public void EnableVictoryWindow(TeamColor teamColor, string action)
    {
        _victoryText.SetText($"{teamColor.ToString()} {action}");
        _victoryPanel.SetActive(true);
    }

    [PunRPC]
    public void RPCEnableEverythingElse()
    {
        for (int i = 0; i < _playerUI.Length; i++)
        {
            _playerUI[i].bibeButton.SetActive(true);
            _playerUI[i].lawinButton.SetActive(true);
            _playerUI[i].agilaButton.SetActive(true);
        }
    }

}
