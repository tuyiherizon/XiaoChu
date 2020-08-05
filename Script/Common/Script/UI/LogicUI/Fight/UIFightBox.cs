using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tables;

public class UIFightBox : UIBase
{
    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIFightBox, UILayer.PopUI, hash);
    }

    public static void ShowStage(StageInfoRecord stageRecord)
    {
        Hashtable hash = new Hashtable();
        hash.Add("StageInfo", stageRecord);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIFightBox, UILayer.PopUI, hash);
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //BallBox.Instance.Init();
        //BallBox.Instance.InitBallInfo();
        //InitBox(BallBox.Instance.BoxWidth, BallBox.Instance.BoxHeight);
        //DebugTest();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Show(Hashtable hash)
    {
        base.Show(hash);
        
        InitBox(BallBox.Instance.BoxWidth, BallBox.Instance.BoxHeight);
    }

    #region box

    public GameObject _BallPrefab;
    public GridLayoutGroup _FightBox;
    public GameObject _EliminateTipGO1;
    public GameObject _EliminateTipGO2;
    public GameObject _OptMask;

    private UIFightBall[][] _BallInfos;
    private int _BoxWidth;
    private int _BoxLength;

    public void OnReset()
    {
        ClearCheckGOs();
        BallBox.Instance.Refresh();
        UpdateBalls();
        //DebugTest();
    }

    public void RefreshNormal()
    {
        ClearCheckGOs();
        BallBox.Instance.RefreshNormal();
        UpdateBalls();
        //DebugTest();
    }

    public void OnBtnAutoEliminate()
    {
        var eliminate = BallBox.Instance.FindPowerEliminate();
        if (eliminate == null)
            return;

        var fromBall = GetBallUI(eliminate.FromBall);
        //_EliminateTipGO1.transform.position = fromBall.transform.position;

        var toBall = GetBallUI(eliminate.ToBall);
        //_EliminateTipGO2.transform.position = toBall.transform.position;

        if (!fromBall.BallInfo.IsCanExChange(toBall.BallInfo))
            return;

        ExChangeBalls(fromBall, toBall);
    }

    public UIFightBall GetBallUI(BallInfo ballInfo, bool isSafe = false)
    {
        return GetBallUI(ballInfo.Pos, isSafe);
    }

    public UIFightBall GetBallUI(Vector2 pos, bool isSafe = false)
    {
        int posX = (int)pos.x;
        int posY = (int)pos.y;

        return GetBallUI(posX, posY, isSafe);
    }

    public UIFightBall GetBallUI(int posX, int posY, bool isSafe = false)
    {
        if (posX < 0 || posX > _BallInfos.Length - 1)
            return null;

        if (posY < 0 || posY > _BoxLength * 2 - 1)
        {
            if (isSafe)
            {
                if (posY > _BoxLength * 2 - 1)
                {
                    return _BallInfos[posX][_BoxLength * 2 - 1];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        return _BallInfos[posX][posY];
    }

    public void InitBox(int x, int y)
    {
        _BoxWidth = x;
        _BoxLength = y;

        _FightBox.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _FightBox.constraintCount = x;

        _BallInfos = new UIFightBall[x][];
        for (int i = 0; i < x; ++i)
        {
            _BallInfos[i] = new UIFightBall[y * 2];
            for (int j = 0; j < y * 2; ++j)
            {
                GameObject ballGO = GameObject.Instantiate(_BallPrefab.gameObject);
                UIFightBall ballInfo = ballGO.GetComponentInChildren<UIFightBall>();

                if (j < y)
                {
                    ballGO.gameObject.SetActive(true);
                }
                else
                {
                    ballGO.gameObject.SetActive(false);
                }
                ballGO.transform.SetParent(_FightBox.transform);
                ballGO.transform.localScale = Vector3.one;
                var localPos = GetBallPosByIdx(0,0, i, j);
                ballGO.transform.localPosition = localPos;

                _BallInfos[i][j] = ballInfo;
                _BallInfos[i][j]._Pos = new Vector2(i, j);
                _BallInfos[i][j].SetFightBox(this);
            }
        }

        for (int j = 0; j < _BoxLength; ++j)
        {
            for (int i = 0; i < _BoxWidth; ++i)
            {
                _BallInfos[i][j].SetBallInfo(BallBox.Instance._BallBoxInfo[i][j]);
                _BallInfos[i][j].ShowBall();
            }
        }

    }

    public void UpdateBalls()
    {
        for (int j = 0; j < _BoxLength; ++j)
        {
            for (int i = 0; i < _BoxWidth; ++i)
            {
                _BallInfos[i][j].ShowBall();
            }
        }
    }

    public float _BallWidth = 165;
    public float _BallHeight = 165;

    public Vector2 GetBallPosByIdx(float curX, float curY, float targetX, float targetY)
    {
       var idxPos = new Vector2((targetX - curX) * _BallWidth, (targetY - curY) * _BallHeight);
       return idxPos;
    }

    public List<Vector3> FindPath(Vector2 fromPos, Vector2 toPos)
    {
        int fromPosX = (int)fromPos.x;
        int fromPosY = (int)fromPos.y;
        int toPosX = (int)toPos.x;
        int toPosY = (int)toPos.y;

        List<Vector3> pathList = new List<Vector3>();
        while (true)
        {
            if (fromPosY > _BoxLength - 1)
            {
                fromPosY = _BoxLength - 1;
            }
            else if (fromPosX > toPosX && fromPosY > toPosY)
            {
                --fromPosX;
                --fromPosY;
            }
            else if (fromPosX < toPosX && fromPosY > toPosY)
            {
                ++fromPosX;
                --fromPosY;
            }
            else if (fromPosX > toPosX)
            {
                --fromPosX;
            }
            else if (fromPosX < toPosX)
            {
                ++fromPosX;
            }
            else if (fromPosY > toPosY)
            {
                --fromPosY;
                
            }
            else
            {

            }
            var uiBall = GetBallUI(fromPosX, fromPosY);
            pathList.Add(uiBall.transform.position);

            if (fromPosX == toPosX && fromPosY == toPosY)
            {
                break;
            }
        }

        return pathList;
    }

    public List<Vector3> GetMovePathPos(List<BallInfo> pathBalls)
    {
        List<Vector3> pathList = new List<Vector3>();
        foreach (var pathBall in pathBalls)
        {
            if (pathBall.Pos.y > _BoxLength - 1)
            {
                int start = (int)pathBall.Pos.y;
                int end = _BoxLength - 1;
                for (int i = start; i >= _BoxLength; --i)
                {
                    var uiBall = GetBallUI((int)pathBall.Pos.x, i, true);
                    pathList.Add(uiBall.transform.position);
                }
            }
            else
            {
                var uiBall = GetBallUI(pathBall.Pos, true);
                pathList.Add(uiBall.transform.position);
            }
        }
        return pathList;
    }

    private float GetFallPathTime(List<BallInfo> pathBalls)
    {
        float pathTime = 0;
        foreach (var pathBall in pathBalls)
        {
            if (pathBall.Pos.y > _BoxLength - 1)
            {
                pathTime += (pathBall.Pos.y - _BoxLength + 1) * 0.1f;
            }
            else
            {
                pathTime += 0.1f;
            }
        }

        return pathTime;
    }

    #endregion

    #region anim

    public GameObject _ElimitCheckPrefab;
    public float _ExchangeBallTime = 0.5f;
    public float _ElimitBallTime = 0.25f;

    private List<GameObject> _ElimitCheckGOs = new List<GameObject>();
    private void CreateCheckGOs(int num)
    {
        if (_ElimitCheckGOs.Count < num)
        {
            int deltaCnt = num - _ElimitCheckGOs.Count;
            for (int i = 0; i < deltaCnt; ++i)
            {
                GameObject checkGO = GameObject.Instantiate(_ElimitCheckPrefab);
                checkGO.transform.SetParent(_ElimitCheckPrefab.transform.parent);
                checkGO.transform.localScale = Vector3.one;
                _ElimitCheckGOs.Add(checkGO);
            }
        }
    }

    public void ClearCheckGOs()
    {
        for (int i = 0; i < _ElimitCheckGOs.Count; ++i)
        {
            _ElimitCheckGOs[i].transform.position = new Vector3(10000, 0, 0);
        }
    }

    public void ExChangeBalls(UIFightBall ballA, UIFightBall ballB)
    {
        if (!ballA.BallInfo.IsCanMove() || !ballB.BallInfo.IsCanMove())
            return;

        StarAnim();
        iTween.MoveTo(ballA._FightBallAnchor.gameObject, ballB.transform.position, _ExchangeBallTime);
        iTween.MoveTo(ballB._FightBallAnchor.gameObject, ballA.transform.position, _ExchangeBallTime);
        StartCoroutine(AnimEnd(ballA, ballB));
    }

    public IEnumerator AnimEnd(UIFightBall ballA, UIFightBall ballB)
    {
        yield return new WaitForSeconds(_ExchangeBallTime);
        BallBox.Instance.MoveBall(ballA.BallInfo, ballB.BallInfo);
        ballA.Exchange(ballB);

        var moveList = new List<BallInfo>() { ballA.BallInfo, ballB.BallInfo };
        var elimitBalls = BallBox.Instance.CheckNormalEliminate(moveList);
        var spElimitMove = BallBox.Instance.CheckSpMove(moveList);
        var spElimitElimit = BallBox.Instance.CheckSpElimit(elimitBalls);
        var exploreBalls = BallBox.Instance.CurrentElimitnate();
        var afterElimit = BallBox.Instance.AfterElimitnate();

        BallBox.AddBallInfos(elimitBalls, spElimitMove);
        BallBox.AddBallInfos(elimitBalls, spElimitElimit);
        BallBox.AddBallInfos(elimitBalls, exploreBalls);
        BallBox.AddBallInfos(elimitBalls, afterElimit);
        if (elimitBalls.Count == 0)
        {
            iTween.MoveTo(ballA._FightBallAnchor.gameObject, ballB.transform.position, _ExchangeBallTime);
            iTween.MoveTo(ballB._FightBallAnchor.gameObject, ballA.transform.position, _ExchangeBallTime);

            yield return new WaitForSeconds(_ExchangeBallTime);

            BallBox.Instance.MoveBack(ballA.BallInfo, ballB.BallInfo);
            ballA.Exchange(ballB);

            EndAnim();
        }
        else
        {
            
            do
            {
                foreach (var elimitBall in elimitBalls)
                {
                    var uiBall = GetBallUI(elimitBall);
                    uiBall.Elimit();
                }

                yield return new WaitForSeconds(_ElimitBallTime);

                foreach (var elimitBall in elimitBalls)
                {
                    var uiBall = GetBallUI(elimitBall);
                    uiBall.ShowBall();

                }
                
                yield return new WaitForSeconds(_ElimitBallTime);

                var fillingTime = Filling();
                yield return new WaitForSeconds(fillingTime);

                elimitBalls = BallBox.Instance.CheckNormalEliminate(_FillingBalls);
                var spSubElimitBalls = BallBox.Instance.CheckSpElimit(elimitBalls);
                var subexploreBalls = BallBox.Instance.CurrentElimitnate();
                var subafterElimit = BallBox.Instance.AfterElimitnate();

                BallBox.AddBallInfos(elimitBalls, spSubElimitBalls);
                BallBox.AddBallInfos(elimitBalls, subexploreBalls);
                BallBox.AddBallInfos(elimitBalls, subafterElimit);
            }
            while (elimitBalls.Count > 0);
        }

        var reShowList = BallBox.Instance.RoundEnd();

        if (reShowList != null && reShowList.Count > 0)
        {
            foreach (var elimitBall in reShowList)
            {
                var uiBall = GetBallUI(elimitBall);
                uiBall.ShowBall();
            }
        }

        var eliminate = BallBox.Instance.FindPowerEliminate();
        if(eliminate == null)
        {
            Debug.Log("No eliminate!!!");
            yield return new WaitForSeconds(1);

            while (eliminate == null)
            {
                RefreshNormal();
                eliminate = BallBox.Instance.FindPowerEliminate();
            }
        }

        EndAnim();
    }

    List<BallInfo> _FillingBalls = new List<BallInfo>();

    public float Filling()
    {
        var fallExchanges = BallBox.Instance.ElimitnateFall();
        //var fillBalls = BallBox.Instance.FillEmpty();
        _FillingBalls.Clear();

        float maxMoveTime = 0.0f;
        for (int i = 0; i < fallExchanges.Count; ++i)
        {
            var uiBall1 = GetBallUI(fallExchanges[i].FromBall);
            if (uiBall1 != null)
            {
                uiBall1.ShowBall();
            }

            var uiBall2 = GetBallUI(fallExchanges[i].ToBall);
            uiBall2.ShowBall();

            uiBall2._FightBallAnchor.anchoredPosition = GetBallPosByIdx(fallExchanges[i].ToBall.Pos.x, fallExchanges[i].ToBall.Pos.y, fallExchanges[i].FromBall.Pos.x, fallExchanges[i].FromBall.Pos.y);
            var movePath = GetMovePathPos(fallExchanges[i].PathBalls);
            //movePath.Add(uiBall2.transform.position);
            float moveTime = Mathf.Abs(movePath.Count) * 0.1f;
            //Debug.Log("MovePos:" + fallExchanges[i].ToBall.Pos + ", anchorePos:" + uiBall2._UIBallInfo.RectTransform.anchoredPosition + ", path:" + movePath.Count);

            Hashtable hash = new Hashtable();
            hash.Add("position", uiBall2.transform.position);
            hash.Add("path", movePath.ToArray());
            hash.Add("speed", 6);
            //hash.Add("time", moveTime);
            iTween.MoveTo(uiBall2._FightBallAnchor.gameObject, hash);

            //uiBall2.UIBallFall(fallExchanges[i].PathBalls);

            if (moveTime > maxMoveTime)
                maxMoveTime = moveTime;

            _FillingBalls.Add(uiBall2.BallInfo);
        }

        
        //for (int i = 0; i < fillBalls.Count; ++i)
        //{
        //    var uiBall2 = GetBallUI(fillBalls[i].ToBall);
        //    uiBall2.ShowBall();

        //    uiBall2._UIBallInfo.RectTransform.anchoredPosition = GetBallPosByIdx(fillBalls[i].ToBall.Pos.x, fillBalls[i].ToBall.Pos.y, fillBalls[i].FromBall.Pos.x, fillBalls[i].FromBall.Pos.y);
        //    float moveTime = (fillBalls[i].FromBall.Pos.x - fillBalls[i].ToBall.Pos.x) + (fillBalls[i].FromBall.Pos.y - fillBalls[i].ToBall.Pos.y) * 0.1f;
        //    Hashtable hash = new Hashtable();
        //    hash.Add("position", uiBall2.transform.position);
        //    //hash.Add("speed", 10);
        //    hash.Add("time", moveTime);
        //    iTween.MoveTo(uiBall2._UIBallInfo.gameObject, hash);

        //    if (moveTime > maxMoveTime)
        //        maxMoveTime = moveTime;
        //}

        return maxMoveTime;
    }

    public void StarAnim()
    {
        _OptMask.SetActive(true);
    }

    public void EndAnim()
    {
        _OptMask.SetActive(false);
    }

    #endregion

    #region opt

    private UIFightBall _DragBall;
    public UIFightBall DragBall
    {
        get
        {
            return _DragBall;
        }
        set
        {
            _DragBall = value;
        }
    }

    #endregion

    public void DebugTest()
    {
        for (int j = BallBox.Instance._BallBoxInfo[0].Length - 1; j >= 0; --j)
        {
            string ballInfo = "";
            for (int i = 0; i < BallBox.Instance._BallBoxInfo.Length; ++i)
            {
                ballInfo += (int)BallBox.Instance._BallBoxInfo[i][j].BallType + " ";
            }
            Debug.Log(ballInfo);
        }

        string testStr = "";
        testStr += (int)BallBox.Instance._BallBoxInfo[0][0].BallType + " ";
        testStr += (int)BallBox.Instance._BallBoxInfo[1][1].BallType + " ";
        testStr += (int)BallBox.Instance._BallBoxInfo[2][2].BallType + " ";
        testStr += (int)BallBox.Instance._BallBoxInfo[3][3].BallType + " ";
        testStr += (int)BallBox.Instance._BallBoxInfo[4][4].BallType + " ";
        testStr += (int)BallBox.Instance._BallBoxInfo[5][5].BallType + " ";
        Debug.Log("UIFIghtBox test:" + testStr);
    }

    public void TestBall()
    {
        var eliminate = BallBox.Instance.FindPowerEliminate();
        if (eliminate == null)
        {
            while (eliminate == null)
            {
                RefreshNormal();
                eliminate = BallBox.Instance.FindPowerEliminate();
            }
        }

        var ballA = GetBallUI(eliminate.FromBall);
        var ballB = GetBallUI(eliminate.ToBall);
        BallBox.Instance.MoveBall(ballA.BallInfo, ballB.BallInfo);

        var moveList = new List<BallInfo>() { ballA.BallInfo, ballB.BallInfo };
        var elimitBalls = BallBox.Instance.CheckNormalEliminate(moveList);
        var spElimitMove = BallBox.Instance.CheckSpMove(moveList);
        var spElimitElimit = BallBox.Instance.CheckSpElimit(elimitBalls);
        var exploreBalls = BallBox.Instance.CurrentElimitnate();

        BallBox.AddBallInfos(elimitBalls, spElimitMove);
        BallBox.AddBallInfos(elimitBalls, spElimitElimit);
        BallBox.AddBallInfos(elimitBalls, exploreBalls);

        do
        {
            var fallExchanges = BallBox.Instance.ElimitnateFall();

            elimitBalls = BallBox.Instance.CheckNormalEliminate(_FillingBalls);
            var spSubElimitBalls = BallBox.Instance.CheckSpElimit(elimitBalls);
            elimitBalls.AddRange(spSubElimitBalls);
            BallBox.Instance.CurrentElimitnate();
            //var subSpElimitBalls = BallBox.Instance.CheckSpElimit(elimitBalls);
            //elimitBalls.AddRange(subSpElimitBalls);
        }
        while (elimitBalls.Count > 0);

        var reShowList = BallBox.Instance.RoundEnd();

        eliminate = BallBox.Instance.FindPowerEliminate();
        if (eliminate == null)
        {
            while (eliminate == null)
            {
                RefreshNormal();
                eliminate = BallBox.Instance.FindPowerEliminate();
            }
        }

        foreach (var uiBallRow in _BallInfos)
        {
            foreach (var uiBall in uiBallRow)
            {
                uiBall.ShowBall();
            }
        }

    }

}
