using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UIFightBall : MonoBehaviour,IDragHandler,IEndDragHandler, IBeginDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform _RectTransform;
    public RectTransform RectTransform
    {
        get
        {
            if (_RectTransform == null)
            {
                _RectTransform = gameObject.GetComponent<RectTransform>();
            }

            return _RectTransform;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FallUpdate();
    }

    #region show

    public RectTransform _FightBallAnchor;
    public Vector2 _Pos;

    public UIBallInfo _NormalBallInfo;
    public UIBallInfo _SPBallInfo;

    public UIBallInfo _BallIce;
    public UIBallInfo _BallClod;
    public UIBallInfo _BallStone;
    public UIBallInfo _BallPosion;
    public UIBallInfo _BallIron;

    public UIBallInfo _BallBombSmallNormal;
    public UIBallInfo _BallBombBigNormal;
    public UIBallInfo _BallBombSmallEnlarge;
    public UIBallInfo _BallBombBigEnlarge;
    public UIBallInfo _BallBombSmallHitTrap;
    public UIBallInfo _BallBombBigHitTrap;
    public UIBallInfo _BallBombSmallReact;
    public UIBallInfo _BallBombBigReact;
    public UIBallInfo _BallBombSmallLighting;
    public UIBallInfo _BallBombBigLighting;
    public UIBallInfo _BallBombSmallAuto;
    public UIBallInfo _BallBombBigAuto;

    public UIBallInfo _BallLineRowNormal;
    public UIBallInfo _BallLineClumnNormal;
    public UIBallInfo _BallLineCrossNormal;
    public UIBallInfo _BallLineRowEnlarge;
    public UIBallInfo _BallLineClumnEnlarge;
    public UIBallInfo _BallLineCrossEnlarge;
    public UIBallInfo _BallLineRowHitTrap;
    public UIBallInfo _BallLineClumnHitTrap;
    public UIBallInfo _BallLineCrossHitTrap;
    public UIBallInfo _BallLineRowReact;
    public UIBallInfo _BallLineClumnReact;
    public UIBallInfo _BallLineCrossReact;
    public UIBallInfo _BallLineRowLighting;
    public UIBallInfo _BallLineClumnLighting;
    public UIBallInfo _BallLineCrossLighting;
    public UIBallInfo _BallLineRowAuto;
    public UIBallInfo _BallLineClumnAuto;
    public UIBallInfo _BallLineCrossAuto;

    private BallInfo _BallInfo;
    public BallInfo BallInfo
    {
        get
        {
            return _BallInfo;
        }
    }

    public void SetBallInfo(BallInfo ballInfo)
    {
        _BallInfo = ballInfo;
    }

    public void ShowBall()
    {
        ShowNormal();
        ShowSP();
    }

    private void ShowNormal()
    {
        if (_BallInfo == null)
            return;

        _NormalBallInfo.ShowBallInfo(_BallInfo);
    }

    private void ClearSPBall()
    {
        if (_SPBallInfo != null)
        {
            ResourcePool.Instance.RecvIldeUIItem(_SPBallInfo.gameObject);
            _SPBallInfo = null;
        }
    }

    private void ShowSP()
    {
        if (_BallInfo == null || _BallInfo.BallSPType == BallType.None)
        {
            ClearSPBall();
            return;
        }

        if (_SPBallInfo != null && _SPBallInfo.BallSPType == _BallInfo.BallSPType)
        {
            _SPBallInfo.ShowBallInfo(_BallInfo);
            return;
        }

        ClearSPBall();
        switch (_BallInfo.BallSPType)
        {
            case BallType.Ice:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallIce.gameObject, _FightBallAnchor);
                break;
            case BallType.Clod:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallClod.gameObject, _FightBallAnchor);
                break;
            case BallType.Stone:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallStone.gameObject, _FightBallAnchor);
                break;
            case BallType.Posion:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallPosion.gameObject, _FightBallAnchor);
                break;
            case BallType.Iron:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallIron.gameObject, _FightBallAnchor);
                break;
            case BallType.BombSmall1:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallBombSmallNormal.gameObject, _FightBallAnchor);
                break;
            case BallType.BombBig1:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallBombBigNormal.gameObject, _FightBallAnchor);
                break;
            case BallType.BombSmallEnlarge1:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallBombSmallEnlarge.gameObject, _FightBallAnchor);
                break;
            case BallType.BombBigEnlarge:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallBombBigEnlarge.gameObject, _FightBallAnchor);
                break;
            case BallType.BombSmallHitTrap:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallBombSmallHitTrap.gameObject, _FightBallAnchor);
                break;
            case BallType.BombBigHitTrap:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallBombBigHitTrap.gameObject, _FightBallAnchor);
                break;
            case BallType.BombSmallReact:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallBombSmallReact.gameObject, _FightBallAnchor);
                break;
            case BallType.BombBigReact:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallBombBigReact.gameObject, _FightBallAnchor);
                break;
            case BallType.BombSmallLighting:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallBombSmallLighting.gameObject, _FightBallAnchor);
                break;
            case BallType.BombBigLighting:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallBombBigLighting.gameObject, _FightBallAnchor);
                break;
            case BallType.BombSmallAuto:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallBombSmallAuto.gameObject, _FightBallAnchor);
                break;
            case BallType.BombBigAuto:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallBombBigAuto.gameObject, _FightBallAnchor);
                break;

            case BallType.LineRow:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineRowNormal.gameObject, _FightBallAnchor);
                break;
            case BallType.LineClumn:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineClumnNormal.gameObject, _FightBallAnchor);
                break;
            case BallType.LineCross:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineCrossNormal.gameObject, _FightBallAnchor);
                break;
            case BallType.LineRowEnlarge:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineRowEnlarge.gameObject, _FightBallAnchor);
                break;
            case BallType.LineClumnEnlarge:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineClumnEnlarge.gameObject, _FightBallAnchor);
                break;
            case BallType.LineCrossEnlarge:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineCrossEnlarge.gameObject, _FightBallAnchor);
                break;
            case BallType.LineRowHitTrap:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineRowHitTrap.gameObject, _FightBallAnchor);
                break;
            case BallType.LineClumnHitTrap:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineClumnHitTrap.gameObject, _FightBallAnchor);
                break;
            case BallType.LineCrossHitTrap:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineCrossHitTrap.gameObject, _FightBallAnchor);
                break;
            case BallType.LineRowReact:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineRowReact.gameObject, _FightBallAnchor);
                break;
            case BallType.LineClumnReact:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineClumnReact.gameObject, _FightBallAnchor);
                break;
            case BallType.LineCrossReact:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineCrossReact.gameObject, _FightBallAnchor);
                break;
            case BallType.LineRowLighting:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineRowLighting.gameObject, _FightBallAnchor);
                break;
            case BallType.LineClumnLighting:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineClumnLighting.gameObject, _FightBallAnchor);
                break;
            case BallType.LineCrossLighting:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineCrossLighting.gameObject, _FightBallAnchor);
                break;
            case BallType.LineRowAuto:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineRowAuto.gameObject, _FightBallAnchor);
                break;
            case BallType.LineClumnAuto:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineClumnAuto.gameObject, _FightBallAnchor);
                break;
            case BallType.LineCrossAuto:
                _SPBallInfo = ResourcePool.Instance.GetIdleUIItem<UIBallInfo>(_BallLineCrossAuto.gameObject, _FightBallAnchor);
                break;
            default:
               
                break;
        }

        if (_SPBallInfo != null)
        {
            _SPBallInfo.ShowBallInfo(_BallInfo);
        }
    }

    private void ResetRoot()
    {
        _FightBallAnchor.transform.SetParent(transform);
        _FightBallAnchor.transform.localPosition = Vector3.zero;
    }

    public void Exchange(UIFightBall otherBall)
    {
        var temp = otherBall._FightBallAnchor;
        otherBall._FightBallAnchor = _FightBallAnchor;
        _FightBallAnchor = temp;

        var tempNormal = otherBall._NormalBallInfo;
        otherBall._NormalBallInfo = _NormalBallInfo;
        _NormalBallInfo = tempNormal;

        var tempSp = otherBall._SPBallInfo;
        otherBall._SPBallInfo = _SPBallInfo;
        _SPBallInfo = tempSp;

        otherBall.ResetRoot();
        ResetRoot();
    }

    public void Elimit()
    {
        //_FightBallAnchor._BallAnim.Play("BallExplore");
    }

    public void Explore()
    {
        ShowSP();
    }

    #endregion

    #region anim

    public float _FallSpeed = 2000;

    private UIFightBox _UIFightBox;

    public void SetFightBox(UIFightBox uiFightBox)
    {
        _UIFightBox = uiFightBox;
    }

    public void UIBallFall(List<BallInfo> anchorPath)
    {
        _FallPath = anchorPath;
        _FightBallAnchor.anchoredPosition = _UIFightBox.GetBallPosByIdx(_Pos.x, _Pos.y, anchorPath[0].Pos.x, anchorPath[0].Pos.y);
        _CurFallIdx = 0;
        FallNextPath();
    }

    private List<BallInfo> _FallPath;
    private int _CurFallIdx = 0;
    private Vector2 _MoveVec = Vector2.zero;
    private Vector2 _TargetPos = Vector2.zero;
    private float _LastTime = 0;

    private void FallNextPath()
    {
        ++_CurFallIdx;
        if (_CurFallIdx > 0 && _CurFallIdx < _FallPath.Count)
        {
            _TargetPos = _UIFightBox.GetBallPosByIdx(_Pos.x, _Pos.y, _FallPath[_CurFallIdx].Pos.x, _FallPath[_CurFallIdx].Pos.y);
            _MoveVec = _TargetPos - _FightBallAnchor.anchoredPosition;
            _LastTime = 1;
        }
        else
        {
            _TargetPos = Vector2.zero;
            _MoveVec = Vector2.zero;

            if (_CurFallIdx == _FallPath.Count - 1)
            {
                _FightBallAnchor.anchoredPosition = Vector2.zero;
            }
        }
    }

    public void FallUpdate()
    {
        if (_MoveVec == Vector2.zero)
            return;

        Vector2 moveVec = _MoveVec.normalized * _FallSpeed * Time.fixedDeltaTime;

        _MoveVec -= moveVec;
        _LastTime -= Time.fixedDeltaTime;
        if (_LastTime < 0)
        {
            _LastTime = 0;
            _MoveVec = Vector2.zero;
        }

        var destPoint = _FightBallAnchor.anchoredPosition + moveVec;
        if (_TargetPos != Vector2.zero)
        {
            var delta = _FightBallAnchor.anchoredPosition - _TargetPos;
            if (delta.magnitude < moveVec.magnitude)
            {
                destPoint = _TargetPos;
                _LastTime = 0;
                FallNextPath();
            }
        }
        _FightBallAnchor.anchoredPosition = destPoint;
    }
    
    #endregion

    #region opt

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _FightBallAnchor.transform.localScale = Vector3.one;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _FightBallAnchor.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        _UIFightBox.DragBall = this;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (BallBox.Instance.IsCanMoveTo(_UIFightBox.DragBall.BallInfo, BallInfo))
        {
            _UIFightBox.ExChangeBalls(this, _UIFightBox.DragBall);
        }
        _UIFightBox.DragBall = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_UIFightBox.DragBall == null)
        {
            _FightBallAnchor.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            return;
        }

        if (BallBox.Instance.IsCanMoveTo(_UIFightBox.DragBall.BallInfo, BallInfo))
        {
            _FightBallAnchor.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (this != _UIFightBox.DragBall)
        {
            _FightBallAnchor.transform.localScale = Vector3.one;
        }
    }

    #endregion
}
