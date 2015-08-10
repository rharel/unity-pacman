using UnityEngine;
using System.Collections;

public class SceneTransitionAnimation : MonoBehaviour 
{
    public float transitionDuration = 1;

    private static readonly string ANIMATOR_SHOW = "Show";
    private static readonly string ANIMATOR_HIDE = "Hide";

    private Animator animator;
    private float animationStartTime = 0;
    private bool fadeOutPending = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.speed = 1.0f / transitionDuration;

        Director.LevelLoadStart += OnLevelLoadStart;
        Director.LevelLoadEnd += OnLevelLoadEnd;
    }
	
    void OnDestroy()
    {
        Director.LevelLoadStart -= OnLevelLoadStart;
        Director.LevelLoadEnd -= OnLevelLoadEnd;
    }

    void Update()
    {
        if (fadeOutPending && TimeUtility.IsTimeout(animationStartTime, Time.time, transitionDuration))
        {
            fadeOutPending = false;
            animator.SetTrigger(ANIMATOR_SHOW);
        }
    }

    private void OnLevelLoadStart()
    {
        animationStartTime = Time.time;
        animator.SetTrigger(ANIMATOR_HIDE);
    }

    private void OnLevelLoadEnd()
    {
        fadeOutPending = true;
    }
}
