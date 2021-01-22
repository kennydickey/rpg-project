using System.Collections.Generic;

namespace RPG.Stats
{
    public interface IModifierProvider
    {
        // enumerate over floats - also IEnumerable makes it possible to use in a foreach loop
        IEnumerable<float> GetAdditiveModifier(Stat stat); // returns a list of additive modifiers for a particular stat
    }
}
