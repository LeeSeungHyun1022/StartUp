using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextDB : MonoBehaviour
{
    string path = Path.Combine(Application.streamingAssetsPath, "TextDB");
    string Sentence;
   
    void Start()
    {
        FileInfo fileInfo = new FileInfo(path);
        if (fileInfo.Exists)
        {
            ReadTxt();
        }
        else
        {
            string[] times = { "100.0", "100.0", "100.0", "100.0", "100.0", "100.0" };

            DirectoryInfo directoryinfo = new DirectoryInfo(Path.GetDirectoryName(path));

            string message = "";

            if (!directoryinfo.Exists)
            {
                directoryinfo.Create();
            }

            foreach (string time in times)
            {
                message += time + "\t";
            }

            FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);

            StreamWriter writer = new StreamWriter(fileStream, System.Text.Encoding.Unicode);

            writer.WriteLine(message);
            writer.Close();

            ReadTxt();
        }
    }

    public string getTime()
    {
        return Sentence;
    }

    public void DBUpdate(string[] clearTimes)
    {
        string[] temp = new string[6];

        Array.Copy(Sentence.Split(';'), 0, temp, 0, 6);

        for(int i=0; i<12; i++)
        {
            if(float.Parse(temp[i%6]) > float.Parse(clearTimes[i]))
            {
                temp[i%6] = clearTimes[i];
            }
        }

        DirectoryInfo directoryinfo = new DirectoryInfo(Path.GetDirectoryName(path));

        if(!directoryinfo.Exists)
        {
            directoryinfo.Create();
        }

        string message = "";

        foreach (string time in temp)
        {
            message += time + "\t";
        }

        message += "\n";

        FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);

        StreamWriter writer = new StreamWriter(fileStream, System.Text.Encoding.Unicode);

        writer.WriteLine(message);
        writer.Close();

        ReadTxt();
    }

    private void ReadTxt()
    {
        string read = "";
        string[] values;

        StreamReader reader = new StreamReader(path);
        read = reader.ReadLine();
        values = read.Split('\t');

        Sentence = "";

        for (int i=0; i<6; i++)
        {
            Sentence += values[i].ToString() + ";";
        }

        reader.Close();
    }
}
