using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Config
{
    public float kp;
    public float ki;
    public float kd;
    public float bias;
    public float reward;
}

public class Trainer : MonoBehaviour
{
    public Environment env;
    public PlatformController controller;

    private Observations obs;
    
    public float exploreMinKp  = -20.0f;
    public float exploreMaxKp  = 20.0f;
    public float exploreMinKi = -10.0f;
    public float exploreMaxKi = 10.0f;
    public float exploreMinKd = -10.0f;
    public float exploreMaxKd = 10.0f;
    public float exploreMaxBias = -10.0f;
    public float exploreMinBias = 10.0f;

    private float gamma = 0.9f;
    private float episodeReward = 0f;

    private const int numConfigs = 10;
    private Config[] configs = new Config[numConfigs];
    private int episodeCount = 0;

    private float maxTime = 0.5f;
    private float time=1;

    private void Update() 
    {
        obs = env.GetObservations();
        episodeReward += obs.reward;
        float angularSpeed = controller.Compute(obs);
        env.SetAngularSpeed(angularSpeed);

        time -= Time.deltaTime;
        if(time <= 0f)
        {
            Globals.episodeReward = episodeReward;
            time = maxTime;
        }

        if(obs.done)
        {
            AddToConfigs();
            Restart();
        }
    }

    private void Restart()
    {
        episodeReward = 0;
        if(episodeCount > 0)
        {
            ExploreController();
        }
        env.Restart();
        controller.Restart();
    }

    private void ExploreController()
    {
        controller.kp = Random.Range(exploreMinKp, exploreMaxKp);
        controller.ki = Random.Range(exploreMinKi, exploreMaxKi);
        controller.kd = Random.Range(exploreMinKd, exploreMaxKd);
        //controller.bias = Random.Range(exploreMinBias, exploreMaxBias);

        UpdateGlobals();
    }

    private void AdjustExplore(int best)
    {
        // === Proportional
        float leftRange = Mathf.Abs(configs[best].kp - exploreMinKp);
        exploreMinKp = configs[best].kp - leftRange*gamma;

        float rightRange = Mathf.Abs(exploreMaxKp - configs[best].kp);
        exploreMaxKp = rightRange*gamma + configs[best].kp;

        // === Integral
        leftRange = Mathf.Abs(configs[best].ki - exploreMinKi);
        exploreMinKi = configs[best].ki -leftRange*gamma;

        rightRange = Mathf.Abs(exploreMaxKi - configs[best].ki);
        exploreMaxKi = rightRange*gamma + configs[best].ki;

        // === Derivative
        leftRange = Mathf.Abs(configs[best].kd - exploreMinKd);
        exploreMinKd = configs[best].kd -leftRange*gamma;

        rightRange = Mathf.Abs(exploreMaxKd - configs[best].kd);
        exploreMaxKd = rightRange*gamma + configs[best].kd;

        // === BIAS
        // leftRange = Mathf.Abs(configs[best].bias - exploreMinBias);
        // exploreMinBias = configs[best].bias - leftRange*gamma;

        // rightRange = Mathf.Abs(exploreMaxBias - configs[best].bias);
        // exploreMaxBias = rightRange*gamma + configs[best].bias;
    }

    private void AddToConfigs()
    {
        configs[episodeCount].kp = controller.kp;
        configs[episodeCount].ki = controller.ki;
        configs[episodeCount].kd = controller.kd;
        configs[episodeCount].bias = controller.bias;
        configs[episodeCount].reward = episodeReward;
        episodeCount++;

        if(episodeCount == numConfigs)
        {
            // Get best config so far (Exploitation)
            int best       = GetBestConfig();
            controller.kp   = configs[best].kp;
            controller.ki   = configs[best].ki;
            controller.kd   = configs[best].kd;
            controller.bias = configs[best].bias;
            Debug.Log("kp: " + controller.kp.ToString() + "\tki: " + controller.ki.ToString() + "\tkd: " + controller.kd.ToString() + "\tReward: " + configs[best].reward);

            AdjustExplore( best );
            episodeCount = 0;
        }
    }

    private int GetBestConfig()
    {
        int index=0;
        float maxReward = configs[0].reward;
        for(int i=1; i<numConfigs; i++)
        {
            if(maxReward < configs[i].reward)
            {
                maxReward = configs[i].reward;
                index = i;
            }
        }
        return index;
    }

    private void UpdateGlobals()
    {
        Globals.kp = controller.kp;
        Globals.ki = controller.ki;
        Globals.kd = controller.kd;
        Globals.bias = controller.bias;

        Globals.episodeNum++;
    }
}
