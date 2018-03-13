//Author: Archie Andrews
using System.Collections;
using UnityEngine;

public class FromTo : MonoBehaviour
{
    [Header("General Settings"), ]
    [Tooltip("How far long the line the target will be when traveling")]
    public AnimationCurve positionOverTime;
    [Tooltip("Will the movement loop")]
    public bool isPingPong = true;

    [Header("Move On Start Settings"), Tooltip("Call movement on start")]
    public bool moveOnStart = true;
    
    [Tooltip("Position the target will move to if Move On Start is set to true")]
    public Vector3 targetPosition;
    [Tooltip("Time taken to move from A to B if Move On Start is set to true")]
    public float timeToTravel = 1;

    private Coroutine fromToLoopRoutine;
    private Vector3 startPos;

    public void Start()
    {
        startPos = transform.position;

        if (moveOnStart)
            MoveFromTo(startPos, startPos + targetPosition, timeToTravel);
    }

    public void MoveFromTo(Vector3 from, Vector3 to, float time)
    {
        if (fromToLoopRoutine != null)
            StopCoroutine(fromToLoopRoutine);

        StartCoroutine(FromToLoop(from, to, time));
    }

    IEnumerator FromToLoop(Vector3 from, Vector3 to, float time)
    {
        float range = 0;

        if (isPingPong)
        {
            while (true)
            {
                while (range < 1)
                {
                    transform.position = Vector3.Lerp(from, to, positionOverTime.Evaluate(range));
                    range += Time.deltaTime / time;
                    yield return null;
                }

                while (range > 0)
                {
                    transform.position = Vector3.Lerp(from, to, positionOverTime.Evaluate(range));
                    range -= Time.deltaTime / time;
                    yield return null;
                }
            }
        }
        else
        {
            while (range < 1)
            {
                transform.position = Vector3.Lerp(from, to, positionOverTime.Evaluate(range));
                range += Time.deltaTime / time;
                yield return null;
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);

        if (Application.isPlaying)
        {
            Gizmos.DrawSphere(startPos + targetPosition, 0.5f);
            Gizmos.DrawSphere(startPos, 0.5f);

            Gizmos.DrawLine(startPos, startPos + targetPosition);
        }
        else
        {
            Gizmos.DrawSphere(transform.position, 0.5f);
            Gizmos.DrawSphere(transform.position + targetPosition, 0.5f);

            Gizmos.DrawLine(transform.position, transform.position + targetPosition);
        }
    }
}
