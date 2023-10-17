using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
//using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Net.Sockets;
using Zenject;
public partial class CanvasA : MonoBehaviour
{
    public void CopyFile()
    {
        //System.Collections.Specialized.StringCollection replacementList = new System.Collections.Specialized.StringCollection();
        //replacementList.Add("C:\\test1.txt");
        //replacementList.Add("C:\\test2.txt");
        //Clipboard.SetFileDropList(replacementList);
        //Clipboard.SetData(DataFormats.FileDrop, (object)"fgdf");
    }
    ///<summary> ссылки на панели 0-Продолжить игру 1-выбрать кольцо 2-кнопка выбора кольца 3-панель общих настроек 4-кнопка настроек 5-панель с ключами(слева сверху) 6-panelSelectLevel </summary>
    public GameObject[] panelsToActive;//0-panel ContinueGame 1-panel select ring 2- buttonSelect 3-panelSelectVolume 4-butt settings 5-keys 6-panelSelectLevel
    ///<summary>слайдеры 0-звук музыки 1-громкость звуков</summary>
    public Slider[] Sliders;
    ///<summary>путь сохранения уровней</summary>
    public string savePath;
    ///<summary>ссылка на контроллер уровней</summary>
    public LevelCreateController levelCreateController;
    ///<summary>текст имени уровня для сохранения</summary>
    public Text textName;
    ///<summary>0-выбор кольца 1-настройки 2-кнопка просмотра</summary>
    public Animator[] buttonsGameScene;
    ///<summary></summary>
    public AudioSource CanvasAudioSource;
    ///<summary></summary>
    public AudioSource Canvas1heartSource;

    /// <summary> 0-effect slow 1- effect fast</summary>
    public Image[] imagesEffects;
    /// <summary> to this values goes effects  0-effect slow 1- effect fast</summary>
    float[] effectValues;
    /// <summary>0-60 1-90 2-120 3-144 </summary>
    public Image[] buttFpsImages;
    public Color greenColor;

    public Text textFPs;
    public float[] deltaTimesFPS;
    public int FPSDeltaTimeId;



    /// <summary>ui obj for login</summary>
    public GameObject LogInAccountObj;
    /// <summary>ui obj startGame</summary>
    public GameObject StartGameUIPanel;
    /// <summary> 0-input name 1-input password 2-test ip</summary>
    public InputField[] ServerAccountFields;
    private CameraScale CameraScale;
    GameState GameState;
    CurrCreateObject CurrCreateObject;
   [Inject]
    public void Construct(CameraScale cameraScale, GameState gameState,
       CurrCreateObject currCreateObject)
    {
        CameraScale = cameraScale;
        GameState = gameState;
        CurrCreateObject = currCreateObject;
    }
    public void Start()
    {
        savePath = UnityEngine.Application.persistentDataPath + "/Resources/Levels/";
        ActiveAnimatorKey(buttonCreatorMode,true);
        UpdateAllLeveNames();
        for (int i = 0; i < 3; i++)
        {
            ActiveAnimatorKey(buttonsGameScene[i], true);
        }
        effectValues = new float[2];
        deltaTimesFPS = new float[60];
        TryLogIn();
    }
    public void ActiveButtonFps(int id)
    {
        for(int i = 0; i < 4; i++)
        {
            buttFpsImages[i].color = new Color(1, 1, 1);
        }
        buttFpsImages[id].color = greenColor;
    }
    ///<summary>загрузить панель продолжения игры</summary>
    public void LoadPanelContinue()
    {
        GameState.GameIsPaused = true;
        ActiveAnimatorKey(panelsToActive[0].GetComponent<Animator>(), true);
        panelsToActive[0].transform.GetChild(2).GetComponent<Animator>().Rebind();
    }
    ///<summary>закрыть панель продолжения игры</summary>
    public void ClosePanelContinue()
    {
        ActiveAnimatorKey(panelsToActive[0].GetComponent<Animator>(), false);
        ActiveAnimatorKey(panelsToActive[2].GetComponent<Animator>(), true);
        ActiveAnimatorKey(panelsToActive[4].GetComponent<Animator>(), true);
        //ObjectsOnScene.a.currAudioController.enabledAudio(true);
    }

