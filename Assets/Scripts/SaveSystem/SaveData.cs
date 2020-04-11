using System.Collections.Generic;

namespace SaveSystem
{
    [System.Serializable]
    public class SaveData
    {
        private static SaveData _current;

        public static SaveData current
        {
            get
            {
                if (_current == null)
                {
                    _current = new SaveData();
                }

                return _current;
            }
            set
            {
                if (value != null)
                {
                    _current = value;
                }
            }
        }

        public CheckpointManagerData checkpointData;
        public int checkpointIndex;
        public int levelIndex;

        public float playerSkillLengths;
        public int playerSkillPoints;
    }
    
    
    
    [System.Serializable]
    public class CheckpointManagerData
    {
        public int currentIndex;
    }
}