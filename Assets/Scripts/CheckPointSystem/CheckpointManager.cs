using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using SA;
using SaveSystem;
using SaveSystem.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CheckPointSystem
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class CheckpointManager : MonoBehaviour
    {
        public static CheckpointManager instance;

        //put checkpoints in order in which they will appear in level
        public List<Checkpoint> checkpoints = new List<Checkpoint>();
        public List<Checkpoint> activeCheckpoints = new List<Checkpoint>();

        public int currentIndex;
        [HideInInspector] public Checkpoint previousCheckpoint;
        [HideInInspector] public Checkpoint currentCheckpoint;

        private PlayerStats player;

        public GameObject CheckpointUI;
        public GameObject travelMenuUi;
        public bool UIOpen;
        public Transform TraverseArea;
        public GameObject buttonPrefab;
        public float buttonPadding;
        [HideInInspector] public List<GameObject> buttonPrefabs = new List<GameObject>();


        void Awake()
        {
            instance = this;
            player = FindObjectOfType<PlayerStats>();
            activeCheckpoints.Clear();
            buttonPrefabs.Clear();
            CloseUI();
            InitializeLevel();
        }

        //at the end of a scene load this function again
        public void InitializeLevel()
        {
            // Debug.Log("Level Index is: " + SaveData.current.levelIndex);

            if (Directory.Exists(Application.persistentDataPath + "/saves/"))
            {
                LoadData();
                // Debug.Log("Level Index is: " + SaveData.current.levelIndex);
                if (SaveData.current.levelIndex != SceneManager.GetActiveScene().buildIndex)
                {
                    SaveData.current.levelIndex = SceneManager.GetActiveScene().buildIndex;
                    SaveData.current.checkpointIndex = 0;
                    currentIndex = SaveData.current.checkpointIndex = 0;
                    currentCheckpoint = checkpoints[currentIndex];
                    previousCheckpoint = currentCheckpoint;
                    SerializationManager.Save("Save", SaveData.current);
                    UpdateIndex();
                }
                else
                {
                    currentIndex = SaveData.current.checkpointIndex;
                    UpdateIndex();
                }
            }
            else
            {
                currentIndex = 0;
                currentCheckpoint = checkpoints[currentIndex];
                previousCheckpoint = currentCheckpoint;
                UpdateIndex();
            }
        }

        public void LoadData()
        {
            SaveData.current =
                (SaveData) SerializationManager.Load(Application.persistentDataPath + "/saves/Save.save");
            UpdateIndex();
            RefreshCheckpoints();
        }

        public void RefreshCheckpoints()
        {
            for (int i = 0; i <= currentIndex; i++)
            {
                checkpoints[i].isTriggered = true;
                activeCheckpoints.Add(checkpoints[i]);
                // Debug.Log(checkpoints[i].isTriggered);
            }
        }

        public void UpdateIndex()
        {
            currentIndex = SaveData.current.checkpointIndex;
            //checkpoints[currentIndex].isTriggered = true;
            GoToCheckpoint(checkpoints[currentIndex]);
        }

        private void FixedUpdate()
        {
            if (UIOpen)
            {
                OpenUI();
            }
            else
            {
                CloseUI();
            }
        }

        public void OpenUI()
        {
            CheckpointUI.SetActive(true);
            //Debug.Log("Open Menu");
        }

        public void CloseUI()
        {
            CheckpointUI.SetActive(false);
            travelMenuUi.SetActive(false);
            UIOpen = false;
        }

        public void CloseTravelMenu()
        {
            CheckpointUI.SetActive(true);
            travelMenuUi.SetActive(false);
        }

        public void GoToCheckpoint(Checkpoint checkpoint)
        {
            //Debug.Log("Going To Checkpoint" + checkpoint.name);
            if (checkpoint.isTriggered)
            {
                //Debug.Log("Goto" + checkpoint.name);
                UpdateCheckpoint(checkpoint);
                CloseUI();

                player.Die();
            }
            else
            {
                //Debug.Log(checkpoint.name + "Isn't Active");
            }
        }

        public void OpenTravelMenu()
        {
            CheckpointUI.SetActive(false);
            travelMenuUi.SetActive(true);
            foreach (Transform button in TraverseArea)
            {
                Destroy(button.gameObject);
            }
            
            buttonPrefabs.Clear();
            
            for (int i = 0; i < activeCheckpoints.Count; i++)
            {
                GameObject buttonObject = Instantiate(buttonPrefab);
                buttonObject.transform.SetParent(TraverseArea.transform, false);
                
                buttonPrefabs.Add(buttonObject);
            }
            
            for (int j = 0; j < buttonPrefabs.Count; j++)
            {
                
                var index = j;

                Vector3 newPos = new Vector3(TraverseArea.transform.position.x, buttonPrefabs[j].transform.position.y + 80f - (j * buttonPadding),
                    TraverseArea.transform.position.z);
                Debug.Log(newPos);

                buttonPrefabs[j].transform.position = newPos;

                buttonPrefabs[j].GetComponentInChildren<Button>().onClick
                    .AddListener(() => GoToCheckpoint(activeCheckpoints[index]));
                buttonPrefabs[j].GetComponentInChildren<Text>().text = activeCheckpoints[j].name;
            }
        }

        // public Vector3 GetNewButtonPos(GameObject button)
        // {
        //     Vector3 newPos = new Vector3(TraverseArea.transform.position.x, TraverseArea.transform.position.y + buttonPadding,
        //         TraverseArea.transform.position.z);
        //     Debug.Log(newPos);
        //     return newPos;
        // }

        public void UpdateCheckpoint(Checkpoint checkpoint)
        {
            //if trying to set the checkpoint this way doesnt work then just go through
            //each checkpoint in checkpoints list set them to false
            //and set the passed in checkpoint is triggered bool to true


            previousCheckpoint = checkpoint;
            currentIndex = checkpoints.IndexOf(checkpoint);
            currentCheckpoint = checkpoints[currentIndex];

            if (checkpoint.isTriggered)
            {
                if (!activeCheckpoints.Contains(checkpoint))
                {
                    activeCheckpoints.Add(checkpoint);
                }
            }

            SaveData.current.checkpointIndex = currentIndex;
        }
    }
}