    ///<summary>активировать панель выбора кольца</summary>
    public void ButtonSelectRingAction()
    {
        if (buttonsGameScene[0].transform.GetChild(2).GetComponent<RectTransform>().localScale.x > 0.9f)
            ActiveAnimatorKey(buttonsGameScene[0].transform.GetChild(2).GetComponent<Animator>(), false);
        else
            ActiveAnimatorKey(buttonsGameScene[0].transform.GetChild(2).GetComponent<Animator>(), true);
    }

    ///<summary>включить панель настроек</summary>
    public void ActivePanelSettings()
    {
        Animator a = panelsToActive[3].GetComponent<Animator>();
        if(a.GetComponent<RectTransform>().localScale.x>0.9f)
            ActiveAnimatorKey(panelsToActive[3].GetComponent<Animator>(), false);
        else
            ActiveAnimatorKey(panelsToActive[3].GetComponent<Animator>(), true);
    }

    ///<summary>выбрать кольцо</summary>
    public void SelectRing(int id)
    {
        //ObjectsOnScene.a.currCameraInGameScene.SetNewRing(id);
    }
    ///<summary>закрыть панель выбора кольца</summary>
    public void CloseSelectRing()
    {
        ActiveAnimatorKey(panelsToActive[1].GetComponent<Animator>(), false);
    }
    /// <summary>
    /// type 0-butt click 1-getRing(CreatorMode)
    /// </summary>
    /// <param name="type"></param>
    public void PlayAudioButtClick(int type)
    {
    }
    /// <summary>
    /// type 0-spawn ring 1-game complete 2-getRing 3-teleport 4-upScore
    /// </summary>
    public void PlayActionClip(int type)
    {
        int selType = type;
        if (type == 0)
            selType = Random.Range(0, 2);
        else if (type == 1)
            selType = 2;
        else if(type == 2)
            selType = Random.Range(3, 5);
        else if (type == 3)
            selType = 5;
        else if (type == 4)
            selType = 6;
    }

    public void Play1HeartSound(bool active)
    {
        if (CanvasAudioSource != null)
        {
            if (active)
            {
                //Canvas1heartSource.volume = ObjectsOnScene.a.currAudioController.VolumeSfx;
                Canvas1heartSource.Play();
            }
            else
                Canvas1heartSource.Stop();
        }
    }

    ///<summary>levelType 0- stolcknov 1- zaxv krystal 2-pobeg 3-speed proxozd 4-pratki 5-fight with boss 6-with enemyes</summary>
    public void SelectLevel(int levelType)
    {
        ActiveAnimatorKey(panelsToActive[6].GetComponent<Animator>(), false);
        if (!GameState.ActiveCreateMode)
        {
            //PlayLevel
            switch (levelType)
            {
                case 0:
                    Debug.Log("Play stolcknov");
                    break;
                case 1:
                    Debug.Log("Play zaxv krystal");
                    break;
                case 2:
                    Debug.Log("Play pobeg");
                    break;
                case 3:
                    Debug.Log("Play speed proxozd");
                    break;
                case 4:
                    Debug.Log("Play pratki");
                    break;
                case 5:
                    Debug.Log("Play fight with boss");
                    break;
                case 6:
                    Debug.Log("Play with enemyes");
                    break;
            }
        }
        else
        {
            //SelectLevelInCreatorMode
            selectedLevelType = levelType;
            EnterCreatorMode();
            switch (levelType)
            {
                case 0:
                    Debug.Log("Create stolcknov");
                    break;
                case 1:
                    Debug.Log("Create zaxv krystal");
                    break;
                case 2:
                    Debug.Log("Create pobeg");
                    break;
                case 3:
                    Debug.Log("Create speed proxozd");
                    break;
                case 4:
                    Debug.Log("Create pratki");
                    break;
                case 5:
                    Debug.Log("Create fight with boss");
                    break;
                case 6:
                    Debug.Log("Create with enemyes");
                    break;
            }
        }
    }

