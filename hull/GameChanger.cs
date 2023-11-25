using UnityEngine;

namespace HullBreakerCompany.hull;
internal class GameChanger : MonoBehaviour
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
}