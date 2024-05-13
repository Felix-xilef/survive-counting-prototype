using System;
using UnityEngine;
using UnityEngine.UIElements;

public class TimerController : MonoBehaviour {

    private Label timerLabel;

    private struct ElementNames {
        public const string TimerLabel = "TimerLabel";
    }

    private void OnEnable() {
        var ui = GetComponent<UIDocument>().rootVisualElement;

        timerLabel = ui.Q<Label>(
            ElementNames.TimerLabel
        );
    }

    public void UpdateTimer(float newValue) {
        var minutes = GetTimeString(
            Math.Floor(newValue / 60)
        );
        var seconds = GetTimeString(
            Math.Floor(newValue % 60)
        );

        timerLabel.text = $"{minutes}:{seconds}";
    }

    private string GetTimeString(double time) {
        var timeString = $"{time}";

        if (timeString.Length == 1) {
            timeString = $"0{timeString}";
        }

        return timeString;
    }
}
