using UnityEngine;
using UnityEngine.UIElements;

public class CounterController : MonoBehaviour {

    private Label counterLabel;

    private struct ElementNames {
        public const string CounterLabel = "CounterLabel";
    }

    private void OnEnable() {
        var ui = GetComponent<UIDocument>().rootVisualElement;

        counterLabel = ui.Q<Label>(
            ElementNames.CounterLabel
        );
    }

    public void UpdateCounter(int newValue) {
        counterLabel.text = $"{newValue}";
    }
}
