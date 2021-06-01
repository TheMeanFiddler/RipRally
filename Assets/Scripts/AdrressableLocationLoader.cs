using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

    public static class AddressableLocationLoader
    {
        public static async Task GetLocations(string label, IList<IResourceLocation> loadedLocations)
        {
            var unloadedLocations = await Addressables.LoadResourceLocationsAsync(label).Task;
            foreach (var item in unloadedLocations)
            {
                loadedLocations.Add(item);
            }
        }
    }

