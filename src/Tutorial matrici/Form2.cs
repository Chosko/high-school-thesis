using System;
using System.Drawing;
using System.Windows.Forms;
using XTeam.LogiX_Technologies;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Planetario;

namespace Applicazione_LogiX
{
    public partial class Form2 : Form
    {
        //dichiarazione delle variabili
        LogiX_Engine lxe = new LogiX_Engine();
        Camera cam = null;
        Mouse mouse = null;
        Keyboard keyboard = null;
        Pianeta Terra = null;
        Pianeta Venere = null;
        Pianeta Mercurio = null;
        Pianeta Marte = null;
        Pianeta Giove = null;
        Pianeta Saturno = null;
        Pianeta Urano = null;
        Pianeta Nettuno = null;
        Model mod = null;
        Satellite Luna = null;
        Satellite Phobos = null;
        Satellite Deimos = null;
        Satellite Io = null;
        Satellite Europa = null;
        Satellite Ganimede = null;
        Satellite Callisto = null;
        PrintScreen info = null;
        Sole Sole = null;
        CieloStellato Sky = null;
        PointLight sunlight = null;
        float time = 0f;
        float timeconst = 0;
        float timeSpeed = 0.04f;
        float fieldOfView = 1.2f*(float)Math.PI / 4;
        int CambiaPianeta = 3;
        Pianeta PianetaSelezionato = null;
        XTexture RingColor = null;
        XTexture RingPattern = null;
        bool hd1 = false;
        bool hd2 = false;
        bool hd3 = false;
        bool hd4 = false;
        bool hd5 = false;
        bool hd6 = false;
        bool hd7 = false;
        bool hd8 = false;
        bool hd9 = false;
        bool hd10 = false;
        bool hd11 = false;
        bool hd12 = false;
        bool hd13 = false;
        bool hd14 = false;
        bool hd15 = false;
        bool hdEnable = true;

        #region "Structure"

        public Form2()
        {
            
            this.ClientSize = new Size(LogiX_Engine.CurrentDisplayWitdh - 20, LogiX_Engine.CurrentDisplayHeight - 20);
            InitializeComponent();
        }

        //MAIN
        //Entry Point Of The Application
        static void Main()
        {
            try
            {
                using (Form2 frm = new Form2())
                {
                    frm.SetObjects();
                    frm.Show();
                    while (frm.Created)
                    {
                        frm.GameLoop();
                        Application.DoEvents();
                    }
                }
            }
            catch (DirectXException e)
            {
                MessageBox.Show("Errore nella Demo. L'Applicazione Sarà Terminata\n\n\n   Ulteriori informazioni:\n\n" + e.ErrorString + "\n\n"+ e.StackTrace);
                Application.Exit();
            }
        }

        //On Paint Event
        protected override void OnPaint(PaintEventArgs e)
        {
            GameLoop();
        }

        #endregion

