using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject pauseMenu;
    public GameObject creditsMenu;

    public Vector2 maxXAndY;
    public Vector2 minXAndY;

    public bool paused;

    private void Start() {
        if (paused) {
            Pause();
        } else {
            Play();
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (paused) {
                Play();
            } else {
                Pause();
            }
        }
    }

    public void Pause() {
        paused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void Play() {
        paused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void Credits() {
        creditsMenu.SetActive(!creditsMenu.activeSelf);
    }

    public void Quit() {
        Application.Quit();
    }
}
