using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class MissionSelector
{
    public const string MISSION_DIR = "Missions/";
    public static MissionSelector INSTANCE;
    public static MissionSelector getInstance()
    {
        if (INSTANCE is null)
        {
            INSTANCE = new MissionSelector();
        }
        return INSTANCE;
    }

    public struct LoadedMission
    {
        public int level;
        public Mission mission;
    }

    public class Chapter
    {
        public string name;
        public int numberOfMission = 0;
        public List<LoadedMission> loaded;
        public Chapter(string name)
        {
            this.name = name;
            this.numberOfMission = 0;
            this.loaded = new List<LoadedMission> ();
        }
    }


    private List<Chapter> _chapters;

    public MissionSelector()
    {
        this._chapters = new List<Chapter>();

    }

    public void WriteMission(string chapter, int mission, Mission m)
    {
        if (mission >= 0 && m != null)
        {
            string filename = MISSION_DIR + chapter + "/mission_" + mission + ".json";
            string data = JsonUtility.ToJson(m);
            System.IO.FileInfo file = new System.IO.FileInfo(filename);
            file.Directory.Create();
            System.IO.File.WriteAllText(file.FullName, data);
        }
    }

    public void Test()
    {
        Debug.Log("Writing in Missions/ChapterTest/mission_0.json");
        DummyBallMission[][] initialPlayground = new DummyBallMission[9][];
        for(int i = 0; i < 9; i++)
        {
            initialPlayground[i] = new DummyBallMission[8];
            for(int j = 0; j < 8; j++)
            {
                initialPlayground[i][j] = null;
            }
        }
        for(int i = 0; i < 8; i++)
        {
            DummyBallMission b = DummyBallMission.BallToDummy(new NormalBall(1, (i % 2 == 0 ? 1 : 0)));
            initialPlayground[0][i] =b;
        }
        List<DummyBallMission>[] initialPrediction = new List<DummyBallMission>[8];
        for(int i = 0; i < 8; i++)
        {
            initialPrediction[i] = new List<DummyBallMission>();
            DummyBallMission b = DummyBallMission.BallToDummy(new NormalBall(2, 0));
            initialPrediction[i].Add(b);
        }
        Mission m = new Mission(initialPlayground, initialPrediction);
        System.IO.FileInfo file = new System.IO.FileInfo(MISSION_DIR + "ChapterTest/mission_0.json");
        file.Directory.Create();

        string json = JsonConvert.SerializeObject(m);
        Debug.Log(json);
        System.IO.File.WriteAllText(file.FullName, json);
        Debug.Log("Writing Done");
    }


    public Mission ReadMission(string chapter, int mission)
    {
        Mission res = null;

        string filename = MISSION_DIR + chapter + "/mission_" + mission + ".json";
        //Read file 
        using (System.IO.StreamReader r = new System.IO.StreamReader(filename))
        {
            string data = r.ReadToEnd();
            Mission m = JsonUtility.FromJson<Mission>(data);
        }
        //Add file to loaded;
        if(res != null)
        {
            //Recupère l'emplacement du chapitre
            int place = -1;
            for(int i = 0, max = this._chapters.Count; i < max && place == -1; i++)
            {
                if (this._chapters[i].name.ToLower().Equals(chapter.ToLower()))
                {
                    place = i;
                }
            }
            //Creer un nouvel Objet LoadedMission
            LoadedMission lm = new LoadedMission();
            lm.level = mission;
            lm.mission = res;
            //Si le chapitre a été trouvé le rajoute dedans
            if(place >= 0)
            {
                this._chapters[place].loaded.Add(lm);
            } 
            else //Sinon creer le chapitre avant de l'ajouté dedans.
            {
                Chapter c = new Chapter(chapter);
                c.loaded.Add(lm);
                c.numberOfMission = ReadNumberOfMission(chapter);
            }
        }
        return res;
    }

    public Mission GetMission(string chapter, int mission)
    {
        Mission res = null;

        int chapterPos = -1;
        for (int i = 0, max = this._chapters.Count; i < max && chapterPos == -1; i++)
        {
            if (this._chapters[i].name.ToLower().Equals(chapter.ToLower()))
            {
                chapterPos = i;
            }
        }
        if (chapterPos >= 0)
        {
            List<LoadedMission> loaded = this._chapters[chapterPos].loaded;
            for (int i = 0, max = loaded.Count; i < max && res == null; i++)
            {
                if (loaded[i].level == mission)
                {
                    res = loaded[i].mission;
                }
            }
        }

        if (res == null)
        {
           res = ReadMission(chapter, mission);
        }
        return res;
    }

    public void PreLoadMission(string chapter, int mission)
    {
        Mission res = null;

        int chapterPos = -1;
        for(int i = 0, max = this._chapters.Count; i < max && chapterPos == -1; i++)
        {
            if(this._chapters[i].name.ToLower().Equals(chapter.ToLower()))
            {
                chapterPos = i;
            }
        }
        if(chapterPos >= 0)
        {
            List<LoadedMission> loaded = this._chapters[chapterPos].loaded;
            for (int i = 0, max = loaded.Count; i < max && res == null; i++)
            {
                if (loaded[i].level == mission)
                {
                    res = loaded[i].mission;
                }
            }
        }

        if (res == null)
        {
            ReadMission(chapter, mission);
        }
    }

    public void LoadChaptersName()
    {
        this._chapters = new List<Chapter>();
        string[] files = System.IO.Directory.GetDirectories(MISSION_DIR);
        for (int i = 0, max = files.Length; i < max; i++)
        {
            Chapter c = new Chapter(files[i]);
            c.numberOfMission = ReadNumberOfMission(files[i]);
            this._chapters.Add(c);
        }
    }
    public void LoadAllMissionOfChapter(string chapter)
    {
        int chapterPos = -1;
        for (int i = 0, max = this._chapters.Count; i < max && chapterPos == -1; i++)
        {
            if (this._chapters[i].name.ToLower().Equals(chapter.ToLower()))
            {
                chapterPos = i;
            }
        }
        if(chapterPos >= 0)
        {
            this._chapters[chapterPos].loaded.Clear();
        } 
        else
        {
            this._chapters.Add(new Chapter(chapter));
        }
        for (int i = 0, max = ReadNumberOfMission(chapter); i < max; i++)
        {
            ReadMission(chapter, i);
        }
    }


    public int ReadNumberOfMission(string chapter)
    {
        int res = 0;
        string[] files = System.IO.Directory.GetFiles(MISSION_DIR + chapter);
        for(int i = 0, max = files.Length; i < max; i++)
        {
            if (files[i].ToLower().EndsWith(".json"))
            {
                res++;
            }
        }
        return res;
    }






}