        #region "Pressione dei tasti (Globale)"

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch ((int)(byte)e.KeyCode)
            {
                case (int)(byte)Keys.Escape:
                    this.Close();
                    Application.Exit();
                    break;
                case ((int)(byte)Keys.W):
                    lxe.SetRenderState(XTeam.LogiX_Technologies.RenderStates.Wireframe);
                    break;
                case ((int)(byte)Keys.S):
                    lxe.SetRenderState(XTeam.LogiX_Technologies.RenderStates.Solid);
                    break;
                case ((int)(byte)Keys.P):
                    lxe.SetRenderState(XTeam.LogiX_Technologies.RenderStates.Point);
                    break;
                case ((int)(byte)Keys.Space):
                    CambiaPianeta++;
                    i = -1;
                    j = -1;
                    k = 0;
                    stop = false;
                    break;
                case ((int)(byte)Keys.D1):
                    CambiaPianeta = 1;
                    i = -1;
                    j = -1;
                    k = 0;
                    stop = false;
                    break;
                case ((int)(byte)Keys.D2):
                    CambiaPianeta = 2;
                    i = -1;
                    j = -1;
                    k = 0;
                    stop = false;
                    break;
                case ((int)(byte)Keys.D3):
                    CambiaPianeta = 3;
                    i = -1;
                    j = -1;
                    k = 0;
                    stop = false;
                    break;
                case ((int)(byte)Keys.D4):
                    CambiaPianeta = 5;
                    i = -1;
                    j = -1;
                    k = 0;
                    stop = false;
                    break;
                case ((int)(byte)Keys.D5):
                    CambiaPianeta = 8;
                    i = -1;
                    j = -1;
                    k = 0;
                    stop = false;
                    break;
                case ((int)(byte)Keys.D6):
                    CambiaPianeta = 13;
                    i = -1;
                    j = -1;
                    k = 0;
                    stop = false;
                    break;
                case ((int)(byte)Keys.D7):
                    CambiaPianeta = 14;
                    i = -1;
                    j = -1;
                    k = 0;
                    stop = false;
                    break;
                case ((int)(byte)Keys.D8):
                    CambiaPianeta = 15;
                    i = -1;
                    j = -1;
                    k = 0;
                    stop = false;
                    break;
                case ((int)(byte)Keys.Add):
                    timeSpeed = timeSpeed + 0.001f;
                    break;
                case ((int)(byte)Keys.Subtract):
                    if (timeSpeed >= 0)
                        timeSpeed = timeSpeed - 0.001f;
                    break;
                case ((int)(byte)Keys.D0):
                        timeSpeed = 0;
                    break;
            }
        }
        #endregion

        #region "Gestione Camera"

        int i = -1;
        int j = -1;
        int k = -1;
        int l = -1;
        float I1;
        bool stop = false;
        float oldTimeSpeed;
        VertexData V1;
        VertexData V2;

        void APunto(VertexData a, int cicliLook, int cicliPos, float Distance)
        {
            if (timeSpeed !=0)
            {
                if (k == 0)
                {
                    stop = true;
                    I1 = timeSpeed / 20;
                    oldTimeSpeed = timeSpeed;
                    k++;
                }
                if (k >= 1)
                {
                    stop = true;
                    timeSpeed = timeSpeed - I1;
                    k++;
                }
                if (k == 20)
                {
                    timeSpeed = 0;
                    stop = true;
                    k = -1;
                    i = 0;
                    j = -1;
                    l = -1;
                }
            }
            else
            {
                if (k == 0)
                {
                    oldTimeSpeed = 0;
                    k = -1;
                    i = 0;
                    j = -1;
                    l = -1;
                }
            }
            if (i == 0)
            {
                stop = true;
                V1 = a - cam.Look;
                V2 = cam.Look - cam.Position;
                V1 = V1 / cicliLook;
                cam.Look = cam.Look + V1;
                i++;
            }
            if (i >= 1)
            {
                stop = true;
                cam.Look = cam.Look + V1;
                i++;
            }
            if (i == cicliLook)
            {
                stop = true;
                i = -1;
                j = 0;
                k = -1;
                l = -1;
            }
            if (j == 0)
            {
                stop = true;
                V1 = a - cam.Position - (VertexData.Normalize(V2)*Distance);
                V1 = V1 / cicliPos;
                cam.Position = cam.Position + V1;
                j++;
            }
            if (j >= 1)
            {
                stop = true;
                cam.Position = cam.Position + V1;
                j++;
            }
            if (j == cicliPos)
            {
                stop = true;
                j = -1;
                i = -1;
                k = -1;
                l = 0;
            }
            if (oldTimeSpeed != 0)
            {
                if (l == 0)
                {
                    stop = true;
                    I1 = oldTimeSpeed / 20;
                    l++;
                }
                if (l >= 1)
                {
                    stop = true;
                    timeSpeed = timeSpeed + I1;
                    l++;
                }
                if (l == 20)
                {
                    timeSpeed = oldTimeSpeed;
                    stop = false;
                    k = -1;
                    i = -1;
                    j = -1;
                    l = -1;
                }
            }
            else
            {
                if (l == 0)
                {
                    stop = false;
                    timeSpeed = 0;
                    k = -1;
                    i = -1;
                    j = -1;
                    l = -1;
                }
            }
        }

