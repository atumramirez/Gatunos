using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterMover : MonoBehaviour
{
    [Header("Character Setup")]
    public List<Sprite> characterSprites;
    public Image characterImage;

    [Header("Movement Setup")]
    public RectTransform startPoint;
    public RectTransform endPoint;

    [Header("UI")]
    public Button triggerButton;

    [Header("Recipe System")]
    public CookingManager cookingManager;

    [Header("Settings")]
    public int totalCharacters = 5;

    private int charactersServed = 0;
    private bool waitingForRecipeResult = false;

    void Start()
    {
        triggerButton.gameObject.SetActive(false);
        StartNextCharacter();
    }

    void StartNextCharacter()
    {
        cookingManager.recipeGoalText.text = "";
        cookingManager.check.SetActive(false);
        if (charactersServed >= totalCharacters)
        {
            Debug.Log("All characters served!");
            characterImage.gameObject.SetActive(false);
            return;
        }

        characterImage.sprite = characterSprites[Random.Range(0, characterSprites.Count)];
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
            cookingManager.GenerateRecipe();
            waitingForRecipeResult = true;
        });
    }

    public void OnRecipeResult(bool correct)
    {
        if (!waitingForRecipeResult) return;

        if (correct)
        {
            waitingForRecipeResult = false;
            StartCoroutine(MoveAndSpawnNext());
        }
        else
        {
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
}