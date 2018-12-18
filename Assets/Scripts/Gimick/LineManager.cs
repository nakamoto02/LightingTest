using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Lumiere.Player;

namespace Lumiere.Gimick
{
    public class LineManager : MonoBehaviour
    {
        [SerializeField] PlayerCore player;
        [SerializeField] PuzzleStage inStage;

        [SerializeField] List<PuzzleStage> puzzleStageAry = new List<PuzzleStage>();

        //=================================================
        void Start()
        {
            // Playerの取得
            if(player == null)
            {
                player = GameObject.FindObjectOfType<PlayerCore>();
            }

            // 子オブジェクトのエリア取得
            inStage = null;
            puzzleStageAry.Clear();
            foreach(Transform child in transform)
            {
                var stage = child.GetComponent<PuzzleStage>();
                if (stage == null) continue;

                stage.TargetPlayer = player;
                puzzleStageAry.Add(stage);
            }
        }
        void Update()
        {
            if (inStage == null) CheckPlayerinStage();
            else CheckPlayerOutArea();
        }
        //-------------------------------------------------
        //  Private
        //-------------------------------------------------
        // 範囲内にいるかどうか
        void CheckPlayerinStage()
        {
            inStage = puzzleStageAry.
                FirstOrDefault(index => index.IsInArea(player.Position));
            if (inStage == null) return;

            // 範囲内に入った
            inStage.AreaEnter();
        }
        // 範囲外に出たかどうか
        void CheckPlayerOutArea()
        {
            if (inStage.IsInArea(player.Position)) return;

            // 範囲外に出た
            inStage.AreaExit();
            inStage = null;
        }
    }
}