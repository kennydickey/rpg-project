using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
    [System.Serializable] // ! notice this is above our class, so all of our fields will try to be serialized
    public class SerializableVector3
    {
        float x, y, z;

        public SerializableVector3(Vector3 vector) // for use in saving system to turn a Vector3 into simple x, y ,z vals to serialize
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public Vector3 ToVector()
        {
            return new Vector3(x, y, z); // x, y, and z floats that we've already made
        }
    }
}
