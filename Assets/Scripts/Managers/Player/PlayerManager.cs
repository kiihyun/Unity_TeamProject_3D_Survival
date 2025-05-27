using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    private static PlayerManager instance;
    public static PlayerManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)   //인스턴스가 null이면 해당 클래스가 인스턴스
        {
            instance = this;
            DontDestroyOnLoad(gameObject);      //씬 로드 시, 삭제되지 않음
        }
        else
        {
            Destroy(gameObject);        //null이 아니면(싱글톤이 또 있으면) 삭제
        }
    }

    public GameObject player;
    public PlayerController controller;
    public PlayerCondition condition;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        controller = player.GetComponent<PlayerController>();
        condition = player.GetComponent<PlayerCondition>();
    }
}

