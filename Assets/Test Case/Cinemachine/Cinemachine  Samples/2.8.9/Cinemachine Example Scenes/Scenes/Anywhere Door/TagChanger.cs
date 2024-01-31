using UnityEngine;

public class TagChanger : MonoBehaviour
{
    public void PlayerTagChanger() => tag = "Player";
    public void UntaggedTagChanger() => tag = "Untagged";
}