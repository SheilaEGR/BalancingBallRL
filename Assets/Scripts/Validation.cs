using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Validation : MonoBehaviour
{
    public Environment env;
    public PlatformController controller;

    public float kp = 1;
    public float ki = 0;
    public float kd = 0;
    public float bias = 0;

    private Observations obs;
    private float maxTime = 0.5f;
    private float time = 0.0f;

    private void Update() 
    {
        obs = env.GetObservations();
        float angularSpeed = controller.Compute(obs);
        env.SetAngularSpeed(angularSpeed);
        
        time -= Time.deltaTime;
        if(time <= 0f)
        {
            UpdateGlobals();
            time = maxTime;
        }

        if(obs.done)
        {
            env.Restart();
            controller.Restart();
        }
    }

    private void Start() 
    {
        controller.kp = kp;
        controller.ki = ki;
        controller.kd = kd;
        controller.bias = bias;

        Globals.kp = controller.kp;
        Globals.ki = controller.ki;
        Globals.kd = controller.kd;
        Globals.bias = controller.bias;

        env.maxSteps = 1000000;
    }

    private void UpdateGlobals()
    {
        Globals.episodeReward = obs.reward;
        Globals.episodeTime += Time.deltaTime;
        Globals.episodeNum = 1;
    }
}
