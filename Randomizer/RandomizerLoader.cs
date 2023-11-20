
using UnityEngine;

namespace Randomizer
{
    public class RandomizerLoader 
    {
        private static GameObject RandomizerObject;

        public static void Init()
        {
            System.Diagnostics.Debug.WriteLine("first------------------------------------------------------");
            RandomizerObject = new GameObject();
            RandomizerObject.AddComponent<RandomizerManager>();
            GameObject.DontDestroyOnLoad(RandomizerObject);
        }
    }
}
