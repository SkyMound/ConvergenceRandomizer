using System.Collections;
using UnityEngine;
using System;
using System.IO;
using System.IO.Pipes;
using DS.Tech.World;
using System.Collections.Generic;

namespace Randomizer
{
    public class RandomizerManager : MonoBehaviour
    {


        public string ArrangementFolder;
        public string Version { get; private set; } = "0.0.1";

        private static RandomizerManager _instance;
        public static RandomizerManager Instance { get { return _instance; } }

        
        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 150, 20), "CR_" + Version);
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        // Use this for initialization
        void Start()
        {
            //Debugger.Init();
            System.Diagnostics.Debug.WriteLine("here------------------------------------------------------");
            StartCoroutine(Init());
            System.Diagnostics.Debug.WriteLine("after_coroutine------------------------------------------------------");
            WorldGraph.Instance.RoomNodes = new List<RoomNode>();
        }

        private IEnumerator Init()
        {
            System.Diagnostics.Debug.WriteLine("working------------------------------------------------------");
            
            string projectPath = string.Empty;
            using (NamedPipeClientStream clientPipe = new NamedPipeClientStream(".", "PipeCR", PipeDirection.In))
            {
                while (!clientPipe.IsConnected)
                {
                    try
                    {
                        clientPipe.Connect();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("not connecting------------------------------------------------------");
                    }
                    yield return null;
                }

                using (StreamReader reader = new StreamReader(clientPipe))
                {
                    // Read the path of the 'target' folder from the named pipe
                    projectPath = reader.ReadLine();
                }
            }

            if (!string.IsNullOrEmpty(projectPath))
            {
                ArrangementFolder = Path.Combine(projectPath, "Arrangements");
                System.Diagnostics.Debug.WriteLine(ArrangementFolder);
            }
        }
    }
}