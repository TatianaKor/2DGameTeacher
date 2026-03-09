using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance;

    [SerializeField] private float displayTime = 4f;
    [SerializeField] private VisualTreeAsset winScreen;
    [SerializeField] private VisualTreeAsset loseScreen;

    private PlayerController player;
    private UIDocument uiDocument;
    private VisualElement healthBar;
    private VisualElement npcDialog;
    private Label robotCounter;

    private float timerDisplay;
    private int robotsCount;
    private int robotsFixedCount;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        healthBar = uiDocument.rootVisualElement.Q<VisualElement>("HealthBar");
        healthBar.style.width = Length.Percent(100);

        npcDialog = uiDocument.rootVisualElement.Q<VisualElement>("NPCDialog");
        npcDialog.style.display = DisplayStyle.None;

        robotCounter = uiDocument.rootVisualElement.Q<Label>("CounterLabel");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        robotsCount = enemies.Length;
        foreach(GameObject enemy in enemies)
        {
            EnemyController controller = enemy.GetComponent<EnemyController>();
            controller.OnFixed += OnRobotFixed;
        }
        UpdateCounter();

        player = GameObject.FindFirstObjectByType<PlayerController>();
        player.OnTalkToNPC += ShowNPCDialog;
    }

    void Update()
    {
        if(timerDisplay > 0)
        {
            timerDisplay -= Time.deltaTime;
            if(timerDisplay <= 0)
            {
                npcDialog.style.display = DisplayStyle.None;
            }
        }
    }

    private void OnRobotFixed()
    {
        robotsFixedCount++;
        UpdateCounter();
    }

    private void UpdateCounter()
    {
        if (robotsFixedCount == robotsCount)
        {
            robotCounter.text = "Talk to NPC";
            player.OnTalkToNPC -= ShowNPCDialog;
            player.OnTalkToNPC += ShowWinScreen;
        }
        else
        {
            robotCounter.text = $"{robotsFixedCount} / {robotsCount}";
        }
    }

    public void UpdateHealthBar(float percents)
    {
        healthBar.style.width = Length.Percent(percents * 100);

        if(percents == 0)
        {
            ShowLoseScreen();
        }
    }

    public void ShowNPCDialog()
    {
        npcDialog.style.display = DisplayStyle.Flex;

        timerDisplay = displayTime;
    }

    public void ShowWinScreen()
    {
        uiDocument.visualTreeAsset = winScreen;
        Invoke(nameof(ReloadSceen), displayTime);
    }

    public void ShowLoseScreen()
    {
        uiDocument.visualTreeAsset = loseScreen;
        Invoke(nameof(ReloadSceen), displayTime);
    }

    private void ReloadSceen()
    {
        SceneManager.LoadScene(0);
    }
}
