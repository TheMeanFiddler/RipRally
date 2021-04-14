using UnityEngine;

public class Spectator:MonoBehaviour
{
    Transform _carPlayer;
    Animator Anim;
    int IdleHash;

    void Start()
    {
        Anim = GetComponent<Animator>();
        IdleHash = Anim.GetCurrentAnimatorStateInfo(0).fullPathHash;
        int AnimType = Random.Range(0, 3); //second number is exclusive
        Anim.SetInteger("AnimType", AnimType);
        float Delay = Random.Range(0f, 5f);
        Anim.speed = 0f;
        if(DrivingPlayManager.Current!=null)
        DrivingPlayManager.Current.OnPlayerInstantate += Current_OnPlayerInstantate;

     
        StartCoroutine(ClipDelay(Delay));
    }
    //clip delay makes each specator start waving at diff time
    System.Collections.IEnumerator ClipDelay(float secs)
    {
        yield return new WaitForSeconds(secs);
        Anim.speed = 1;
    }

    private void Current_OnPlayerInstantate(Transform trPlayer)
    {
        _carPlayer = trPlayer;
    }

    void Update()
    {
        if (PlayerManager.Type == "CarPlayer" || PlayerManager.Type == "Replayer")
        {
            if (_carPlayer == null) return;
            transform.LookAt(_carPlayer);
            Anim.SetFloat("PlayerDist", Vector3.Distance(_carPlayer.position, transform.position));
        }
        
    }
    void Destroy()
    {
        if(DrivingPlayManager.Current!=null)
        DrivingPlayManager.Current.OnPlayerInstantate -= Current_OnPlayerInstantate;
    }
}

