using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public float kp = 1;
    public float ki = 0;
    public float kd = 0;
    public float bias = 0f;
    public float speedSetPoint = 0;

    private float It = 0.0f;
    private float prevError = 0.0f;

    // =====        PUBLIC METHODS
    public float Compute(Observations obs)
    {
        return SpeedControl(obs);
    }

    public void Restart()
    {
        It = 0.0f;
        prevError = 0.0f;
    }

    // =====        PRIVATE METHODS
    private void SpeedControl(Observations obs)
    {
        float ballSpeed = obs.ballVelocity.x;
        float speedError = ballSpeed - speedSetPoint;

        // === Proportional
        float P = speedError*kp;

        // === Integral
        float I = speedError*ki*Time.deltaTime;
        It += I;

        // === Derivative
        float D = kd*(speedError - prevError)*Time.deltaTime;
        prevError = speedError;

        return P + It + D + bias;
    }
}
