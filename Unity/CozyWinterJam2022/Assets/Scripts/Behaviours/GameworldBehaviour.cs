namespace HNS.CozyWinterJam2022.Behaviours
{
    using HNS.CozyWinterJam2022.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("CWJ2022/Gameworld")]
    public class GameworldBehaviour : MonoBehaviour
    {
        #region Members

        public int MapWidth;
        public int MapHeight;

        public float YearTimeToStart;
        public float YearTimeSpeed;

        public float MaxChristmasCheer;
        public float StartingChristmasCheer;
        public float ChristmasCheerPerYearEnd;

        public Image BuildingWorkersPanel;
        public Image ChristmasCheerbar;

        public AudioSource SoundEffectSource;

        public AudioClip BuildingSFX;
        public AudioClip WinSFX;
        public AudioClip LoseSFX;

        public Image WonPanel;

        public Dictionary<Tuple<float, float>, BuildingBehaviour> Buildings { get; set; }

        public Dictionary<Tuple<float, float>, ResourceBehaviour> ResourceObjects { get; set; }

        public float[] Inventory { get; set; }

        public int[] AvailableWorkers { get; set; }

        public int[,] WorldMap { get; set; }

        public float ChristmasCheer { get; set; }

        public float YearTimeLeft { get; set; }

        public int Year { get; set; }

        public List<List<Tuple<ProduceableResourceCategory, float>>> AllYearEndGoals { get; set; }

        public List<Tuple<ProduceableResourceCategory, float>> CurrentYearEndGoals { get; set; }

        public Dictionary<BuildingType, List<ProduceableResourceCategory>> BuildingResourceCostCategories { get; set; }

        public Dictionary<BuildingType, List<float>> BuildingResourceCostAmounts { get; set; }

        protected ToDoListBehaviour ToDoList { get; set; }

        protected DialogueBehaviour Dialogue { get; set; }

        protected Dictionary<BuildingType, bool> BuildingTypesAvailable { get; set; } 
        
        protected bool IsYearOver { get; set; }
        
        #endregion

        #region Event Handlers

        private void Building_BuildComplete(object sender, EventArgs e)
        {
            var building = (BuildingBehaviour)sender;

            for (int i = 0; i < building.WorkersProducedCategories.Length; i++)
            {
                var category = building.WorkersProducedCategories[i];
                var categoryIndex = (int)category;
                var amount = building.WorkersProducedAmounts[i];
                AvailableWorkers[categoryIndex] += amount;
            }

            if (building.BuildingType == BuildingType.Farm)
            {
                var cellX = (int)Mathf
                    .Round(building.transform.position.x + 25);

                var cellY = (int)Mathf
                    .Round(building.transform.position.z + 25);

                for (int cy = cellY - 1; cy <= cellY + 1; cy++)
                {
                    for (int cx = cellX - 1; cx <= cellX + 1; cx++)
                    {
                        if (cx < 0 || cx >= 50 || cy < 0 || cy >= 50)
                        {
                            continue;
                        }

                        if (cx == cellX && 
                            cy == cellY)
                        {
                            continue;
                        }

                        CreateResource(cx, cy, ProduceableResourceCategory.Food);
                    }
                }
            }
        }

        #endregion

        #region Methods

        public void BuildBuilding(float x, float z, BuildingType buildingType)
        {
            if (BuildingExists(x, z))
            {
                return;
            }

            var prefab = Resources
                .Load<BuildingBehaviour>($"Prefabs/Buildings/{buildingType}");

            var building = Instantiate(prefab);

            building.transform.position = new Vector3(x, 0, z);

            building
                .PositionProgressBar();
            
            AddBuilding(x, z, building);

            SoundEffectSource
                .PlayOneShot(BuildingSFX, 1);
        }

        public bool BuildingExists(float x, float z)
        {
            var key = new Tuple<float, float>(x, z);

            return Buildings
                .ContainsKey(key);
        }

        public void AddBuilding(float x, float z, BuildingBehaviour building)
        {
            var key = new Tuple<float, float>(x, z);
            Buildings[key] = building;
            building.BuildComplete += Building_BuildComplete;

            var categories = BuildingResourceCostCategories[building.BuildingType];
            var amounts = BuildingResourceCostAmounts[building.BuildingType];

            for (int i = 0; i < categories.Count; i++)
            {
                var category = categories[i];
                var categoryIndex = (int)category;
                var amount = amounts[i];
                Inventory[categoryIndex] -= amount;
            }
        }

        public void DestroyBuilding(BuildingBehaviour building)
        {
            var key = new Tuple<float, float>(building.transform.position.x, building.transform.position.z);

            if(!Buildings.ContainsKey(key))
            {
                return;
            }

            if (Buildings[key] != building)
            {
                return;
            }

            Buildings
                .Remove(key);

            Destroy(building.gameObject);

        }
        public void DestroyResource(ResourceBehaviour resourceBehaviour)
        {
            var key = new Tuple<float, float>(resourceBehaviour.transform.position.x + 25, resourceBehaviour.transform.position.z + 25);

            if (!ResourceObjects.ContainsKey(key))
            {
                return;
            }

            if (ResourceObjects[key] != resourceBehaviour)
            {
                return;
            }

            ResourceObjects
                .Remove(key);

            Destroy(resourceBehaviour.gameObject);
        }

        public void CreateResource(int x, int z, ProduceableResourceCategory resourceCategory)
        {
            var key = new Tuple<float, float>(x, z);

            if (ResourceObjects
                .ContainsKey(key))
            {
                return;
            }

            int resourceCategoryIndex = (int)resourceCategory;

            var prefabName = $"{resourceCategory}";

            if (resourceCategory == ProduceableResourceCategory.Wood)
            {
                int treeType = UnityEngine
                    .Random
                    .Range(1, 5);

                prefabName = $"Wood_{treeType}";
            }

            var prefab = Resources
                .Load<ResourceBehaviour>($"Prefabs/Resources/{prefabName}");

            var resourceObject = Instantiate(prefab);
            resourceObject.transform.position = new Vector3(x - 25, 0, z - 25);
            resourceObject.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            resourceObject.AmounLeft = resourceObject.StartingAmount;

            ResourceObjects[key] = resourceObject;

            WorldMap[z, x] = resourceCategoryIndex;
        }

        protected void CreateWorldMap()
        {
            WorldMap = new int[MapHeight, MapWidth];

            List<ProduceableResourceCategory> potentialResources = new List<ProduceableResourceCategory>
            {
                ProduceableResourceCategory.Wood,
                ProduceableResourceCategory.Wood,
                ProduceableResourceCategory.Wood,
                ProduceableResourceCategory.Gingerbread,
                ProduceableResourceCategory.Coal
            };

            for (int z = 0; z < MapHeight; z++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    WorldMap[z, x] = -1;

                    if (UnityEngine.Random.Range(0, 100) > 80)
                    {
                        var resourceIndex = UnityEngine
                            .Random
                            .Range(0, potentialResources.Count);

                        CreateResource(x, z, potentialResources[resourceIndex]);
                    }
                }
            }
        }

        protected void UpdateCheerBar()
        {
            var ratio = ChristmasCheer / MaxChristmasCheer;

            ChristmasCheerbar.fillAmount = ratio;
        }

        protected void DoEndOfYear()
        {
            bool passedYear = true;
            foreach (var tuple in CurrentYearEndGoals)
            {
                var resourceCategory = tuple.Item1;
                var resourceCategoryIndex = (int)resourceCategory;
                var amountRequired = tuple.Item2;

                var amountOnHand = Inventory[resourceCategoryIndex];
                if (amountOnHand < amountRequired)
                {
                    passedYear = false;
                }

                Inventory[resourceCategoryIndex] -= amountRequired;
                if (Inventory[resourceCategoryIndex] <= 0)
                {
                    Inventory[resourceCategoryIndex] = 0;
                }
            }

            Inventory[(int)ProduceableResourceCategory.Present1] = 0;

            if (passedYear)
            {
                SoundEffectSource
                    .PlayOneShot(WinSFX, 1);
                ChristmasCheer += ChristmasCheerPerYearEnd;
            }
            else
            {
                SoundEffectSource
                    .PlayOneShot(LoseSFX, 1);
                ChristmasCheer -= ChristmasCheerPerYearEnd;
            }

            UpdateCheerBar();            
            IsYearOver = true;

            if (passedYear)
            {
                Dialogue
                    .ShowYearMessage(Year + 1);
            }
            else
            {
                Year--;
                Dialogue
                    .ShowYearMessage(Year + 1);
            }
        }

        public ResourceBehaviour GetResourceSurroundingPosition(Vector3 position, ProduceableResourceCategory category)
        {
            var cellX = (int)Mathf
                .Round(position.x + 25);

            var cellY = (int)Mathf
                .Round(position.z + 25);

            for (int cy = cellY - 1; cy <= cellY + 1; cy++)
            {
                for (int cx = cellX - 1; cx <= cellX + 1; cx++)
                {
                    if (cx < 0 || cx >= 50 || cy < 0 || cy >= 50)
                    {
                        continue;
                    }

                    if (cx == cellX && 
                        cy == cellY)
                    {
                        continue;
                    }

                    var key = new Tuple<float, float>(cx, cy);
                    if (!ResourceObjects
                        .ContainsKey(key))
                    {
                        continue;
                    }

                    var resourceObject = ResourceObjects[key];

                    if (resourceObject.ResourceCategory != category)
                    {
                        continue;
                    }

                    if (resourceObject.AmounLeft <= 0)
                    {
                        continue;
                    }

                    return resourceObject;
                }
            }

            return null;
        }

        public int CountResourcesForBuilding(Vector3 position, int categoryIndex)
        {
            int resourcesFound = 0;

            var cellX = (int)Mathf
                .Round(position.x + 25);

            var cellY = (int)Mathf
                .Round(position.z + 25);

            for (int cy = cellY - 1; cy <= cellY + 1; cy++)
            {
                for (int cx = cellX - 1; cx <= cellX + 1; cx++)
                {
                    if (cx < 0 || cx >= 50 || cy < 0 || cy >= 50)
                    {
                        continue;
                    }

                    if (cx == cellX && 
                        cy == cellY)
                    {
                        continue;
                    }

                    var mapValue = WorldMap[cy, cx];
                    if (mapValue != categoryIndex)
                    {
                        continue;
                    }

                    resourcesFound++;
                }
            }

            return resourcesFound;
        }

        protected void ProduceResources()
        {
            foreach (var building in Buildings.Values)
            {
                if (!building.IsBuilt)
                {
                    continue;
                }

                var workers = building.WorkersPresent;

                if( building.BuildingType == BuildingType.ArtificersHouse ||
                    building.BuildingType == BuildingType.ArtisansHouse ||
                    building.BuildingType == BuildingType.ElfHouse)
                {
                    workers = 1;
                }

                bool allRequiredResourcesOnHand = true;
                for (int i = 0; i < building.ResourcesConsumedCategories.Length; i++)
                {
                    var category = building.ResourcesConsumedCategories[i];
                    var categoryIndex = (int)category;
                    var amount = building.ResourcesConsumedAmounts[i] * Time.deltaTime;

                    if (amount > Inventory[categoryIndex])
                    {
                        allRequiredResourcesOnHand = false;
                        break;
                    }
                }

                if (!allRequiredResourcesOnHand)
                {
                    continue;
                }

                for (int i = 0; i < building.ResourcesConsumedCategories.Length; i++)
                {
                    var category = building.ResourcesConsumedCategories[i];
                    var categoryIndex = (int)category;
                    var amount = building.ResourcesConsumedAmounts[i] * Time.deltaTime;

                    Inventory[categoryIndex] -= amount;
                }

                for (int i = 0; i < building.ResourcesProducedCategories.Length; i++)
                {
                    var category = building.ResourcesProducedCategories[i];
                    var categoryIndex = (int)category;

                    bool requiresResource = false;
                    ResourceBehaviour resourceFound = null;
                    if (category == ProduceableResourceCategory.Coal ||
                        category == ProduceableResourceCategory.Gingerbread ||
                        category == ProduceableResourceCategory.Steel ||
                        category == ProduceableResourceCategory.Wood)
                    {
                        resourceFound = GetResourceSurroundingPosition(building.transform.position, category);
                        requiresResource = true;
                    }
                    else if (category == ProduceableResourceCategory.Food &&
                        building.BuildingType == BuildingType.Lumbercamp)
                    {
                        resourceFound = GetResourceSurroundingPosition(building.transform.position, category);
                        requiresResource = true;
                    }
                    else if (category == ProduceableResourceCategory.Food &&
                        building.BuildingType == BuildingType.Farm)
                    {
                        resourceFound = GetResourceSurroundingPosition(building.transform.position, category);
                        requiresResource = true;
                    }

                    if (resourceFound != null)
                    {
                        resourceFound.AmounLeft -= resourceFound.PerTickUsedAmount * Time.deltaTime;
                        if (resourceFound.AmounLeft <= 0)
                        {
                            resourceFound.AmounLeft = 0;
                            DestroyResource(resourceFound);
                        }
                    }

                    if (requiresResource && resourceFound == null)
                    {
                        continue;
                    }

                    var amount = building.ResourcesProducedAmounts[i];

                    amount *= workers;

                    Inventory[categoryIndex] += amount * Time.deltaTime;
                }
            }
        }

        public bool IsBuildingAvailable(BuildingType buildingType)
        {
            if (BuildingTypesAvailable[buildingType] == false)
            {
                return false;
            }

            var categories = BuildingResourceCostCategories[buildingType];
            var amounts = BuildingResourceCostAmounts[buildingType];

            for (int i = 0; i < categories.Count; i++)
            {
                var category = categories[i];
                var categoryIndex = (int)category;
                var amount = amounts[i];
                var onHand = Inventory[categoryIndex];

                if (onHand < amount)
                {
                    return false;
                }
            }

            return true;
       }

        public void StartNewYear(int year)
        {
            if (year >= AllYearEndGoals.Count)
            {
                Dialogue
                    .ShowYearMessage(3);

                return;
            }

            Inventory[(int)ProduceableResourceCategory.Food] = 100;
            Inventory[(int)ProduceableResourceCategory.Wood] = 200;
            Inventory[(int)ProduceableResourceCategory.Cookies] = 500;
            Inventory[(int)ProduceableResourceCategory.Gingerbread] = 100;

            CurrentYearEndGoals = AllYearEndGoals[year];
            Year = year;

            ToDoList
                .SetGoals(CurrentYearEndGoals);

            YearTimeLeft = YearTimeToStart;
            IsYearOver = false;

            var roads = FindObjectOfType<RoadsBehaviour>();

            roads
                .ClearAllRoads();

            var toDestroy = Buildings
                .Values
                .ToList();

            foreach (var building in toDestroy)
            {
                DestroyBuilding(building);
            }

            Buildings
                .Clear();
           
            BuildBuilding(0, 0, BuildingType.SantasWorkshop);

            BuildingWorkersPanel
                .gameObject
                .SetActive(false);
        }

        protected void Update()
        {
            if (IsYearOver)
            {
                return;
            }
            
            ProduceResources();

            YearTimeLeft -= Time.deltaTime * YearTimeSpeed;
            if (YearTimeLeft <= 0)
            {
                DoEndOfYear();
            }            
        }

        protected void Awake()
        {
            Year = -1;
            IsYearOver = true;

            Dialogue = FindObjectOfType<DialogueBehaviour>();

            BuildingTypesAvailable = new Dictionary<BuildingType, bool>
            {
                [BuildingType.Lumbercamp] = true,
                [BuildingType.ElfHouse] = true,
                [BuildingType.Farm] = true,
                [BuildingType.HuntingLodge] = false,
                [BuildingType.GingerbreadQuarry] = true,
                [BuildingType.CoalMine] = true,
                [BuildingType.Workshop1] = true,
                [BuildingType.Workshop2] = false,
                [BuildingType.Workshop3] = false,
                [BuildingType.Refinery] = false,
                [BuildingType.ArtisansHouse] = true,
                [BuildingType.ArtificersHouse] = false,
                [BuildingType.SantasWorkshop] = false
            };

            BuildingResourceCostCategories = new Dictionary<BuildingType, List<ProduceableResourceCategory>>();
            BuildingResourceCostAmounts = new Dictionary<BuildingType, List<float>>();

            var buildingTypes = Enum
                .GetValues(typeof(BuildingType));

            foreach (BuildingType buildingType in buildingTypes)
            {
                var prefab = Resources
                    .Load<BuildingBehaviour>($"Prefabs/Buildings/{buildingType}");

                BuildingResourceCostCategories[buildingType] = prefab
                    .ResourceCostCategories
                    .ToList();

                BuildingResourceCostAmounts[buildingType] = prefab
                    .ResourceCostAmounts
                    .ToList();
            }

            Buildings = new Dictionary<Tuple<float, float>, BuildingBehaviour>();
            ResourceObjects = new Dictionary<Tuple<float, float>, ResourceBehaviour>();

            var resources = Enum
                .GetValues(typeof(ProduceableResourceCategory));

            Inventory = new float[resources.Length];

            var workers = Enum
               .GetValues(typeof(WorkerCategory));

            AvailableWorkers = new int[workers.Length];           

            AllYearEndGoals = new List<List<Tuple<ProduceableResourceCategory, float>>>();

            var yearGoals = new List<Tuple<ProduceableResourceCategory, float>>();
            yearGoals
                .Add(new Tuple<ProduceableResourceCategory, float>(ProduceableResourceCategory.Present1, 100));

            AllYearEndGoals
                .Add(yearGoals);

            yearGoals = new List<Tuple<ProduceableResourceCategory, float>>();
            yearGoals
                .Add(new Tuple<ProduceableResourceCategory, float>(ProduceableResourceCategory.Present1, 300));

            AllYearEndGoals
                .Add(yearGoals);

            yearGoals = new List<Tuple<ProduceableResourceCategory, float>>();
            yearGoals
                .Add(new Tuple<ProduceableResourceCategory, float>(ProduceableResourceCategory.Present1, 700));

            AllYearEndGoals
                .Add(yearGoals);

            ToDoList = FindObjectOfType<ToDoListBehaviour>(true);

            ChristmasCheer = StartingChristmasCheer;

            UpdateCheerBar();
            CreateWorldMap();
        }

        #endregion
    }
}