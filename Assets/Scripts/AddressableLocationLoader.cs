using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using System.Linq;

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
    public static async Task<IResourceLocation> GetLocation(string label)
    {
        AsyncOperationHandle<IList<IResourceLocation>> h1 = Addressables.LoadResourceLocationsAsync(label);
        await h1.Task;
        return h1.Result.FirstOrDefault();
    }
}

