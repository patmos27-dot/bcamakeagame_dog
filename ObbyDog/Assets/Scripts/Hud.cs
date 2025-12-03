using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene loading




public class Hud : MonoBehaviour
{
  [SerializeField]
  private TMP_Text timer = null;




  private float time = 300.0f; // Start at 5 minutes
  private float lastSecondBlink = -1.0f;
  Coroutine blinkCoroutine = null;
  Color textColor = Color.black;
  private static bool isTimerRunning = true;




  private static Hud instance;




  void Awake()
  {
      instance = this;
  }




  void Start()
{
   // Reset timer every time the Game scene loads
   time = 300.0f;          // Start at 5 minutes
   isTimerRunning = true;  // Make sure the timer is running
   lastSecondBlink = -1.0f;


   // Animate HUD sliding into place
   RectTransform rectTransform = transform as RectTransform;
   rectTransform.anchoredPosition = new Vector2(0.0f, 300.0f);
   rectTransform.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.InOutQuad);


   // Cache the original text color
   textColor = timer.color;


   // Initialize the timer display immediately
   TimeSpan span = TimeSpan.FromSeconds(time);
   timer.text = span.ToString(@"mm\:ss");
}






  // 🔄 This used to reset the timer — now it goes to Main Menu
  public void GoToMainMenu()
  {
      SceneManager.LoadScene("MainMenu"); // Make sure the scene name matches!
  }




  void Update()
  {
      if (isTimerRunning)
      {
          time -= Time.deltaTime; // Count down
          time = Math.Max(time, 0.0f);




          TimeSpan span = TimeSpan.FromSeconds(time);
          timer.text = span.ToString(@"mm\:ss");




          if (time <= 0.0f)
          {
              isTimerRunning = false;
              SceneManager.LoadScene("Lose"); // Load Lose scene when time runs out
          }




          if (time <= 60.0f)
          {
              float flooredTime = Mathf.Floor(time);
              if (flooredTime < lastSecondBlink || lastSecondBlink < 0)
              {
                  lastSecondBlink = flooredTime;
                  if (blinkCoroutine != null)
                  {
                      StopCoroutine(blinkCoroutine);
                  }
                  blinkCoroutine = StartCoroutine(DoBlink(0.75f));
              }
          }
      }
  }




  public static void StopTimer()
  {
      if (instance != null)
      {
          isTimerRunning = false;
          Debug.Log("Timer stopped at: " + instance.timer.text);
      }
  }




  IEnumerator DoBlink(float duration)
  {
      yield return null;




      RectTransform rectTransform = timer.transform as RectTransform;
      rectTransform.localScale = Vector3.one;
      timer.color = textColor;




      Tween colorRedTween = timer.DOColor(Color.red, duration * 0.1f);
      Tween colorYellowTween = timer.DOColor(Color.yellow, duration * 0.1f);




      DG.Tweening.Sequence seq = DOTween.Sequence();
      seq.Append(colorRedTween);
      seq.Append(colorYellowTween);
      seq.SetLoops(-1);




      seq.Play();




      Tween scaleTween = null;
      for (int i = 0; i < 3; ++i)
      {
          scaleTween = rectTransform.DOScale(1.5f, duration * 0.125f);
          yield return scaleTween.WaitForCompletion();




          scaleTween.Kill();
          scaleTween = rectTransform.DOScale(0.8f, duration * 0.125f);
          yield return scaleTween.WaitForCompletion();
      }




      scaleTween.Kill();
      seq.Kill();
      colorRedTween.Kill();
      colorYellowTween.Kill();




      rectTransform.DOScale(1.0f, duration * 0.2f);
      timer.DOColor(textColor, duration * 0.25f);
  }
}