        void GuardaPunto(VertexData look)
        {
            mouse.CameraTrackBall(cam, look, 0.1f, 1000000f / 1f, cam.FieldOfView);
        }
        void FermaTempo()
        {

        }


        #endregion

        #region "Settaggio degli Oggetti"

        private void SetObjects()
        {
            //lxe.SetAutoEngine(this, true);
            //lxe.SetManualEngine(this, LogiX_Engine.CurrentDisplayHeight, LogiX_Engine.CurrentDisplayWitdh, LogiX_Engine.CurrentDisplayRefreshRate, Format.X8R8G8B8, 0, SwapEffect.Discard, DeviceType.Hardware, CreateFlags.SoftwareVertexProcessing, MultiSampleType.FourSamples);
            lxe.SetManualEngine(this, 0, SwapEffect.Discard, DeviceType.Hardware, CreateFlags.SoftwareVertexProcessing, MultiSampleType.FourSamples);

            cam = new Camera(new VertexData(0,0,-1000), VertexData.Empty, new VertexData(0,1,0),(float)Math.PI/4, 2.0f, 10000000000, 1);
            
            //cam.AspectRatio = (float)LogiX_Engine.CurrentDisplayWitdh / (float)LogiX_Engine.CurrentDisplayHeight;
            cam.AspectRatio = (float)this.ClientSize.Width / (float)this.ClientSize.Height;
            mouse = new Mouse(this);
            mouse.DistanceForCameraTrackBall = 7000;
            cam.FieldOfView = 2 * (float)Math.PI / 6;
            keyboard = new Keyboard(this);

            Sole = new Sole();
            sunlight = new PointLight(new VertexData(0, 0, 0), Color.FromArgb(50, 50, 50), Color.FromArgb(15,15,15), Color.OrangeRed, 10000000000000000000);
            sunlight.Attenuation0 = 1;
            lxe.SetRenderState(XTeam.LogiX_Technologies.RenderStates.Solid);
            Terra = new Pianeta(149.6f, 6370, 0.48f, 1, 365, new XTexture("MEDIA\\texterrahd.jpg"), "Terra");
            Venere = new Pianeta(108, 6052, 0, -117, 224.70059f, new XTexture("MEDIA\\venusmap.jpg"), "Venere");
            Mercurio = new Pianeta(57.909f, 2440, 0, 58.6462f, 87.969f, new XTexture("MEDIA\\mercury.jpg"), "Mercurio");
            Marte = new Pianeta(228, 3392.8f, 0, 1.025957f, 686.979f, new XTexture("MEDIA\\mars.jpg"), "Marte");
            Giove = new Pianeta(778.412f, 71492, 0, 0.413538021f, 4333.2867f, new XTexture("MEDIA\\jupiter.jpg"), "Giove");
            Saturno = new Pianeta(1426.725413f, 60268, 0.47f, 0.449375f, 10756.1995f, new XTexture("MEDIA\\saturn.jpg"), "Saturno");
            Urano = new Pianeta(2870.972220f, 25559, 1.71f, -0.71875f, 30685.55f, new XTexture("MEDIA\\uranus.jpg"), "Urano");
            Nettuno = new Pianeta(4498.252900f, 24764, 0.49f, 0.67125f, 60223.3528f,new XTexture("MEDIA\\neptune.jpg"), "Nettuno");
            Luna = new Satellite(Terra, 384.400f, 1738, 0, 28, 28, new XTexture("MEDIA\\texlunahd.jpg"), "Luna");
            Phobos = new Satellite(Marte, 9.377f, 200, 0, 1, 0.2916666f, new XTexture("MEDIA\\texphobos.jpg"), "Phobos", new Model("MEDIA\\phobos.x", 0));
            Deimos = new Satellite(Marte, 23.460f, 200, 0, 0.31891023f, 0.31891023f, new XTexture("MEDIA\\deimos.jpg"), "Deimos", new Model("MEDIA\\deimos.x", 0));
            Io = new Satellite(Giove, 421.700f, 1821.3f, 0, 1.769137786f, 1.769137786f, new XTexture("MEDIA\\io.jpg"), "Io");
            Europa = new Satellite(Giove, 671.034f, 1560.8f, 0, 3.551181041f, 3.551181041f, new XTexture("MEDIA\\europa.jpg"), "Europa");
            Ganimede = new Satellite(Giove, 1070.400f, 2631.2f, 0, 7.15455296f, 7.15455296f, new XTexture("MEDIA\\ganimede.jpg"), "Ganimede");
            Callisto = new Satellite(Giove, 1882.700f, 2410.3f, 0, 16.6890184f, 16.6890184f, new XTexture("MEDIA\\callisto.jpg"), "Ganimede");

            Sky = new CieloStellato();
            info = new PrintScreen("arial", 16);
            PianetaSelezionato = Terra;
            mod = new Model("MEDIA\\Sfera4.x", 0);
            RingColor = new XTexture("MEDIA\\saturnringcolor.jpg");
            RingPattern = new XTexture("MEDIA\\saturnringpattern.jpg");
            Saturno.CreaAnelli(RingColor, RingPattern);
            Urano.CreaAnelli(new XTexture("MEDIA\\uranusringcolour1.jpg"), new XTexture("MEDIA\\uranusringtrans.png"));

            if (!hdEnable)
            {
                hd1 = hd2 = hd3 = hd4 = hd5 = hd6 = hd7 = hd8 = hd9 = hd10 = hd11 = hd12 = hd13 = hd14 = hd15 = true;

            }
        }

