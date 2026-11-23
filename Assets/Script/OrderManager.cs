using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Generates random orders from BurgerRain's ingredient list, checks caught
// ingredients against the current order, and drives the order UI.
public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private BurgerRain burgerRain;
    [Tooltip("Ingredient that must appear at least once in every order (drag the Patty prefab here).")]
    [SerializeField] private GameObject requiredIngredient;

    [Header("UI")]
    [Tooltip("Parent for order icons, e.g. a top-left panel with a Horizontal Layout Group.")]
    [SerializeField] private Transform orderContainer;
    [Tooltip("An Image prefab used for each ingredient in the order.")]
    [SerializeField] private Image orderIconPrefab;
    [SerializeField] private TMP_Text ordersCompletedText;
    [SerializeField] private Color collectedTint = new Color(1f, 1f, 1f, 0.25f);

    private class OrderSlot
    {
        public Sprite sprite;
        public Image icon;
        public bool collected;
    }

    private readonly List<OrderSlot> slots = new List<OrderSlot>();
    private readonly List<GameObject> caughtOnStick = new List<GameObject>();
    private List<Sprite> availableSprites = new List<Sprite>();
    private Sprite requiredSprite;
    private int ordersCompleted;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void Start()
    {
        availableSprites = new List<Sprite>();
        foreach (GameObject prefab in burgerRain.burgersIngredients)
        {
            Sprite s = GetSprite(prefab);
            if (s != null) availableSprites.Add(s);
            else Debug.LogWarning($"OrderManager: {prefab.name} has no SpriteRenderer.");
        }

        requiredSprite = GetSprite(requiredIngredient);

        UpdateCounterText();
        GenerateOrder();
    }

    private static Sprite GetSprite(GameObject go)
    {
        if (go == null) return null;
        SpriteRenderer sr = go.GetComponentInChildren<SpriteRenderer>();
        return sr != null ? sr.sprite : null;
    }

    // 80%: 3-5 items. 20%: 2 or 6 (evenly split).
    private static int RollOrderSize()
    {
        if (Random.value < 0.8f) return Random.Range(3, 6);
        return Random.value < 0.5f ? 2 : 6;
    }

    public void GenerateOrder()
    {
        foreach (OrderSlot slot in slots)
        {
            if (slot.icon != null) Destroy(slot.icon.gameObject);
        }
        slots.Clear();

        if (availableSprites.Count == 0) return;

        int size = RollOrderSize();
        List<Sprite> picks = new List<Sprite>();

        if (requiredSprite != null) picks.Add(requiredSprite);
        while (picks.Count < size)
        {
            picks.Add(availableSprites[Random.Range(0, availableSprites.Count)]);
        }

        // Shuffle so the patty isn't always first in the list.
        for (int i = picks.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (picks[i], picks[j]) = (picks[j], picks[i]);
        }

        foreach (Sprite s in picks)
        {
            Image icon = Instantiate(orderIconPrefab, orderContainer);
            icon.sprite = s;
            icon.preserveAspect = true;
            icon.color = Color.white;
            slots.Add(new OrderSlot { sprite = s, icon = icon });
        }
    }

    // Called by DestroyBurger when an ingredient sticks to the joust.
    public void IngredientCaught(GameObject ingredient, Sprite sprite)
    {
        caughtOnStick.Add(ingredient);

        OrderSlot match = slots.Find(slot => !slot.collected && slot.sprite == sprite);
        if (match == null)
        {
            // Not on the order, or already have enough of it.
            if (StarManager.Instance != null) StarManager.Instance.LoseStar();
            return;
        }

        match.collected = true;
        if (match.icon != null) match.icon.color = collectedTint;

        if (slots.TrueForAll(slot => slot.collected))
        {
            CompleteOrder();
        }
    }

    private void CompleteOrder()
    {
        ordersCompleted++;
        UpdateCounterText();

        foreach (GameObject go in caughtOnStick)
        {
            if (go != null) Destroy(go);
        }
        caughtOnStick.Clear();

        GenerateOrder();
    }

    private void UpdateCounterText()
    {
        if (ordersCompletedText != null) ordersCompletedText.text = $"Orders: {ordersCompleted}";
    }
}
