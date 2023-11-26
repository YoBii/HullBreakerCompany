using System;
using System.Collections;
using UnityEngine;

namespace HullBreakerCompany.hull;
internal class HullManager : MonoBehaviour
{
    public TimeOfDay timeOfDay;
    public void Update()
    {
        if (timeOfDay == null)
        {
            timeOfDay = FindFirstObjectByType<TimeOfDay>();
        }
        else
        {
            timeOfDay.quotaVariables.baseIncrease = 256;
        }
        
    }
    
    public static HullManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ExecuteAfterDelay(Action action, float delay)
    {
        StartCoroutine(DelayedExecution(action, delay));
    }
    
    public void RepeatingExecute(Action action, float delay, float interval)
    {
        StartCoroutine(RepeatingExecution(action, delay, interval));
    }

    private IEnumerator RepeatingExecution(Action action, float delay, float interval)
    {
        yield return new WaitForSeconds(delay);
        while (true)
        {
            action.Invoke();
            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator DelayedExecution(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }
}