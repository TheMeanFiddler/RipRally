
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts
{
    class AssetRefLoader
    {
        public async Task CreateAssetAddToList<T>(AssetReference asRef, IList<T> completedObjects) where T : Object
        {
            completedObjects.Add(await asRef.InstantiateAsync().Task as T);
        }

        public async Task CreateAssetsAddToList<T>(IList<AssetReference> asRefs, IList<T> completedObjects) where T : Object
        {
            foreach(AssetReference asRef in asRefs)
            {
                completedObjects.Add(await asRef.InstantiateAsync().Task as T);
            }
            
        }
    }
}
