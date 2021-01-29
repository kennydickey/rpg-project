using System.Collections;
using System.Collections.Generic;
using System.IO;
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
                stream.WriteByte(0xc2); // writes the byte value of 102, so.. f
                stream.WriteByte(0xa1); // etc..
                                        //or..
                byte[] bytes = Encoding.UTF8.GetBytes("¡Hola Mundo"); // type of byte array
                //write api..
                stream.Write(bytes, 0, bytes.Length); // Writes each byte, specify start and last
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
                byte[] buffer = new byte[stream.Length]; // manually write into file to test
                //read api..

                stream.Read(buffer, 0, buffer.Length); // start reading from beginning to end of buffer
                // One of GetStrings overloads takes in Byte[] bytes and returns a decoded string
                print(Encoding.UTF8.GetString(buffer));              
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
            //return Path.Combine(Application.persistentDataPath);


        }
    }

}