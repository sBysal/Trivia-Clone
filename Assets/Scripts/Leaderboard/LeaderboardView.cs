using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardView : MonoBehaviour, ILeaderboardObserver      //Controls leaderboard panel ui.
{
    private enum ScrollDireciton { Down, Up };

    [SerializeField] private ObjectPool _objectPool;
    [SerializeField] private GameObject _scoreItemPrefab;
    [SerializeField] private int _bufferSize = 3;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private int _viewingItemCount = 5;

    private readonly List<Score> _scores = new();
    private readonly List<GameObject> _scoreItems = new();
    private int _currentIndex = 0;
    private float _itemHeight;
    private float _itemWidth;
    private bool _isUpdating = false;
    private ScrollDireciton _scrollDirection;

    private void Awake()
    {
        _scrollRect.scrollSensitivity = 0.25f;
        _itemHeight = _scoreItemPrefab.GetComponent<RectTransform>().rect.height;
        _itemWidth = _scoreItemPrefab.GetComponent<RectTransform>().rect.width;
        _scrollRect.GetComponent<RectTransform>().sizeDelta = new Vector2(_itemWidth, _itemHeight * _viewingItemCount);
        _scrollRect.onValueChanged.AddListener(OnScroll);
    }

    private void OnDestroy()
    {
        _scrollRect.onValueChanged.RemoveListener(OnScroll);
    }

    public void OnLeaderboardUpdate(List<Score> scoreList)      //Updates the leaderboard.
    {
        _scores.Clear();
        _scores.AddRange(scoreList);
        _currentIndex = 0;
        DestroyScoreItems();
        CreateScoreItems();
        _scrollRect.content.anchoredPosition = Vector2.zero;
    }

    public void DestroyScoreItems()     //Send all score items to pool.
    {
        foreach (GameObject scoreItem in _scoreItems)
        {
            _objectPool.ReturnObjectToPool(scoreItem);
        }
        _scoreItems.Clear();
    }

    private void CreateScoreItems()     //Gets score items from pool.
    {
        Vector2 startInstPos = new Vector2(0,-_currentIndex * _itemHeight);
        _scrollRect.content.sizeDelta = new Vector2(_itemWidth, _itemHeight * _scores.Count);
        int endIndex = Mathf.Min(_currentIndex + _viewingItemCount + _bufferSize, _scores.Count);
        GameObject newScoreItem;
        TMP_Text itemText;
        for (int i = _currentIndex; i < endIndex; i++)
        {
            newScoreItem = _objectPool.GetObjectFromPool();
            newScoreItem.transform.SetParent(_scrollRect.content);
            newScoreItem.transform.localScale = Vector2.one;
            newScoreItem.transform.localPosition = startInstPos - new Vector2(0, (i - _currentIndex) * _itemHeight + _scrollRect.content.sizeDelta.y % _itemHeight);
            itemText = newScoreItem.GetComponentInChildren<TMP_Text>();
            itemText.text = $"{_scores[i].rank}. {_scores[i].nickname}: {_scores[i].score}";
            _scoreItems.Add(newScoreItem);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollRect.content);

        _currentIndex = Mathf.Clamp(_currentIndex, 0, _scores.Count - _viewingItemCount);
    }

    private void RefreshScoreItems()    //Refreshes score items.
    {   
        int deletedItemIndex = _scrollDirection == ScrollDireciton.Down ? _currentIndex - (_bufferSize + 1) : _currentIndex + _viewingItemCount + _bufferSize;
        int addedItemIndex = _scrollDirection == ScrollDireciton.Down ? _currentIndex + (_viewingItemCount - 1) + _bufferSize : _currentIndex - _bufferSize;
        if (((_scrollDirection == ScrollDireciton.Down && _currentIndex > _bufferSize) || (_scrollDirection == ScrollDireciton.Up && _currentIndex <= _scores.Count - _viewingItemCount)) &
            deletedItemIndex >= 0 && deletedItemIndex < _scores.Count)
        {
            int listRemoveIndex = _scrollDirection == ScrollDireciton.Down ? 0 : _scoreItems.Count - 1;
            _objectPool.ReturnObjectToPool(_scoreItems[listRemoveIndex]);
            _scoreItems.RemoveAt(listRemoveIndex);
        }
        if(addedItemIndex >= 0 && addedItemIndex < _scores.Count)
        {
            Vector2 addedPosition = _scrollDirection == ScrollDireciton.Down ? (Vector2)_scoreItems.Last().transform.localPosition - new Vector2(0, _itemHeight) :
              (Vector2)_scoreItems.First().transform.localPosition + new Vector2(0, _itemHeight);
            GameObject newScoreItem = _objectPool.GetObjectFromPool();
            newScoreItem.transform.SetParent(_scrollRect.content);
            newScoreItem.transform.localScale = Vector2.one;
            newScoreItem.transform.localPosition = addedPosition;
            var itemText = newScoreItem.GetComponentInChildren<TMP_Text>();
            itemText.text = $"{_scores[addedItemIndex].rank}. {_scores[addedItemIndex].nickname}: {_scores[addedItemIndex].score}";
            int listInsertIndex = _scrollDirection == ScrollDireciton.Down ? _scoreItems.Count : 0;
            _scoreItems.Insert(listInsertIndex, newScoreItem);

        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollRect.content);
    }

    private void OnScroll(Vector2 scrollDelta)  //Calculates top of the scoreItem index which views in leaderboard panel while scrolling.
    {
        _isUpdating = false;
        float contentPosition = _scrollRect.content.anchoredPosition.y;        
        int newCurrentIndex = Mathf.FloorToInt(contentPosition / _itemHeight);
        if (!_isUpdating && _currentIndex != newCurrentIndex && newCurrentIndex >= 0 && newCurrentIndex <= _scores.Count - _viewingItemCount)
        {
            _isUpdating = true;
            _scrollDirection = newCurrentIndex > _currentIndex ? ScrollDireciton.Down : ScrollDireciton.Up;
            _currentIndex = newCurrentIndex;
            RefreshScoreItems();
            _isUpdating = false;
        }
    }
}