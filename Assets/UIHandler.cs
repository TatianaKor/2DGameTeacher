using UnityEngine;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance;

    [SerializeField] private float displayTime = 4f;

    private UIDocument uiDocument;
    private VisualElement healthBar;
    private VisualElement npcDialog;

    private float timerDisplay;

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

    public void UpdateHealthBar(float percents)
    {
        healthBar.style.width = Length.Percent(percents * 100);
    }

    public void ShowNPCDialog()
    {
        npcDialog.style.display = DisplayStyle.Flex;

        timerDisplay = displayTime;
    }
}
