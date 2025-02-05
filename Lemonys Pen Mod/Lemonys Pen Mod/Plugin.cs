using BepInEx;
using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Utilla;
using GorillaLocomotion;
using System.IO;
using System.Reflection;



namespace Lemonys_Pen_Mod
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */

    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;
        public static Plugin instance;
        public static AssetBundle bundle;
        public static string Assetname = "lemonpen";
        public static GameObject assetBundleParent;
        public int CurColornum = 1;
        public int MinColornum = 1;
        public int MaxColornum = 8;
        public Color penColor = Color.black;
        public GameObject DrawingParent;


        void Start()
        {
            /* A lot of Gorilla Tag systems will not be set up when start is called /*
			/* Put code in OnGameInitialized to avoid null references */

            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnEnable()
        {
            /* Set up your mod here */
            /* Code here runs at the start and whenever your mod is enabled*/



            HarmonyPatches.ApplyHarmonyPatches();
        }

        void OnDisable()
        {
            /* Undo mod setup here */
            /* This provides support for toggling mods with ComputerInterface, please implement it :) */
            /* Code here runs whenever your mod is disabled (including if it disabled on startup)*/

            HarmonyPatches.RemoveHarmonyPatches();
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            instance = this;
            /* Code here runs after the game initializes (i.e. GorillaLocomotion.Player.Instance != null) */
            bundle = LoadAssetBundle("Lemonys Pen Mod." + Assetname);
            assetBundleParent = Instantiate(bundle.LoadAsset<GameObject>("LemonPen"));


        }

        void Update()
        {

            if (NetworkSystem.Instance.InRoom && NetworkSystem.Instance.GameModeString.Contains("MODDED"))
            {
                var Player = GorillaLocomotion.Player.Instance;
                bool drawButton = ControllerInputPoller.instance.rightControllerPrimaryButton;
                bool colorButton = ControllerInputPoller.instance.rightControllerSecondaryButton;
                AudioSource sugmanuts = assetBundleParent.transform.GetChild(1).gameObject.GetComponent<AudioSource>();
                AudioClip ligmaballs = assetBundleParent.transform.GetChild(1).gameObject.GetComponent<AudioClip>();
                assetBundleParent.transform.position = Player.rightHandFollower.transform.position;
                assetBundleParent.transform.rotation = Player.rightHandFollower.transform.rotation;
                GameObject PenModel = assetBundleParent.transform.GetChild(0).gameObject;
                PenModel.GetComponent<MeshRenderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                PenModel.GetComponent<MeshRenderer>().material.color = penColor;
                DrawingParent.SetActive(true);



                if (drawButton)
                {
                    GameObject penTip = assetBundleParent.transform.GetChild(0).GetChild(0).gameObject;
                    GameObject drawCube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    drawCube.transform.position = penTip.transform.position;
                    drawCube.GetComponent<MeshRenderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                    drawCube.GetComponent<MeshRenderer>().material.color = penColor;
                    drawCube.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
                    drawCube.GetComponent<SphereCollider>().enabled = false;
                    drawCube.transform.SetParent(DrawingParent.transform);


                }

                if (colorButton)
                {
                    CurColornum++;
                    if (CurColornum > MaxColornum)
                    {

                        CurColornum = MinColornum;

                    }

                    if (CurColornum == 1)
                    {

                        penColor = Color.black;

                    }


                    if (CurColornum == 2)
                    {

                        penColor = Color.blue;

                    }


                    if (CurColornum == 3)
                    {

                        penColor = Color.red;

                    }

                    if (CurColornum == 4)
                    {

                        penColor = Color.yellow;

                    }


                    if (CurColornum == 5)
                    {

                        penColor = Color.cyan;

                    }
                    if (CurColornum == 6)
                    {

                        penColor = Color.magenta;

                    }
                    if (CurColornum == 7)
                    {

                        penColor = Color.white;

                    }
                    if (CurColornum == 8)
                    {

                        penColor = Color.HSVToRGB(274, 77, 70);

                    }


                }



            }
            else 
            {

                DrawingParent.SetActive(false);

            }


        }

        /* This attribute tells Utilla to call this method when a modded room is joined */

        public void OnJoin(string gamemode)
        {
            /* Activate your mod here */
            /* This code will run regardless of if the mod is enabled*/
            DrawingParent.SetActive(false);
            inRoom = true;
        }

        /* This attribute tells Utilla to call this method when a modded room is left */

        public void OnLeave(string gamemode)
        {
            /* Deactivate your mod here */
            /* This code will run regardless of if the mod is enabled*/
            inRoom = false;

        }

        public AssetBundle LoadAssetBundle(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            AssetBundle bundle = AssetBundle.LoadFromStream(stream);
            stream.Close();
            return bundle;


        }



    }
}
