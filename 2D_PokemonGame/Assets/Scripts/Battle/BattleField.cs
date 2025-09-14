using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleField
{
    public WeatherCondition Weather { get; private set; }
    public int? WeatherDuration { get; set; }
    public void SetWeahter(WeatherConditionID weatherId, int? weatherDuration = null)
    {
        if (weatherId == WeatherConditionID.none)
            Weather = null;
        else
            Weather = WeatherConditionsDB.Conditions[weatherId];

        WeatherDuration = weatherDuration;
    }
}
