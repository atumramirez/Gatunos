using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CharacterMover : MonoBehaviour
{
    [Header("Character Setup")]
    public List<Sprite> characterSprites;
    public Image characterImage;

    [Header("Movement Setup")]
    public RectTransform startPoint;
    public RectTransform endPoint;

    [Header("UI")]
    public Button startButton;
    public Button triggerButton;
    public TextMeshProUGUI customersLeftText;

    [Header("Recipe System")]
    public CookingManager cookingManager;

    [Header("Settings")]
    public int minCharacters = 3;
    public int maxCharacters = 5;

    [Header("Special Customer Settings")]
    public List<Sprite> specialCustomerSprites;
    private bool currentCustomerIsSpecial = false;

    [Header("Fade Settings")]
    public IrCuzinha IrCuzinha;

    private int totalCharacters = 0;
    private int charactersServed = 0;
    private bool waitingForRecipeResult = false;

    void Start()
    {
        triggerButton.gameObject.SetActive(false);
        characterImage.gameObject.SetActive(false);
        customersLeftText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
        startButton.onClick.AddListener(BeginGame);
    }

    void BeginGame()
    {
        totalCharacters = Random.Range(minCharacters, maxCharacters + 1);
        charactersServed = 0;
        customersLeftText.gameObject.SetActive(true);
        startButton.gameObject.SetActive(false);
        StartNextCharacter();
    }

    void StartNextCharacter()
    {
        cookingManager.recipeGoalText.text = "";
        cookingManager.check.SetActive(false);

        UpdateCustomersLeftText();

        if (charactersServed >= totalCharacters)
        {
            characterImage.gameObject.SetActive(false);
            customersLeftText.text = "All customers served!";
            IrCuzinha.TransitionTo(IrCuzinha.AbrirRestaurante);
            return;
        }

        currentCustomerIsSpecial = Random.value < 0.3f; 

        characterImage.sprite = currentCustomerIsSpecial
            ? specialCustomerSprites[Random.Range(0, specialCustomerSprites.Count)]
            : characterSprites[Random.Range(0, characterSprites.Count)];

        characterImage.rectTransform.anchoredPosition = startPoint.anchoredPosition;
        characterImage.gameObject.SetActive(true);

        StartCoroutine(MoveToPosition(endPoint.anchoredPosition, OnCharacterArrived));
    }

    IEnumerator MoveToPosition(Vector2 targetPos, System.Action onArrive = null)
    {
        float speed = 200f;
        RectTransform rect = characterImage.rectTransform;

        while (Vector2.Distance(rect.anchoredPosition, targetPos) > 1f)
        {
            rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, targetPos, speed * Time.deltaTime);
            yield return null;
        }

        rect.anchoredPosition = targetPos;
        onArrive?.Invoke();
    }

    void OnCharacterArrived()
    {
        triggerButton.gameObject.SetActive(true);
        triggerButton.onClick.RemoveAllListeners();
        triggerButton.onClick.AddListener(() =>
        {
            triggerButton.gameObject.SetActive(false);
            cookingManager.GenerateRecipe(currentCustomerIsSpecial);
            waitingForRecipeResult = true;
        });
    }

    public void OnRecipeResult(bool correct, bool wasForbidden)
    {
        if (!waitingForRecipeResult) return;

        waitingForRecipeResult = false;

        if (wasForbidden)
        {
            IrCuzinha.TransitionTo(IrCuzinha.MostrarFinalMau);
        }

        if (correct)
        {
            StartCoroutine(MoveAndSpawnNext());
        }
    }

    IEnumerator MoveAndSpawnNext()
    {
        Vector2 offScreen = new Vector2(-770f, -95f);
        yield return StartCoroutine(MoveToPosition(offScreen));
        characterImage.gameObject.SetActive(false);
        charactersServed++;

        yield return new WaitForSeconds(0.5f);
        StartNextCharacter();
    }

    void UpdateCustomersLeftText()
    {
        int remaining = totalCharacters - charactersServed;
        customersLeftText.text = $"Customers Left: {remaining}";
    }
}