    public void UpdateSelectLevelPanel(bool playing)
    {
        if (playing)
        {
            panelsToActive[6].transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = "Играть";
            UpdateSelectLevelCountPlayers();
        }
        else
        {
            panelsToActive[6].transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = "Создать уровень";
            panelsToActive[6].transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetComponent<Text>().text = "альбомная";
            panelsToActive[6].transform.GetChild(0).GetChild(0).GetChild(2).GetChild(1).GetComponent<Text>().text = "альбомная";
            panelsToActive[6].transform.GetChild(0).GetChild(0).GetChild(3).GetChild(1).GetComponent<Text>().text = "вертикальная";
            panelsToActive[6].transform.GetChild(0).GetChild(0).GetChild(4).GetChild(1).GetComponent<Text>().text = "вертикальная";
            panelsToActive[6].transform.GetChild(0).GetChild(0).GetChild(5).GetChild(1).GetComponent<Text>().text = "альбомная";
            panelsToActive[6].transform.GetChild(0).GetChild(0).GetChild(6).GetChild(1).GetComponent<Text>().text = "альбомная";
            panelsToActive[6].transform.GetChild(0).GetChild(0).GetChild(7).GetChild(1).GetComponent<Text>().text = "альбомная";
        }
    }
    public void UpdateSelectLevelCountPlayers()
    {
        Debug.Log("Update select level count players NEED UPDATE");
    }

    /// <summary> 0-effect slow 1- effect fast</summary>
    public void SetEffect(int id, float value)
    {
        effectValues[id] = value;
    }

    public void ButtLogInAction()
    {
        if (ServerAccountFields[0].text.Length < 3) return;
        if (ServerAccountFields[1].text.Length < 4) return;
        NetworkSend.TryOpenAccount(ServerAccountFields[0].text, ServerAccountFields[1].text);
    }
    public void ButtSingUpAction()
    {
        if (ServerAccountFields[0].text.Length < 3) return;
        if (ServerAccountFields[1].text.Length < 4) return;
        NetworkSend.TryCreateAccount(ServerAccountFields[0].text, ServerAccountFields[1].text);
    }
    void TryLogIn()
    {
        return;
        string accName = PlayerPrefs.GetString("AccountName");
        string accPassword = PlayerPrefs.GetString("AccountPassword");
        if(accName != null && accPassword != null)
        {
            NetworkSend.TryOpenAccount(accName, accPassword);
        }
    }
    public void AfterLogIn()
    {
        LogInAccountObj.SetActive(false);
    }
    public void AfterStartGame()
    {
        StartGameUIPanel.SetActive(false);
    }
    public void TryIp()
    {
        Socket clientSocket2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSocket2.ReceiveBufferSize = 4096;//185.6.24.112//169.254.246.215
        clientSocket2.SendBufferSize = 4096;//"192.168.0.107"
        string ip = ServerAccountFields[2].text;
        string ip2 = ServerAccountFields[3].text;
        const int port = 14732;
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        if(ip2.Length<2)
            clientSocket2.BeginConnect(new IPAddress[2] { IPAddress.Parse("185.6.24.112"), IPAddress.Parse(ip) }, port, new System.AsyncCallback(ClientTCP.ClientConnectCallback), clientSocket2);
        else
            clientSocket2.BeginConnect(new IPAddress[3] { IPAddress.Parse("185.6.24.112"), IPAddress.Parse(ip), IPAddress.Parse(ip) }, port, new System.AsyncCallback(ClientTCP.ClientConnectCallback), clientSocket2);
    }
}
public partial class CanvasA
{
    /// <summary>gameType: 2-(1vs1)</summary>
    public void TryFindNetworkGame(int gameType)
    {
        switch (gameType)
        {
            case 2:
                NetworkSend.StartFindGame(gameType);
                break;
        }
    }
}
public partial class CanvasA
{
    ///<summary> ссылка на родителя всех уровней </summary>
    public GameObject CurrLevel;
    public Transform Level;
    ///<summary> базовый уровени для открытия 0-столкновение 1-захват кристаллов 2-побег 3-скоростное проъождение 4-прятки 5-бой с боссои 6-с ботами</summary>
    public GameObject[] BaseLevelForOpen;
    ///<summary> выбранный обьект для редактирования </summary>
    public MovablePartLevel createLevelSelectedObject;
    ///<summary> все 6 ui контроллеров </summary>
    public CreateLevelParController[] allPartControllers;
    ///<summary> выбран ли мод выделения обьекта </summary>
    public bool isSelectModeEnabled;
    ///<summary> последний нажатый ui контроллер </summary>
    public CreateLevelParController selectedController;
    ///<summary> родитель кнопок поворота и скэйла </summary>
    public Animator buttonsChangeTransform;
    ///<summary> родитель кнопок установки сложности </summary>
    public Animator panelSelectDifficulty;
    ///<summary> родитель кнопок изменения размера камеры </summary>
    public Animator panelChangeCameraScale;
    ///<summary> родитель кнопок создания обьекта </summary>
    public Animator panelCreateObject;
    ///<summary> родитель основных кнопок (создания нового уровня и сохранения его) </summary>
    public Animator panelMainLevelButtons;
    ///<summary> кнопка перехода от создания уровня к игре и наоборот </summary>
    public Animator buttonCreatorMode;
    ///<summary> панель установки родителя </summary>
    public Animator panelSelectParent;
    ///<summary> панель с кнопками рестарта и выхода</summary>
    public Animator panelRestartAndExitPlayLev;
    ///<summary> контроллер установки родителя </summary>
    public CreateLevelParController selectParentController;
    ///<summary> две кнопки удалить и скопировать </summary>
    public Animator[] buttonsDeleteAndCopy;
    ///<summary> текст открытого уровня </summary>
    public Text OpenLevelName;
    ///<summary> текст выбранного обьекта </summary>
    public Text SelectObjectName;
    ///<summary> имена всех уровней </summary>
    public List<string> levelNames;
    ///<summary> текущий выбранный ид уровня </summary>
    int selectedLevelId = 0;
    ///<summary> текущий выбранный тип уровня 0- stolcknov 1- zaxv krystal 2-pobeg 3-speed proxozd 4-pratki 5-fight with boss 6-with enemyes </summary>
    int selectedLevelType = 0;
    ///<summary>аним главных кнопок 0-креат левел 1-опен 2-плай 3-саве 4-поделиться </summary>
    public Animator[] createLevelMainButtonsAnimators;
    ///<summary> 0 - green most 1-green butt ... </summary>
    public GameObject[] mostsAndButtons;
    ///<summary>аниматоры рандом актив 0-панель 1- плюс кнопка 2-минус 3-persent 4-wall</summary>
    public Animator[] randomActiveObjs;
    ///<summary>  </summary>
    public Animator buttBuyCreatorMode;

