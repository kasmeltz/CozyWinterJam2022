namespace HNS.CozyWinterJam2022.Behaviours
{
    using HNS.CozyWinterJam2022.Models;
    using System;
    using System.Collections.Generic;
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

        public Image ChristmasCheerbar;

        public Dictionary<Tuple<float, float>, BuildingBehaviour> Buildings { get; set; }

        public float[] Inventory { get; set; }

        public int[] AvailableWorkers { get; set; }
      
        public int[,] WorldMap { get; set; }

        public float ChristmasCheer { get; set; }

        public float YearTimeLeft { get; set; }

        public float Year { get; set; }

        public List<List<Tuple<ProduceableResourceCategory, float>>> AllYearEndGoals { get; set; }

        public List<Tuple<ProduceableResourceCategory, float>> CurrentYearEndGoals { get; set; }

        protected ToDoListBehaviour ToDoList { get; set; }

        protected Dictionary<BuildingType, bool> BuildingTypesAvailable { get; set; }

        protected Dictionary<BuildingType, List<ProduceableResourceCategory>> BuildingResourceCostCategories { get; set; }
        
        protected Dictionary<BuildingType, List<float>> BuildingResourceCostAmounts { get; set; }

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
            
            /*
            if (Buildings.Count > 1)
            {
                var roads = FindObjectOfType<RoadsBehaviour>();

                var firstVertex = new Vector3(building.transform.position.x, -0.5f, building.transform.position.z);
                var secondVertex = new Vector3(building.transform.position.x, -0.5f, building.transform.position.z);

                float minDistance = float.MaxValue;
                foreach (var kvp in Buildings)
                {
                    var otherCoordinates = kvp.Key;

                    var dx = otherCoordinates.Item1 - firstVertex.x;
                    var dy = otherCoordinates.Item2 - firstVertex.z;

                    var d = (dx * dx) + (dy * dy);
                    if (d > 0 && d < minDistance)
                    {
                        minDistance = d;
                        secondVertex.x = otherCoordinates.Item1;
                        secondVertex.z = otherCoordinates.Item2;
                    }
                }

                roads
                    .AddRoads(firstVertex, secondVertex);

                //roads
                    //.AddRoads(firstVertex, new Vector3(0,-0.5f,0));
            }
            */
        }

        #endregion

        #region Methods

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

        protected void CreateWorldMap()
        {
            WorldMap = new int[MapHeight, MapWidth];

            List<ProduceableResourceCategory> potentialResources = new List<ProduceableResourceCategory>
            {
                ProduceableResourceCategory.Wood,
                ProduceableResourceCategory.Gingerbread,
                ProduceableResourceCategory.Coal,
                ProduceableResourceCategory.Steel,
            };

            for (int z = 0; z < MapHeight; z++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    WorldMap[z, x] = -1;

                    if (UnityEngine.Random.Range(0, 100) > 70)
                    {
                        var resourceIndex = UnityEngine
                            .Random
                            .Range(0, potentialResources.Count);

                        var resourceType = potentialResources[resourceIndex];
                        var resourceTypeIndex = (int)resourceType;

                        var prefab = Resources
                            .Load<ResourceBehaviour>($"Prefabs/Resources/{resourceType}");

                        var resourceObject = Instantiate(prefab);
                        resourceObject.transform.position = new Vector3(x - 25, 0, z - 25);
                        resourceObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

                        WorldMap[z, x] = resourceTypeIndex;
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
            foreach(var tuple in CurrentYearEndGoals)
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
            }

            if (passedYear)
            {
                ChristmasCheer += ChristmasCheerPerYearEnd;
            }
            else
            {
                ChristmasCheer -= ChristmasCheerPerYearEnd;
            }

            UpdateCheerBar();
        }

        protected void ProduceResources()
        {
            foreach (var building in Buildings.Values)
            {
                if (!building.IsBuilt)
                {
                    continue;
                }

                bool allRequiredResourcesOnHand = true;
                for(int i = 0;i < building.ResourcesConsumedCategories.Length;i++)
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
                    int resourcesFound = 0;

                    if (category == ProduceableResourceCategory.Coal ||
                        category == ProduceableResourceCategory.Gingerbread || 
                        category == ProduceableResourceCategory.Steel ||
                        category == ProduceableResourceCategory.Wood)
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

                                var mapValue = WorldMap[cy, cx];
                                if (mapValue != categoryIndex)
                                {
                                    continue;
                                }

                                resourcesFound++;
                            }
                        }
                    }
                    else
                    {
                        resourcesFound = 1;
                    }

                    // TO DO - HOW DO DIFFERENT RESOURCES AROUND THE BUILDING AFFECT THE PRODUCTION?
                    var amount = building.ResourcesProducedAmounts[i];
                    amount *= resourcesFound;

                    // TO DO - HOW DO DIFFERENT WORKERS AFFECT THE PRODUCTION?
                    var workers = building.WorkersPresent[0];

                    // TO DO - HOW DO MORE WORKERS EFFECT PRODUCTION?
                    var workerBonus = amount * workers;
                    amount += workerBonus;

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

            for (int i = 0; i < categories.Count;i++)
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

        protected void StartNewYear(int year)
        {
            CurrentYearEndGoals = AllYearEndGoals[0];
            Year = year;

            ToDoList
                .SetGoals(CurrentYearEndGoals);
        }

        protected void Update()
        {
            ProduceResources();            

            YearTimeLeft -= Time.deltaTime * YearTimeSpeed;
            if (YearTimeLeft <= 0)
            {
                DoEndOfYear();
            }
        }

        protected void Awake()
        {
            BuildingTypesAvailable = new Dictionary<BuildingType, bool>
            {
                [BuildingType.Lumbercamp] = true,
                [BuildingType.ElfHouse] = true,
                [BuildingType.Farm] = false,
                [BuildingType.HuntingLodge] = false,
                [BuildingType.GingerbreadQuarry] = false,
                [BuildingType.CoalMine] = false,
                [BuildingType.Workshop1] = false,
                [BuildingType.Workshop2] = false,
                [BuildingType.Workshop3] = false,
                [BuildingType.Refinery] = false
            };

            BuildingResourceCostCategories = new Dictionary<BuildingType, List<ProduceableResourceCategory>>();
            BuildingResourceCostAmounts = new Dictionary<BuildingType, List<float>>();

            BuildingResourceCostCategories[BuildingType.Lumbercamp] = new List<ProduceableResourceCategory> { };
            BuildingResourceCostAmounts[BuildingType.Lumbercamp] = new List<float> { };
            BuildingResourceCostCategories[BuildingType.ElfHouse] = new List<ProduceableResourceCategory> { };
            BuildingResourceCostAmounts[BuildingType.ElfHouse] = new List<float> { };
            BuildingResourceCostCategories[BuildingType.Farm] = new List<ProduceableResourceCategory> { };
            BuildingResourceCostAmounts[BuildingType.Farm] = new List<float> { };
            BuildingResourceCostCategories[BuildingType.HuntingLodge] = new List<ProduceableResourceCategory> { };
            BuildingResourceCostAmounts[BuildingType.HuntingLodge] = new List<float> { };
            BuildingResourceCostCategories[BuildingType.GingerbreadQuarry] = new List<ProduceableResourceCategory> { };
            BuildingResourceCostAmounts[BuildingType.GingerbreadQuarry] = new List<float> { };
            BuildingResourceCostCategories[BuildingType.CoalMine] = new List<ProduceableResourceCategory> { };
            BuildingResourceCostAmounts[BuildingType.CoalMine] = new List<float> { };
            BuildingResourceCostCategories[BuildingType.Workshop1] = new List<ProduceableResourceCategory> { };
            BuildingResourceCostAmounts[BuildingType.Workshop1] = new List<float> { };
            BuildingResourceCostCategories[BuildingType.Workshop2] = new List<ProduceableResourceCategory> { };
            BuildingResourceCostAmounts[BuildingType.Workshop2] = new List<float> { };
            BuildingResourceCostCategories[BuildingType.Workshop3] = new List<ProduceableResourceCategory> { };
            BuildingResourceCostAmounts[BuildingType.Workshop3] = new List<float> { };
            BuildingResourceCostCategories[BuildingType.Refinery] = new List<ProduceableResourceCategory> { };
            BuildingResourceCostAmounts[BuildingType.Refinery] = new List<float> { };

            Buildings = new Dictionary<Tuple<float, float>, BuildingBehaviour>();

            var resources = Enum
                .GetValues(typeof(ProduceableResourceCategory));

            Inventory = new float[resources.Length];

            var workers = Enum
               .GetValues(typeof(WorkerCategory));

            AvailableWorkers = new int[workers.Length];

            YearTimeLeft = YearTimeToStart;            

            AllYearEndGoals = new List<List<Tuple<ProduceableResourceCategory, float>>>();

            var firstYearGoals = new List<Tuple<ProduceableResourceCategory, float>>();
            firstYearGoals
                .Add(new Tuple<ProduceableResourceCategory, float>(ProduceableResourceCategory.Present1, 5));

            AllYearEndGoals
                .Add(firstYearGoals);

            ToDoList = FindObjectOfType<ToDoListBehaviour>();

            StartNewYear(0);

            ChristmasCheer = StartingChristmasCheer;

            UpdateCheerBar();
            CreateWorldMap();            
        }
       
        #endregion
    }
}