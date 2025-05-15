using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool potatoTask = false;
    public bool cleanTask = false;
    public bool coalTask = false;
    public bool sleepTask = false;
    public bool driveTask = false;
    public bool fecthPotatoesTask = false;
    public bool fecthCoalTask = false;
    public bool hatchUnlocked = false;
    public bool talkTask = false;
    public bool searchForPotatoes = false;
    public bool hasKey = false;

    public GameObject potatoSack;
    public GameObject coalSack;
    public static GameManager _instance;
    public int currentDay = 1;
    public List<string> tasksDay1;
    public List<string> tasksDay2;
    public List<string> tasksDay3;
    public List<string> tasksDay4;
    public List<string> tasksDay5;
    public List<string> tasksDay6;
    public List<string> tasksDay7;
    public List<string> tasksDay10;
    public List<string> currentDayTasks;
    public string currentTask = "";

    private int taskIndex = 0;

    public GameObject willieToHide;

    public DateFade dateFader;

    private bool night1 = true;

    private bool currentTaskDone = false;
    public bool underTheDeck = false;
    public BedInteraction bedInteraction;
    public bool day8reset = false;
    private bool sceneReloaded = false;
    private float reloadTime;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                _instance = go.AddComponent<GameManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Example: Start the game on the first day
        StartDay(currentDay);
    }

    private void LateUpdate()
    {
        if (currentDay == 8)
        {
            currentTask = "";
            bedInteraction.canInteract = false;
            hatchUnlocked = true;
            sleepTask = false;
        }
    }

    void FixedUpdate()
    {
        if (dateFader == null) {
            dateFader = FindAnyObjectByType<DateFade>();
        }
        if (day8reset)
        {
            if (!sceneReloaded)
            {
                currentDay = 7;
                reloadTime = Time.time;
                SceneManager.LoadScene("GameplayScene");
                sceneReloaded = true;
            }
            if (currentDay != 8 && reloadTime != Time.time)
            {
                willieToHide = FindAnyObjectByType<FindMe>().gameObject;
                willieToHide.SetActive(false);
                bedInteraction = FindFirstObjectByType<BedInteraction>();
                bedInteraction.fader.FadeIn(0.01f);
                Debug.Log("interacting with bed via code");
                currentTask = "Nukkumaan";
                sleepTask = true;
                bedInteraction.canInteract = true;
                bedInteraction.Interact();
                day8reset = false;
                sceneReloaded = false;
            }
        }

        if (currentDay == 8)
        {
            hatchUnlocked = true;
            sleepTask = false;
        }
        switch (currentTask)
        {
            case string p when p.StartsWith("Peruna"):
                potatoTask = true;
                break;
            case "Hiili":
                coalTask = true;
                break;
            case "Nukkumaan":
                sleepTask = true;
                break;
            case "Siivoa":
                cleanTask = true;
                break;
            case "Ruori":
                driveTask = true;
                break;
            case "HaePeruna":
                fecthPotatoesTask = true;
                hatchUnlocked = true;
                break;
            case "HaeHiili":
                fecthCoalTask = true;
                hatchUnlocked = true;
                break;
            case "EtsiPerunaa":
                searchForPotatoes = true;
                hatchUnlocked = true;
                break;
            case "EtsiWillie":
                talkTask = true;
                hatchUnlocked = true;
                break;
            case "NukkumaanYöllä":
                hatchUnlocked = true;
                sleepTask = true;
                break;
            case string s when s.StartsWith("Puhu"):
                talkTask = true;
                break;
            default:
                Debug.Log("No task available");
                Debug.Log(currentTask);
                break;
        }
    }

    // Example method to start a new day
    public void StartDay(int day)
    {
        currentDay = day;
        if (currentDay == 8)
        {
            return;
        }
        // Set currentDayTasks to the tasks for the current day
        if (currentDay > 2)
        {
            potatoSack.SetActive(false);
            coalSack.SetActive(false);
        }
        switch (currentDay)
        {
            case 1:
                currentDayTasks = tasksDay1;
                break;
            case 2:
                currentDayTasks = tasksDay2;
                break;
            case 3:
                currentDayTasks = tasksDay3;
                break;
            case 4:
                currentDayTasks = tasksDay4;
                break;
            case 5:
                currentDayTasks = tasksDay5;
                break;
            case 6:
                currentDayTasks = tasksDay6;
                break;
            case 7:
                currentDayTasks = tasksDay7;
                break;
            case 10:
                currentDayTasks = tasksDay10;
                break;
            default:
                Debug.LogWarning("No tasks defined for Day " + day);
                currentDayTasks = new List<string>(); // Set to an empty array if no tasks are defined
                break;
        }
        currentTask = currentDayTasks[0];
        Debug.Log(currentTask);
        // Example: Print tasks for the day
        Debug.Log("Day " + currentDay + " Started!");
    }

    public void TaskDone()
    {
        potatoTask = false;
        cleanTask = false;
        coalTask = false;
        sleepTask = false;
        driveTask = false;
        fecthPotatoesTask = false;
        fecthCoalTask = false;
        hatchUnlocked = false;
        searchForPotatoes = false;
        talkTask = false;
        if (currentTask == "Nukkumaan" || currentTask == "NukkumaanYöllä")
        {
            if (currentDay != 8)
            {
                AdvanceToNextDay();
            }
        }
        else
        {
            taskIndex += 1;
            try
            {
                currentTask = currentDayTasks[taskIndex];
            }
            catch (Exception e)
            {
                currentTask = "";
            }
        }
    }

    public void AdvanceToNextDay()
    {
        currentDay++;
        if (currentDay == 4)
        {
            if (night1)
            {
                currentDay = 10;
            }
        }
        if (currentDay == 11)
        {
            night1 = false;
            currentDay = 4;
        }

        taskIndex = 0;
        if (currentDay <= 10)
        {
            currentDayTasks = GetTasksForCurrentDay();
            StartDay(currentDay);
        }
        else
        {
            Debug.LogWarning("No more days defined.");
        }
    }

    public string GetCurrentTask()
    {
        return currentTask;
    }
    public int GetCurrentDay()
    {
        return currentDay;
    }



    // Helper method to get tasks for the current day
    private List<string> GetTasksForCurrentDay()
    {
        switch (currentDay)
        {
            case 1:
                return tasksDay1;
            case 2:
                return tasksDay2;
            case 3:
                return tasksDay3;
            case 4:
                return tasksDay4;
            case 5:
                return tasksDay5;
            case 6:
                return tasksDay6;
            case 7:
                return tasksDay7;
            case 10:
                return tasksDay10;
            default:
                Debug.LogWarning("No tasks defined for Day " + currentDay);
                return new List<string>(); // Set to an empty array if no tasks are defined
        }
    }
}
