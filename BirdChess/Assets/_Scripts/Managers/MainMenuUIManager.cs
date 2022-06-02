using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public enum Difficulty
{
    TUTORIAL,
    EASY,
    MEDIUM,
    HARD
}
public class MainMenuUIManager : MonoBehaviour
{
    private Difficulty _difficulty;
    private string _currentPosition;
    private bool _isCoroutineRunning;
    [SerializeField] private Image _skinImage;
    [SerializeField] private GameObject _boardIndicator;
    private UIInteractionEvent _confirm;
    private UIInteractionEvent _goBack;

    #region Back Events
    [Header("BACK EVENTS")]
    [SerializeField] private UIInteractionEvent _singlePlayerGoBack;
    [SerializeField] private UIInteractionEvent _computerGoBack;
    [SerializeField] private UIInteractionEvent _puzzleGoBack;
    [SerializeField] private UIInteractionEvent _skirmishGoBack;
    [SerializeField] private UIInteractionEvent _multiplayerGoBack;
    [SerializeField] private UIInteractionEvent _casualGoBack;
    [SerializeField] private UIInteractionEvent _rankedGoBack;
    [SerializeField] private UIInteractionEvent _skirmishMultiplayerGoBack;
    #endregion

    private void Start()
    {
        UpdateSkinIndicator();
        UpdateBoardIndicator();
    }

    public void SetCurrentPosition(string pos)
    {
        _currentPosition = pos.ToUpper();
    }

    public void SetConfirm(UIInteractionEvent interactionEvent)
    {
        _confirm = interactionEvent;
    }

    public void ExecuteEvents(UIInteractionEvent interactionEvents)
    {
        if (_isCoroutineRunning) return;

        StartCoroutine(EventSequence(interactionEvents));
    }

    IEnumerator EventSequence(UIInteractionEvent interactionEvents)
    {
        _isCoroutineRunning = true;

        foreach (UIEvent uIEvent in interactionEvents.OnClickEvents)
        {
            float timeTillNextEvent = uIEvent.timeTillNextEvent;

            while (timeTillNextEvent > 0)
            {
                timeTillNextEvent -= Time.unscaledDeltaTime;
                yield return null;
            }

            uIEvent.Event.Invoke();
        }

        _isCoroutineRunning = false;
        yield return null;
    }

    public void GoBack()
    {
        if (_isCoroutineRunning && _goBack != null) return;

        StartCoroutine(EventSequence(GoBackEvent()));
    }

    public void UpdateSkinIndicator()
    {
        foreach (TayogSpriteSet tayogSpriteSet in VisualsManager.Instance.tayogSpriteSetCollection.tayogSpriteSets)
        {
            if (tayogSpriteSet.ID == PlayerPrefs.GetString("SpriteID"))
            {
                _skinImage.sprite = tayogSpriteSet.indicator;
            }
        }
    }

    public void UpdateBoardIndicator()
    {
        foreach (BoardVisual boardVisual in VisualsManager.Instance.boardVisualsCollection.boardVisualsCollection)
        {
            if (boardVisual.boardID == PlayerPrefs.GetString(TayogRef.BOARD_ID))
            {
                if (_boardIndicator.transform.childCount > 0)
                {
                    foreach (Transform child in _boardIndicator.transform)
                    {
                        Destroy(child.gameObject);
                    }

                    GameObject board = Instantiate(boardVisual.boardPrefab);
                    board.transform.SetParent(_boardIndicator.transform);
                    board.transform.localPosition = new Vector3(0, 0, 0) + boardVisual.offset;
                    board.transform.localRotation = new Quaternion(0,0,0,0);
                    board.transform.localScale = new Vector3(1,1,1);
                }
            }
        }
    }

    private UIInteractionEvent GoBackEvent()
    {
        switch (_currentPosition)
        {
            case "SINGLEPLAYER":
                _goBack = _singlePlayerGoBack;
                break;
            case "PUZZLE":
                _goBack = _puzzleGoBack;
                break;
            case "COMPUTER":
                _goBack = _computerGoBack;
                break;
            case "SKIRMISH":
                _goBack = _skirmishGoBack;
                break;
            case "MULTIPLAYER":
                _goBack = _multiplayerGoBack;
                break;
            case "CASUAL":
                _goBack = _casualGoBack;
                break;
            case "RANKED":
                _goBack = _rankedGoBack;
                break;
            case "MULTIPLAYERSKIRMISH":
                _goBack = _skirmishMultiplayerGoBack;
                break;

        }
        return _goBack;
    }

    public void Confirm()
    {
        if (_isCoroutineRunning || _confirm == null) return;

        StartCoroutine(EventSequence(_confirm));
    }
}
