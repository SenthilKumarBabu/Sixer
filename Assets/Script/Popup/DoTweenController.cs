using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DoTweenController
{

    public static void killAll()
    {
        if (clearSequence != null)
        {
            for (int i = 0; i < clearSequence.Count; i++)
            {
                clearSequence[i].Kill();
            }

            clearSequence.Clear();
        }
    }

    public static void startcoinAnimation(GameObject obj,Vector3 fromPos,Vector3 toPos,float startDelay=0.0f,float scaleDownDelay=0.0f,float scaledownDuration =0.0f,float endScaleVal =0.0f, Ease easeType = Ease.OutFlash,float duration =1.0f,byte type=0,bool objhide=false)
    {
       
            obj.gameObject.SetActive(true);
            obj.transform.localScale = new Vector3(0, 0, 0);

            DOscale(obj, 0.0f, 1.0f, 1.0f, startdelay: startDelay);
            DoSmoothmovePos(obj, fromPos, toPos, duration, ease: true, easeType: easeType, startDelay: startDelay, type:type);
            DoRotate(obj, new Vector3(0, 0, 0), Vector3.forward * 220f, 1.0f, initDuration: 0.0f, startDelay: startDelay);
            DOscale(obj, scaledownDuration, 1.0f, endScaleVal, startdelay: scaleDownDelay,objhide: objhide);
     
    }

    public static void startBoostAnimation(RectTransform obj, Vector3 fromPos, Vector3 toPos, float startDelay = 0.0f, float scaleDownDelay = 0.0f, float scaledownDuration = 0.0f, float intScaleVal = 1.0f, float endScaleVal = 1.0f,float scaleDownsmoothFinish = 0.0f, Ease easeType = Ease.OutFlash, byte type = 2)
    {
            DoSmoothmovePos(obj.gameObject, fromPos, toPos, 1.0f, ease: true, easeType: easeType, startDelay: startDelay, type: type);
            DOscale(obj.gameObject, scaledownDuration, intScaleVal, endScaleVal, startdelay: scaleDownDelay);
    }



    public static List<Sequence> clearSequence =new List<Sequence>();

    public static IEnumerator barFillerValue(int v1, int v2, float duration, float delay, Text fillText = null)
    {
        if (v1 < 0)
        {
            v1 = 0;
        }
        if (v2 < 0)
        {
            v2 = 0;
        }
        yield return new WaitForSeconds(delay);

        /* for (float i = v1; i <= v2; i++)
         {
             //Debug.Log("barFillerValue==" + i);
            // val = i;
             Main.text = i.ToString();
             yield return CoroutineUtil.WaitForRealSeconds(duration / (v2 - v1));
         }*/
        var val = v2 - v1;
        int incVal = (int)(val * 0.1f);
        if (val >= 10)
        {
            while (duration > 0)
            {
                //int incVal = (int)((v2 - v1) * 0.01f);
                v1 += incVal;
                //fillText.text = CONTROLLER.NumberSystem(v1);
                fillText.text = v1.ToString();
                yield return new WaitForSeconds(0.1f);  //0.1f
                duration -= 0.1f;
                if (duration < 0 || v1 > v2)
                {
                    v1 = v2;
                    //fillText.text = CONTROLLER.NumberSystem(v1);
                    fillText.text = v1.ToString();
                    yield break;
                }

            }
        }
        else
        {
            for (float i = v1; i <= v2; i++)
            {
                //Debug.Log("barFillerValue==" + i);
                // val = i;
                fillText.text = i.ToString();
                yield return new WaitForSeconds(duration / (v2 - v1));
            }
        }

    }
    public static void DofadeOutTofadeIn<T>(T obj, float duration = 0.1f, float initialFadevalue = 0.0f, float intermediatevalue = 0.0f, float finalFadeValue = 1.0f,float startDelay = 0.0f, bool ease = false, Ease easeType = Ease.OutFlash)
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }

        if (typeof(T) == typeof(Image))
        {
            if (ease == true)
            {
                var img = obj as Image;


                Sequence mysequence = DOTween.Sequence();
                clearSequence.Add(mysequence);

                mysequence.AppendInterval(startDelay).SetEase(easeType)
                .Append(img.DOFade(initialFadevalue, 0.0f))
                .Append(img.DOFade(intermediatevalue, duration / 2))
                .Append(img.DOFade(finalFadeValue, duration / 2))
                .AppendCallback(() =>
                {
                    mysequence.Kill();
                });
            }
            else
            {
                var img = obj as Image;


                Sequence mysequence = DOTween.Sequence();
                clearSequence.Add(mysequence);

                mysequence.AppendInterval(startDelay)
                .Append(img.DOFade(initialFadevalue, 0.0f))
                .Append(img.DOFade(intermediatevalue, duration / 2))
                .Append(img.DOFade(finalFadeValue, duration / 2))
                 .AppendCallback(() =>
                  {
                      mysequence.Kill();
                  });
            }

        }
        else if (typeof(T) == typeof(RawImage))
        {
            if (ease == true)
            {
                var img = obj as RawImage;


                Sequence mysequence = DOTween.Sequence();
                clearSequence.Add(mysequence);

                mysequence.AppendInterval(startDelay).SetEase(easeType)
                .Append(img.DOFade(initialFadevalue, 0.0f))
                .Append(img.DOFade(intermediatevalue, duration / 2))
                .Append(img.DOFade(finalFadeValue, duration / 2))
                .AppendCallback(() =>
                   {
                       mysequence.Kill();
                   });
            }
            else
            {
                var img = obj as RawImage;


                Sequence mysequence = DOTween.Sequence();
                clearSequence.Add(mysequence);

                mysequence.AppendInterval(startDelay)
                .Append(img.DOFade(initialFadevalue, 0.0f))
                .Append(img.DOFade(intermediatevalue, duration / 2))
                .Append(img.DOFade(finalFadeValue, duration / 2))
                  .AppendCallback(() =>
                   {
                       mysequence.Kill();
                   });
            }

        }

    }

    public static void DOShakePosition(GameObject obj, float duration = 0.1f,float startDelay = 0.0f,float strength=1, int vibrate = 10)
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }

        Sequence mysequence = DOTween.Sequence();
        clearSequence.Add(mysequence);

        mysequence.AppendInterval(startDelay)
        .Append(obj.transform.DOShakePosition(duration,strength: strength,vibrate))
        .AppendCallback(() =>
         {
             mysequence.Kill();
         });




    }
   
    public static void DofadeText(Text obj, float duration = 0.1f, float initialFadevalue = 0.0f, float middleFadevalue = 0.0f, float finalFadeValue = 1.0f, bool objhide = false, float startDelay = 0.0f, int numbeOfFade = 0,bool setUpdate = false,bool ease =false,Ease easeType =Ease.InOutElastic)
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }
        Sequence mysequence = DOTween.Sequence();
        clearSequence.Add(mysequence);
        if (numbeOfFade == 0)
        {
            if (ease == false)
            {
                
                mysequence.SetUpdate(setUpdate)
                .AppendInterval(startDelay)
                .Append(obj.DOFade(initialFadevalue, 0.0f))
                .Append(obj.DOFade(middleFadevalue, duration))
                .Append(obj.DOFade(finalFadeValue, duration))



                .AppendCallback(() =>
                {
                    if (objhide == true)
                    {
                        obj.gameObject.SetActive(false);
                    }
                    if(setUpdate == false)
                    mysequence.Kill();
                });
            }
            else
            {

                mysequence.SetUpdate(setUpdate).SetEase(easeType)
                    .AppendInterval(startDelay)
                    .Append(obj.DOFade(initialFadevalue, 0.0f))
                    .Append(obj.DOFade(middleFadevalue, duration))
                    .Append(obj.DOFade(finalFadeValue, duration))



                    .AppendCallback(() =>
                    {
                        if (objhide == true)
                        {
                            obj.gameObject.SetActive(false);
                        }
                        if (setUpdate == false)
                            mysequence.Kill();
                    });
                
            }
        }
        else
         if (numbeOfFade == 1)
        {
            if (ease == false)
            {
                mysequence.SetUpdate(setUpdate)
     .AppendInterval(startDelay)
     .Append(obj.DOFade(finalFadeValue, duration / 2))
     //.Append(obj.transform.GetComponent<Image>().DOFade(initialFadevalue, duration/3))
     //.Append(obj.transform.GetComponent<Image>().DOFade(finalFadeValue, duration/2))
     .Append(obj.DOFade(initialFadevalue, duration))



     .AppendCallback(() =>
     {

         if (objhide == true)
         {
             obj.gameObject.SetActive(false);
         }
         if (setUpdate == false)
             mysequence.Kill();
     });
            }
            else
            {
                mysequence.SetUpdate(setUpdate).SetEase(easeType)
   .AppendInterval(startDelay)
   .Append(obj.DOFade(finalFadeValue, duration / 2))
   //.Append(obj.transform.GetComponent<Image>().DOFade(initialFadevalue, duration/3))
   //.Append(obj.transform.GetComponent<Image>().DOFade(finalFadeValue, duration/2))
   .Append(obj.DOFade(initialFadevalue, duration))



   .AppendCallback(() =>
   {

       if (objhide == true)
       {
           obj.gameObject.SetActive(false);
       }
       if (setUpdate == false)
           mysequence.Kill();
   });
            }
        }


    }
    public static void DofadeRawImage(RawImage obj, float duration = 0.1f, float initialFadevalue = 0.0f, float finalFadeValue = 1.0f, bool objhide = false, float startDelay = 0.0f, int numbeOfFade = 0)
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }
        Sequence mysequence = DOTween.Sequence();
        clearSequence.Add(mysequence);
        if (numbeOfFade == 0)
        {
            mysequence
            .AppendInterval(startDelay)
            .Append(obj.DOFade(initialFadevalue, 0.0f))
            .Append(obj.DOFade(finalFadeValue, duration))



            .AppendCallback(() =>
            {
                if (objhide == true)
                {
                    obj.gameObject.SetActive(false);
                }
                mysequence.Kill();
            });
        }
        else
         if (numbeOfFade == 1)
        {

            mysequence
     .AppendInterval(startDelay)
     .Append(obj.DOFade(finalFadeValue, duration / 2))
     //.Append(obj.transform.GetComponent<Image>().DOFade(initialFadevalue, duration/3))
     //.Append(obj.transform.GetComponent<Image>().DOFade(finalFadeValue, duration/2))
     .Append(obj.DOFade(initialFadevalue, duration))



     .AppendCallback(() =>
     {

         if (objhide == true)
         {
             obj.gameObject.SetActive(false);
         }
         mysequence.Kill();
     });
        }


    }
    
    public static void Dofade<T>(T obj, float duration = 0.1f, float initialFadevalue = 0.0f, float finalFadeValue = 1.0f,bool objhide = false,float startDelay= 0.0f,int numbeOfFade = 0,bool setUpdate = false,bool ease=false,Ease easeType =Ease.OutFlash, bool objDestroy = false,int setloops =0)
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }
        Sequence mysequence = DOTween.Sequence();
        clearSequence.Add(mysequence);
        if (typeof(T) == typeof(Image))
        {
            var img = obj as Image;
            if (numbeOfFade == 0)
            {
                if (ease == true)
                {
                    mysequence.SetUpdate(setUpdate).SetEase(easeType).SetLoops(setloops)
                    .AppendInterval(startDelay)
                    .Append(img.DOFade(initialFadevalue, 0.0f))
                    .Append(img.DOFade(finalFadeValue, duration))



                    .AppendCallback(() =>
                    {
                        if (objhide == true)
                        {
                            img.gameObject.SetActive(false);
                        }
                        if (setUpdate == false)
                            mysequence.Kill();
                    });
                }
                else
                {
                    mysequence.SetUpdate(setUpdate).SetLoops(setloops)
                   .AppendInterval(startDelay)
                   .Append(img.DOFade(initialFadevalue, 0.0f))
                   .Append(img.DOFade(finalFadeValue, duration))



                   .AppendCallback(() =>
                   {
                       if (objhide == true)
                       {
                           img.gameObject.SetActive(false);
                       }
                       if (setUpdate == false)
                           mysequence.Kill();
                   });
                }
            }
            else
             if (numbeOfFade == 1)
            {
                if (ease == true)
                {
                    mysequence.SetUpdate(setUpdate).SetEase(easeType).SetLoops(setloops)
         .AppendInterval(startDelay)
         .Append(img.DOFade(finalFadeValue, duration / 2))
         .Append(img.DOFade(initialFadevalue, duration / 2))



         .AppendCallback(() =>
         {

             if (objhide == true)
             {
                 img.gameObject.SetActive(false);
             }
             if (setUpdate == false)
                 mysequence.Kill();
         });
                }
                else
                {
                    mysequence.SetUpdate(setUpdate).SetLoops(setloops)
      .AppendInterval(startDelay)
      .Append(img.DOFade(finalFadeValue, duration / 2))
      .Append(img.DOFade(initialFadevalue, duration / 2))



      .AppendCallback(() =>
      {

          if (objhide == true)
          {
              img.gameObject.SetActive(false);
          }
          if (setUpdate == false)
              mysequence.Kill();
      });
                }
            }

        }
        else if (typeof(T) == typeof(RawImage))
        {
            var img = obj as RawImage;
            if (numbeOfFade == 0)
            {
                if (ease == true)
                {
                    mysequence.SetUpdate(setUpdate).SetEase(easeType).SetLoops(setloops)
                .AppendInterval(startDelay)
                .Append(img.DOFade(initialFadevalue, 0.0f))
                .Append(img.DOFade(finalFadeValue, duration))



                .AppendCallback(() =>
                {
                    if (objhide == true)
                    {
                        img.gameObject.SetActive(false);
                    }
                    if (setUpdate == false)
                        mysequence.Kill();
                });
                }
                else
                {
                    mysequence.SetUpdate(setUpdate).SetLoops(setloops)
                .AppendInterval(startDelay)
                .Append(img.DOFade(initialFadevalue, 0.0f))
                .Append(img.DOFade(finalFadeValue, duration))



                .AppendCallback(() =>
                {
                    if (objhide == true)
                    {
                        img.gameObject.SetActive(false);
                    }
                    if (setUpdate == false)
                        mysequence.Kill();
                });
                }
            }
            else
             if (numbeOfFade == 1)
            {
                if (ease == true)
                {
                    mysequence.SetUpdate(setUpdate).SetEase(easeType).SetLoops(setloops)
         .AppendInterval(startDelay)
         .Append(img.DOFade(finalFadeValue, duration / 2))
         .Append(img.DOFade(initialFadevalue, duration / 2))



         .AppendCallback(() =>
         {

             if (objhide == true)
             {
                 img.gameObject.SetActive(false);
             }
             if (setUpdate == false)
                 mysequence.Kill();
         });
                }
                else
                {
                    mysequence.SetUpdate(setUpdate).SetLoops(setloops)
.AppendInterval(startDelay)
.Append(img.DOFade(finalFadeValue, duration / 2))
.Append(img.DOFade(initialFadevalue, duration / 2))



.AppendCallback(() =>
{

    if (objhide == true)
    {
        img.gameObject.SetActive(false);
    }
    if (setUpdate == false)
        mysequence.Kill();
});
                }
            }

        }
        else if (typeof(T) == typeof(Outline))
        {
            var img = obj as Outline;
            if (ease == true)
            {
                mysequence.SetUpdate(setUpdate).SetEase(easeType).SetLoops(setloops)
                .AppendInterval(startDelay)
                .Append(img.DOFade(initialFadevalue, 0.0f))
                .Append(img.DOFade(finalFadeValue, duration))



                .AppendCallback(() =>
                {
                    if (objhide == true)
                    {
                        img.gameObject.SetActive(false);
                    }
                    if (setUpdate == false)
                        mysequence.Kill();
                });
            }
            else
            {
                mysequence.SetUpdate(setUpdate).SetLoops(setloops)
               .AppendInterval(startDelay)
               .Append(img.DOFade(initialFadevalue, 0.0f))
               .Append(img.DOFade(finalFadeValue, duration))



               .AppendCallback(() =>
               {
                   if (objhide == true)
                   {
                       img.gameObject.SetActive(false);
                   }
                   if (setUpdate == false)
                       mysequence.Kill();
               });
            }
        }
        }
    public static void stopParticle(GameObject obj,float startDelay = 0.0f,bool particlehide = false,ParticleSystem particle = default)
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }

        Sequence mysequence = DOTween.Sequence();
        clearSequence.Add(mysequence);

        mysequence.AppendInterval(startDelay)
          .AppendCallback(() =>
          {

              if (particlehide == true)
              {
                  obj.SetActive(false);
                  particle.Stop();
              }
              mysequence.Kill();
          });

    }
    public static void DoplayParticle(GameObject obj, float startDelay = 0.0f, bool particleEnable = false, ParticleSystem particle = default)
    {
        //Debug.Log("DoplayParticle=");
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }

        Sequence mysequence = DOTween.Sequence();
        clearSequence.Add(mysequence);

        mysequence.AppendInterval(startDelay)
          .AppendCallback(() =>
          {

              if (particleEnable == true)
              {
                  obj.SetActive(true);
                  particle.Play();
              }
              mysequence.Kill();
          });

    }
    
    /* public static void DOscale(GameObject obj, float duration = 0.1f, float initialScalevalue = 0.0f, float finalScaleValue = 1.0f,float startdelay =0.0f, bool objhide = false,bool customForPaperFold =false,bool objenable = false, bool objClothenable = false)
     {
         if (DOTween.Sequence() != null)
         {
             DOTween.Sequence().Kill();
         }

         DOTween.Sequence()
          .AppendInterval(startdelay)
         .Append(obj.transform.DOScale(initialScalevalue, 0.0f))
         .Append(obj.transform.DOScale(finalScaleValue, duration))


             .AppendCallback(() =>
              {

                  if (objhide == true)
                  {
                      obj.SetActive(false);
                  }

                  if(objenable == true)
                  {
                      obj.SetActive(true);
                  }


              });


 }*/
   
    public static void DOscale(GameObject obj, float duration = 0.1f, float initialScalevalue = 0.0f, float finalScaleValue = 1.0f, float startdelay = 0.0f, bool objhide = false, bool objenable = false,bool ease = false,Ease easeType = Ease.InFlash,bool setUpdate =false,int setloops =0)
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }
        Sequence mysequence = DOTween.Sequence();
        clearSequence.Add(mysequence);
        if (ease == false)
        {
            mysequence.SetUpdate(setUpdate).SetLoops(setloops)
            .AppendInterval(startdelay)
            .Append(obj.transform.DOScale(initialScalevalue, 0.0f))
            .Append(obj.transform.DOScale(finalScaleValue, duration))


                .AppendCallback(() =>
                {

                    if (objhide == true)
                    {
                        obj.SetActive(false);
                    }

                    if (objenable == true)
                    {
                        obj.SetActive(true);
                    }
                    if (setUpdate == false)
                        mysequence.Kill();
                });
        }
        else
        {
            mysequence.SetUpdate(setUpdate).SetLoops(setloops)
           .AppendInterval(startdelay)
           .Append(obj.transform.DOScale(initialScalevalue, 0.0f))
           .Append(obj.transform.DOScale(finalScaleValue, duration).SetEase(easeType))


       .AppendCallback(() =>
       {

           if (objhide == true)
           {
               obj.SetActive(false);
           }

           if (objenable == true)
           {
               obj.SetActive(true);
           }
           if (setUpdate == false)
               mysequence.Kill();

       });
        }


    }
    public static void DoFill(Image obj, float duration = 0.1f, float startdelay = 0.0f, float endvalue=0.0f, float startValue = 0.0f,bool ease = false,Ease easeType = Ease.InOutSine,bool objEnableAfterAnim =false,bool setUpdate=false)
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }
        Sequence mysequence = DOTween.Sequence();
        clearSequence.Add(mysequence);
        if (ease == false)
        {
            mysequence.SetUpdate(setUpdate)
             .AppendInterval(startdelay)
             .Append(obj.DOFillAmount(startValue, 0.0f))
            .Append(obj.DOFillAmount(endvalue, duration))

                .AppendCallback(() =>
                {
                    if(objEnableAfterAnim == true)
                    {
                        obj.gameObject.SetActive(true);
                    }
                    if (setUpdate == false)
                        mysequence.Kill();
                });
        }
        else
        {
            mysequence.SetUpdate(setUpdate)
               .AppendInterval(startdelay)
               .Append(obj.DOFillAmount(startValue, 0.0f))
               .Append(obj.DOFillAmount(endvalue, duration).SetEase(easeType))

        .AppendCallback(() =>
        {
            if (objEnableAfterAnim == true)
            {
                obj.gameObject.SetActive(true);
            }
            if (setUpdate == false)
                mysequence.Kill();
        });
    }


    }
    public static void DoFillEase(Image obj, float duration = 0.1f, float startdelay = 0.0f, float startValue = 0.0f, float endvalue = 0.0f)
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }
        Sequence mysequence = DOTween.Sequence();
        clearSequence.Add(mysequence);
        mysequence
         .AppendInterval(startdelay)
         .Append(obj.DOFillAmount(startValue, 0.0f))
        .Append(obj.DOFillAmount(endvalue, duration).SetEase(Ease.OutBack))

            .AppendCallback(() =>
            {
                mysequence.Kill();
            });


    }
    public static void DOsizeDelta(RectTransform obj, float duration = 0.1f,Vector2 size = default , float startdelay = 0.0f, bool objhide = false, bool customForPaperFold = false, bool objenable = false, bool destroyclone = false,bool ease=false,Ease easeType = Ease.InOutElastic)
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }
        Sequence mysequence = DOTween.Sequence();
        clearSequence.Add(mysequence);
        if (ease == false)
        {
            mysequence
             .AppendInterval(startdelay)
            .Append(obj.DOSizeDelta(size, duration))
       
            .AppendCallback(() =>
            {

                if (objhide == true)
                {
                    obj.gameObject.SetActive(false);
                }

                if (objenable == true)
                {
                    obj.gameObject.SetActive(true);
                }

                if (destroyclone == true)
                {
                    
                }
                mysequence.Kill();
            });
        }
        else
        {
            mysequence
            .AppendInterval(startdelay)
           .Append(obj.DOSizeDelta(size, duration).SetEase(easeType))

           .AppendCallback(() =>
           {

               if (objhide == true)
               {
                   obj.gameObject.SetActive(false);
               }

               if (objenable == true)
               {
                   obj.gameObject.SetActive(true);
               }


               mysequence.Kill();
           });
        }



    }
    public static void DOscaleX(GameObject obj, float duration = 0.1f, float initialScalevalue = 0.0f, float finalScaleValue = 1.0f,float startDelay= 0.0f,bool ease =false, Ease easeType = Ease.InOutBack,bool setUpdate =false)
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }
        Sequence mysequence = DOTween.Sequence();
        clearSequence.Add(mysequence);
        if (ease == false)
        {
            mysequence.SetUpdate(setUpdate)
            .AppendInterval(startDelay)
            .Append(obj.transform.DOScaleX(initialScalevalue, 0))
            .Append(obj.transform.DOScaleX(finalScaleValue, duration))
               .AppendCallback(() =>
                {
                    if (setUpdate == false)
                        mysequence.Kill();
                });
        }
        else
        {
            mysequence.SetUpdate(setUpdate)
           .SetEase(easeType)
           .AppendInterval(startDelay)
           .Append(obj.transform.DOScaleX(initialScalevalue, 0))
           .Append(obj.transform.DOScaleX(finalScaleValue, duration))
            .AppendCallback(() =>
             {
                 if (setUpdate == false)
                     mysequence.Kill();
             });
        }

    }
    public static void DOscaleXCustom(GameObject obj, float duration = 0.1f, float initialScalevalue = 0.0f, float midScalevalue = 0.0f, float finalScaleValue = 1.0f, float startDelay = 0.0f)
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }
        Sequence mysequence = DOTween.Sequence();
        clearSequence.Add(mysequence);
        mysequence
        .AppendInterval(startDelay)
        .Append(obj.transform.DOScaleX(midScalevalue, duration / 2))
        .Append(obj.transform.DOScaleX(finalScaleValue, duration))
         .AppendCallback(() =>
          {

              mysequence.Kill();
          });


    }
    public static void DOscaleYCustom(GameObject obj, float duration = 0.1f, float initialScalevalue = 0.0f, float midScalevalue = 0.0f, float finalScaleValue = 1.0f, float startDelay = 0.0f, bool objhide = false)
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }
        Sequence mysequence = DOTween.Sequence();
        clearSequence.Add(mysequence);
        mysequence
        .AppendInterval(startDelay)
        .Append(obj.transform.DOScaleY(midScalevalue, duration / 2))
        .Append(obj.transform.DOScaleY(finalScaleValue, duration))
           .AppendCallback(() =>
            {

                if (objhide == true)
                {
                    obj.SetActive(false);
                }
                mysequence.Kill();
            });

    }
    public static void DOscaleZ(GameObject obj, float duration = 0.1f, float initialScalevalue = 0.0f, float finalScaleValue = 1.0f, float startDelay = 0.0f)
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }

        Sequence mysequence = DOTween.Sequence();
        clearSequence.Add(mysequence);
        mysequence.AppendInterval(startDelay)
        .Append(obj.transform.DOScaleZ(initialScalevalue, 0))
        .Append(obj.transform.DOScaleZ(finalScaleValue, duration))
          .AppendCallback(() =>
           {

               mysequence.Kill();
           });

    }
    public static void DocolorText(Text obj, float duration = 0.1f, Color startValue = default(Color), Color endvalue = default(Color), float startDelay = 0.0f)
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }
        Sequence mysequence = DOTween.Sequence();
        clearSequence.Add(mysequence);
        mysequence
          .AppendInterval(startDelay)
        .Append(obj.DOColor(startValue, 0.0f))
        .Append(obj.DOColor(endvalue, duration))
         .AppendCallback(() =>
          {

              mysequence.Kill();
          });


    }
    public static void DocolorImg(Image obj, float duration = 0.1f, Color startValue = default(Color), Color endvalue = default(Color), float startDelay = 0.0f,bool ease =false,Ease easeType =Ease.InOutFlash)
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }
        Sequence mysequence = DOTween.Sequence();
        clearSequence.Add(mysequence);
        if (ease == false)
        {
            mysequence
            .AppendInterval(startDelay)
            .Append(obj.DOColor(startValue, 0.0f))
            .Append(obj.DOColor(endvalue, duration))
            .AppendCallback(() =>
             {

                 mysequence.Kill();
             });
        }
        else
        {
            mysequence.SetEase(easeType)
           .AppendInterval(startDelay)
           .Append(obj.DOColor(startValue, 0.0f))
           .Append(obj.DOColor(endvalue, duration))
             .AppendCallback(() =>
              {

                  mysequence.Kill();
              });
        }


    }
    public static void DOscaleY(GameObject obj, float duration = 0.1f, float initialScalevalue = 0.0f, float finalScaleValue = 1.0f,bool objhide = false, float startDelay = 0.0f, bool ease = false, Ease easeType = Ease.InOutBack,bool setUpdate = false)
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }
        Sequence mysequence = DOTween.Sequence();
        clearSequence.Add(mysequence);
        if (ease == false)
        {
            mysequence.SetUpdate(setUpdate)
            .AppendInterval(startDelay)
            .Append(obj.transform.DOScaleY(initialScalevalue, 0.0f))
            .Append(obj.transform.DOScaleY(finalScaleValue, duration))

             .AppendCallback(() =>
              {

                  if (objhide == true)
                  {
                      obj.SetActive(false);
                  }
                  if (setUpdate == false)
                      mysequence.Kill();
              });
        }
        else
        {
            mysequence.SetUpdate(setUpdate)
         .AppendInterval(startDelay)
         .Append(obj.transform.DOScaleY(initialScalevalue, 0.0f))
         .Append(obj.transform.DOScaleY(finalScaleValue, duration).SetEase(easeType))

          .AppendCallback(() =>
          {

              if (objhide == true)
              {
                  obj.SetActive(false);
                  
              }
              if (setUpdate == false)
                  mysequence.Kill();
          });
        }

    }
    public static void DOZoominAndBlink<T>(T obj,float Zoominduration = 0.1f, float blinkSpeed = 0.04f,float initialScalevalue =0.0f,float finalScaleValue= 1.0f,float startdelay = 0.0f,Ease easeType = Ease.InOutSine)
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }
        Sequence mysequence = DOTween.Sequence();
        clearSequence.Add(mysequence);
        if (typeof(T) == typeof(Image))
        {
            var img = obj as Image;
            mysequence.SetEase(easeType)
        .AppendInterval(startdelay)
        .Append(img.DOFade(0.88f, 0.0f))
        .Append(img.transform.DOScale(finalScaleValue + 0.005f, Zoominduration))
        .Append(img.DOFade(1.0f, 0.0f))
        .AppendInterval(0.07f)
        .Append(img.transform.DOScale(finalScaleValue, blinkSpeed))
        .Append(img.DOFade(0.88f, 0.0f))
        .AppendInterval(0.07f)
        .Append(img.transform.DOScale(finalScaleValue + 0.005f, blinkSpeed))
        .Append(img.DOFade(1.0f, 0.0f))
        .AppendInterval(0.07f)
        .Append(img.transform.DOScale(finalScaleValue, blinkSpeed))
        .Append(img.DOFade(0.88f, 0.0f))
        .AppendInterval(0.07f)
        .Append(img.DOFade(1.0f, 0.0f))
              .AppendCallback(() =>
               {
                   mysequence.Kill();
               });
        }
        else if (typeof(T) == typeof(RawImage))
        {
            var img = obj as RawImage;
            mysequence.SetEase(easeType)
        .AppendInterval(startdelay)
        .Append(img.DOFade(0.88f, 0.0f))
        .Append(img.transform.DOScale(finalScaleValue + 0.005f, Zoominduration))
        .Append(img.DOFade(1.0f, 0.0f))
        .AppendInterval(0.07f)
        .Append(img.transform.DOScale(finalScaleValue, blinkSpeed))
        .Append(img.DOFade(0.88f, 0.0f))
        .AppendInterval(0.07f)
        .Append(img.transform.DOScale(finalScaleValue + 0.005f, blinkSpeed))
        .Append(img.DOFade(1.0f, 0.0f))
        .AppendInterval(0.07f)
        .Append(img.transform.DOScale(finalScaleValue, blinkSpeed))
        .Append(img.DOFade(0.88f, 0.0f))
        .AppendInterval(0.07f)
        .Append(img.DOFade(1.0f, 0.0f))
            .AppendCallback(() =>
             {
                 mysequence.Kill();
             });
        }


            //.Append(obj.DOFade(1.0f, blinkSpeed));
        }
    public static void DOZoominAndBlinkRaw(RawImage obj, float Zoominduration = 0.1f, float blinkSpeed = 0.04f, float initialScalevalue = 0.0f, float finalScaleValue = 1.0f, float startdelay = 0.0f)
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }
        Sequence mysequence = DOTween.Sequence();
        clearSequence.Add(mysequence);
        mysequence
        .AppendInterval(startdelay)
        .Append(obj.DOFade(0.8f, 0.0f))
        .Append(obj.transform.DOScale(finalScaleValue + 0.005f, Zoominduration))
        .Append(obj.DOFade(1.0f, 0.0f))
        .AppendInterval(0.07f)
        .Append(obj.transform.DOScale(finalScaleValue, blinkSpeed))
        .Append(obj.DOFade(0.8f, 0.0f))
        .AppendInterval(0.07f)
        .Append(obj.transform.DOScale(finalScaleValue + 0.005f, blinkSpeed))
        .Append(obj.DOFade(1.0f, 0.0f))
        .AppendInterval(0.07f)
        .Append(obj.transform.DOScale(finalScaleValue, blinkSpeed))
        .Append(obj.DOFade(0.8f, 0.0f))
        .AppendInterval(0.07f)
        .Append(obj.DOFade(1.0f, 0.0f))
         .AppendCallback(() =>
          {
              mysequence.Kill();
          });
        //.Append(obj.DOFade(1.0f, blinkSpeed));
    }
    public static void DOBlink(GameObject obj, float blinkSpeed = 0.04f, float initialScalevalue = 0.0f, float finalScaleValue = 1.0f, float startdelay = 0.0f)
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }

        Sequence mysequence = DOTween.Sequence();
        clearSequence.Add(mysequence);
        mysequence.AppendInterval(startdelay)
        .Append(obj.transform.DOScale(finalScaleValue, blinkSpeed))
        //.Append(obj.GetComponent<Image>().DOFade(0.5f, 0.0f))
        .Append(obj.transform.DOScale(finalScaleValue + 0.05f, blinkSpeed))
        //.Append(obj.transform.GetComponent<Image>().DOFade(1.0f, 0.0f))
        .Append(obj.transform.DOScale(finalScaleValue, blinkSpeed))
        .AppendCallback(() =>
         {
             mysequence.Kill();
         });
        //.Append(obj.transform.GetComponent<Image>().DOFade(0.5f, 0.0f))
        //.Append(obj.transform.GetComponent<Image>().DOFade(1.0f, blinkSpeed));
        // Debug.Log("Blink===1" + obj.name);
    }

    /* public static void clothEnable (GameObject obj, Vector3 startLocalPos = default(Vector3), Vector3 endLocalPos = default(Vector3), float durantion = 0.0f, float startDelay = 0.0f, bool objHide = false, bool objClothHide = false)
     {
         if (DOTween.Sequence() != null)
         {
             DOTween.Sequence().Kill();
         }
         DOTween.Sequence()
         .Append(obj.GetComponent<RectTransform>().DOLocalRotate(obj.transform.localEulerAngles, startDelay))


          .AppendCallback(() =>
          {

              if (objHide == true)
              {
                  // obj.GetComponent<Cloth>().enabled = false;
                  obj.SetActive(false);
              }
              else if (objClothHide == true)
              {
                  obj.GetComponent<Cloth>().enabled = false;
              }
          });
     }*/
    public static void DoSmoothmoveY(RectTransform obj, float startLocalPos = 0.0f, float endLocalPos = 0.0f, float durantion = 0.0f, float startDelay = 0.0f, bool objHide = false, bool objDestroy = false, bool ease = false, Ease easeType = Ease.OutSine, bool setUpdate = false)
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }
        Sequence mysequence = DOTween.Sequence();
        clearSequence.Add(mysequence);
        if (ease == false)
        {
            mysequence.SetUpdate(setUpdate)
            .AppendInterval(startDelay)
            .Append(obj.DOAnchorPosY(startLocalPos, 0.0f))
            .Append(obj.DOAnchorPosY(endLocalPos, durantion))

             .AppendCallback(() =>
             {

                 if (objHide == true)
                 {
                     // obj.GetComponent<Cloth>().enabled = false;
                     obj.gameObject.SetActive(false);
                 }
                 else if (objDestroy == true)
                 {

                 }
                 if (setUpdate == false)
                     mysequence.Kill();
             });
        }
        else
        {
            mysequence.SetUpdate(setUpdate)
         .AppendInterval(startDelay)
         .Append(obj.DOAnchorPosY(startLocalPos, 0.0f))
         .Append(obj.DOAnchorPosY(endLocalPos, durantion).SetEase(easeType))

         .AppendCallback(() =>
         {

             if (objHide == true)
             {
                 // obj.GetComponent<Cloth>().enabled = false;
                 obj.gameObject.SetActive(false);
             }
             else if (objDestroy == true)
             {

             }
             if (setUpdate == false)
                 mysequence.Kill();
         });
        }
    }
    public static void DoSmoothmoveX(RectTransform obj, float startLocalPos = 0.0f, float endLocalPos = 0.0f, float durantion = 0.0f, float startDelay = 0.0f, bool objHide = false, bool objDestroy = false, bool ease = false, Ease easeType = Ease.OutSine,bool setUpdate = false)
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }
        Sequence mysequence = DOTween.Sequence();
        clearSequence.Add(mysequence);
        if (ease == false)
        {
            mysequence.SetUpdate(setUpdate)
            .AppendInterval(startDelay)
            .Append(obj.DOAnchorPosX(startLocalPos, 0.0f))
            .Append(obj.DOAnchorPosX(endLocalPos, durantion))

             .AppendCallback(() =>
             {

                 if (objHide == true)
                 {
                     // obj.GetComponent<Cloth>().enabled = false;
                     obj.gameObject.SetActive(false);
                 }
                 else if (objDestroy == true)
                 {

                 }
                 if (setUpdate == false)
                     mysequence.Kill();
             });
        }
        else
        {
            mysequence.SetUpdate(setUpdate)
         .AppendInterval(startDelay)
         .Append(obj.DOAnchorPosX(startLocalPos, 0.0f))
         .Append(obj.DOAnchorPosX(endLocalPos, durantion).SetEase(easeType))

         .AppendCallback(() =>
         {

             if (objHide == true)
             {
                 // obj.GetComponent<Cloth>().enabled = false;
                 obj.gameObject.SetActive(false);
             }
             else if (objDestroy == true)
             {

             }
             if (setUpdate == false)
                 mysequence.Kill();
         });
        }
    }
    public static void DoSmoothmove(RectTransform obj, Vector3 startLocalPos = default(Vector3), Vector3 endLocalPos = default(Vector3), float durantion = 0.0f,float startDelay = 0.0f,bool objHide = false,bool objDestroy =false,bool ease = false,Ease easeType =Ease.OutSine,bool setUpdate = false,int setloops =0)
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }
       Sequence  mysequence = DOTween.Sequence();
        clearSequence.Add(mysequence);
        if (ease == false)
        {
            mysequence.SetUpdate(setUpdate).SetLoops(setloops)
            .AppendInterval(startDelay)
            .Append(obj.DOAnchorPos(startLocalPos, 0.0f))
            .Append(obj.DOAnchorPos(endLocalPos, durantion))

             .AppendCallback(() =>
              {

                  if (objHide == true)
                  {
                  // obj.GetComponent<Cloth>().enabled = false;
                  obj.gameObject.SetActive(false);
                  }
                  else if (objDestroy == true)
                  {

                  }
                  if (setUpdate == false)
                      mysequence.Kill();
              });
        }
        else
        {
            mysequence.SetUpdate(setUpdate).SetLoops(setloops)
         .AppendInterval(startDelay)
         .Append(obj.DOAnchorPos(startLocalPos, 0.0f))
         .Append(obj.DOAnchorPos(endLocalPos, durantion).SetEase(easeType))

         .AppendCallback(() =>
         {

          if (objHide == true)
          {
                     // obj.GetComponent<Cloth>().enabled = false;
                     obj.gameObject.SetActive(false);
          }
         else if (objDestroy == true)
         {

         }
             if (setUpdate == false)
                 mysequence.Kill();
        });
        }
    }
    public static void DoSmoothmovePos(GameObject obj, Vector3 startLocalPos = default(Vector3), Vector3 endLocalPos = default(Vector3), float durantion = 0.0f, float startDelay = 0.0f, bool ease = false,Ease easeType = Ease.OutSine,byte type =0)
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }
        Sequence mysequence = DOTween.Sequence();
        if (type == 0)
        {
            clearSequence.Add(mysequence);
            mysequence.AppendInterval(startDelay)
            .Append(obj.GetComponent<RectTransform>().DOAnchorPos(startLocalPos, 0.0f))
            .Append(obj.transform.DOMove(endLocalPos, durantion).SetEase(easeType))
              .AppendCallback(() =>
               {

                   mysequence.Kill();
               });
        }
        else if(type == 1)
        {
            clearSequence.Add(mysequence);
            mysequence.AppendInterval(startDelay)
            .Append(obj.transform.DOMove(startLocalPos, 0.0f))
            .Append(obj.transform.DOMove(endLocalPos, durantion).SetEase(easeType))
              .AppendCallback(() =>
              {

                  mysequence.Kill();
              });
        }
        else
        {
            clearSequence.Add(mysequence);
            mysequence.AppendInterval(startDelay)
            .Append(obj.GetComponent<RectTransform>().DOAnchorPos(startLocalPos, 0.0f))
            .Append(obj.GetComponent<RectTransform>().DOAnchorPos(endLocalPos, durantion).SetEase(easeType))
              .AppendCallback(() =>
              {

                  mysequence.Kill();
              });

        }
    }
    public static void DoRotate(GameObject obj, Vector3 initialValue = default(Vector3),Vector3 endValue = default(Vector3),float endDuration = 0.0f, float initDuration = 0.0f, float startDelay = 0.0f)
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }
        Sequence mysequence = DOTween.Sequence();
        clearSequence.Add(mysequence);
        mysequence
         .AppendInterval(startDelay)
        .Append(obj.GetComponent<RectTransform>().DOLocalRotate(initialValue, initDuration, RotateMode.FastBeyond360))
        .Append(obj.GetComponent<RectTransform>().DOLocalRotate(endValue, endDuration, RotateMode.FastBeyond360))
        .AppendCallback(() =>
         {

             mysequence.Kill();
         });

    }
    public static void DoContinousRotate(GameObject obj, Vector3 initialValue = default(Vector3), Vector3 endValue = default(Vector3), float endDuration = 0.0f, float initDuration = 0.0f)
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }
        Sequence mysequence = DOTween.Sequence();
        clearSequence.Add(mysequence);
        //.Append(obj.GetComponent<RectTransform>().DOLocalRotate(initialValue, initDuration, RotateMode.Fast))
        mysequence.Append(obj.GetComponent<RectTransform>().DOLocalRotate(endValue, endDuration, RotateMode.FastBeyond360).SetLoops(-1))
         .AppendCallback(() =>
          {

              mysequence.Kill();
          });
        //transform.DORotate(rot, 2f, RotateMode.Fast).SetLoops(-1).SetEase(Ease.Linear);
    }
    public static void DoText(Text obj,string endValue= "",float duration = 0.0f, float startDelay = 0.0f, string startValue = "")
    {
        if (DOTween.Sequence() != null)
        {
            DOTween.Sequence().Kill();
        }
        obj.text = "";
        Sequence mysequence = DOTween.Sequence();
        clearSequence.Add(mysequence);
        mysequence.AppendInterval(startDelay)
        .Append(obj.DOText(startValue, 0.0f))
        .Append(obj.DOText(endValue, duration))
         .AppendCallback(() =>
          {

              mysequence.Kill();
          });

    }

    /*public void StartAnimation(GameObject obj, DoTweenType type,Vector3 startPos = default(Vector3),Vector3 endPos = default(Vector3),float durantion = 0.0f)
        {

        if (type == DoTweenType.MovementOneWay)
        {
            if (_targetLocation == Vector3.zero)
                _targetLocation = obj.transform.position;
            obj.transform.DOMove(_targetLocation, _moveDuration).SetEase(_moveEase);
        }
        else if (type == DoTweenType.MovementTwoWay)
        {
            if (_targetLocation == Vector3.zero)
                _targetLocation = obj.transform.position;
            StartCoroutine(MoveWithBothWays(obj,_targetLocation, _moveDuration, _moveEase));
        }
        else if (type == DoTweenType.MovementTwoWayWithSequence)
        {
            if (_targetLocation == Vector3.zero)
                _targetLocation = obj.transform.position;
            Vector3 originalLocation = obj.transform.position;
            DOTween.Sequence()
                .Append(obj.transform.DOMove(_targetLocation, _moveDuration).SetEase(_moveEase))
                .Append(obj.transform.DOMove(originalLocation, _moveDuration).SetEase(_moveEase));
        }
        else if (type == DoTweenType.MovementOneWayColorChange)
        {
            if (_targetLocation == Vector3.zero)
                _targetLocation = obj.transform.position;
            DOTween.Sequence()
                .Append(obj.transform.DOMove(_targetLocation, _moveDuration).SetEase(_moveEase))
                .Append(obj.transform.GetComponent<Renderer>().material
                .DOColor(_targetColor, _colorChangeDuration).SetEase(_moveEase));
        }
        else if (type == DoTweenType.MovementOneWayColorChangeAndScale)
        {
            if (_targetLocation == Vector3.zero)
                _targetLocation = obj.transform.position;
            DOTween.Sequence()
                .Append(obj.transform.DOMove(_targetLocation, _moveDuration).SetEase(_moveEase))
                .Append(obj.transform.DOScale(_scaleMultiplier, _moveDuration / 2.0f).SetEase(_moveEase))
                .Append(obj.transform.GetComponent<Renderer>().material
                .DOColor(_targetColor, _colorChangeDuration).SetEase(_moveEase));
        }
        else
        if (type == DoTweenType.ZoominAndBlink)
        {
            if (DOTween.Sequence() != null)
            {
                DOTween.Sequence().Kill();
            }

            DOTween.Sequence()
            .Append(obj.transform.DOScale(0.0f, 0.1f))
            .Append(obj.transform.DOScale(1.05f, 0.1f))
            .Append(obj.transform.DOScale(1.0f, 0.04f))
            .Append(obj.transform.GetComponent<Image>().DOFade(0.5f, 0.0f))
            .Append(obj.transform.DOScale(1.05f, 0.04f))
            .Append(obj.transform.GetComponent<Image>().DOFade(1.0f, 0.0f))
            .Append(obj.transform.DOScale(1.0f, 0.04f))
            .Append(obj.transform.GetComponent<Image>().DOFade(0.5f, 0.0f))
            .Append(obj.transform.GetComponent<Image>().DOFade(1.0f, 0.04f));


            //.Insert(2 * 0.2f, obj.GetComponent<Image>().DOColor(new Color(255.0F, 255.0F, 255.0F, 255.0F), 0.0f).SetEase(Ease.InOutSine))
            //.Insert(2 * 0.2f, obj.GetComponent<Image>().DOColor(Color.white, 0.15f).SetEase(Ease.InOutSine));
        }
        else if (type == DoTweenType.Smoothmove)
        {
            if (DOTween.Sequence() != null)
            {
                DOTween.Sequence().Kill();
            }
            DOTween.Sequence()
            .Append(obj.transform.DOLocalMove(startPos, 0.0f))
            .Append(obj.transform.DOLocalMove(endPos, durantion));

        }

        }*/

   /* private IEnumerator MoveWithBothWays(GameObject obj,Vector3 _targetLocation,float _moveDuration, Ease _moveEase)
    {
        Vector3 originalLocation = obj.transform.position;
        obj.transform.DOMove(_targetLocation, _moveDuration).SetEase(_moveEase);
        yield return new WaitForSeconds(_moveDuration);
        obj.transform.DOMove(originalLocation, _moveDuration).SetEase(_moveEase);
    }*/
}
