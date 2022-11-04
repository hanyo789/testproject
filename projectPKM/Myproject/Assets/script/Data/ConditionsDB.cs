using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionsDB
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
    public static Dictionary<ConditionID, Condition> Conditions{get; set;} =new Dictionary<ConditionID, Condition>()
    {
        {
            ConditionID.psn,
            new Condition()
            {
                Name = "Poison",
                StartMessage = "has been poisoned!",
                OnAfterTurn =(Pokemon pokemon) => 
                {
                    pokemon.UpdateHP(pokemon.MaxHp / 8);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is hurt by poison! ");
                }
            }
        },
        {
            ConditionID.brn,
            new Condition()
            {
                Name = "Burn",
                StartMessage = "has been burned!",
                OnAfterTurn =(Pokemon pokemon) => 
                {
                    pokemon.UpdateHP(pokemon.MaxHp / 10);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is hurt by burn! ");
                    
                    
                }
            }
        },
        {
            ConditionID.par,
            new Condition()
            {
                Name = "Paralyzed",
                StartMessage = "has been paralyzed!",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if (Random.Range(1, 5) == 1)
                    {
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} has been paralyzed and can not move!");
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            ConditionID.frz,
            new Condition()
            {
                Name = "Freeze",
                StartMessage = "has been frozen!",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if (Random.Range(1, 5) == 1)
                    {
                        pokemon.CureStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} has been free from frozen!");
                        return true;
                    }
                    return false;
                }
            }
        },
        {
            ConditionID.slp,
            new Condition()
            {
                Name = "Sleep",
                StartMessage = "has fallen asleep!",
                OnStart = (Pokemon pokemon) =>
                {
                    pokemon.StatusTime = Random.Range(1,3);

                    
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if (pokemon.StatusTime <= 0)
                    {
                        pokemon.CureStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} woke up!");
                        return true;
                    }
                    pokemon.StatusTime--;
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is sleeping!");
                    return false;
                }
                
            }
        },
        {
            ConditionID.confusion,
            new Condition()
            {
                Name = "Confusion",
                StartMessage = "has been confused!",
                OnStart = (Pokemon pokemon) =>
                {
                    pokemon.OtherStatusTime = Random.Range(1,4);

                    
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if (pokemon.OtherStatusTime <= 0)
                    {
                        pokemon.CureOtherStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} snapped out of confusion!");
                        return true;
                    }
                    pokemon.OtherStatusTime--;
                    if (Random.Range(1,3) ==1)
                        return true;

                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is confused!");
                    pokemon.UpdateHP(pokemon.MaxHp / 12);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} hurt itself in confusion!");
                    return false;   
                
                }
            }
        
        }
    };
}
public enum ConditionID
{
    none,psn,brn,slp,par,frz,
    confusion
}
// reduce atk when burn
//if (attacker.Status.Id == ConditionId.Brn)
//     burnModifier = 0.5 