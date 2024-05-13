using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour {

    [SerializeField]
    private GameObject howToPlay;

    private struct ElementNames {
        public const string PlayButton = "PlayButton";
        public const string HowToPlayButton = "HowToPlayButton";
    }

    private void OnEnable() {
        var ui = GetComponent<UIDocument>().rootVisualElement;

        var playButton = ui.Q<Button>(ElementNames.PlayButton);
        playButton.clicked += OnPlayButtonClicked;

        var howToPlayButton = ui.Q<Button>(ElementNames.HowToPlayButton);
        howToPlayButton.clicked += OnHowToPlayButtonClicked;
    }

    private void OnPlayButtonClicked() {
        gameObject.SetActive(false);

        GameManagerGO.Instance.gameState = GameState.Playing;
    }

    private void OnHowToPlayButtonClicked() {
        gameObject.SetActive(false);

        howToPlay.SetActive(true);
    }
}
