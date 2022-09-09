using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SettingsData 
{
    public class Data
    {
        public int msaa;
        public float renderScale;
        public int vSync;
        public float musicVolume;
        public float soundEffectVolume;
        public string langue;

        public void Apply(Data d)
        {
            this.msaa = d.msaa;
            this.renderScale = d.renderScale;
            this.vSync = d.vSync;
            this.musicVolume = d.musicVolume;
            this.soundEffectVolume = d.soundEffectVolume;
            this.langue = d.langue;
        }
    }

    public static Data SavedSettings = new Data();
    public static Data UnsavedSettings = new Data();

    public static void ApplySavedToUnsaved()
    {
        UnsavedSettings.Apply(SettingsData.SavedSettings);
    }
    public static void ApplyUnsavedToSaved()
    {
        SavedSettings.Apply(UnsavedSettings);
    }

}
