using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);
            // use file stream to put pringles into the tube
            //FileMode is an enum with options such as append, create, etc
            //Filemode.Create creates a new file and overwrites existing
            using (FileStream stream = File.Open(path, FileMode.Create)) // place stream in a using
            {
                //stream.WriteByte(0xc2); // writes the byte value of 102, so.. f
                //stream.WriteByte(0xa1); // etc..
                 //or..
                //byte[] bytes = Encoding.UTF8.GetBytes("¡Hola Mundo"); // type of byte array

                Transform playerTransform = GetPlayerTransform();
                // now serialized by BinaryFormatter formatter vv
                //byte[] buffer = SerializeVector(playerTransform.position);

                BinaryFormatter formatter = new BinaryFormatter(); // calling bf constructor
                // simplified x, y ,z format of Vector3 using a constructor of our own method
                SerializableVector3 position = new SerializableVector3(playerTransform.position);
                formatter.Serialize(stream, position); // Serialize to and what

                //write api..
                //stream.Write(bytes, 0, bytes.Length); // Writes each byte, specify start and last
                //now done by serializer vv
                //stream.Write(buffer, 0, buffer.Length); // for writing to transform buffer
                // exiting using method automatically closes the file stream               
            }           
             //stream.Close(); // always close file stream, not needed however with 'using'
        }
     
        public void Load(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("loading from " + path);
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                // buffer is a place we create to place data into while specifying how many bytes are required
                //now serialized in BinaryFormatter formatter
                //byte[] buffer = new byte[stream.Length]; // manually write into file to test
                //read api.. 
                //stream.Read(buffer, 0, buffer.Length); // start reading from beginning to end of buffer
                // One of GetStrings overloads takes in Byte[] bytes and returns a decoded string
                //print(Encoding.UTF8.GetString(buffer));

                // deserialize buffer and assign to player transform
                Transform playerTransform = GetPlayerTransform();
                BinaryFormatter formatter = new BinaryFormatter(); // calling bf constructor
                // Cast! -converting an obj into a specific type v      v
                SerializableVector3 position = (SerializableVector3)formatter.Deserialize(stream); // takes in a stream
                // convert serializeble V3 into a normal Vector3 using our method
                playerTransform.position = position.ToVector(); //assign to player transform in game
                //now done with BinaryFormatter formatter vv
                //playerTransform.position = DeserializeVector(buffer);

            }
        }

        private Transform GetPlayerTransform() // get player Vector 3 to serialize
        {
            return GameObject.FindWithTag("Player").transform;
        }

        // serialize our vectors to be written to file
        private byte[] SerializeVector(Vector3 vector) 
        {
            byte[] vectorBytes = new byte[3 * 4]; //space for 3 floats which have a byte size of 4 each
            //                      array to copy to v          v start index
            BitConverter.GetBytes(vector.x).CopyTo(vectorBytes, 0); // place each value into it's positon in vectorBytes Array
            BitConverter.GetBytes(vector.y).CopyTo(vectorBytes, 4);
            BitConverter.GetBytes(vector.y).CopyTo(vectorBytes, 8);
            return vectorBytes;
        }
        // deserialize our file buffer back into Vector3s
        private Vector3 DeserializeVector(byte[] buffer)
        {
            Vector3 result = new Vector3();
            result.x = BitConverter.ToSingle(buffer, 0); // filling in Vector3 result params
            result.y = BitConverter.ToSingle(buffer, 4);    //v
            result.z = BitConverter.ToSingle(buffer, 8);    //v
            return result;
        }
 
        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
            //return Path.Combine(Application.persistentDataPath);
        }
    }

}