    ///<summary> метод для открытия и закрытия обьектов </summary>
    public void ActiveAnimatorKey(Animator anim, bool active)
    {
        if (active)
            anim.Play("KeyOpen");
        else
            anim.Play("KeyClose");
    }
    ///<summary> создать новый префаб уровня старый будет удален </summary>
    void CreateLevel(int levelType = -1, bool openLevel = false)
    {
        if (Level != null)
        {
            DestroyImmediate(Level.gameObject);
        }
        GameObject g = null;
        if(levelType ==-1)
            g=GameObject.Instantiate(BaseLevelForOpen[selectedLevelType], CurrLevel.transform);
        else
            g = GameObject.Instantiate(BaseLevelForOpen[levelType], CurrLevel.transform);
        g.transform.position = CurrLevel.transform.position;
        g.name = "Lev";
        Level = g.transform;
        ActiveAnimatorKey(buttonsChangeTransform, false);
        ActivePanelChangeCameraScale(true);
        ActivePanelCreateObject(true);
        ActiveAnimatorKey(createLevelMainButtonsAnimators[2], true);
        ActiveAnimatorKey(createLevelMainButtonsAnimators[3], true);
        ActiveAnimatorKey(createLevelMainButtonsAnimators[4], true);
        ActiveAnimatorKey(panelSelectDifficulty, true);
    }
    public void CreateLevelButtAct()
    {
        CreateLevel();
    }

    ///<summary> активировать нужное число контроллеров </summary>
    public void SelectCreateLevelObjectControllers(int count)//указать нужное количество контроллеров
    {
        for (int i = 0; i < 6; i++)
        {
            if(allPartControllers[i].GetComponent<RectTransform>().localScale.x == 1)
                allPartControllers[i].GetComponent<Animator>().Rebind();           
        }
        for (int i = 0; i < count; i++)
        {
            allPartControllers[i].GetComponent<Animator>().Play("KeyOpen");
        }
    }

