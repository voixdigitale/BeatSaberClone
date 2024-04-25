using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VelocityEstimator : MonoBehaviour
{
    [Tooltip("The number of frames to average over for velocity estimation.")]
    public int frameCount = 5;

    [Tooltip("The number of frames to average over for angular velocity estimation")]
    public int angularFrameCount = 10;

    public bool estimateOnAwake = false;

    private Coroutine routine;
    private int sampleCount;
    private Queue<Vector3> velocitySamples;
    private Queue<Vector3> angularVelocitySamples;


    void Awake()
    {
        velocitySamples = new Queue<Vector3>();
        angularVelocitySamples = new Queue<Vector3>();

        if (estimateOnAwake)
        {
            BeginEstimation();
        }
    }

    void BeginEstimation()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
        }

        sampleCount = 0;
        velocitySamples.Clear();
        angularVelocitySamples.Clear();

        routine = StartCoroutine(EstimateVelocityCoroutine());
    }

    void EndEstimation()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }
    }

    IEnumerator EstimateVelocityCoroutine()
    {
        while (true)
        {
            Vector3 previousPosition = transform.position;
            Quaternion previousRotation = transform.rotation;

            yield return new WaitForFixedUpdate();

            Vector3 velocity = (transform.position - previousPosition) / Time.fixedDeltaTime;
            Vector3 angularVelocity = (transform.rotation.eulerAngles - previousRotation.eulerAngles) / Time.fixedDeltaTime;

            velocitySamples.Enqueue(velocity);
            angularVelocitySamples.Enqueue(angularVelocity);

            if (velocitySamples.Count > frameCount)
            {
                velocitySamples.Dequeue();
            }

            if (angularVelocitySamples.Count > angularFrameCount)
            {
                angularVelocitySamples.Dequeue();
            }

            sampleCount++;
        }
    }

    public Vector3 GetVelocityEstimate()
    {
        Vector3 sum = Vector3.zero;
        foreach (Vector3 sample in velocitySamples)
        {
            sum += sample;
        }

        return velocitySamples.Count > 0 ? sum / velocitySamples.Count : sum;
    }

    public Vector3 GetAngularVelocityEstimate()
    {
        Vector3 sum = Vector3.zero;
        foreach (Vector3 sample in angularVelocitySamples)
        {
            sum += sample;
        }

        return angularVelocitySamples.Count > 0 ? sum / angularVelocitySamples.Count : sum;
    }

    public Vector3 GetAccelerationEstimate()
    {
        if (velocitySamples.Count < 2)
        {
            return Vector3.zero;
        }

        Vector3 sum = Vector3.zero;
        Vector3[] samples = velocitySamples.ToArray();
        for (int i = 1; i < samples.Length; i++)
        {
            sum += samples[i] - samples[i - 1];
        }

        return sum / samples.Length;
    }
}
