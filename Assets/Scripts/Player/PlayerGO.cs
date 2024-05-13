using UnityEngine;

public class PlayerGO : MonoBehaviour {
    
    public static PlayerGO Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }
}