        #endregion

        #region "Gameloop"

        #region "Struttura del Gameloop"
        private void GameLoop()
        {
            lxe.ClearFrame(Color.White);
            lxe.SetLightMode(true, false);
            lxe.SetCullMode(CullMode.CounterClockwise);
            lxe.StartRender();

            OperazioniWriting();

            /*mod.Position = cam.Look;
            mod.Scaling = new VertexData(1, 1, 1) * 0.01f;
            mod.RenderMe(new XMaterial(Color.Blue, Color.Red, Color.Yellow, 25), 0);            
            */
    
            KeyPressed();

            OperazioniRendering();

            cam.FieldOfView = fieldOfView * fieldOfView * fieldOfView;
            cam.Capture();

            time = time + timeSpeed * timeSpeed;
            timeconst = timeconst + 0.02f;

            OperazioniSelezionePianeta();


            lxe.EndRender();
        }

        #endregion

        #region "Operazioni di Scrittura a Schermo"

        private void OperazioniWriting()
        {
            //LogiX_Engine.PrintErrors();
            info.Write(PianetaSelezionato.ToString(), 0, 0, Color.Green);
            //info.Write(time.ToString(), 0, 35, Color.Green);
            //info.Write(timeSpeed.ToString(), 0, 50, Color.Green);
            //info.Write(mouse.DistanceForCameraTrackBall.ToString(), 0, 65, Color.Green);
        }

        #endregion

        #region "Operazioni di Selezione dei Pianeti"

