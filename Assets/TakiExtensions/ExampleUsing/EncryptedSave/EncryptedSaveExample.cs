using TakiExtensions.TakiExtension.EncryptedSave;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TakiExtensions.ExampleUsing.EncryptedSave
{
    public class EncryptedSaveExample : MonoBehaviour
    {

        class SaveStructure
        {
            public string saveString = "";
        }

        

        [SerializeField] TMP_InputField inputField;
        [SerializeField]Button saveButton;
        [SerializeField]Button loadButton;

        // Use this for initialization
        void Awake()
        {
            saveButton.onClick.AddListener(Save);
            loadButton.onClick.AddListener(Load);
        }


        void Save()
        {
            SaveStructure s = new SaveStructure();
            s.saveString = inputField.text;
            SaveDataManager.SavePlayerData(s, "SaveSample.txt");

            Debug.Log("セーブしました。");

        }

        void Load()
        {
            SaveStructure s = SaveDataManager.LoadPlayerData<SaveStructure>("SaveSample.txt");
            inputField.text = s.saveString;
            Debug.Log("ロードしました。");

        }


    }
}