using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Windows;

using SharpCrokite.Core.StaticDataUpdater.JSONModels;

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

        private List<ICategoryJSON> categories;
        private int maxTries = 3;

        private List<ICategoryJSON> Categories
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

        public IEnumerable<IEnumerable<ITypeJSON>> RetrieveAsteroidTypesPerGroup()
        {
            using HttpClient client = new();
            client.BaseAddress = new Uri(EsiBaseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            ICategoryJSON asteroidCategory = Categories.Single(c => c.name == "Asteroid");

            IEnumerable<IGroupJSON> asteroidGroups = GetGroupsFromCategory(client, asteroidCategory);

            List<IEnumerable<ITypeJSON>> asteroidListPerType = new();

            foreach (IGroupJSON asteroidGroup in asteroidGroups)
            {
                asteroidListPerType.Add(GetTypesPerGroup(client, asteroidGroup));
            }

            return asteroidListPerType;
        }

        public IEnumerable<IEnumerable<ITypeJSON>> RetrieveMaterialTypesPerGroup()
        {
            using HttpClient client = new();
            client.BaseAddress = new Uri(EsiBaseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            ICategoryJSON materialCategory = Categories.Single(c => c.name == "Material");

            IEnumerable<IGroupJSON> materialGroups = GetGroupsFromCategory(client, materialCategory)
                .Where(g => g.name == "Mineral" || g.name == "Moon Materials" || g.name == "Ice Product");

            List<IEnumerable<ITypeJSON>> materialTypesPerGroup = new();

            foreach (IGroupJSON materialGroup in materialGroups)
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

        public IEnumerable<IMaterialContentJSON> RetrieveMaterialContent()
        {
            using WebClient client = new();
            Uri uri = new(MaterialContentUrl);

            string responseString = client.DownloadString(uri.AbsoluteUri);

            IEnumerable<IMaterialContentJSON> listOfMaterialContent = JsonSerializer.Deserialize<List<MaterialContentJSON>>(responseString);
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

                List<ICategoryJSON> listOfCategories = new();

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

        private ICategoryJSON GetCategoryInfo(HttpClient client, string categoryId)
        {
            HttpResponseMessage response = client.GetAsync($"{UniverseRoute}{CategoriesRoutePart}{categoryId}").Result;
            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;

                ICategoryJSON categoryJSON = JsonSerializer.Deserialize<CategoryJSON>(responseString);

                return categoryJSON;
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

        private IEnumerable<IGroupJSON> GetGroupsFromCategory(HttpClient client, ICategoryJSON category)
        {
            List<IGroupJSON> listOfGroups = new();
            foreach (int groupId in category.groups)
            {
                HttpResponseMessage response = client.GetAsync($"{UniverseRoute}{GroupsRoutePart}{groupId}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;

                    IGroupJSON groupJSON = JsonSerializer.Deserialize<GroupJSON>(responseString);

                    listOfGroups.Add(groupJSON);
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

        private IEnumerable<ITypeJSON> GetTypesPerGroup(HttpClient client, IGroupJSON group)
        {
            List<ITypeJSON> typeList = new();
            foreach (int typeid in group.types)
            {
                for (int currentTry = 1; currentTry <= maxTries; currentTry++)
                {
                    HttpResponseMessage response = client.GetAsync($"{UniverseRoute}{TypesRoutePart}{typeid}").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;

                        ITypeJSON typeJSON = JsonSerializer.Deserialize<TypeJSON>(responseString);

                        typeList.Add(typeJSON);

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
