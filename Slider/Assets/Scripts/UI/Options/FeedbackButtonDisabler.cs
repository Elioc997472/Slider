using UnityEngine;
using UnityEngine.UI;

public class FeedbackButtonDisabler : MonoBehaviour {
    [SerializeField] private Toggle colorblindToggle;
    [SerializeField] private Button feedbackButton;
    [SerializeField] private Button resetButton;

    private void OnEnable() {
        SetFeedbackButton(GameManager.CurrentPlatform != GameManager.Platform.Xbox);
    }

    private void SetFeedbackButton(bool enabled) {
        feedbackButton.gameObject.SetActive(enabled);

        if (enabled) {
            Navigation colorblindNavigation = colorblindToggle.navigation;
            colorblindNavigation.selectOnDown = feedbackButton;
            colorblindToggle.navigation = colorblindNavigation;

            Navigation resetButtonNavigation = resetButton.navigation;
            resetButtonNavigation.selectOnUp = feedbackButton;
            resetButton.navigation = resetButtonNavigation;
        }
        else
        {
            Navigation colorblindNavigation = colorblindToggle.navigation;
            colorblindNavigation.selectOnDown = resetButton;
            colorblindToggle.navigation = colorblindNavigation;

            Navigation resetButtonNavigation = resetButton.navigation;
            resetButtonNavigation.selectOnUp = colorblindToggle;
            resetButton.navigation = resetButtonNavigation;
        }
    }
}