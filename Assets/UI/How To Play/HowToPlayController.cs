using UnityEngine;
using UnityEngine.UIElements;

public class HowToPlayController : MonoBehaviour {

    [SerializeField]
    private GameObject mainMenu;

    private VisualElement ui;

    private Button closeButton;

    private struct ElementNames {
        public const string CloseButton = "CloseButton";
    }

    private void OnEnable() {
        ui = GetComponent<UIDocument>().rootVisualElement;

        closeButton = ui.Q<Button>(ElementNames.CloseButton);
        closeButton.clicked += OnCloseButtonClicked;
    }

    private void OnCloseButtonClicked() {
        gameObject.SetActive(false);

        mainMenu.SetActive(true);
    }
}
