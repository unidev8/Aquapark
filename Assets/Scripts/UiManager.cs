using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

namespace PathCreation.Examples
{

    public class UiManager : MonoBehaviour
    {


        public TextMeshProUGUI playerPos;

        public Image fillBar;

        [HideInInspector]
        public DistanceCounter playerDistanceCounter;
        private TextMeshPro playerText;
        [HideInInspector]
        private Finish finish;
        private PathFollower playerPath;

        public Image[] leaders;
        public Sprite GreenBg;
        public GameObject victoryImage;


        public List<AI> AIList = new List<AI>();

        public bool finishOnce;


        private void Awake()
        {
            playerDistanceCounter = GameObject.FindGameObjectWithTag("Player").GetComponent<DistanceCounter>();
            finish = GameObject.FindGameObjectWithTag("Finish").GetComponent<Finish>();
            playerText = playerDistanceCounter.text;
            playerPath = playerDistanceCounter.GetComponent<PathFollower>();

            finishOnce = false;

        }

        void Update()
        {     
            playerPos.text = playerText.text;

            float finishDistance = playerPath.pathCreator.path.GetClosestDistanceAlongPath(finish.transform.position);

            fillBar.fillAmount = playerPath.distanceTravelled / finishDistance;

            if(playerPath.isfinish && !finishOnce)
            {
                finishOnce = true;
                SetLeaderBoard();
            }
        }


        List<T> GetUniqueRandomElements<T>(List<T> inputList, int count)
        {
            List<T> inputListClone = new List<T>(inputList);
            Shuffle(inputListClone);
            return inputListClone.GetRange(0, count);
        }

        void Shuffle<T>(List<T> inputList)
        {
            for (int i = 0; i < inputList.Count - 1; i++)
            {
                T temp = inputList[i];
                int rand = Random.Range(i, inputList.Count);
                inputList[i] = inputList[rand];
                inputList[rand] = temp;
            }
        }

        public void SetLeaderBoard()
        {
            leaders[playerDistanceCounter.rank - 1].sprite = GreenBg;
            leaders[playerDistanceCounter.rank - 1].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Player" + " : " + 
                Player.Instance.GetComponent <Player>().score;

            if(playerDistanceCounter.rank<=3)
            {
                victoryImage.SetActive(true);
            }
            else
            {
                victoryImage.SetActive(false);
            }

            AIList = AIList.OrderBy(obj => obj.gameObject.GetComponent<DistanceCounter>().rank).ToList();
            //var uniqueRandomList = GetUniqueRandomElements(NameList, 6);

            int j = 0;
            for (int i = 0; i < leaders.Length; i++)
            {
                if(i != playerDistanceCounter.rank - 1)
                {
                    leaders[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = AIList[j].strName + " : " + AIList [j].score;
                    j++;
                }                
            }

            for (int i = 0; i < leaders.Length; i++)
            {
                leaders[i].rectTransform.DOLocalMoveX(0, (i + 0.5f) * 0.15f).SetDelay(3.75f).SetEase(Ease.OutBack);
            }

        }

    }
}