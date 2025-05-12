using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class CookingManager : MonoBehaviour
{
    [System.Serializable]
    public class Ingredient
    {
        public string name;
        public Sprite sprite;
        public Button button;
    }

    [System.Serializable]
    public class Recipe
    {
        public string recipeName;
        public List<string> ingredients;
    }

    public List<string> forbiddenIngredients;
    public List<Recipe> possibleRecipes;
    public List<Recipe> forbiddenRecipes;

    private bool currentCustomerIsSpecial = false; 
    private List<string> currentRecipe = new List<string>();

    public List<Ingredient> allIngredients;
    public Transform plateParent;
    public GameObject plateIngredientPrefab;
    public GameObject check;
    public TextMeshProUGUI recipeGoalText;
    public TextMeshProUGUI resultText;
    public Button checkRecipeButton;
    public Button generateRecipeButton;

    private List<string> plate = new List<string>();
    private Dictionary<string, GameObject> ingredientVisuals = new Dictionary<string, GameObject>();

    void Start()
    {
        check.SetActive(false);
        foreach (var ingredient in allIngredients)
        {
            ingredient.button.onClick.AddListener(() => ToggleIngredient(ingredient));
        }

        checkRecipeButton.onClick.AddListener(CheckRecipe);
    }

    void ToggleIngredient(Ingredient ingredient)
    {
        if (plate.Contains(ingredient.name))
        {
            plate.Remove(ingredient.name);
            Destroy(ingredientVisuals[ingredient.name]);
            ingredientVisuals.Remove(ingredient.name);
        }
        else
        {
            plate.Add(ingredient.name);
            GameObject visual = Instantiate(plateIngredientPrefab, plateParent);
            visual.GetComponent<Image>().sprite = ingredient.sprite;
            ingredientVisuals[ingredient.name] = visual;
        }
    }

    void CheckRecipe()
    {
        var sortedPlate = plate.OrderBy(i => i).ToList();
        var sortedRecipe = currentRecipe.OrderBy(i => i).ToList();

        bool hasForbidden = plate.Any(i => forbiddenIngredients.Contains(i));

        if (hasForbidden)
        {
            ClearPlate();
            RecipeWasForbidden();
            return;
        }

        if (sortedPlate.SequenceEqual(sortedRecipe))
        {
            check.SetActive(true);
            ClearPlate();
            RecipeWasCorrect();
        }
        else
        {
            ClearPlate();
            RecipeWasWrong();
        }
    }

    public void RecipeWasForbidden()
    {
        characterMover.OnRecipeResult(false, true);
    }

    public void GenerateRecipe(bool isSpecialCustomer)
    {
        currentCustomerIsSpecial = isSpecialCustomer;

        if (isSpecialCustomer)
        {
            Recipe recipe = forbiddenRecipes[Random.Range(0, forbiddenRecipes.Count)];
            currentRecipe = recipe.ingredients.Where(i => !forbiddenIngredients.Contains(i)).ToList();
            recipeGoalText.text = $"Pedido: {recipe.recipeName}";
        }
        else
        {
            Recipe recipe = possibleRecipes[Random.Range(0, possibleRecipes.Count)];
            currentRecipe = new List<string>(recipe.ingredients);
            recipeGoalText.text = $"Pedido: {recipe.recipeName}";
        }
    }

    public CharacterMover characterMover;

    public void RecipeWasCorrect()
    {
        characterMover.OnRecipeResult(true, false);
    }

    public void RecipeWasWrong()
    {
        characterMover.OnRecipeResult(false, false);
    }

    void ClearPlate()
    {
        foreach (var visual in ingredientVisuals.Values)
        {
            Destroy(visual);
        }

        ingredientVisuals.Clear();
        plate.Clear();
    }
}