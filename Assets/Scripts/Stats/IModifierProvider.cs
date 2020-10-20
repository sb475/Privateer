using System.Collections.Generic;

namespace RPG.Stats
{
 public interface IModifierProvider
 {
     IEnumerable<float> GetAdditiveModifiers(StatName stat);
     IEnumerable<float> GetPercentageModifiers(StatName stat);

 }
}