using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabSwitcher : MonoBehaviour
{
    public PageSwiper pageSwiper;
    public Button historyButton;
    public Button workStatsButton;

    void Start()
    {
        // Add listeners to the buttons
        historyButton.onClick.AddListener(() =>
        {
            pageSwiper.allowSwipe = true;
            SwipeToPage(1);
        });
        
        workStatsButton.onClick.AddListener(() =>
        {
            pageSwiper.allowSwipe = true;
            SwipeToPage(2);
        });
    }

    void SwipeToPage(int page)
    {
        // Calculate the simulated swipe distance based on the desired page
        float swipeDistance = (page - pageSwiper.currentPage) * Screen.width;

        // Create a new PointerEventData object
        var data = new PointerEventData(EventSystem.current)
        {
            // Set the press and release positions to simulate the swipe
            pressPosition = new Vector2(Screen.width / 2, Screen.height / 2),
            position = new Vector2(Screen.width / 2 - swipeDistance, Screen.height / 2)
        };

        // Call the OnEndDrag method to perform the swipe
        pageSwiper.OnEndDrag(data);
    }
}