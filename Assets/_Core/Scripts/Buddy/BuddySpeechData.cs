using UnityEngine;

[CreateAssetMenu(fileName = "New BuddySpeechData", menuName = "ScriptableObjects/BuddySpeechData", order = 1)]
public class BuddySpeechData : ScriptableObject
{
    [System.Serializable]
    public class SpeechData
    {
        [TextArea(1,3)] public string[] texts;
        public float cooldownTime;
    }

    public SpeechData pickMeUpData;
    public SpeechData niceToMeetYouData;
    public SpeechData getShakenData;
    public SpeechData goExploreData;
}
