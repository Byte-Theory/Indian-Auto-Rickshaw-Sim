using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Tween : MonoBehaviour
{
    [Tooltip("true if you want to use multiple tween")]
    public bool canPlayMultipleTween;

    [Tooltip("true is you want to disable game object on complete")]
    public bool disableGameObjectOnComplete;

    [Space]
    public TweenData[] tweenCollection;

    [SerializeField]

    private void Awake()
    {
        DOTween.Init();
    }
    private void OnEnable()
    {
        TweenEnable();
    }

    private void OnDisable()
    {

    }

    public void PlayTween(string tweenName)
    {
        if(string.IsNullOrEmpty(tweenName))
        {
            return;
        }

        foreach (var item in tweenCollection)
        {
            if(item.tweenName == tweenName && item.tweenEvent == TweenEvents.custom)
            {
                PlayTween(item);
            }
        }
    }

    public void PlayTween(int tweenIndex)
    {
        if (tweenIndex < 0 || tweenIndex >= tweenCollection.Length)
        {
            return;
        }

        if (tweenCollection[tweenIndex].tweenEvent == TweenEvents.custom)
        {
            PlayTween(tweenCollection[tweenIndex]);
        }
    }

    public void TweenEnable()
    {
        foreach (TweenData tweenData in tweenCollection)
        {
            if(tweenData.tweenEvent == TweenEvents.enable)
            {
                PlayTween(tweenData);
            }
        }
    }

    public void TweenDisable()
    {
        UnityEvent disableEvent = new UnityEvent();
        disableEvent.AddListener(() => gameObject.SetActive(!disableGameObjectOnComplete));

        foreach (TweenData tweenData in tweenCollection)
        {
            if (tweenData.tweenEvent == TweenEvents.disable)
            {
                PlayTween(tweenData, disableEvent);
            }
        }
    }

    public void PlayTween(TweenData tweenData, UnityEvent callback = null)
    {
        if(DOTween.IsTweening(transform) && !canPlayMultipleTween)
        {
            DOTween.Kill(transform);
        }

        List<UnityEvent> tweenCompeteEvents = new List<UnityEvent>();
        tweenCompeteEvents.Add(tweenData.OnComplete);
        tweenCompeteEvents.Add(callback);

        switch (tweenData.tweenType)
        {
            case TweenType.move:
                if (tweenData.tdMove.isLocalSpace)
                {
                    if(tweenData.useTweenInitialValue)
                    {
                        transform.localPosition = tweenData.tdMove.tweenStartValue;
                    }

                    if (tweenData.tdMove.numOfJumps > 0)
                    {
                        transform.DOLocalJump(tweenData.tdMove.tweenEndValue, tweenData.tdMove.jumpPower, tweenData.tdMove.numOfJumps, tweenData.tdMove.tweenDuration, tweenData.tdMove.isSnapping)
                                    .SetDelay(tweenData.tdMove.tweenDelay).SetEase(tweenData.tdMove.easeType). OnComplete(() => { PlayOnComplete(tweenCompeteEvents); });
                    }
                    else
                    {
                        transform.DOLocalMove(tweenData.tdMove.tweenEndValue, tweenData.tdMove.tweenDuration, tweenData.tdMove.isSnapping)
                                    .SetDelay(tweenData.tdMove.tweenDelay).SetEase(tweenData.tdMove.easeType).OnComplete(() => { PlayOnComplete(tweenCompeteEvents); });
                    }
                }
                else
                {
                    if (tweenData.useTweenInitialValue)
                    {
                        transform.position = tweenData.tdMove.tweenStartValue;
                    }

                    if (tweenData.tdMove.numOfJumps > 0)
                    {
                        transform.DOJump(tweenData.tdMove.tweenEndValue, tweenData.tdMove.jumpPower, tweenData.tdMove.numOfJumps, tweenData.tdMove.tweenDuration, tweenData.tdMove.isSnapping);
                    }
                    else
                    {
                        transform.DOMove(tweenData.tdMove.tweenEndValue, tweenData.tdMove.tweenDuration, tweenData.tdMove.isSnapping);
                    }
                }
                break;

            case TweenType.rotate:
                if (tweenData.tdRotate.isLocalSpace)
                {
                    if (tweenData.useTweenInitialValue)
                    {
                        transform.localRotation = Quaternion.Euler(tweenData.tdRotate.tweenStartValue);
                    }

                    transform.DOLocalRotate(tweenData.tdRotate.tweenEndValue, tweenData.tdRotate.tweenDuration, tweenData.tdRotate.rotateMode)
                                    .SetDelay(tweenData.tdRotate.tweenDelay).SetEase(tweenData.tdRotate.easeType).OnComplete(() => { PlayOnComplete(tweenCompeteEvents); });
                }
                else
                {
                    if (tweenData.useTweenInitialValue)
                    {
                        transform.rotation = Quaternion.Euler(tweenData.tdRotate.tweenStartValue);
                    }

                    transform.DORotate(tweenData.tdRotate.tweenEndValue, tweenData.tdRotate.tweenDuration, tweenData.tdRotate.rotateMode)
                                    .SetDelay(tweenData.tdRotate.tweenDelay).SetEase(tweenData.tdRotate.easeType).OnComplete(() => { PlayOnComplete(tweenCompeteEvents); });
                }
                break;

            case TweenType.scale:
                if (tweenData.useTweenInitialValue)
                {
                    transform.localScale = tweenData.tdScale.tweenStartValue;
                }

                transform.DOScale(tweenData.tdScale.tweenEndValue, tweenData.tdScale.tweenDuration)
                                    .SetDelay(tweenData.tdScale.tweenDelay).SetEase(tweenData.tdScale.easeType).OnComplete(() => { PlayOnComplete(tweenCompeteEvents); });
                break;

            case TweenType.punch:
                if(tweenData.tdPunch.type == 0)
                {
                    transform.DOPunchPosition(tweenData.tdPunch.puch, tweenData.tdPunch.tweenDuration, tweenData.tdPunch.vibrato, tweenData.tdPunch.elasticity, tweenData.tdPunch.isSnapping)
                                    .SetDelay(tweenData.tdPunch.tweenDelay).SetEase(tweenData.tdPunch.easeType).OnComplete(() => { PlayOnComplete(tweenCompeteEvents); });
                }
                else if (tweenData.tdPunch.type == 1)
                {
                    transform.DOPunchRotation(tweenData.tdPunch.puch, tweenData.tdPunch.tweenDuration, tweenData.tdPunch.vibrato, tweenData.tdPunch.elasticity)
                                    .SetDelay(tweenData.tdPunch.tweenDelay).SetEase(tweenData.tdPunch.easeType).OnComplete(() => { PlayOnComplete(tweenCompeteEvents); });
                }
                else if (tweenData.tdPunch.type == 2)
                {
                    transform.DOPunchRotation(tweenData.tdPunch.puch, tweenData.tdPunch.tweenDuration, tweenData.tdPunch.vibrato, tweenData.tdPunch.elasticity)
                                    .SetDelay(tweenData.tdPunch.tweenDelay).SetEase(tweenData.tdPunch.easeType).OnComplete(() => { PlayOnComplete(tweenCompeteEvents); });
                }
                break;

            case TweenType.shake:
                if (tweenData.tdShake.type == 0)
                {
                    transform.DOShakePosition(tweenData.tdShake.tweenDuration, tweenData.tdShake.strength, tweenData.tdShake.vibrato,
                                                tweenData.tdShake.randomness, tweenData.tdShake.isSnapping, tweenData.tdShake.fadeOut, tweenData.tdShake.shakeRandomnessMode)
                                    .SetDelay(tweenData.tdShake.tweenDelay).SetEase(tweenData.tdShake.easeType).OnComplete(() => { PlayOnComplete(tweenCompeteEvents); });
                }
                else if (tweenData.tdShake.type == 1)
                {
                    transform.DOShakeRotation(tweenData.tdShake.tweenDuration, tweenData.tdShake.strength, tweenData.tdShake.vibrato,
                                                tweenData.tdShake.randomness, tweenData.tdShake.fadeOut, tweenData.tdShake.shakeRandomnessMode)
                                    .SetDelay(tweenData.tdShake.tweenDelay).SetEase(tweenData.tdShake.easeType).OnComplete(() => { PlayOnComplete(tweenCompeteEvents); });
                }
                else if (tweenData.tdShake.type == 2)
                {
                    transform.DOShakeScale(tweenData.tdShake.tweenDuration, tweenData.tdShake.strength, tweenData.tdShake.vibrato,
                                                tweenData.tdShake.randomness, tweenData.tdShake.fadeOut, tweenData.tdShake.shakeRandomnessMode)
                                    .SetDelay(tweenData.tdShake.tweenDelay).SetEase(tweenData.tdShake.easeType).OnComplete(() => { PlayOnComplete(tweenCompeteEvents); });
                }
                break;

            case TweenType.path:
                if(tweenData.tdPath.isLocalSpace)
                {
                    transform.DOLocalPath(tweenData.tdPath.waypoints, tweenData.tdPath.tweenDuration, tweenData.tdPath.pathType,
                                            tweenData.tdPath.pathMode, tweenData.tdPath.resolution, tweenData.tdPath.gizmoColor)
                                    .SetDelay(tweenData.tdPath.tweenDelay).SetEase(tweenData.tdPath.easeType).OnComplete(() => { PlayOnComplete(tweenCompeteEvents); });
                }
                else
                {
                    transform.DOPath(tweenData.tdPath.waypoints, tweenData.tdPath.tweenDuration, tweenData.tdPath.pathType,
                                            tweenData.tdPath.pathMode, tweenData.tdPath.resolution, tweenData.tdPath.gizmoColor)
                                    .SetDelay(tweenData.tdPath.tweenDelay).SetEase(tweenData.tdPath.easeType).OnComplete(() => { PlayOnComplete(tweenCompeteEvents); });
                }
                break;

            case TweenType.spriterenderercolor:
                if (tweenData.useTweenInitialValue)
                {
                    GetComponent<SpriteRenderer>().color = tweenData.tdSpriteRendererColor.tweenStartValue;
                }

                GetComponent<SpriteRenderer>().DOColor(tweenData.tdSpriteRendererColor.tweenEndValue, tweenData.tdSpriteRendererColor.tweenDuration)
                                    .SetDelay(tweenData.tdSpriteRendererColor.tweenDelay).SetEase(tweenData.tdSpriteRendererColor.easeType).OnComplete(() => { PlayOnComplete(tweenCompeteEvents); });
                break;

            case TweenType.spriterendererfade:
                if (tweenData.useTweenInitialValue)
                {
                    GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g,
                                                                        GetComponent<SpriteRenderer>().color.b, tweenData.tdSpriteRendererFade.tweenStartValue);
                }
                GetComponent<SpriteRenderer>().DOFade(tweenData.tdSpriteRendererFade.tweenEndValue, tweenData.tdSpriteRendererFade.tweenDuration)
                                    .SetDelay(tweenData.tdSpriteRendererFade.tweenDelay).SetEase(tweenData.tdSpriteRendererFade.easeType).OnComplete(() => { PlayOnComplete(tweenCompeteEvents); });
                break;

            case TweenType.materialcolor:
                GetComponent<Renderer>().material.DOColor(tweenData.tdMaterialColor.tweenEndValue, tweenData.tdMaterialColor.tweenDuration)
                                    .SetDelay(tweenData.tdMaterialColor.tweenDelay).SetEase(tweenData.tdMaterialColor.easeType).OnComplete(() => { PlayOnComplete(tweenCompeteEvents); });
                break;

            case TweenType.materialfade:
                GetComponent<Renderer>().material.DOFade(tweenData.tdMaterialFade.tweenEndValue, tweenData.tdMaterialFade.tweenDuration)
                                    .SetDelay(tweenData.tdMaterialFade.tweenDelay).SetEase(tweenData.tdMaterialFade.easeType).OnComplete(() => { PlayOnComplete(tweenCompeteEvents); });
                break;
        }
    }

    public void PlayOnComplete(List<UnityEvent> tweenEvents)
    {
        foreach (var item in tweenEvents)
        {
            if(item != null)
            {
                item.Invoke();
            }
        }
    }
}
