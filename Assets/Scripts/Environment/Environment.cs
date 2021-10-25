using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Observations
{
    public float distanceToLeft;
    public float distanceToRight;
    public Vector2 ballVelocity;
    public float reward;
    public bool done;
}

public class Environment : MonoBehaviour
{
    public Platform platform;
    public Ball ball;
    public int maxSteps = 2000;

    private float episodeReward = 0.0f;
    private int episodeNum = 1;
    private int stepNum = 0;
    private float timeCount = 0;
    private Observations obs;

    // =====        PUBLIC METHODS
    public void Restart()
    {
        platform.Restart();
        ball.Restart();

        episodeReward   = 0.0f;
        stepNum         = 0;
        timeCount       = 0.0f;
        obs.done        = false;
    }

    public Observations GetObservations()
    {
        return obs;
    }

    public void SetAngularSpeed(float angularSpeed)
    {
        platform.SetAngularSpeed(angularSpeed);
    }

    // =====        UNITY METHODS
    private void Update() 
    {
        if(obs.done) return;

        obs.distanceToLeft  = ball.GetDistanceToLeft();
        obs.distanceToRight = ball.GetDistanceToRight();
        obs.ballVelocity    = ball.GetVelocity();
        obs.reward          = GetReward();
        episodeReward       += obs.reward;
        timeCount           += Time.deltaTime;
        stepNum++;

        if(stepNum >= maxSteps || ball.Fell())
        {
            Debug.Log("Episode " + episodeNum.ToString() + "\tReward: " + episodeReward.ToString());
            episodeNum++;
            obs.done = true;
        }
    }

    // =====        OTHER PRIVATE METHODS
    private float GetReward()
    {
        float distReward = 10f - Mathf.Abs(obs.distanceToLeft - obs.distanceToRight);
        float speedReward = -Mathf.Abs(obs.ballVelocity.x);
        return speedReward + timeCount;
    }
}
