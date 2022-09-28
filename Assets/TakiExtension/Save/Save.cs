using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TakiExtension.Save
{
    /// <summary>
    /// ���ۂɃZ�[�u�f�[�^���Ǘ�����N���X�ł��B
    /// </summary>
    public class Save
    {
        //���s���萔
        //�v���b�g�t�H�[�����ɕۑ��ꏊ��ς���
        private static readonly string SavePath = Application.persistentDataPath + "/";

        //���Ȃǂ̃f�[�^�̓R���p�C�����萔�Ƃ��邱�ƂŁA���s���x���㏸��_���B
        //�����͎蓮�œ��͂���K�v������B���ɂ���l����D���ɕς��Ă��������B
        private const string DefaultAesKey32byte = @"11112222333344445555666677778888";
        private const string DefaultAesIV16byte = @"1111222233334444";

        private const int KeySize = 256;
        private const int BlockSize = 128;

        /// <summary>
        /// Unity��JsonUtility�𗘗p���邱�ƂŁAJsonUtility���Ή������N���X��Json�`���ŕۑ����܂��B
        /// �n�[�h�R�[�f�B���O���ꂽ���Ə������x�N�g�����g�p���܂��B�ۑ��ꏊ������ł��B
        /// </summary>
        /// <typeparam name="ClassType">�Z�[�u����N���X�̌^�B���ׂẴ����o�ϐ��̓V���A���C�Y�\�ł���K�v������܂��B</typeparam>
        /// <param name="instance">�Z�[�u����N���X�̃C���X�^���X�B</param>
        /// <param name="fileName">�t�@�C�����B�p�X�͕K�v����܂���B</param>
        static public void SavePlayerData<ClassType>(ClassType instance, string fileName)
        {
            string path = SavePath + fileName;
            SavePlayerData(instance, path, DefaultAesIV16byte, DefaultAesKey32byte);
        }
        /// <summary>
        /// �t�@�C����ǂݍ��݁AUnity��JsonUtility�𗘗p���邱�ƂŁA�w�肵���N���X�Ƀp�[�X���܂��B
        /// </summary>
        /// <typeparam name="ClassType">�Z�[�u����N���X�̌^�B���ׂẴ����o�ϐ��̓V���A���C�Y�\�ł���K�v������܂��B</typeparam>
        /// <param name="fileName">�ǂݍ��ރt�@�C�����B�p�X�͕K�v����܂���B</param>
        /// <returns>�p�[�X���ꂽ�C���X�^���X</returns>
        static public ClassType LoadPlayerData<ClassType>(string fileName)
        {
            string path = SavePath + fileName;
            return LoadPlayerData<ClassType>(path, DefaultAesIV16byte, DefaultAesKey32byte);
        }



        /// <summary>
        /// Unity��JsonUtility�𗘗p���邱�ƂŁAJsonUtility���Ή������N���X��Json�`���ŕۑ����܂��B
        /// </summary>
        /// <typeparam name="ClassType">�Z�[�u����N���X�̌^�B���ׂẴ����o�ϐ��̓V���A���C�Y�\�ł���K�v������܂��B</typeparam>
        /// <param name="instance">�Z�[�u����N���X�̃C���X�^���X�B</param>
        /// <param name="path">�Z�[�u����p�X�B�t�@�C���̖��O���܂ޕK�v������܂��B</param>
        /// <param name="aesIV">AES�ɂ��Í����̏������x�N�g���B</param>
        /// <param name="aesKey">AES�ɂ��Í����L�[�B</param>
        static public void SavePlayerData<ClassType>(ClassType instance, string path, string aesIV, string aesKey)
        {
            //�����𖞂����Ȃ��Ȃ瑁�����^�[��
            if (!IsSatisfyStringLength(aesKey, aesIV))
            {
                Debug.Log("�������G���[");
                return;
            }

            //�Z�[�u�������N���X��Json�ɕϊ�
            string jsonData = JsonUtility.ToJson(instance);
            //Json�e�L�X�g�f�[�^�̈Í���
            string cipher = Encrypt(jsonData, aesIV, aesKey);
            //StreamWriter�ŏ�������
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.GetEncoding("utf-8")))
            {
                writer.WriteLine(cipher);
            }
        }

        /// <summary>
        /// �t�@�C����ǂݍ��݁AUnity��JsonUtility�𗘗p���邱�ƂŁA�w�肵���N���X�Ƀp�[�X���܂��B
        /// </summary>
        /// <typeparam name="ClassType">�Z�[�u����N���X�̌^�B���ׂẴ����o�ϐ��̓V���A���C�Y�\�ł���K�v������܂��B</typeparam>
        /// <param name="path">�Z�[�u����p�X�B�t�@�C���̖��O���܂ޕK�v������܂��B</param>
        /// <param name="aesIV">AES�ɂ��Í����̏������x�N�g���B</param>
        /// <param name="aesKey">AES�ɂ��Í����L�[�B</param>
        /// <returns>�p�[�X���ꂽ�C���X�^���X</returns>
        static public ClassType LoadPlayerData<ClassType>(string path, string aesIV, string aesKey)
        {
            //�t�@�C�������݂��Ȃ��Ȃ瑁�����^�[��
            if (!File.Exists(path))
            {
                Debug.Log("�t�@�C���͑��݂��܂���");
                return default;
            }
            if (!IsSatisfyStringLength(aesKey, aesIV))
            {
                Debug.Log("�������G���[");
                return default;
            }
            //�Í�����ǂݎ�������͂�ۑ�����ϐ�
            string cipher;
            //StreamReader�ňÍ����̓ǂݍ���
            using (StreamReader sr = new StreamReader(path, Encoding.GetEncoding("utf-8")))
            {
                cipher = sr.ReadToEnd();
            }
            //�������s���B
            string plain = Decrypt(cipher, aesIV, aesKey);

            //���������f�[�^�̃p�[�X�����݂�B
            ClassType plainedInstance = default;
            try
            {
                plainedInstance = JsonUtility.FromJson<ClassType>(plain);//ref �����N���X�̃C���X�^���X�ɏ����i�[����
            }
            catch
            {
                Debug.Log("�Z�[�u�f�[�^�̃p�[�X�Ɏ��s���܂����B");
            }

            //������ɂ��挋�ʂ����^�[������B
            return plainedInstance;
        }

        #region �ȉ��͔���J�̊֐��ł��B���̃N���X���g�����͓��Ɉӎ����Ȃ��Ă��ǂ��Ǝv���܂��B

        /// <summary>
        /// AES���g���ĕ�������Í������܂�
        /// </summary>
        /// <param name="text">�Í������镶����</param>
        /// <param name="iv">�������x�N�g��</param>
        /// <param name="key">�Í����L�[</param>
        /// <returns>�Í������ꂽ������</returns>
        static string Encrypt(string text, string iv, string key)
        {

            using (AesManaged aes = CreateAESManager(key, iv))
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                byte[] encrypted;
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream ctStream = new CryptoStream(mStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(ctStream))
                        {
                            sw.Write(text);
                        }
                        encrypted = mStream.ToArray();
                    }
                }
                return System.Convert.ToBase64String(encrypted);
            }
        }

        /// <summary>
        /// AES���g���ĈÍ����𕜍����܂�
        /// </summary>
        /// <param name="cipher">�Í������ꂽ������</param>
        /// <param name="iv">�������x�N�g��</param>
        /// <param name="key">�Í����L�[</param>
        /// <returns>�������ꂽ������</returns>
        static string Decrypt(string cipher, string iv, string key)
        {
            using (AesManaged aes = CreateAESManager(key, iv))
            {
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                string plain = string.Empty;
                using (MemoryStream mStream = new MemoryStream(System.Convert.FromBase64String(cipher)))
                {
                    using (CryptoStream ctStream = new CryptoStream(mStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(ctStream))
                        {
                            plain = sr.ReadLine();
                        }
                    }
                }
                return plain;
            }
        }

        /// <summary>
        /// AES�Ǘ��N���X���쐬����֐��B�K�v�ȏ��������܂Ƃ߂��B
        /// </summary>
        /// <param name="key">�Í����L�[</param>
        /// <param name="iv">�������x�N�g��</param>
        /// <returns></returns>
        static AesManaged CreateAESManager(string key, string iv)
        {
            AesManaged aes = new AesManaged();
            aes.BlockSize = BlockSize;
            aes.KeySize = KeySize;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            aes.IV = Encoding.UTF8.GetBytes(iv);
            aes.Key = Encoding.UTF8.GetBytes(key);

            return aes;

        }

        /// <summary>
        /// �L�[�⏉�����x�N�g���̃T�C�Y�������Ă��邩�m�F���܂��B
        /// </summary>
        static bool IsSatisfyStringLength(string key, string iv)
        {
            return key.Length * 8 == KeySize && iv.Length * 8 == BlockSize;
        }

        #endregion
    }
}