    ///<summary> сохранить уровень в соответсвии с input field </summary>
    public void SaveLevel()
    {        
        if (Level != null)
        {
            //if (!Directory.Exists("Assets/Resources/LevelPrefabs"))
                //AssetDatabase.CreateFolder("Assets/Resources", "LevelPrefabs");
            string s = textName.text;
            if (s.Length == 0)
                s = "1";
            string localPath = "Assets/Resources/LevelPrefabs/" + s + ".prefab";
            //localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
            List<MovablePartLevel> allMovableParts = new List<MovablePartLevel>();
            CheckMovablePartLevels(Level);
            void CheckMovablePartLevels(Transform t)
            {
                if (t.GetComponent<MovablePartLevel>() != null)
                    allMovableParts.Add(t.GetComponent<MovablePartLevel>());
                if (t.childCount > 0)
                {
                    for (int i = 0; i < t.childCount; i++)
                        CheckMovablePartLevels(t.GetChild(i));
                }
            }
            MovPartLevParams[] allParts = new MovPartLevParams[allMovableParts.Count];
            for(int i = 0; i < allMovableParts.Count; i++)
            {
                allMovableParts[i].SetParametersToSave();
                allParts[i] =new MovPartLevParams();
                MovPartLevParams m = allParts[i];
                m.ObjId = allMovableParts[i].ObjId;
                m.ListId = allMovableParts[i].ListId;
                m.IndexObj = allMovableParts[i].IndexObj;
                m.OtherParameters = allMovableParts[i].OtherParameters;
                m.mainParams = allMovableParts[i].mainParams;
                m.MainIndex = allMovableParts[i].MainIndex;
                m.OtherIndex = allMovableParts[i].OtherIndex;
                m.type = allMovableParts[i].type;
                m.typeGun = allMovableParts[i].typeGun;
                m.PosX = allMovableParts[i].PosX;
                m.PosY = allMovableParts[i].PosY;
                m.RotZ = allMovableParts[i].RotZ;
                m.ScaleX = allMovableParts[i].ScaleX;
                m.ScaleY = allMovableParts[i].ScaleY;
            }
            //SavedLevelParameters savedPars = new SavedLevelParameters { allParts = allParts, levelType = Level.GetComponent<Level>().levelType};
            //Debug.Log("save path: " + savePath + s);
            //using (FileStream fs = File.Open(savePath + s, FileMode.OpenOrCreate))
            //{
            //    BinaryFormatter b = new BinaryFormatter();
            //    b.Serialize(fs, (object)savedPars);
            //}
            //PrefabUtility.SaveAsPrefabAsset(CurrLevel.transform.Find("Lev").gameObject, localPath);
            UpdateAllLeveNames();
        }
    }
    ///<summary> чекнуть и обновить доступные имена уровней </summary>
    public void UpdateAllLeveNames()
    {
        if(!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        string[] names = Directory.GetFileSystemEntries(savePath);
        for (int i = 0; i < names.Length; i++)//Remove
        {
            string stemp = Path.GetFileNameWithoutExtension(names[i]);
            for (int j = 0; j < stemp.Length; j++)
            {
                if (stemp[j] == '.')
                {
                    stemp = stemp.Remove(j);
                }
            }
            names[i] = stemp;
        }
        for (int i = 0; i < names.Length; i++)
        {
            if (names[i] != null)
            {
                string s = names[i];
                for (int j = i + 1; j < names.Length; j++)
                {
                    if (names[j] != null)
                        if (s.Equals((string)names[j]))
                            names[j] = null;
                }
            }
        }
        levelNames = new List<string>();
        for (int i = 0; i < names.Length; i++)
            if (names[i] != null)
            {
                levelNames.Add(names[i]);
            }
        if (levelNames.Count > 0)
            ActiveAnimatorKey(createLevelMainButtonsAnimators[1], true);
        else
        {
            ActiveAnimatorKey(createLevelMainButtonsAnimators[1], false);
            ActiveAnimatorKey(createLevelMainButtonsAnimators[1].transform.GetChild(1).GetComponent<Animator>(), false);
        }
    }
    ///<summary> открыть уровень с текущим ид </summary>
    public void OpenLevel()
    {
        if (levelNames.Count > 0)
            if (levelNames[selectedLevelId] != null)
            {
                //CreateLevel((GameObject)Resources.Load("Levels/" + levelNames[selectedLevelId]));
                textName.transform.parent.GetComponent<InputField>().text = levelNames[selectedLevelId];
                MovPartLevParams[] allParts;
                SavedLevelParameters savedPars;
                using (FileStream fs = File.Open(savePath + levelNames[selectedLevelId], FileMode.Open))
                {
                    BinaryFormatter b = new BinaryFormatter();
                    savedPars = (SavedLevelParameters)b.Deserialize(fs);
                    allParts = savedPars.allParts;
                }
                CreateLevel(savedPars.levelType, true);
                //Level.GetComponent<Level>().levelType = savedPars.levelType;
                CurrCreateObject c = CurrCreateObject;
                MovablePartLevel[] allMoveObj = new MovablePartLevel[allParts.Length];
                for(int i = 0; i < allParts.Length; i++)
                {
                    c.CurrObject = allParts[i].ObjId;
                    c.CurrList = allParts[i].ListId;
                    allMoveObj[i] = (c.CreateCurrObject2()).GetComponent<MovablePartLevel>();
                    allMoveObj[i].SetSavedParams(allParts[i]);
                }
                for(int i =0;i<allMoveObj.Length;i++)
                {
                    allMoveObj[i].SetParametersAfterOpen();
                }
            }
    }

    ///<summary> удалить уровень с текущим ид </summary>
    public void DeleteCurrLevel()
    {
        if (levelNames.Count > 0)
        {
            string localPath = "Assets/Resources/LevelPrefabs/" + levelNames[selectedLevelId] + ".prefab";
            //AssetDatabase.DeleteAsset(localPath);
            selectedLevelId = 0;
            UpdateAllLeveNames();
            UpdateLevelName();
        }
    }
    ///<summary> изменить ид плюс и минус </summary>
    public void ChangeOpenLevelId(int i)
    {
        selectedLevelId += i;
        if (selectedLevelId < 0)
            selectedLevelId = levelNames.Count - 1;
        else if (selectedLevelId > levelNames.Count - 1)
            selectedLevelId = 0;
        UpdateLevelName();
    }
    ///<summary> изменить только текст ui для выбранного уровня </summary>
    public void UpdateLevelName()
    {
        if (levelNames.Count > 0)
        {
            OpenLevelName.text = levelNames[selectedLevelId];
        }
        else
        {
            OpenLevelName.text = "non levels, create...";
        }
    }

    ///<summary> тестовый запуск уровня. выход из создания и запуск тестового уровня </summary>
    public void TestPlayLevel()
    {
        if (Level != null)
        {
            SaveLevel();
            ExitCreatorMode(1);
            levelCreateController.TestPlayLevel(Level.gameObject);
            levelCreateController.testLevel = true;
            ActiveAnimatorKey(buttonCreatorMode, false);
            ActiveAnimatorKey(panelRestartAndExitPlayLev, true);
        }
    }  
    GameObject InstantiateDiffMechanism(GameObject gi)
    {
        GameObject g = Instantiate(gi, Level);
        g.transform.localPosition = Vector3.zero;
        g.GetComponent<MovablePartLevel>().nonDeletable = true;
        return g;
    }

    public void StartSelectParent()
    {
        selectParentController.textValueParameter.text = "???";
        isSelectModeEnabled = true;
        selectedController = selectParentController;
    }
    public void RemoveParent()
    {
        createLevelSelectedObject.transform.SetParent(Level);
        selectParentController.textValueParameter.text = "Lev";
    }

    public void ButtBuyCreatorModeAction()
    {
        if (buttBuyCreatorMode.transform.GetChild(1).GetComponent<RectTransform>().localScale.x > 0.9f)
        {
            ActiveAnimatorKey(buttBuyCreatorMode.transform.GetChild(1).GetComponent<Animator>(), false);
        }
        else
            ActiveAnimatorKey(buttBuyCreatorMode.transform.GetChild(1).GetComponent<Animator>(), true);
    }

}
public partial class CanvasA
{
    public bool activeRotateButton1 = false;
    public bool activeRotateButton2 = false;
    public bool activeScaleXButtUp = false;
    public bool activeScaleYButtUp = false;
    public bool activeScaleXButtDown = false;
    public bool activeScaleYButtDown = false;
    public bool activeScalePlusCamera = false;
    public bool activeScaleMinusCamera = false;
    float kpScaleCamera = 0f;
    public void DownRotateButton()
    {
        activeRotateButton1 = true;
    }
    public void UpRotateButton()
    {
        activeRotateButton1 = false;
    }
    public void DownRotateButton2()
    {
        activeRotateButton2 = true;
    }
    public void UpRotateButton2()
    {
        activeRotateButton2 = false;
    }
    public void DownButtonScaleXButtUp()
    {
        activeScaleXButtUp = true;
    }
    public void UpButtonScaleXButtUp()
    {
        activeScaleXButtUp = false;
    }
    public void DownButtonScaleYButtUp()
    {
        activeScaleYButtUp = true;
    }
    public void UpButtonScaleYButtUp()
    {
        activeScaleYButtUp = false;
    }
    public void DownButtonScaleXButtDown()
    {
        activeScaleXButtDown = true;
    }
    public void UpButtonScaleXButtDown()
    {
        activeScaleXButtDown = false;
    }
    public void DownButtonScaleYButtDown()
    {
        activeScaleYButtDown = true;
    }
    public void UpButtonScaleYButtDown()
    {
        activeScaleYButtDown = false;
    }
    public void DownButtonScalePlusCamera()
    {
        activeScalePlusCamera = true;
    }
    public void UpButtonScalePlusCamera()
    {
        activeScalePlusCamera = false;
    }
    public void DownButtonScaleMinusCamera()
    {
        activeScaleMinusCamera = true;
    }
    public void UpButtonScaleMinusCamera()
    {
        activeScaleMinusCamera = false;
    }
    private void Update()//Scale of camera
    {
        imagesEffects[0].color = new Color(1, 1, 1, Mathf.Lerp(imagesEffects[0].color.a, effectValues[0], Time.unscaledTime * 0.001f));
        imagesEffects[1].color = new Color(1, 1, 1, Mathf.Lerp(imagesEffects[1].color.a, effectValues[1], Time.unscaledTime * 0.001f));

        deltaTimesFPS[FPSDeltaTimeId] = Time.unscaledDeltaTime;
        FPSDeltaTimeId++;
        if (FPSDeltaTimeId == 60)
        {
            FPSDeltaTimeId = 0;
            float averageTime = 0;
            for(int i = 0; i < 60; i++)
            {
                averageTime += deltaTimesFPS[i];
            }
            averageTime /= 60;
            deltaTimesFPS = new float[60];
            textFPs.text = "" + (int)(1 / averageTime);
        }
    }
    public void UnselectSelObject()
    {
        createLevelSelectedObject = null;
        ActiveButtonsDeleteAndCopy(false);
        ActiveAnimatorKey(buttonsChangeTransform, false);
        ActiveAnimatorKey(panelSelectParent, false);
        SelectCreateLevelObjectControllers(0);
        RefreshStateRandomActiveObj();
        SelectObjectName.text = "";
    }
    public void SelectSelObject(MovablePartLevel m)
    {
        createLevelSelectedObject = m;
        SelectObjectName.text = m.gameObject.name;
        ActiveAnimatorKey(randomActiveObjs[0], !m.nonDeletable);
        RefreshStateRandomActiveObj();
        ActiveButtonsDeleteAndCopy(!m.nonDeletable);
        ActiveAnimatorKey(panelSelectParent, true);
    }
    public void ButtonModeAction()
    {
        if (!GameState.ActiveCreateMode)
        {
            //EnterCreatorMode();
            GameState.ActiveCreateMode = true;
            ActiveAnimatorKey(panelsToActive[6].GetComponent<Animator>(), true);
            UpdateSelectLevelPanel(false);
        }
        else
            ExitCreatorMode(0);
    }
    public void EnterCreatorMode()
    {
        //levelCreateController.ClearAllLevels();
        GameState.SetToCreateLevel();
        ActivePanelMainButtons(true);
        buttonCreatorMode.transform.GetChild(0).GetComponent<Text>().text = "Game mode";
        ActiveAnimatorKey(buttonCreatorMode, true);
        //ObjectsOnScene.a.currTimeController.SetSlowTime(true);
        ActiveAnimatorKey(createLevelMainButtonsAnimators[0], true);
        if(levelNames.Count>0)
            ActiveAnimatorKey(createLevelMainButtonsAnimators[1], true);
        if (Level != null)
        {
            ActiveAnimatorKey(createLevelMainButtonsAnimators[2], true);
            ActiveAnimatorKey(createLevelMainButtonsAnimators[3], true);
            ActiveAnimatorKey(createLevelMainButtonsAnimators[4], true);
            ActiveAnimatorKey(panelChangeCameraScale, true);
            ActiveAnimatorKey(panelCreateObject, true);
            ActiveAnimatorKey(panelSelectDifficulty, true);
        }
        ActiveAnimatorKey(panelRestartAndExitPlayLev, false);
        for(int i =0;i<3;i++)
            ActiveAnimatorKey(buttonsGameScene[i], false);
        //ObjectsOnScene.a.currCameraInGameScene.transform.Find("Light").gameObject.SetActive(true);
    }
    /// <summary> type: 0-base move 1-to test level</summary>
    public void ExitCreatorMode(int type)
    {
        ActivePanelChangeCameraScale(false);
        ActiveAnimatorKey(buttonsChangeTransform, false);
        ActivePanelCreateObject(false);
        UnselectSelObject();
        ActivePanelMainButtons(false);
        GameState.SetToGame();
        buttonCreatorMode.transform.GetChild(0).GetComponent<Text>().text = "Creator mode";
        SelectCreateLevelObjectControllers(0);
        levelCreateController.testLevel = false;
        if (type == 0)
        {
            levelCreateController.AddLevel();
            for (int i = 0; i < 3; i++)
                ActiveAnimatorKey(buttonsGameScene[i], true);
        }
        else if (type == 1)
        {
            ActiveAnimatorKey(buttonsGameScene[2], true);
        }
        //ObjectsOnScene.a.currTimeController.SetSlowTime(false);
        for (int i = 0; i < 5; i++)
        {
            ActiveAnimatorKey(createLevelMainButtonsAnimators[i], false);
        }
        ActiveAnimatorKey(panelSelectDifficulty, false);
        //ObjectsOnScene.a.currCameraInGameScene.transform.Find("Light").gameObject.SetActive(false);
    }
    public void ActivePanelChangeCameraScale(bool active)
    {
        if (active)
            panelChangeCameraScale.Play("KeyOpen");
        else
            panelChangeCameraScale.Play("KeyClose");
    }
    public void ActivePanelCreateObject(bool active)
    {
        if (active)
            panelCreateObject.Play("KeyOpen");
        else
            panelCreateObject.Play("KeyClose");
    }
    public void ActiveButtonsDeleteAndCopy(bool active)
    {
        if (active)
        {
            buttonsDeleteAndCopy[0].Play("KeyOpen");
            buttonsDeleteAndCopy[1].Play("KeyOpen");
        }
        else
        {
            buttonsDeleteAndCopy[0].Play("KeyClose");
            buttonsDeleteAndCopy[1].Play("KeyClose");
        }
    }
    public void ActivePanelMainButtons(bool active)
    {
        if (active)
            panelMainLevelButtons.Play("KeyOpen");
        else
            panelMainLevelButtons.Play("KeyClose");
    }
    public void RefreshStateRandomActiveObj()
    {
        if (createLevelSelectedObject != null)
        {
            RandomActiveObject r = createLevelSelectedObject.GetComponent<RandomActiveObject>();
            if (r != null)
            {
                ActiveAnimatorKey(randomActiveObjs[1], false);
                ActiveAnimatorKey(randomActiveObjs[2], true);
                ActiveAnimatorKey(randomActiveObjs[3], true);
                ActiveAnimatorKey(randomActiveObjs[4], true);
                randomActiveObjs[3].GetComponent<CreateLevelParController>().SetParameters("persent value", r.randomValue, 150f, 0);
                if (r.wall != null)
                    randomActiveObjs[4].GetComponent<CreateLevelParController>().textValueParameter.text = r.wall.gameObject.name;
                else
                    randomActiveObjs[4].GetComponent<CreateLevelParController>().textValueParameter.text = "null";
                //randomActiveObjs[4].GetComponent<CreateLevelParController>().SetParameters("wall",r.wall, 150f, 0);
            }
            else
            {
                ActiveAnimatorKey(randomActiveObjs[1], true);
                ActiveAnimatorKey(randomActiveObjs[2], false);
                ActiveAnimatorKey(randomActiveObjs[3], false);
                ActiveAnimatorKey(randomActiveObjs[4], false);
            }
        }
        else
        {
            for(int i =0;i<5;i++)
                ActiveAnimatorKey(randomActiveObjs[i], false);
        }
    }
    public void AddRandomActiveObject()
    {
        if(createLevelSelectedObject.GetComponent<RandomActiveObject>() == null)
        {
            createLevelSelectedObject.gameObject.AddComponent<RandomActiveObject>();
        }
        RefreshStateRandomActiveObj();
    }
    public void RemoveRandomActiveObject()
    {
        if (createLevelSelectedObject.GetComponent<RandomActiveObject>() != null)
        {
            DestroyImmediate(createLevelSelectedObject.gameObject.GetComponent<RandomActiveObject>());
        }
        RefreshStateRandomActiveObj();
    }

}
[System.Serializable]
public class SavedLevelParameters
{
    public MovPartLevParams[] allParts;
    public int levelType;
}

