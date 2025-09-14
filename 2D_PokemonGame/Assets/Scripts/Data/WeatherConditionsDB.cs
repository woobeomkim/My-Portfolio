using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherConditionsDB 
{
    public static void Init()
    {
        foreach (var kvp in Conditions)
        {
            var conditionId = kvp.Key;
            var condition = kvp.Value;

            condition.Id = conditionId;
        }
    }

    public static Dictionary<WeatherConditionID, WeatherCondition> Conditions = new Dictionary<WeatherConditionID, WeatherCondition>()
    {
        {
            WeatherConditionID.sandstorm,
            new WeatherCondition()
            {
                Name = "SandStrom",
                StartMessage = "A sandstorm is raging",
                EffectMessage = "The sandstorm rages",
                EndMessage = "The sandstorm subsided",
                StartMessageByMove = "A sandstorm brwed",
                OnWeatherEffect = (Pokemon pokemon) =>
                {
                    if(pokemon.IsOfType(PokemonType.Ground) || pokemon.IsOfType(PokemonType.Rock))
                        return;

                    pokemon.DecreaseHP(Mathf.CeilToInt(pokemon.MaxHp / 16));
                    pokemon.AddStatusEvent(StatusEventType.Damage , $"{pokemon.Base.Name}(이)가 모래바람에 덮쳐졌다!");
                },
            }
        },
        {
            WeatherConditionID.hail,
            new WeatherCondition()
            {
                Name = "Hail",
                StartMessage = "It's hailing",
                EffectMessage = "The hail continues to fail",
                EndMessage = "The hail stopped",
                StartMessageByMove = "It started to hail",
                OnWeatherEffect = (Pokemon pokemon) =>
                {
                    if(pokemon.IsOfType(PokemonType.Ice))
                        return;

                    pokemon.DecreaseHP(Mathf.CeilToInt(pokemon.MaxHp / 16));
                    pokemon.AddStatusEvent(StatusEventType.Damage , $"{pokemon.Base.Name}(이)가 눈바람에 덮쳐졌다!");
                },
            }
        },
        {
            WeatherConditionID.rain,
            new WeatherCondition()
            {
                Name = "Rain",
                StartMessage = "It's raining",
                EffectMessage = "The rain continues to fail",
                EndMessage = "The rain stopped",
                StartMessageByMove = "It started to rain",
                OnDamageModify = (Move move) =>
                {
                    if(move.Base.Type == PokemonType.Water)
                        return 1.5f;
                    else if(move.Base.Type == PokemonType.Fire)
                        return 0.5f;

                    return 1.0f;
                }
            }
        },
        {
            WeatherConditionID.harshsunlight,
            new WeatherCondition()
            {
                Name = "Harsh Sunlight",
                StartMessage = "The sunlight is harsh",
                EffectMessage = "The sunlight is harsh",
                EndMessage = "The sunlight faded",
                StartMessageByMove = "The sunlight turned harsh",
                OnDamageModify = (Move move) =>
                {
                    if(move.Base.Type == PokemonType.Fire)
                        return 1.5f;
                    else if(move.Base.Type == PokemonType.Water)
                        return 0.5f;

                    return 1.0f;
                }
            }
        },
    };

}

public class WeatherCondition
{
    public WeatherConditionID Id { get; set; }
    public string Name { get; set; }
    public string StartMessage { get; set; }
    public string EffectMessage { get; set; }
    public string EndMessage { get; set; }
    public string StartMessageByMove { get; set; }
    
    public Action<Pokemon> OnWeatherEffect { get; set; }
    public Func<Move,float> OnDamageModify { get; set; }
}

public enum WeatherConditionID
{
    none, sandstorm, hail, rain, harshsunlight
}
