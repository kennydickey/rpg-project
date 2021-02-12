using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public IEnumerator LoadLastScene(string saveFile) //IEnum to load.. and then restore state
        {
            Dictionary<string, object> state = LoadFile(saveFile); //getting state
            if (state.ContainsKey("lastSceneBuildIndex"))
            {
                int buildIndex = (int)state["lastSceneBuildIndex"];
                if (buildIndex != SceneManager.GetActiveScene().buildIndex) //if current does not = SceneManager idx
                {
                    yield return SceneManager.LoadSceneAsync(buildIndex); //yield to wait until async operation is finished
                }
            }          
            RestoreState(state); //restore state either way
        }

        public void Save(string saveFile)
        {
            // state as a type of dictionary holding saveFile data
            Dictionary<string, object> state = LoadFile(saveFile);
            CaptureState(state); 
            SaveFile(saveFile, state);
            //string path = GetPathFromSaveFile(saveFile); X
            //print("Saving to " + path); X
            // use file stream to put pringles into the tube
            //FileMode is an enum with options such as append, create, etc
            //Filemode.Create creates a new file and overwrites existing
            //using (FileStream stream = File.Open(path, FileMode.Create)) X// place stream in a using
            {
                //stream.WriteByte(0xc2); // writes the byte value of 102, so.. f
                //stream.WriteByte(0xa1); // etc..
                //or..
                //byte[] bytes = Encoding.UTF8.GetBytes("¡Hola Mundo"); // type of byte array

                //Transform playerTransform = GetPlayerTransform();
                // now serialized by BinaryFormatter formatter vv
                //byte[] buffer = SerializeVector(playerTransform.position);

                //BinaryFormatter formatter = new BinaryFormatter(); X// calling bf constructor
                // simplified x, y ,z format of Vector3 using a constructor of our own method
                //SerializableVector3 position = new SerializableVector3(playerTransform.position);
                //formatter.Serialize(stream, CaptureState()); X// Serialize to and what

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
            RestoreState(LoadFile(saveFile));
            //string path = GetPathFromSaveFile(saveFile);X
            //print("loading from " + path);X
            //using (FileStream stream = File.Open(path, FileMode.Open))X
            //{X
            // buffer is a place we create to place data into while specifying how many bytes are required
            //now serialized in BinaryFormatter formatter
            //byte[] buffer = new byte[stream.Length]; // manually write into file to test
            //read api.. 
            //stream.Read(buffer, 0, buffer.Length); // start reading from beginning to end of buffer
            // One of GetStrings overloads takes in Byte[] bytes and returns a decoded string
            //print(Encoding.UTF8.GetString(buffer));

            // deserialize buffer and assign to player transform
            //Transform playerTransform = GetPlayerTransform();
            //BinaryFormatter formatter = new BinaryFormatter(); X// calling bf constructor
            // Cast! -converting an obj into a specific type v      v
            //SerializableVector3 position = (SerializableVector3)formatter.Deserialize(stream); // takes in a stream
            //RestoreState(formatter.Deserialize(stream));X
            // convert serializeble V3 into a normal Vector3 using our method
            //playerTransform.position = position.ToVector(); //assign to player transform in game
            //now done with BinaryFormatter formatter vv
            //playerTransform.position = DeserializeVector(buffer);
            //}X           
        }

        // method takes in file to save to and and state to capture
        private void SaveFile(string saveFile, object state)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);
            using (FileStream stream = File.Open(path, FileMode.Create)) // place stream in a using
            {
                BinaryFormatter formatter = new BinaryFormatter(); // calling bf constructor
                formatter.Serialize(stream, state); // Serialize to and what
            }

        }

        private Dictionary<string, object> LoadFile(string saveFile) // Dictionary type of LoadFile
        {
            string path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }
            using (FileStream stream = File.Open(path, FileMode.Open)) 
            {
                BinaryFormatter formatter = new BinaryFormatter(); // calling bf constructor
                // return cast as a dictionary so LoadFile knows that it is such
                return (Dictionary<string, object>)formatter.Deserialize(stream); 
            }
        }

        //private Transform GetPlayerTransform() // get player Vector 3 to serialize
        //{
        //    return GameObject.FindWithTag("Player").transform;
        //}


        private void CaptureState(Dictionary<string, object> state) // we need CaptureState() to be this type of dictionary
        {
            //state["hellow"] = 4;
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                //stor captured state into our dictionary
                state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
            }
            // after this, check how many entities are being captured by checking console when saving

            // lastSceneBuildIndex is a string in our state dictionary and buildIndex is the value
            state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }
        private void RestoreState (Dictionary<string, object> state) // only passing in dictionary type of state
        {
            // a cast to make sure that stateDict knows it is this type so we can access it as so
            // update - no longer needed as we've declared state as a dictionary type while passing into the method
            //Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                string id = saveable.GetUniqueIdentifier();
                if (state.ContainsKey(id))
                {
                    saveable.RestoreState(state[id]);
                }
            }

        }

        //Example of serializing vectors
        //// serialize our vectors to be written to file
        //private byte[] SerializeVector(Vector3 vector) 
        //{
        //    byte[] vectorBytes = new byte[3 * 4]; //space for 3 floats which have a byte size of 4 each
        //    //                      array to copy to v          v start index
        //    BitConverter.GetBytes(vector.x).CopyTo(vectorBytes, 0); // place each value into it's positon in vectorBytes Array
        //    BitConverter.GetBytes(vector.y).CopyTo(vectorBytes, 4);
        //    BitConverter.GetBytes(vector.y).CopyTo(vectorBytes, 8);
        //    return vectorBytes;
        //}
        //// deserialize our file buffer back into Vector3s
        //private Vector3 DeserializeVector(byte[] buffer)
        //{
        //    Vector3 result = new Vector3();
        //    result.x = BitConverter.ToSingle(buffer, 0); // filling in Vector3 result params
        //    result.y = BitConverter.ToSingle(buffer, 4);    //v
        //    result.z = BitConverter.ToSingle(buffer, 8);    //v
        //    return result;
        //}

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
            //return Path.Combine(Application.persistentDataPath);
        }
    }

}