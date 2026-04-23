using UnityEngine;

// Place this on any persistent GameObject in MainScene (e.g. a GameManager object).
// All NpcEnemyHealthComponent deaths will call CleanseTracker.Instance.RegisterCleanse().

public class CleanseTracker : MonoBehaviour
{
    public static CleanseTracker Instance { get; private set; }

    [Header("Settings")]
    public int npcsToWin = 6; // total hostile NPCs that must die

    private int _cleanseCount = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterCleanse()
    {
        _cleanseCount++;
        Debug.Log($"Cleansed {_cleanseCount}/{npcsToWin}");

        if (_cleanseCount >= npcsToWin)
        {
            WinScreen.Instance?.Show();
        }
    }
}
