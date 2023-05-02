using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardView : MonoBehaviour, ILeaderboardObserver
{
    private enum ScrollDireciton { Down, Up };

    [SerializeField] private GameObject scoreItemPrefab;
    [SerializeField] private int bufferSize = 3;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private int viewingItemCount = 5;

    private readonly List<Score> scores = new();
    private readonly List<GameObject> scoreItems = new();
    private int currentIndex = 0;
    private float itemHeight;
    private float itemWidth;
    private bool isUpdating = false;
    private ScrollDireciton scrollDirection;

    private void Awake()
    {
        scrollRect.scrollSensitivity = 0.25f;
        itemHeight = scoreItemPrefab.GetComponent<RectTransform>().rect.height;
        itemWidth = scoreItemPrefab.GetComponent<RectTransform>().rect.width;
        scrollRect.GetComponent<RectTransform>().sizeDelta = new Vector2(itemWidth, itemHeight * viewingItemCount);
        scrollRect.onValueChanged.AddListener(OnScroll);
    }

    private void OnDestroy()
    {
        scrollRect.onValueChanged.RemoveListener(OnScroll);
    }

    public void OnLeaderboardUpdate(List<Score> scoreList)
    {
        scores.Clear();
        scores.AddRange(scoreList);
        currentIndex = 0;
        DestroyScoreItems();
        CreateScoreItems();
    }

    private void DestroyScoreItems()
    {
        foreach (GameObject scoreItem in scoreItems)
        {
            Destroy(scoreItem);
        }
        scoreItems.Clear();
    }

    private void CreateScoreItems()
    {
        Vector2 startInstPos = new Vector2(0,-currentIndex * itemHeight);
        scrollRect.content.sizeDelta = new Vector2(itemWidth, itemHeight * scores.Count);
        int endIndex = Mathf.Min(currentIndex + viewingItemCount + bufferSize, scores.Count);
        GameObject newScoreItem;
        TMP_Text itemText;
        for (int i = currentIndex; i < endIndex; i++)
        {
            newScoreItem = Instantiate(scoreItemPrefab, scrollRect.content);
            newScoreItem.transform.localPosition = startInstPos - new Vector2(0, (i - currentIndex) * itemHeight + scrollRect.content.sizeDelta.y % itemHeight);
            itemText = newScoreItem.GetComponentInChildren<TMP_Text>();
            itemText.text = $"{scores[i].rank}. {scores[i].nickname}: {scores[i].score}";
            scoreItems.Add(newScoreItem);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);

        currentIndex = Mathf.Clamp(currentIndex, 0, scores.Count - viewingItemCount);
    }

    private void RefreshScoreItems()
    {   
        int deletedItemIndex = scrollDirection == ScrollDireciton.Down ? currentIndex - (bufferSize + 1) : currentIndex + viewingItemCount + bufferSize;
        int addedItemIndex = scrollDirection == ScrollDireciton.Down ? currentIndex + (viewingItemCount - 1) + bufferSize : currentIndex - bufferSize;
        if (((scrollDirection == ScrollDireciton.Down && currentIndex > bufferSize) || (scrollDirection == ScrollDireciton.Up && currentIndex <= scores.Count - viewingItemCount)) &
            deletedItemIndex >= 0 && deletedItemIndex < scores.Count)
        {
            int listRemoveIndex = scrollDirection == ScrollDireciton.Down ? 0 : scoreItems.Count - 1;
            Destroy(scoreItems[listRemoveIndex]);
            scoreItems.RemoveAt(listRemoveIndex);
        }
        if(addedItemIndex >= 0 && addedItemIndex < scores.Count)
        {
            Vector2 addedPosition = scrollDirection == ScrollDireciton.Down ? (Vector2)scoreItems.Last().transform.localPosition - new Vector2(0, itemHeight) :
              (Vector2)scoreItems.First().transform.localPosition + new Vector2(0, itemHeight);
            GameObject newScoreItem = Instantiate(scoreItemPrefab, scrollRect.content);
            newScoreItem.transform.localPosition = addedPosition;
            var itemText = newScoreItem.GetComponentInChildren<TMP_Text>();
            itemText.text = $"{scores[addedItemIndex].rank}. {scores[addedItemIndex].nickname}: {scores[addedItemIndex].score}";
            int listInsertIndex = scrollDirection == ScrollDireciton.Down ? scoreItems.Count : 0;
            scoreItems.Insert(listInsertIndex, newScoreItem);

        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
    }

    private void OnScroll(Vector2 scrollDelta)
    {
        isUpdating = false;
        float contentPosition = scrollRect.content.anchoredPosition.y;        
        int newCurrentIndex = Mathf.FloorToInt(contentPosition / itemHeight);
        if (!isUpdating && currentIndex != newCurrentIndex && newCurrentIndex >= 0 && newCurrentIndex <= scores.Count - viewingItemCount)
        {
            isUpdating = true;
            scrollDirection = newCurrentIndex > currentIndex ? ScrollDireciton.Down : ScrollDireciton.Up;
            currentIndex = newCurrentIndex;
            RefreshScoreItems();
            UpdateContentPosition();
            isUpdating = false;
        }
    }

    private void UpdateContentPosition()
    {
        float contentPosition = currentIndex * itemHeight;
        scrollRect.content.anchoredPosition = new Vector2(scrollRect.content.anchoredPosition.x, contentPosition);        
    }
}