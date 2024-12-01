using System;
using TakiExtensions.TakiExtension.Singleton;
using UnityEngine;
using UnityEngine.Serialization;

namespace TakiExtensions.TakiExtension.Dialog
{
    public class DialogManagerDD : MonoBehaviourSingleton<DialogManagerDD>
    { 
        [SerializeField] private DialogParent dialogPrefab;
        protected override void LateAwake()
        {
            CreateYesNoDialog();
        }

        public void CreateYesNoDialog()
        {
            DialogParent obj = Instantiate(dialogPrefab);
            DialogPresenter presenter = obj.presenter;
            presenter.InitializeConfirm("sample","sample text", () =>
            {
                DialogParent obj2 = Instantiate(dialogPrefab);
                DialogPresenter presenter2 = obj2.presenter;
                presenter2.InitializeYesNo("sample2","sample2",()=>{Debug.Log("OK!");}, () => { Debug.Log("No!!"); });
            });
        }
        
    }
}