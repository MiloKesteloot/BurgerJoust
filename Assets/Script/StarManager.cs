using System.Collections;
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

    [Header("Lose-star flash")]
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private int flashCount = 4;
    [Tooltip("Seconds for each on and each off phase of a flash.")]
    [SerializeField] private float flashInterval = 0.1f;

    private int stars;
    private bool gameOver;
    private Color[] originalColors;
    private Coroutine flashRoutine;

    private void Awake()
    {
        Instance = this;
        stars = starImages.Count;
        originalColors = new Color[starImages.Count];
        for (int i = 0; i < starImages.Count; i++)
            originalColors[i] = starImages[i].color;
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

        // Flash stars red
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
            SetStarColors(false);
        }
        flashRoutine = StartCoroutine(Flash());

        if (stars <= 0)
        {
            gameOver = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private IEnumerator Flash()
    {
        for (int i = 0; i < flashCount; i++)
        {
            SetStarColors(true);
            yield return new WaitForSeconds(flashInterval);
            SetStarColors(false);
            yield return new WaitForSeconds(flashInterval);
        }
        flashRoutine = null;
    }

    private void SetStarColors(bool red)
    {
        for (int i = 0; i < starImages.Count; i++)
            starImages[i].color = red ? flashColor : originalColors[i];
    }
}