        private void OperazioniSelezionePianeta()
        {
            mouse.Acquire();

            switch (CambiaPianeta)
            {
                case 1:
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 6000;
                        hd1 = true;
                    }
                    APunto(Mercurio.position, 40, 60, -mouse.DistanceLookPosition);
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 6000;
                        PianetaSelezionato = Mercurio;
                    }
                    break;
                case 2:
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 7000;
                        hd2 = true;
                    }
                    APunto(Venere.position, 40, 60, -mouse.DistanceLookPosition);
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 7000;
                        PianetaSelezionato = Venere;
                    }
                    break;
                case 3:
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 7000;
                        hd3 = true;
                    }
                    APunto(Terra.position, 40, 60, -mouse.DistanceLookPosition);
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 7000;
                        PianetaSelezionato = Terra;
                    }
                    break;
                case 4:
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 5000;
                        hd4 = true;
                    }
                    APunto(Luna.position, 40, 60, -mouse.DistanceLookPosition);
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 5000;
                        PianetaSelezionato = Terra;
                    }
                    break;
                case 5:
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 7000;
                        hd5 = true;
                    }
                    APunto(Marte.position, 40, 60, -mouse.DistanceLookPosition);
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 7000;
                        PianetaSelezionato = Marte;
                    }
                    break;
                case 6:
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 3500;
                        hd6 = false;
                    }
                    APunto(Phobos.position, 40, 60, -mouse.DistanceLookPosition);
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 3500;
                        PianetaSelezionato = Marte;
                    }
                    break;
                case 7:
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 4000;
                        hd7 = false;
                    }
                    APunto(Deimos.position, 40, 60, -mouse.DistanceLookPosition);
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 4000;
                        PianetaSelezionato = Marte;
                    }
                    break;
                case 8:
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 12000;
                        hd8 = true;
                    }
                    APunto(Giove.position, 40, 60, -mouse.DistanceLookPosition);
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 12000;
                        PianetaSelezionato = Giove;
                    }
                    break;
                case 9:
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 6000;
                        hd9 = true;
                    }
                    APunto(Io.position, 40, 60, -mouse.DistanceLookPosition);
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 6000;
                        PianetaSelezionato = Giove;
                    }
                    break;
                case 10:
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 6000;
                        hd10 = true;
                    }
                    APunto(Europa.position, 40, 60, -mouse.DistanceLookPosition);
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 6000;
                        PianetaSelezionato = Giove;
                    }
                    break;
                case 11:
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 6000;
                        hd11 = true;
                    }
                    APunto(Ganimede.position, 40, 60, -mouse.DistanceLookPosition);
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 6000;
                        PianetaSelezionato = Giove;
                    }
                    break;
                case 12:
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 6000;
                        hd12 = true;
                    }
                    APunto(Callisto.position, 40, 60, -mouse.DistanceLookPosition);
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 6000;
                        PianetaSelezionato = Giove;
                    }
                    break;
                case 13:
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 12000;
                        hd13 = true;
                    }
                    APunto(Saturno.position, 40, 60, -mouse.DistanceLookPosition);
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 12000;
                        PianetaSelezionato = Saturno;
                    }
                    break;
                case 14:
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 12000;
                        hd14 = true;
                    }
                    APunto(Urano.position, 40, 60, -mouse.DistanceLookPosition);
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 12000;
                        PianetaSelezionato = Urano;
                    }
                    break;
                case 15:
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 10000;
                        hd15 = true;
                    }
                    APunto(Nettuno.position, 40, 60, -mouse.DistanceLookPosition);
                    if (stop == true)
                    {
                        mouse.DistanceForCameraTrackBall = 10000;
                        PianetaSelezionato = Nettuno;
                    }
                    break;
                case 16:
                    CambiaPianeta = 15;
                    break;
            }

            if (stop == false)
            {
                switch (CambiaPianeta)
                {
                    case 1:
                        if (hdEnable)
                        {
                            hd1 = true;
                            hd2 = false;
                            hd3 = false;
                            hd4 = false;
                            hd5 = false;
                            hd6 = false;
                            hd7 = false;
                            hd8 = false;
                            hd9 = false;
                            hd10 = false;
                            hd11 = false;
                            hd12 = false;
                            hd13 = false;
                            hd14 = false;
                            hd15 = false;
                        }
                        GuardaPunto(Mercurio.position);
                        PianetaSelezionato = Mercurio;
                        cam.NearPlane = 1.5f;
                        break;
                    case 2:
                        if (hdEnable)
                        {
                            hd1 = false;
                            hd2 = true;
                            hd3 = false;
                            hd4 = false;
                            hd5 = false;
                            hd6 = false;
                            hd7 = false;
                            hd8 = false;
                            hd9 = false;
                            hd10 = false;
                            hd11 = false;
                            hd12 = false;
                            hd13 = false;
                            hd14 = false;
                            hd15 = false;
                        }
                        GuardaPunto(Venere.position);
                        PianetaSelezionato = Venere;
                        cam.NearPlane = 2;
                        break;
                    case 3:
                        if (hdEnable)
                        {
                            hd1 = false;
                            hd2 = false;
                            hd3 = true;
                            hd4 = true;
                            hd5 = false;
                            hd6 = false;
                            hd7 = false;
                            hd8 = false;
                            hd9 = false;
                            hd10 = false;
                            hd11 = false;
                            hd12 = false;
                            hd13 = false;
                            hd14 = false;
                            hd15 = false;
                        }
                        GuardaPunto(Terra.position);
                        PianetaSelezionato = Terra;
                        cam.NearPlane = 2;
                        break;
                    case 4:
                        if (hdEnable)
                        {
                            hd1 = false;
                            hd2 = false;
                            hd3 = true;
                            hd4 = true;
                            hd5 = false;
                            hd6 = false;
                            hd7 = false;
                            hd8 = false;
                            hd9 = false;
                            hd10 = false;
                            hd11 = false;
                            hd12 = false;
                            hd13 = false;
                            hd14 = false;
                            hd15 = false;
                        }
                        GuardaPunto(Luna.position);
                        PianetaSelezionato = Terra;
                        hd4 = true;
                        cam.NearPlane = 1;
                        break;
                    case 5:
                        GuardaPunto(Marte.position);
                        PianetaSelezionato = Marte;
                        cam.NearPlane = 2;
                        if (hdEnable)
                        {
                            hd1 = false;
                            hd2 = false;
                            hd3 = false;
                            hd4 = false;
                            hd5 = true;
                            hd6 = false;
                            hd7 = false;
                            hd8 = false;
                            hd9 = false;
                            hd10 = false;
                            hd11 = false;
                            hd12 = false;
                            hd13 = false;
                            hd14 = false;
                            hd15 = false;
                        }
                        break;
                    case 6:
                        if (hdEnable)
                        {
                            hd1 = false;
                            hd2 = false;
                            hd3 = false;
                            hd4 = false;
                            hd5 = true;
                            hd6 = false;
                            hd7 = false;
                            hd8 = false;
                            hd9 = false;
                            hd10 = false;
                            hd11 = false;
                            hd12 = false;
                            hd13 = false;
                            hd14 = false;
                            hd15 = false;
                        }
                        GuardaPunto(Phobos.position);
                        PianetaSelezionato = Marte;
                        cam.NearPlane = 0.05f;
                        break;
                    case 7:
                        if (hdEnable)
                        {
                            hd1 = false;
                            hd2 = false;
                            hd3 = false;
                            hd4 = false;
                            hd5 = true;
                            hd6 = false;
                            hd7 = false;
                            hd8 = false;
                            hd9 = false;
                            hd10 = false;
                            hd11 = false;
                            hd12 = false;
                            hd13 = false;
                            hd14 = false;
                            hd15 = false;
                        }
                        GuardaPunto(Deimos.position);
                        PianetaSelezionato = Marte;
                        cam.NearPlane = 0.05f;
                        break;
                    case 8:
                        if (hdEnable)
                        {
                            hd1 = false;
                            hd2 = false;
                            hd3 = false;
                            hd4 = false;
                            hd5 = false;
                            hd6 = false;
                            hd7 = false;
                            hd8 = true;
                            hd9 = true;
                            hd10 = true;
                            hd11 = true;
                            hd12 = true;
                            hd13 = false;
                            hd14 = false;
                            hd15 = false;
                        }
                        GuardaPunto(Giove.position);
                        PianetaSelezionato = Giove;
                        cam.NearPlane = 10;
                        break;
                    case 9:
                        if (hdEnable)
                        {
                            hd1 = false;
                            hd2 = false;
                            hd3 = false;
                            hd4 = false;
                            hd5 = false;
                            hd6 = false;
                            hd7 = false;
                            hd8 = true;
                            hd9 = true;
                            hd10 = true;
                            hd11 = true;
                            hd12 = true;
                            hd13 = false;
                            hd14 = false;
                            hd15 = false;
                        }
                        GuardaPunto(Io.position);
                        PianetaSelezionato = Giove;
                        cam.NearPlane = 2;
                        break;
                    case 10:
                        if (hdEnable)
                        {
                            hd1 = false;
                            hd2 = false;
                            hd3 = false;
                            hd4 = false;
                            hd5 = false;
                            hd6 = false;
                            hd7 = false;
                            hd8 = true;
                            hd9 = true;
                            hd10 = true;
                            hd11 = true;
                            hd12 = true;
                            hd13 = false;
                            hd14 = false;
                            hd15 = false;
                        }
                        GuardaPunto(Europa.position);
                        PianetaSelezionato = Giove;
                        cam.NearPlane = 2;
                        break;
                    case 11:
                        if (hdEnable)
                        {
                            hd1 = false;
                            hd2 = false;
                            hd3 = false;
                            hd4 = false;
                            hd5 = false;
                            hd6 = false;
                            hd7 = false;
                            hd8 = true;
                            hd9 = true;
                            hd10 = true;
                            hd11 = true;
                            hd12 = true;
                            hd13 = false;
                            hd14 = false;
                            hd15 = false;
                        }
                        GuardaPunto(Ganimede.position);
                        PianetaSelezionato = Giove;
                        cam.NearPlane = 2;
                        break;
                    case 12:
                        if (hdEnable)
                        {
                            hd1 = false;
                            hd2 = false;
                            hd3 = false;
                            hd4 = false;
                            hd5 = false;
                            hd6 = false;
                            hd7 = false;
                            hd8 = true;
                            hd9 = true;
                            hd10 = true;
                            hd11 = true;
                            hd12 = true;
                            hd13 = false;
                            hd14 = false;
                            hd15 = false;
                        }
                        GuardaPunto(Callisto.position);
                        PianetaSelezionato = Giove;
                        cam.NearPlane = 2;
                        break;
                    case 13:
                        if (hdEnable)
                        {
                            hd1 = false;
                            hd2 = false;
                            hd3 = false;
                            hd4 = false;
                            hd5 = false;
                            hd6 = false;
                            hd7 = false;
                            hd8 = false;
                            hd9 = false;
                            hd10 = false;
                            hd11 = false;
                            hd12 = false;
                            hd13 = true;
                            hd14 = false;
                            hd15 = false;
                        }
                        GuardaPunto(Saturno.position);
                        PianetaSelezionato = Saturno;
                        cam.NearPlane = 10;
                        break;
                    case 14:
                        if (hdEnable)
                        {
                            hd1 = false;
                            hd2 = false;
                            hd3 = false;
                            hd4 = false;
                            hd5 = false;
                            hd6 = false;
                            hd7 = false;
                            hd8 = false;
                            hd9 = false;
                            hd10 = false;
                            hd11 = false;
                            hd12 = false;
                            hd13 = false;
                            hd14 = true;
                            hd15 = false;
                        }
                        GuardaPunto(Urano.position);
                        PianetaSelezionato = Urano;
                        cam.NearPlane = 10;
                        break;
                    case 15:
                        if (hdEnable)
                        {
                            hd1 = false;
                            hd2 = false;
                            hd3 = false;
                            hd4 = false;
                            hd5 = false;
                            hd6 = false;
                            hd7 = false;
                            hd8 = false;
                            hd9 = false;
                            hd10 = false;
                            hd11 = false;
                            hd12 = false;
                            hd13 = false;
                            hd14 = false;
                            hd15 = true;
                        }
                        GuardaPunto(Nettuno.position);
                        PianetaSelezionato = Nettuno;
                        cam.NearPlane = 13;
                        break;
                    case 16:
                        CambiaPianeta = 15;
                        break;
                }
            }
        }
        #endregion

        #region "Operazioni di Rendering"

        private void OperazioniRendering()
        {
            sunlight.Enabled = false;
            sunlight.SetPointLight();
            lxe.SetFilterQuality(FilterQuality.Anisotropic);

            Sky.RenderizzaCieloStellato(timeconst, lxe, 1f / (fieldOfView * fieldOfView * fieldOfView));

            Sole.RenderizzaSole(timeconst, 1f / (fieldOfView * fieldOfView));
            Sole.RenderizzaAlone(lxe, cam);

            sunlight.Enabled = true;
            sunlight.SetPointLight();

            Mercurio.RenderizzaPianeta(time, lxe, hd1);

            Venere.RenderizzaPianeta(time, lxe, hd2);
            Venere.RenderizzaAtmosfera(time, 1.06f, new XMaterial(Color.FromArgb(60, 40, 20), Color.FromArgb(200, 160, 120), Color.FromArgb(0, 0, 0), 25), lxe, hd2);

            Terra.RenderizzaPianeta(time, lxe, hd3);
            Terra.RenderizzaAtmosfera(time, 1.02f, new XMaterial(Color.FromArgb(0, 20, 80), Color.Cyan, Color.FromArgb(0, 0, 0), 25), lxe, hd3);
            Luna.RenderizzaSatellite(time, lxe, hd4);

            Marte.RenderizzaPianeta(time, lxe, hd5);
            Phobos.RenderizzaSatellite(time, lxe, hd6);
            Deimos.RenderizzaSatellite(time, lxe, hd7);

            sunlight.Diffuse = Color.FromArgb(130, 130, 130);
            sunlight.SetPointLight();
            Giove.RenderizzaPianeta(time, lxe, hd8);
            sunlight.Diffuse = Color.FromArgb(15, 15, 15);
            sunlight.SetPointLight();
            Io.RenderizzaSatellite(time, lxe, hd9);
            Europa.RenderizzaSatellite(time, lxe,hd10);
            Ganimede.RenderizzaSatellite(time, lxe,hd11);
            Callisto.RenderizzaSatellite(time, lxe, hd12);

            sunlight.Diffuse = Color.FromArgb(130, 130, 130);
            sunlight.SetPointLight();
            Saturno.RenderizzaPianeta(time, lxe, hd13);
            lxe.SetAlphaMode(BlendOperation.Add, Blend.SourceAlpha);
            Saturno.RenderizzaAnelli(time, 0.25f);
            lxe.UnSetAlphaMode();
            sunlight.Diffuse = Color.FromArgb(15, 15, 15);
            sunlight.SetPointLight();

            sunlight.Diffuse = Color.FromArgb(130, 130, 130);
            sunlight.SetPointLight();
            Urano.RenderizzaPianeta(time, lxe, hd14);
            lxe.SetAlphaMode(BlendOperation.Add, Blend.SourceAlpha);
            Urano.RenderizzaAnelli(time, 0.25f);
            lxe.UnSetAlphaMode();
            sunlight.Diffuse = Color.FromArgb(15, 15, 15);
            sunlight.SetPointLight();

            sunlight.Diffuse = Color.FromArgb(130, 130, 130);
            sunlight.SetPointLight();
            Nettuno.RenderizzaPianeta(time, lxe, hd15);
            sunlight.Diffuse = Color.FromArgb(15, 15, 15);
            sunlight.SetPointLight();
        }

        #endregion

        #region "Gestione dei Tasti"

        private void KeyPressed()
        {
            if (keyboard.KeyboardState[Microsoft.DirectX.DirectInput.Key.A])
            {
                fieldOfView = fieldOfView + 0.002f;
            }
            if (keyboard.KeyboardState[Microsoft.DirectX.DirectInput.Key.Z])
            {
                fieldOfView = fieldOfView - 0.002f;
            }
        }
        #endregion

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        #endregion
    }
}