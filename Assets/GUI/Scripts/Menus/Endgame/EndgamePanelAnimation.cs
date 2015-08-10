using UnityEngine;
using System.Collections;

public class EndgamePanelAnimation : MonoBehaviour 
{
    public float appearanceDelay = 0.5f;
    public EndgameStatusHeader endgameStatusHeader;
    public ScoreBreakdownAnimation scoreBreakdownAnimation;

    private static readonly string ANIMATOR_VISIBILITY = "Visibility";

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        gameObject.SetActive(false);

        GameManager.End += OnGameEnd;
    }

    void OnDestroy()
    {
        GameManager.End -= OnGameEnd;
    }

    private void OnGameEnd()
    {
        gameObject.SetActive(true);
        endgameStatusHeader.UpdateDisplay();
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        yield return new WaitForSeconds(appearanceDelay);
        animator.SetBool(ANIMATOR_VISIBILITY, true);
        scoreBreakdownAnimation.StartAnimation();
    }
}
