using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    //only allowed in interfaceare things that can be implemented in other classes such as methods or properties
    public interface IAction 
    {
        void Cancel();
    }
}

