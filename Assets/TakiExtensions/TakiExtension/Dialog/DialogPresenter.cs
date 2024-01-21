using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TakiExtensions.TakiExtension.Dialog
{
    public class DialogPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject rootObject;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI body;
        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;
        [SerializeField] private Button confirmButton;

        private Transform myTF;

        public void InitializeYesNo(string titleText,string bodyText, Action yes,Action no)
        {
            title.text = titleText;
            body.text = bodyText;
            
            yesButton.gameObject.SetActive(true);
            noButton.gameObject.SetActive(true);
            confirmButton.gameObject.SetActive(false);
            
            yesButton.onClick.RemoveAllListeners();
            noButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener(()=>StartCoroutine(FinalizeDialog(yes)));
            noButton.onClick.AddListener(()=> StartCoroutine(FinalizeDialog(no)));
            StartCoroutine(InitializeDialog());
        }
        
        public void InitializeConfirm(string titleText,string bodyText, Action confirm)
        {
            title.text = titleText;
            body.text = bodyText;
            
            yesButton.gameObject.SetActive(false);
            noButton.gameObject.SetActive(false);
            confirmButton.gameObject.SetActive(true);
            
            yesButton.onClick.RemoveAllListeners();
            noButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(()=> StartCoroutine(FinalizeDialog(confirm)));
            StartCoroutine(InitializeDialog());
        }


        /// <summary>
        /// アニメーションとボタン押せる可否を設定する。
        /// </summary>
        /// <returns></returns>
        private IEnumerator InitializeDialog()
        {
            yesButton.interactable = false;
            noButton.interactable = false;
            confirmButton.interactable = false;
            
            myTF = gameObject.transform;
            int loop = 50;
            for (int i = 1; i <= loop; i++)
            {
                myTF.localScale = Vector3.one * i / loop;
                yield return new WaitForFixedUpdate();
            }
            
            yesButton.interactable = true;
            noButton.interactable = true;
            confirmButton.interactable = true;
        }

        private IEnumerator FinalizeDialog(Action action)
        {
            yesButton.interactable = false;
            noButton.interactable = false;
            confirmButton.interactable = false;
            
            myTF = gameObject.transform;
            int loop = 50;
            for (int i = 1; i <= loop; i++)
            {
                myTF.localScale = Vector3.one * (50-i) / loop;
                yield return new WaitForFixedUpdate();
            }

            action.Invoke();
            
            Destroy(rootObject);
        }

    }
}