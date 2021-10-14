using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Windows;

using SharpCrokite.Core.StaticDataUpdater.EsiJsonModels;

namespace SharpCrokite.Core.StaticDataUpdater
{
    public class EsiStaticDataRetriever
    {
        private const string EsiBaseUrl = "https://esi.evetech.net/latest/";
        private const string MaterialContentUrl = "http://sde.zzeve.com/invTypeMaterials.json";
        private const string EveTechBaseUrl = "https://images.evetech.net/types/";
        private const string UniverseRoute = "universe/";
        private const string CategoriesRoutePart = "categories/";
        private const string GroupsRoutePart = "groups/";
        private const string TypesRoutePart = "types/";
        private const string IconRoutePart = "icon/";

        private List<EsiCategoryJson> categories;
        private int maxTries = 3;

        private List<EsiCategoryJson> Categories
        {
            get
            {
                if(categories == null)
                {
                    LoadCategoriesFromEsi();
                }
                return categories;
            }
        }

        public IEnumerable<IEnumerable<EsiTypeJson>> RetrieveAsteroidTypesPerGroup()
        {
            using HttpClient client = new();
            client.BaseAddress = new Uri(EsiBaseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            EsiCategoryJson asteroidCategory = Categories.Single(c => c.name == "Asteroid");

            IEnumerable<EsiGroupJson> asteroidGroups = GetGroupsFromCategory(client, asteroidCategory);

            List<IEnumerable<EsiTypeJson>> asteroidListPerType = new();

            foreach (EsiGroupJson asteroidGroup in asteroidGroups)
            {
                asteroidListPerType.Add(GetTypesPerGroup(client, asteroidGroup));
            }

            return asteroidListPerType;
        }

        public IEnumerable<IEnumerable<EsiTypeJson>> RetrieveMaterialTypesPerGroup()
        {
            using HttpClient client = new();
            client.BaseAddress = new Uri(EsiBaseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            EsiCategoryJson materialCategory = Categories.Single(c => c.name == "Material");

            IEnumerable<EsiGroupJson> materialGroups = GetGroupsFromCategory(client, materialCategory)
                .Where(g => g.name == "Mineral" || g.name == "Moon Materials" || g.name == "Ice Product");

            List<IEnumerable<EsiTypeJson>> materialTypesPerGroup = new();

            foreach (EsiGroupJson materialGroup in materialGroups)
            {
                materialTypesPerGroup.Add(GetTypesPerGroup(client, materialGroup));
            }

            return materialTypesPerGroup;
        }

        internal byte[] GetIconForTypeId(int typeId)
        {
            using WebClient client = new();
            Uri uri = new($"{EveTechBaseUrl}{typeId}/{IconRoutePart}");

            byte[] responseStream = client.DownloadData(uri.AbsoluteUri);

            return responseStream;
        }

        public IEnumerable<EsiMaterialContentJson> RetrieveMaterialContent()
        {
            using WebClient client = new();
            Uri uri = new(MaterialContentUrl);

            string responseString = client.DownloadString(uri.AbsoluteUri);

            IEnumerable<EsiMaterialContentJson> listOfMaterialContent = JsonSerializer.Deserialize<List<EsiMaterialContentJson>>(responseString);
            var veldsparmaterial = listOfMaterialContent.Single(m => m.typeID == 1230);
            return listOfMaterialContent;
        }

        private void LoadCategoriesFromEsi()
        {
            using HttpClient client = new();
            client.BaseAddress = new Uri(EsiBaseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync($"{UniverseRoute}{CategoriesRoutePart}").Result;
            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;

                responseString = responseString.TrimStart('[').TrimEnd(']');

                IList<string> categoryIdList = responseString.Split(',').ToList();

                List<EsiCategoryJson> listOfCategories = new();

                foreach (string categoryId in categoryIdList)
                {
                    listOfCategories.Add(GetCategoryInfo(client, categoryId));
                }

                categories = listOfCategories.Where(c => c.published).ToList();
            }
            else
            {
                MessageBox.Show($"Call to {EsiBaseUrl}{UniverseRoute}{CategoriesRoutePart} failed!\nReason:\n{response.ReasonPhrase}\nStatuscode:{response.StatusCode}",
                    "Failed during API call!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private EsiCategoryJson GetCategoryInfo(HttpClient client, string categoryId)
        {
            HttpResponseMessage response = client.GetAsync($"{UniverseRoute}{CategoriesRoutePart}{categoryId}").Result;
            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;

                EsiCategoryJson categoryJson = JsonSerializer.Deserialize<EsiCategoryJson>(responseString);

                return categoryJson;
            }
            else
            {
                MessageBox.Show($"Call to {EsiBaseUrl}{UniverseRoute}{CategoriesRoutePart}{categoryId} failed!\nReason:\n{response.ReasonPhrase}\nStatuscode:{response.StatusCode}",
                    "Failed during API call!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return null;
            }
        }

        private IEnumerable<EsiGroupJson> GetGroupsFromCategory(HttpClient client, EsiCategoryJson category)
        {
            List<EsiGroupJson> listOfGroups = new();
            foreach (int groupId in category.groups)
            {
                HttpResponseMessage response = client.GetAsync($"{UniverseRoute}{GroupsRoutePart}{groupId}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    EsiGroupJson groupJson = JsonSerializer.Deserialize<EsiGroupJson>(responseString);

                    listOfGroups.Add(groupJson);
                }
                else
                {
                    MessageBox.Show($"Call to {EsiBaseUrl}{UniverseRoute}{GroupsRoutePart}{groupId} failed!\nReason:\n{response.ReasonPhrase}\nStatuscode:{response.StatusCode}",
                        "Failed during API call!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            return listOfGroups.Where(g => g.published);
        }

        private IEnumerable<EsiTypeJson> GetTypesPerGroup(HttpClient client, EsiGroupJson group)
        {
            List<EsiTypeJson> typeList = new();
            foreach (int typeid in group.types)
            {
                for (int currentTry = 1; currentTry <= maxTries; currentTry++)
                {
                    HttpResponseMessage response = client.GetAsync($"{UniverseRoute}{TypesRoutePart}{typeid}").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;

                        EsiTypeJson typeJson = JsonSerializer.Deserialize<EsiTypeJson>(responseString);

                        typeList.Add(typeJson);

                        currentTry = maxTries;
                    }
                    else
                    {
                        Debug.WriteLine($"The API call to {EsiBaseUrl}{UniverseRoute}{TypesRoutePart}{typeid} failed. Try: {currentTry}");
                        if (currentTry == 3)
                        {
                            throw new HttpRequestException($"The API call to {EsiBaseUrl}{UniverseRoute}{TypesRoutePart}{typeid} failed three times!\n" +
                                $"Reason:\n{response.ReasonPhrase}\n" +
                                $"Statuscode:{response.StatusCode}");
                        }
                    }
                }
            }
            return typeList.Where(t => t.published && t.market_group_id != 0);
        }
    }
}
