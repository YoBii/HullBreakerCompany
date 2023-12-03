using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HullBreakerCompany.Event;
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
            if (Plugin.ChangeQuotaValue)
            {
                timeOfDay.quotaVariables.baseIncrease = Plugin.QuotaIncrease;
            }
        }
    }

    public static HullManager Instance;

    private void Awake()
    { 
        Instance = this;
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
    
    public static void SendChatEventMessage(HullEvent hEvent)
    {
        if (HUDManager.Instance != null && hEvent != null)
        {
            var message = !Plugin.UseShortChatMessages ? hEvent.GetMessage() : hEvent.GetShortMessage();
            
            HUDManager.Instance.AddTextToChatOnServer(message);
        } else {
            Plugin.Mls.LogInfo("Could not find HUDManager instance" +  "\n" + hEvent.GetMessage());
        }
    }
    
    public static void SendChatEventMessage(string message)
    {
        if (HUDManager.Instance != null && message != null)
        {
            HUDManager.Instance.AddTextToChatOnServer(message);
        } else {
            Plugin.Mls.LogInfo("Could not find HUDManager instance" +  "\n" + message);
        }
    }
}