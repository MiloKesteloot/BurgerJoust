using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Tracks the player's stars. Losing the last one reloads the scene.
public class StarManager : MonoBehaviour
{
    public static StarManager Instance { get; private set; }

    [Tooltip("Star images, left to right. The starting star count equals the number of images.")]
    [SerializeField] private List<Image> starImages = new List<Image>();
    [Tooltip("Optional: sprite for a lost star. If empty, lost stars are hidden.")]
    [SerializeField] private Sprite emptyStarSprite;

    private int stars;
    private bool gameOver;

    private void Awake()
    {
        Instance = this;
        stars = starImages.Count;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void LoseStar()
    {
        if (gameOver || stars <= 0) return;

        stars--;
        Image lost = starImages[stars]; // remove from the right
        if (emptyStarSprite != null) lost.sprite = emptyStarSprite;
        else lost.enabled = false;

        if (stars <= 0)
        {
            gameOver = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
