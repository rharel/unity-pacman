using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreBreakdownAnimation : MonoBehaviour 
{
    public CounterAnimation coinScoreAnimation;
    public CounterAnimation ghostScoreAnimation;
    public CounterAnimation totalScoreAnimation;

    private static readonly float ANIMATION_DELAY = 0.5f;

	public void StartAnimation()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        yield return new WaitForSeconds(ANIMATION_DELAY);

        coinScoreAnimation.TargetValue = ScoreManager.CoinsCollected;
        ghostScoreAnimation.TargetValue = ScoreManager.GhostsKilled;
        totalScoreAnimation.TargetValue = ScoreManager.Score;
    }
}
