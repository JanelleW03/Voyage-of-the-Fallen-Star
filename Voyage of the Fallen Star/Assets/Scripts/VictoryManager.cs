using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager Instance { get; private set; }

    [Header("Assign these in the Inspector")]
    public NpcEnemyHealthComponent npc1;
    public NpcEnemyHealthComponent npc2;
    public LucienHealthComponent boss;

    private bool _npc1Defeated = false;
    private bool _npc2Defeated = false;
    private bool _bossDefeated = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void ReportDefeated(NpcEnemyHealthComponent defeated)
    {
        if (defeated == npc1) _npc1Defeated = true;
        else if (defeated == npc2) _npc2Defeated = true;
        CheckWinCondition();
    }

    public void ReportBossDefeated()
    {
        _bossDefeated = true;
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (_npc1Defeated && _npc2Defeated && _bossDefeated)
        {
            if (WinScreen.Instance != null)
                WinScreen.Instance.Show();
            else
                Debug.LogError("WinScreen.Instance is null!");
        }
    }
}