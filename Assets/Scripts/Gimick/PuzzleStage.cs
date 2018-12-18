using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Lumiere.Gimick.Lamp;
using Lumiere.Player;

namespace Lumiere.Gimick
{
    public enum LineAreaState
    {
        Wait,
        TopSearch,
        AllSearch,
        Connecting,
        Clear
    }

    public class PuzzleStage : MonoBehaviour
    {
        AreaLineRenderer areaRenderer;

        // 範囲
        public Vector3 areaPosition = new Vector3(0.0f, 0.0f, 0.0f);    // ローカル座標
        public Vector3 areaSize     = new Vector3(10.0f, 10.0f, 10.0f); // サイズ
        public Vector3 areaPivot    = new Vector3(0.5f, 0.0f, 0.5f);    // 中心

        // IDrawLine
        IDrawLine   leadObject;
        [SerializeField] LineFirefly connectObj;
        [SerializeField] StartLamp startLamp;
        [SerializeField] EndLamp   endLamp;
        [SerializeField] List<BaseStageLamp> stageLampAry = new List<BaseStageLamp>();

        //-------------------------------------------------
        //  プロパティ
        //-------------------------------------------------
        public LineAreaState AreaState { get; private set; }
        public PlayerCore TargetPlayer { get; set; }
        //=================================================
        void Start()
        {
            // AreaLineRendererを取得
            areaRenderer = GetComponent<AreaLineRenderer>();

            // 初期化
            ResetLine();

            AreaState = LineAreaState.Wait;
        }
        void Update()
        {
            switch (AreaState)
            {
                case LineAreaState.Wait: return;
                case LineAreaState.TopSearch:  TopDrawLineSearch(); break;
                case LineAreaState.AllSearch:  AllDrawLineSearch(); break;
                case LineAreaState.Connecting:   break;
                case LineAreaState.Clear: break;
            }
        }
        void OnDrawGizmosSelected()
        {
            Vector3 pivotBase = new Vector3(0.5f, 0.5f, 0.5f);

            // Cube
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(
                transform.position + areaPosition + Vector3.Scale(areaSize, pivotBase - areaPivot),
                areaSize
                );

            // Center
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(
                transform.position + areaPosition,
                0.1f
                );
        }
        //-------------------------------------------------
        //  Public
        //-------------------------------------------------
        // 範囲内であるかどうか
        public bool IsInArea(Vector3 position)
        {
            return
                areaPosition.x - areaSize.x * areaPivot.x       <= position.x - transform.position.x &&
                areaPosition.x + areaSize.x * (1 - areaPivot.x) >= position.x - transform.position.x &&
                areaPosition.y - areaSize.y * areaPivot.y       <= position.y - transform.position.y &&
                areaPosition.y + areaSize.y * (1 - areaPivot.y) >= position.y - transform.position.y &&
                areaPosition.z - areaSize.z * areaPivot.z       <= position.z - transform.position.z &&
                areaPosition.z + areaSize.z * (1 - areaPivot.z) >= position.z - transform.position.z;
        }
        // プレイヤーがエリア内に入った時
        public void AreaEnter()
        {
            areaRenderer.LineEnabled(true);
            if (AreaState == LineAreaState.Clear) return;

            // クリア状態でないとき
            AreaState = LineAreaState.TopSearch;
        }
        // プレイヤーがエリア外に出たとき
        public void AreaExit()
        {
            if (AreaState == LineAreaState.Clear) return;
            areaRenderer.LineEnabled(false);

            // クリア状態でないとき
            // StageLamp以外がleadObjectの時
            if (AreaState != LineAreaState.TopSearch) TopLineCut();
            // 接続中の時
            if (AreaState == LineAreaState.Connecting) connectObj.EndProcessing();

            AreaState = LineAreaState.Wait;
        }
        //-------------------------------------------------
        //  Private
        //-------------------------------------------------
        // 先端のみ検索
        void TopDrawLineSearch()
        {
            if (!TargetPlayer.IsDraw) return;

            var lamp = leadObject as BaseStageLamp;
            if(lamp.IsActionInRange(TargetPlayer.DrawPosition))
            {
                // プレイヤーに線をつなぐ
                StartConnecting(TargetPlayer, PlayerConnected);
            }
        }

        // 全体を検索
        void AllDrawLineSearch()
        {
            if (areaRenderer.IsHitLine())
            {
                TopLineCut();
                AreaState = LineAreaState.TopSearch;
                return;
            }

            foreach(BaseStageLamp lamp in stageLampAry)
            {
                if (IsOneBeforeLine(lamp)) continue; // 一つ前のランプ
                if (!lamp.IsDraw) continue;
                if (!lamp.IsActionInRange(TargetPlayer.DrawPosition)) continue;
                
                if (lamp is StartLamp)
                {
                    StartConnecting(lamp, StartLampConnected);
                } else 
                if (lamp is EndLamp)
                {
                    StartConnecting(lamp, EndLampConnected);
                } else
                {
                    StartConnecting(lamp, StageLampConnected);
                }

                areaRenderer.RemoveLineObject(TargetPlayer);
                break;
            }
        }

        // 接続開始
        void StartConnecting(IDrawLine target, ConnectedEventHandler action)
        {
            // 生成
            connectObj.Initialize(leadObject.DrawPosition, target, action);

            // 設定
            LineConnect(connectObj);
            AreaState = LineAreaState.Connecting;
        }

        // プレイヤーに接続した際
        void PlayerConnected(IDrawLine target)
        {
            if (AreaState != LineAreaState.Connecting) return;
            TopChangeLineConnect(target);
            AreaState = LineAreaState.AllSearch;
        }

        // ステージランプに接続した際
        void StageLampConnected(IDrawLine target)
        {
            if (AreaState != LineAreaState.Connecting) return;
            TopChangeLineConnect(target);
            AreaState = LineAreaState.TopSearch;

            var lamp = leadObject as BaseStageLamp;
            lamp.ConnectLineAction();
        }

        // スタートランプに接続した際
        void StartLampConnected(IDrawLine target)
        {
            if (AreaState != LineAreaState.Connecting) return;
            ResetLine();
        }

        // エンドランプに接続した際
        void EndLampConnected(IDrawLine target)
        {
            if (AreaState != LineAreaState.Connecting) return;

            bool isClear = stageLampAry.
                Where(index => !(index is StartLamp) && !(index is EndLamp)).
                All(index => !index.IsDraw);

            if(isClear)
            {   // クリア判定
                AreaState = LineAreaState.Clear;


            } else
            {
                TopLineCut();
                AreaState = LineAreaState.TopSearch;
            }
        }

        // 先頭を変えて線をつなぐ
        void TopChangeLineConnect(IDrawLine next)
        {
            areaRenderer.RemoveLineObject(leadObject);
            LineConnect(next);
        }

        // 線をつなぐ
        void LineConnect(IDrawLine next)
        {
            areaRenderer.AddLineObject(next);
            leadObject = next;
        }

        // 先端の線を切る
        void TopLineCut()
        {
            areaRenderer.RemoveLineObject(leadObject);
            leadObject = areaRenderer.TopLineObject;
        }

        // リセット
        void ResetLine()
        {
            foreach (var lamp in stageLampAry)
            {
                lamp.Initialize();
            }

            leadObject = startLamp;
            areaRenderer.InitLineObjects(leadObject);
            AreaState = LineAreaState.TopSearch;
        }

        // 線を一つ戻してつなぎなおす
        bool IsOneBeforeLine(BaseStageLamp lamp) { return lamp == (BaseStageLamp)areaRenderer.SecondLineObject; }
    }
}