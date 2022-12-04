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

        public Image ChristmasCheerbar;

        public AudioClip[] MusicSongs;

        public AudioSource[] AudioSources;

        public float MusicSegmentLength;

        public Dictionary<Tuple<float, float>, BuildingBehaviour> Buildings { get; set; }

        public Dictionary<Tuple<float, float>, ResourceBehaviour> ResourceObjects { get; set; }

        public float[] Inventory { get; set; }

        public int[] AvailableWorkers { get; set; }

        public int[,] WorldMap { get; set; }

        public float ChristmasCheer { get; set; }

        public float YearTimeLeft { get; set; }

        public float Year { get; set; }

        public List<List<Tuple<ProduceableResourceCategory, float>>> AllYearEndGoals { get; set; }

        public List<Tuple<ProduceableResourceCategory, float>> CurrentYearEndGoals { get; set; }

        public Dictionary<BuildingType, List<ProduceableResourceCategory>> BuildingResourceCostCategories { get; set; }

        public Dictionary<BuildingType, List<float>> BuildingResourceCostAmounts { get; set; }

        protected ToDoListBehaviour ToDoList { get; set; }

        protected Dictionary<BuildingType, bool> BuildingTypesAvailable { get; set; } 
        
        protected bool IsYearOver { get; set; }

        protected int CurrentMusic { get; set; }

        protected int CurrentAudioSourceIndex { get; set; }

        protected int OtherAudioSourceIndex { get; set; }

        protected float CurrentMusicTimer { get; set; }

        protected float FadeOutVolume { get; set; }

        protected float FadeInVolume { get; set; }

        protected bool MusicIsFading { get; set; }

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

            if (Buildings.Count > 1)
            {
                var roads = FindObjectOfType<RoadsBehaviour>();

                var firstVertex = new Vector3(building.transform.position.x, -0.5f, building.transform.position.z);
                var secondVertex = new Vector3(0, -0.5f, 0);

                roads
                    .AddRoads(firstVertex, secondVertex);

                /*
                 //var secondVertex = new Vector3(building.transform.position.x, -0.5f, building.transform.position.z);
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
                */
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

        public void DestroyResource(ResourceBehaviour resourceBehaviour)
        {
            var key = new Tuple<float, float>(resourceBehaviour.transform.position.x + 25, resourceBehaviour.transform.position.z + 25);

            if (!ResourceObjects.ContainsKey(key))
            {
                return;
            }

            if (!ResourceObjects[key] == resourceBehaviour)
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

            var prefab = Resources
                .Load<ResourceBehaviour>($"Prefabs/Resources/{resourceCategory}");

            var resourceObject = Instantiate(prefab);
            resourceObject.transform.position = new Vector3(x - 25, 0, z - 25);
            resourceObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
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
            
            IsYearOver = true;
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

        protected void StartNewYear(int year)
        {
            CurrentYearEndGoals = AllYearEndGoals[0];
            Year = year;
            IsYearOver = false;

            ToDoList
                .SetGoals(CurrentYearEndGoals);
        }

        protected void ChangeMusic(int index)
        {
            if (index > 5)
            {
                return;
            }

            OtherAudioSourceIndex = 1 - CurrentAudioSourceIndex;

            var currentAudioSource = AudioSources[CurrentAudioSourceIndex];
            var otherAudioSource = AudioSources[OtherAudioSourceIndex];

            //float time = 0;
            if (otherAudioSource.isPlaying)
            {
                //time = otherAudioSource.time;
            }

            currentAudioSource
                .Stop();

            currentAudioSource.clip = MusicSongs[index];
            //currentAudioSource.time = time;
            currentAudioSource
                .Play();

            FadeInVolume = 0;
            FadeOutVolume = 1;

            MusicIsFading = true;
            CurrentMusic = index;
            CurrentMusicTimer = MusicSegmentLength;
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

            if (MusicIsFading)
            {
                FadeInVolume += Time.deltaTime;
                if (FadeInVolume >= 1)
                {
                    FadeInVolume = 1;
                }

                AudioSources[CurrentAudioSourceIndex].volume = FadeInVolume;

                FadeOutVolume -= Time.deltaTime;
                if (FadeOutVolume <= 0)
                {
                    FadeOutVolume = 0;
                }

                AudioSources[OtherAudioSourceIndex].volume = FadeOutVolume;

                if (FadeInVolume == 1 && 
                    FadeOutVolume == 0)
                {
                    MusicIsFading = false;
                    AudioSources[OtherAudioSourceIndex]
                        .Stop();

                    CurrentAudioSourceIndex = OtherAudioSourceIndex;
                }
            }

            CurrentMusicTimer -= Time.deltaTime;
            if (CurrentMusicTimer <= 0)
            {
                ChangeMusic(CurrentMusic + 1);
            }
        }

        protected void Awake()
        {
            CurrentAudioSourceIndex = 0;
            ChangeMusic(0);

            BuildingTypesAvailable = new Dictionary<BuildingType, bool>
            {
                [BuildingType.Lumbercamp] = true,
                [BuildingType.ElfHouse] = true,
                [BuildingType.Farm] = true,
                [BuildingType.HuntingLodge] = false,
                [BuildingType.GingerbreadQuarry] = true,
                [BuildingType.CoalMine] = false,
                [BuildingType.Workshop1] = true,
                [BuildingType.Workshop2] = false,
                [BuildingType.Workshop3] = false,
                [BuildingType.Refinery] = false,
                [BuildingType.ArtisansHouse] = true,
                [BuildingType.ArtificersHouse] = false
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
            Inventory[(int)ProduceableResourceCategory.Food] = 100;
            Inventory[(int)ProduceableResourceCategory.Wood] = 200;
            Inventory[(int)ProduceableResourceCategory.Cookies] = 100;
            Inventory[(int)ProduceableResourceCategory.Gingerbread] = 100;

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

            ToDoList = FindObjectOfType<ToDoListBehaviour>(true);

            StartNewYear(0);

            ChristmasCheer = StartingChristmasCheer;

            UpdateCheerBar();
            CreateWorldMap();

            BuildBuilding(0, 0, BuildingType.Workshop1);
        }

        #endregion
    }
}