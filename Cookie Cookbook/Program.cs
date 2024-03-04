using Cookie_CookBook;
using System.Text.Json;


namespace Cookie_CookBook
{

    public class Ingredient
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string PreparationInstructions { get; set; }
    }

    public class Recipe
    {
        public List<int> IngredientIDs { get; set; } = new List<int>();
    }

    class Program
    {
        private const string RecipeFileName = "recipes";
        private const string FileFormat = "json";

        private static List<Ingredient> availableIngredients = new List<Ingredient>();
        private static List<Recipe> existingRecipes = new List<Recipe>();

        static void Main()
        {
            LoadIngredients();
            LoadRecipes();

            if (existingRecipes.Count > 0)
            {
                Console.WriteLine("Existing recipes are:\n");
                PrintAllRecipes();
            }

            Console.WriteLine("Create a new cookie recipe! Available ingredients are: ");
            PrintAvailableIngredients();

            List<int> selectedIngredientIDs = SelectIngredients();

            if (selectedIngredientIDs.Count == 0)
            {
                Console.WriteLine("No ingredients have been selected. Recipe will not be saved.");
            }
            else
            {
                Recipe newRecipe = new Recipe() { IngredientIDs = selectedIngredientIDs };
                Console.WriteLine("Recipe added:");
                PrintSingleRecipe(newRecipe);
                existingRecipes.Add(newRecipe);

                SaveRecipes();
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static void LoadIngredients()
        {
            availableIngredients = new List<Ingredient>
            {
                new Ingredient { ID = 1, Name = "Wheat flour", PreparationInstructions = "Sieve. Add to other ingredients."},
                new Ingredient { ID = 2, Name = "Coconut flour", PreparationInstructions = "Sieve. Add to other ingredients."},
                new Ingredient { ID = 3, Name = "Butter", PreparationInstructions = "Melt on low heat. Add to other ingredients."},
                new Ingredient { ID = 4, Name = "Chocolate", PreparationInstructions = "Melt in a water bath. Add to other ingredients."},
                new Ingredient { ID = 5, Name = "Sugar", PreparationInstructions = "Add to other ingredients."},
                new Ingredient { ID = 6, Name = "Cardamom", PreparationInstructions = "Take half a teaspoon. Add to other ingredients."},
                new Ingredient { ID = 7, Name = "Cinnamon", PreparationInstructions = "Take half a teaspoon. Add to other ingredients."},
                new Ingredient { ID = 8, Name = "Cocoa powder", PreparationInstructions = "Add to other ingredients."},

            };
        }

        private static void LoadRecipes()
        {
            string filePath = RecipeFileName + FileFormat;

            if (File.Exists(filePath))
            {
                string fileContent = File.ReadAllText(filePath);
                existingRecipes = JsonSerializer.Deserialize<List<Recipe>>(fileContent);
            }
        }

        private static void SaveRecipes()
        {
            string filePath = RecipeFileName + FileFormat;
            string fileContent = JsonSerializer.Serialize(existingRecipes);
            File.WriteAllText(filePath, fileContent);
        }

        private static void PrintAllRecipes()
        {
            for (int i = 0; i < existingRecipes.Count; i++)
            {
                Console.WriteLine($"***** {i + 1} *****");
                PrintSingleRecipe(existingRecipes[i]);
                Console.WriteLine();
            }
        }

        private static void PrintSingleRecipe(Recipe recipe)
        {
            foreach (int ingredientID in recipe.IngredientIDs)
            {
                Ingredient ingredient = availableIngredients.Find(i => i.ID == ingredientID);
                Console.WriteLine($"{ingredient.Name}. {ingredient.PreparationInstructions}");
            }
        }

        private static void PrintAvailableIngredients()
        {
            foreach (Ingredient ingredient in availableIngredients)
            {
                Console.WriteLine($"{ingredient.ID}. {ingredient.Name}");
            }
        }

        private static List<int> SelectIngredients()
        {
            List<int> selectedIngredientIDs = new List<int>();

            while (true)
            {
                Console.WriteLine("Add an ingredient by its ID or type anything else if finished.");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int selectedID))
                {
                    Ingredient selectedIngredient = availableIngredients.Find(i => i.ID == selectedID);

                    if (selectedIngredient != null)
                    {
                        selectedIngredientIDs.Add(selectedID);
                        Console.WriteLine($"{selectedIngredient.Name} added to the recipe.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid ingredient ID. Try again.");
                    }
                }
                else
                {
                    break;
                }
            }

            return selectedIngredientIDs;
        }
    }
}