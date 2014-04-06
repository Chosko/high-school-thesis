using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;

//LogiX Engine v 0.1
//By XTeam


namespace XTeam.LogiX_Technologies
{

    #region "DevX"

    /// <summary>
    /// Classe madre.
    /// Contiene le variabili utili a tutte le classi.
    /// </summary>
    public class DevX
    {
        //Variabili di stato primarie
        static protected Microsoft.DirectX.Direct3D.Device device;
        static protected Microsoft.DirectX.Direct3D.Device device0;
        protected PresentParameters my_params_x = new PresentParameters();
        static protected int LightCount = -1;

        #region "Errors"

        /// <summary>
        /// Classe LXE ERRORS
        /// Rileva gli errori di tutte le classi e li localizza.
        /// </summary>
        protected static class LXE_Errors
        {
            static string allErrors = "No Errors";
            static int count = 0;

            public static void AddError(string ObjectType, string ErrorName)
            {
                if (count == 0)
                {
                    allErrors = "Error in " + ObjectType + ": " + ErrorName;
                }
                else
                {
                    allErrors = allErrors + "; Error in " + ObjectType + ": " + ErrorName;
                }
                count++;
            }

            public static void ResetErrors()
            {
                allErrors = "";
            }

            static public string AllErrors
            {
                get { return allErrors; }
            }
        }

        #endregion
    }

    #endregion

    #region "LogiX_Engine"

    /// <summary>
    /// Contiene le funzioni di base per la creazione e l'impostazione di un ambiente 3D
    /// </summary>
    public class LogiX_Engine : DevX
    {
        #region "Device"

        /// <summary>
        /// Settaggio manuale di un Device windowed
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="BackBufferCount"></param>
        /// <param name="SwapEffect"></param>
        /// <param name="DeviceType"></param>
        /// <param name="CreateFlags"></param>
        /// <returns></returns>
        public bool SetManualEngine(Control handle, int BackBufferCount, SwapEffect SwapEffect, Microsoft.DirectX.Direct3D.DeviceType DeviceType, CreateFlags CreateFlags, MultiSampleType Multisample)
        {
            try
            {
                my_params_x.Windowed = true;
                my_params_x.EnableAutoDepthStencil = true;
                my_params_x.AutoDepthStencilFormat = DepthFormat.D16;
                my_params_x.BackBufferCount = 1;
                my_params_x.SwapEffect = SwapEffect;
                my_params_x.MultiSample = Multisample;
                device = new Microsoft.DirectX.Direct3D.Device(0, DeviceType, handle, CreateFlags, my_params_x);
                errorsToPrint = new PrintScreen("arial", 12);
                return true;
            }
            catch (DirectXException)
            {
                errorsToPrint = new PrintScreen("arial", 12);
                return false;
            }
        }

        /// <summary>
        /// Settaggio manuale di un Device fullscreen
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="BackBufferHeight"></param>
        /// <param name="BackBufferWidth"></param>
        /// <param name="RefreshRateInHz"></param>
        /// <param name="BackBufferFormat"></param>
        /// <param name="BackBufferCount"></param>
        /// <param name="SwapEffect"></param>
        /// <param name="DeviceType"></param>
        /// <param name="CreateFlags"></param>
        /// <param name="MultiSample"></param>
        /// <returns></returns>
        public bool SetManualEngine(Control handle, int BackBufferHeight, int BackBufferWidth, int RefreshRateInHz, Format BackBufferFormat, int BackBufferCount, SwapEffect SwapEffect, Microsoft.DirectX.Direct3D.DeviceType DeviceType, CreateFlags CreateFlags, MultiSampleType MultiSample)
        {
            try
            {
                my_params_x.Windowed = false;
                my_params_x.BackBufferHeight = BackBufferHeight;
                my_params_x.BackBufferWidth = BackBufferWidth;
                my_params_x.BackBufferFormat = BackBufferFormat;
                my_params_x.FullScreenRefreshRateInHz = RefreshRateInHz;
                my_params_x.MultiSample = MultiSample;
                my_params_x.EnableAutoDepthStencil = true;
                my_params_x.AutoDepthStencilFormat = DepthFormat.D16;
                my_params_x.BackBufferCount = 1;
                my_params_x.SwapEffect = SwapEffect;
                device = new Microsoft.DirectX.Direct3D.Device(0, DeviceType, handle, CreateFlags, my_params_x);
                errorsToPrint = new PrintScreen("arial", 12);
                return true;
            }
            catch (DirectXException)
            {
                errorsToPrint = new PrintScreen("arial", 12);
                return false;
            }
        }

        /// <summary>
        /// Settaggio automatico di un Device
        /// </summary>
        /// <param name="Handle"></param>
        /// <param name="Windowed"></param>
        /// <returns></returns>
        public bool SetAutoEngine(Control Handle, bool Windowed)
        {
            try
            {
                if (!Windowed)
                {
                    my_params_x.Windowed = false;
                    my_params_x.BackBufferHeight = CurrentDisplayHeight;
                    my_params_x.BackBufferWidth = CurrentDisplayWitdh;
                    my_params_x.BackBufferFormat = CurrentDisplayFormat;
                    my_params_x.FullScreenRefreshRateInHz = CurrentDisplayRefreshRate;
                    my_params_x.MultiSample = MultiSampleType.TwoSamples;
                    my_params_x.EnableAutoDepthStencil = true;
                    my_params_x.AutoDepthStencilFormat = DepthFormat.D16;
                    my_params_x.BackBufferCount = 1;
                    my_params_x.SwapEffect = SwapEffect.Discard;
                    device = new Microsoft.DirectX.Direct3D.Device(0, Microsoft.DirectX.Direct3D.DeviceType.Hardware, Handle, CreateFlags.SoftwareVertexProcessing, my_params_x);
                }
                else
                {
                    my_params_x.Windowed = true;
                    my_params_x.BackBufferFormat = CurrentDisplayFormat;
                    my_params_x.MultiSample = MultiSampleType.TwoSamples;
                    my_params_x.EnableAutoDepthStencil = true;
                    my_params_x.AutoDepthStencilFormat = DepthFormat.D16;
                    my_params_x.BackBufferCount = 1;
                    my_params_x.SwapEffect = SwapEffect.Discard;
                    device = new Microsoft.DirectX.Direct3D.Device(0, Microsoft.DirectX.Direct3D.DeviceType.Hardware, Handle, CreateFlags.SoftwareVertexProcessing, my_params_x);
                }
                errorsToPrint = new PrintScreen("arial", 12);
                return true;
            }
            catch (DirectXException)
            {
                errorsToPrint = new PrintScreen("arial", 12);
                return false;
            }
        }

        /// <summary>
        /// Ritorna il nome della scheda grafica
        /// </summary>
        public static string VideoCardName
        {
            get { return Microsoft.DirectX.Direct3D.Manager.Adapters.Default.Information.Description; }
        }

        /// <summary>
        /// Ritorna la versione dei driver della scheda grafica
        /// </summary>
        public static string VideoCardDriverVersion
        {
            get { return Microsoft.DirectX.Direct3D.Manager.Adapters.Default.Information.DriverVersion.ToString(); }
        }

        /// <summary>
        /// Ritorna la modalità attuale dello schermo
        /// </summary>
        public static DisplayMode CurrentDisplayMode
        {
            get { return Microsoft.DirectX.Direct3D.Manager.Adapters.Default.CurrentDisplayMode; }
        }

        /// <summary>
        /// Ritorna il formato attuale dello schermo
        /// </summary>
        public static Format CurrentDisplayFormat
        {
            get { return Microsoft.DirectX.Direct3D.Manager.Adapters.Default.CurrentDisplayMode.Format; }
        }

        /// <summary>
        /// Ritorna la lunghezza attuale dello schermo, in pixels
        /// </summary>
        public static int CurrentDisplayWitdh
        {
            get { return Microsoft.DirectX.Direct3D.Manager.Adapters.Default.CurrentDisplayMode.Width; }
        }

        /// <summary>
        /// Ritorna l'altezza attuale dello schermo, in pixels
        /// </summary>
        public static int CurrentDisplayHeight
        {
            get { return Microsoft.DirectX.Direct3D.Manager.Adapters.Default.CurrentDisplayMode.Height; }
        }

        /// <summary>
        /// Ritorna la frequenza attuale di refresh, in Hertz
        /// </summary>
        public static int CurrentDisplayRefreshRate
        {
            get { return Microsoft.DirectX.Direct3D.Manager.Adapters.Default.CurrentDisplayMode.RefreshRate; }
        }

        /// <summary>
        /// Ritorna o imposta il Device in uso
        /// </summary>
        public static Microsoft.DirectX.Direct3D.Device Device
        {
            get { return device; }
            set { device = value; }
        }

        /// <summary>
        /// Ritorna o imposta il Device secondario
        /// </summary>
        public static Microsoft.DirectX.Direct3D.Device Device0
        {
            get { return device0; }
            set { device0 = value; }
        }

        #endregion

        #region "Render"

        //FUNZIONI DI RENDERING STANDARD

        /// <summary>
        /// Inizializza un frame
        /// </summary>
        /// <returns></returns>
        public bool StartRender()
        {
            try
            {
                device.BeginScene();
                return true;
            }
            catch
            {
                return true;
            }

        }

        /// <summary>
        /// Termina un frame
        /// </summary>
        /// <returns></returns>
        public bool EndRender()
        {
            try
            {
                device.EndScene();
                device.Present();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Pulisce lo schermo
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public bool ClearFrame(Color color)
        {
            try
            {
                if (device.RenderState.ZBufferEnable == true)
                {
                    device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, color, 1.0f, 0);
                }
                else
                {
                    device.Clear(ClearFlags.Target, color, 1.0f, 0);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Imposta il senso orario o antiorario di rendering delle facce dei poligoni.
        /// </summary>
        /// <param name="Cullmode"></param>
        /// <returns></returns>
        public bool SetCullMode(CullMode Cullmode)
        {
            try
            {
                switch (Cullmode)
                {
                    case CullMode.None:
                        device.RenderState.CullMode = Cull.None;
                        return true;
                    case CullMode.Clockwise:
                        device.RenderState.CullMode = Cull.Clockwise;
                        return true;
                    case CullMode.CounterClockwise:
                        device.RenderState.CullMode = Cull.CounterClockwise;
                        return true;
                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// Attiva o disattiva le modalità lighting e specular
        /// </summary>
        /// <param name="Lighting"></param>
        /// <param name="SpecularEnabled"></param>
        /// <returns></returns>
        public bool SetLightMode(bool Lighting, bool SpecularEnabled)
        {
            try
            {
                device.RenderState.SpecularEnable = SpecularEnabled;
                device.RenderState.Lighting = Lighting;
                device.RenderState.ZBufferEnable = true;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Imposta il tipo di riempimento dei poligoni.
        /// </summary>
        /// <param name="RenderState"></param>
        /// <returns></returns>
        public bool SetRenderState(RenderStates RenderState)
        {
            switch (RenderState)
            {
                case RenderStates.Wireframe:
                    device.RenderState.FillMode = FillMode.WireFrame;
                    return true;
                case RenderStates.Solid:
                    device.RenderState.FillMode = FillMode.Solid;
                    return true;
                case RenderStates.Point:
                    device.RenderState.FillMode = FillMode.Point;
                    return true;
                default:
                    return false;

            }
        }

        /// <summary>
        /// Inizializza l'effetto di Alpha Blending
        /// </summary>
        /// <returns></returns>
        public bool SetAlphaMode()
        {
            try
            {
                device.RenderState.AlphaBlendEnable = true;
                device.RenderState.BlendOperation = BlendOperation.Add;
                device.RenderState.SourceBlend = Blend.SourceColor;
                device.RenderState.DestinationBlend = Blend.InvDestinationColor;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SetAlphaMode(BlendOperation Operation, Blend Blend)
        {
            try
            {
                device.RenderState.AlphaBlendEnable = true;
                device.RenderState.BlendOperation = Operation;
                device.RenderState.SourceBlend = Blend;
                device.RenderState.DestinationBlend = Blend;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Termina l'effetto di Alpha Blending
        /// </summary>
        /// <returns></returns>
        public bool UnSetAlphaMode()
        {
            try
            {
                device.RenderState.AlphaBlendEnable = false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Ritorna o imposta la luce ambientale
        /// </summary>
        public Color AmbientLight
        {
            get { return device.RenderState.Ambient; }
            set { device.RenderState.Ambient = value; }
        }

        /// <summary>
        /// Imposta i filtri per le textures
        /// </summary>
        /// <param name="quality"></param>
        /// <returns></returns>
        public bool SetFilterQuality(FilterQuality quality)
        {
            switch (quality)
            {
                case FilterQuality.None:
                    device.SamplerState[0].MinFilter = TextureFilter.None;
                    device.SamplerState[0].MagFilter = TextureFilter.None;
                    return true;
                case FilterQuality.Point:
                    device.SamplerState[0].MinFilter = TextureFilter.Point;
                    device.SamplerState[0].MagFilter = TextureFilter.Point;
                    return true;
                case FilterQuality.Linear:
                    device.SamplerState[0].MinFilter = TextureFilter.Linear;
                    device.SamplerState[0].MagFilter = TextureFilter.Linear;
                    return true;
                case FilterQuality.Anisotropic:
                    device.SamplerState[0].MinFilter = TextureFilter.Anisotropic;
                    device.SamplerState[0].MagFilter = TextureFilter.Anisotropic;
                    return true;
                case FilterQuality.PyramidalQuad:
                    device.SamplerState[0].MinFilter = TextureFilter.PyramidalQuad;
                    device.SamplerState[0].MagFilter = TextureFilter.PyramidalQuad;
                    return true;
                case FilterQuality.GaussianQuad:
                    device.SamplerState[0].MinFilter = TextureFilter.GaussianQuad;
                    device.SamplerState[0].MagFilter = TextureFilter.GaussianQuad;
                    return true;
                default:
                    return false;
            }
        }


        #endregion

        #region "Errors"

        static PrintScreen errorsToPrint;

        /// <summary>
        /// Ritorna gli errori controllati dal LogiX Engine
        /// </summary>
        static public string Errors
        {
            get { return LXE_Errors.AllErrors; }
        }

        /// <summary>
        /// Stampa a schermo gli errori rilevati dal LogiX Engine
        /// </summary>
        /// <param name="Color"></param>
        /// <param name="XPosition"></param>
        /// <param name="YPosition"></param>
        static public void PrintErrors(Color Color, int XPosition, int YPosition)
        {
            errorsToPrint.Write(Errors, XPosition, YPosition, Color);
        }

        /// <summary>
        /// Stampa a schermo gli errori rilevati dal LogiX Engine
        /// </summary>
        static public void PrintErrors()
        {
            errorsToPrint.Write(Errors, 0, 0, Color.Red);
        }

        #endregion
    }

    #endregion

    #region "Enum"

    #region "FilterQuality"

    /// <summary>
    /// Modalità di filtraggio textures
    /// </summary>
    public enum FilterQuality
    {
        /// <summary>
        /// Nessun filtro applicato
        /// </summary>
        None = 0,

        /// <summary>
        /// Filtro point
        /// </summary>
        Point = 1,

        /// <summary>
        /// Filtro lineare
        /// </summary>
        Linear = 2,

        /// <summary>
        /// Filtro anisotropico
        /// </summary>
        Anisotropic = 3,

        /// <summary>
        /// Fitltro piramidale quadrato
        /// </summary>
        PyramidalQuad = 4,

        /// <summary>
        /// Filtro gaussiano quadrato
        /// </summary>
        GaussianQuad = 5,
    }

    #endregion

    #region "RenderStates"

    /// <summary>
    /// Modalità di riempimento dei poligoni
    /// </summary>
    public enum RenderStates
    {
        /// <summary>
        /// Riempimento delle facce
        /// </summary>
        Solid = 0,

        /// <summary>
        /// Riempimento degli spigoli
        /// </summary>
        Wireframe = 1,

        /// <summary>
        /// Riempimento dei vertici
        /// </summary>
        Point = 2,
    }

    #endregion

    #region "CullMode"

    /// <summary>
    /// Modalità di riempimento delle facce dei poligoni 
    /// </summary>
    public enum CullMode
    {
        /// <summary>
        /// Riempimento di entrambe le facce dei poligoni
        /// </summary>
        None = 0,

        /// <summary>
        /// Riempimento delle facce dei poligoni con i vertici ordinati in senso orario
        /// </summary>
        Clockwise = 1,

        /// <summary>
        /// Riempimento delle facce dei poligoni con i vertici ordinati in senso antiorario
        /// </summary>
        CounterClockwise = 2,
    }

    #endregion

    #region "XEffects"

    /// <summary>
    /// Tipi di effetti base
    /// </summary>
    public enum XEffects
    {
        /// <summary>
        /// Effetto di luce direzionale per-pixel
        /// </summary>
        PerPixelDirectionalLight = 0,

        /// <summary>
        /// Effetto di luce puntiforme per-pixel
        /// </summary>
        PerPixelPointLight = 1,

        /// <summary>
        /// Effetto di rilievo con luce putiforme per pixel
        /// </summary>
        BumpPerPixelPointLight = 2,

        /// <summary>
        /// Effetto di riflessione
        /// </summary>
        Reflect = 3,
    }



    #endregion

    #endregion

    #region "Camera"

    /// <summary>
    /// Definisce un oggetto camera per determinare la correzione prospettica e la proiezione su schermo dell'ambiente 3D
    /// </summary>
    public class Camera : DevX
    {
        private VertexData my_position;
        private VertexData my_look;
        private VertexData my_up;
        private float fieldView = 0;
        private float near = 0;
        private float far = 0;
        private float aspectratio = 0;
        private bool correct;

        /// <summary>
        /// Inizializza manualmente una camera
        /// </summary>
        /// <param name="Position"></param>
        /// <param name="TargetPoint"></param>
        /// <param name="UpVector"></param>
        /// <param name="FieldOfView"></param>
        /// <param name="NearPlane"></param>
        /// <param name="FarPlane"></param>
        /// <param name="AspectRatio"></param>
        public Camera(VertexData Position, VertexData TargetPoint, VertexData UpVector, float FieldOfView, float NearPlane, float FarPlane, float AspectRatio)
        {
            try
            {
                fieldView = FieldOfView;
                near = NearPlane;
                far = FarPlane;
                aspectratio = AspectRatio;
                my_position = Position;
                my_look = TargetPoint;
                my_up = UpVector;
                correct = true;
            }
            catch
            {
                Error("OnCreateObject");
            }
        }

        /// <summary>
        /// Inizializza automaticamente una camera
        /// </summary>
        /// <param name="Position"></param>
        /// <param name="TargetPoint"></param>
        public Camera(VertexData Position, VertexData TargetPoint)
        {
            try
            {
                my_position = Position;
                my_look = TargetPoint;
                correct = true;
            }
            catch
            {
                Error("OnCreateObject");
            }
        }

        /// <summary>
        /// Imposta la correzione prospettica e la proiezione a schermo del Device in uso
        /// </summary>
        /// <returns></returns>
        public bool Capture()
        {
            try
            {
                device.Transform.View = Matrix.LookAtLH(new Vector3(Position.X, Position.Y, Position.Z), new Vector3(Look.X, Look.Y, Look.Z), new Vector3(UpVector.X, UpVector.Y, UpVector.Z));
                device.Transform.Projection = Matrix.PerspectiveFovLH(FieldOfView, AspectRatio, NearPlane, FarPlane);
                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    Error("OnCapture");
                return false;
            }
        }

        /// <summary>
        /// Imposta la correzione prospettica e la proiezione a schermo di un Device
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public bool Capture(Microsoft.DirectX.Direct3D.Device device)
        {
            try
            {
                device.Transform.View = Matrix.LookAtLH(new Vector3(Position.X, Position.Y, Position.Z), new Vector3(Look.X, Look.Y, Look.Z), new Vector3(UpVector.X, UpVector.Y, UpVector.Z));
                device.Transform.Projection = Matrix.PerspectiveFovLH(FieldOfView, AspectRatio, NearPlane, FarPlane);
                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    Error("OnCapture");
                return false;
            }
        }

        /// <summary>
        /// Ritorna o imposta la posizione della camera
        /// </summary>
        public VertexData Position
        {
            get { return my_position; }
            set { my_position = value; }
        }

        /// <summary>
        /// Ritorna o imposta il punto di osservazione della camera
        /// </summary>
        public VertexData Look
        {
            get { return my_look; }
            set { my_look = value; }
        }

        /// <summary>
        /// Ritorna o imposta il verso "alto" della camera
        /// </summary>
        public VertexData UpVector
        {
            get
            {
                if (my_up == null) my_up = new VertexData(0, 1, 0);
                return my_up;
            }
            set { my_up = value; }
        }

        /// <summary>
        /// Ritorna o imposta il campo di vista della camera
        /// </summary>
        public float FieldOfView
        {
            get
            {
                if (fieldView == 0) fieldView = (float)Math.PI / 4;
                return fieldView;
            }
            set
            {
                fieldView = value;
            }
        }

        /// <summary>
        /// Ritorna o imposta il piano limite di rendering più vicino alla camera
        /// </summary>
        public float NearPlane
        {
            get
            {
                if (near == 0) near = 0.5f;
                return near;
            }
            set
            {
                near = value;
            }
        }

        /// <summary>
        /// Ritorna o imposta il piano limite più lontano dalla camera
        /// </summary>
        public float FarPlane
        {
            get
            {
                if (far == 0) far = 1000.0f;
                return far;
            }
            set
            {
                far = value;
            }
        }

        /// <summary>
        /// Ritorna o imposta la proporzione lunghezza/altezza del frame
        /// </summary>
        public float AspectRatio
        {
            get
            {
                if (aspectratio == 0) aspectratio = 1.0f;
                return aspectratio;
            }
            set
            {
                aspectratio = value;
            }
        }

        /// <summary>
        /// Ritorna "true" se non ci sono errori nell'oggetto
        /// </summary>
        public bool AmICorrect
        {
            get { return correct; }
        }

        /// <summary>
        /// Aggiunge un errore alla lista di errori del LogiX Engine
        /// </summary>
        /// <param name="ErrorName"></param>
        void Error(string ErrorName)
        {
            correct = false;
            LXE_Errors.AddError("Camera", ErrorName);
        }
    }
    #endregion

    #region "Triangle"

    /// <summary>
    /// Definise un oggetto Triangolo
    /// </summary>
    public class Triangle : DevX
    {
        private VertexBuffer my_vertex_buffer;
        private Texture my_texture;
        private bool correct = true;
        private bool textured;
        private int quality;
        private XMaterial mat;

        /// <summary>
        /// Inizializza un Triangolo con un materiale applicato
        /// </summary>
        /// <param name="Vertex1"></param>
        /// <param name="Vertex2"></param>
        /// <param name="Vertex3"></param>
        /// <param name="Material"></param>
        public Triangle(VertexData Vertex1, VertexData Vertex2, VertexData Vertex3, XMaterial Material)
        {
            try
            {
                mat = Material;
                my_vertex_buffer = new VertexBuffer(typeof(CustomVertex.PositionNormalColored), 3, device, 0, CustomVertex.PositionNormalColored.Format, Pool.Default);
                CustomVertex.PositionNormalColored[] verts = (CustomVertex.PositionNormalColored[])my_vertex_buffer.Lock(0, 0);
                verts[0].X = Vertex1.X; verts[0].Y = Vertex1.Y; verts[0].Z = Vertex1.Z; verts[0].Color = Material.Ambient.ToArgb();
                verts[1].X = Vertex2.X; verts[1].Y = Vertex2.Y; verts[1].Z = Vertex2.Z; verts[1].Color = Material.Ambient.ToArgb(); ;
                verts[2].X = Vertex3.X; verts[2].Y = Vertex3.Y; verts[2].Z = Vertex3.Z; verts[2].Color = Material.Ambient.ToArgb();
                my_vertex_buffer.Unlock();
                correct = true;
                textured = false;
            }
            catch
            {
                Error("OnCreateObject");
            }
        }

        /// <summary>
        /// Inizializza un Triangolo da texturizzare
        /// </summary>
        /// <param name="Vertex1"></param>
        /// <param name="Vertex2"></param>
        /// <param name="Vertex3"></param>
        /// <param name="TexturePath"></param>
        /// <param name="QualityValue"></param>
        public Triangle(VertexData Vertex1, VertexData Vertex2, VertexData Vertex3, string TexturePath, int QualityValue)
        {
            try
            {
                //da modificare i valori Tu Tv
                my_texture = TextureLoader.FromFile(device, TexturePath, 0, 0, 0, Usage.None, Format.Unknown, Pool.Default, Filter.Linear, Filter.Linear, 0);
                //my_texture = TextureLoader.FromFile(device, TexturePath);
                my_vertex_buffer = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), 3, device, 0, CustomVertex.PositionNormalTextured.Format, Pool.Default);
                CustomVertex.PositionNormalTextured[] verts = (CustomVertex.PositionNormalTextured[])my_vertex_buffer.Lock(0, 0);
                verts[0].X = Vertex1.X; verts[0].Y = Vertex1.Y; verts[0].Z = Vertex1.Z;
                verts[0].Tu = 0.5f;
                verts[0].Tv = 0.5f;
                verts[1].X = Vertex2.X; verts[1].Y = Vertex2.Y; verts[1].Z = Vertex2.Z;
                verts[1].Tu = 1.0f;
                verts[1].Tv = 1.0f;
                verts[2].X = Vertex3.X; verts[2].Y = Vertex3.Y; verts[2].Z = Vertex3.Z;
                verts[2].Tu = 0.0f;
                verts[2].Tv = 1.0f;
                my_vertex_buffer.Unlock();
                quality = QualityValue;
                correct = true;
                textured = true;
            }
            catch
            {
                Error("OnCreateObject");
            }
        }

        /// <summary>
        /// Renderizza il triangolo
        /// </summary>
        /// <returns></returns>
        public bool RenderMe()
        {
            try
            {
                if (textured == false)
                {
                    Material.SetMaterial();
                    device.SetStreamSource(0, my_vertex_buffer, 0);
                    device.VertexFormat = CustomVertex.PositionNormalColored.Format;
                    device.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
                }
                else
                {
                    device.SetTexture(0, my_texture);

                    switch (quality)
                    {
                        case 0:
                            device.SamplerState[0].MinFilter = TextureFilter.None;
                            device.SamplerState[0].MagFilter = TextureFilter.None;
                            break;
                        case 1:
                            device.SamplerState[0].MinFilter = TextureFilter.Point;
                            device.SamplerState[0].MagFilter = TextureFilter.Point;
                            break;
                        case 2:
                            device.SamplerState[0].MinFilter = TextureFilter.Linear;
                            device.SamplerState[0].MagFilter = TextureFilter.Linear;
                            break;
                        case 3:
                            device.SamplerState[0].MinFilter = TextureFilter.Anisotropic;
                            device.SamplerState[0].MagFilter = TextureFilter.Anisotropic;
                            break;
                        case 4:
                            device.SamplerState[0].MinFilter = TextureFilter.PyramidalQuad;
                            device.SamplerState[0].MagFilter = TextureFilter.PyramidalQuad;
                            break;
                        case 5:
                            device.SamplerState[0].MinFilter = TextureFilter.GaussianQuad;
                            device.SamplerState[0].MagFilter = TextureFilter.GaussianQuad;
                            break;
                        default:
                            if (AmICorrect == true)
                                Error("FilteringError");
                            return false;
                    }

                    device.TextureState[0].ColorOperation = TextureOperation.Modulate;
                    device.TextureState[0].ColorArgument1 = TextureArgument.TextureColor;
                    device.TextureState[0].ColorArgument2 = TextureArgument.Diffuse;
                    device.TextureState[0].AlphaOperation = TextureOperation.Disable;
                    device.SetStreamSource(0, my_vertex_buffer, 0);
                    device.VertexFormat = CustomVertex.PositionNormalTextured.Format;
                    device.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
                    device.SetTexture(0, null);
                }
                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    Error("RenderingError");
                return false;
            }
        }

        /// <summary>
        /// Ritorna "true" se non ci sono errori nell'oggetto
        /// </summary>
        public bool AmICorrect
        {
            get { return correct; }
        }

        /// <summary>
        /// Ritorna o imposta il materiale nel caso di un triangolo inizializzato con il materiale
        /// </summary>
        public XMaterial Material
        {
            get { return mat; }
            set { mat = value; }
        }

        /// <summary>
        /// Aggiunge un errore alla lista di errori del LogiX Engine
        /// </summary>
        /// <param name="ErrorName"></param>
        void Error(string ErrorName)
        {
            correct = false;
            LXE_Errors.AddError("Triangle", ErrorName);
        }
    }

    #endregion

    #region "XMaterial"

    /// <summary>
    /// Definisce un oggetto Materiale per l'applicazione di esso ai poligoni
    /// </summary>
    public class XMaterial : DevX
    {
        private Material mat = new Material();
        private bool correct;

        /// <summary>
        /// Crea un'istanza della classe XMaterial
        /// </summary>
        public XMaterial()
        {
            correct = true;
        }

        /// <summary>
        /// Inizializza un materiale
        /// </summary>
        /// <param name="Ambient"></param>
        /// <param name="Diffuse"></param>
        /// <param name="Specular"></param>
        /// <param name="SpecularSharpness"></param>
        public XMaterial(Color Ambient, Color Diffuse, Color Specular, float SpecularSharpness)
        {
            try
            {
                mat.Ambient = Ambient;
                mat.Diffuse = Diffuse;
                mat.Specular = Specular;
                mat.SpecularSharpness = SpecularSharpness;
                correct = true;
            }
            catch
            {
                Error("OnCreateObject");
            }
        }

        /// <summary>
        /// Imposta il Materiale come materiale universale
        /// </summary>
        /// <returns></returns>
        public bool SetMaterial()
        {

            try
            {
                device.Material = mat;
                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    Error("OnSetMaterial");
                return false;
            }

        }

        /// <summary>
        /// Resetta il Materiale universale
        /// </summary>
        /// <returns></returns>
        public static bool ResetMaterials()
        {
            try
            {
                LogiX_Engine.Device.Material = new Material();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Ritorna o imposta la componente Ambient del materiale
        /// </summary>
        public Color Ambient
        {
            get { return mat.Ambient; }
            set { mat.Ambient = value; }
        }

        /// <summary>
        /// Ritorna o imposta la componente Diffuse del materiale
        /// </summary>
        public Color Diffuse
        {
            get { return mat.Diffuse; }
            set { mat.Diffuse = value; }
        }

        /// <summary>
        /// Ritorna o imposta la componente Specular del materiale
        /// </summary>
        public Color Specular
        {
            get { return mat.Specular; }
            set { mat.Specular = value; }
        }

        /// <summary>
        /// Ritorna o imposta il corrispondente materiale gestito da Microsoft DirectX
        /// </summary>
        public Material DXMaterial
        {
            get { return mat; }
            set { mat = value; }
        }

        /// <summary>
        /// Ritorna o imposta il valore di brillantezza della componente Specular
        /// </summary>
        public float SpecularSharpness
        {
            get { return mat.SpecularSharpness; }
            set { mat.SpecularSharpness = value; }
        }

        /// <summary>
        /// Ritorna "true" se non ci sono errori nell'oggetto
        /// </summary>
        public bool AmICorrect
        {
            get { return correct; }
        }

        /// <summary>
        /// Aggiunge un errore alla lista di errori del LogiX Engine
        /// </summary>
        /// <param name="ErrorName"></param>
        void Error(string ErrorName)
        {
            correct = false;
            LXE_Errors.AddError("XMaterial", ErrorName);
        }
    }
    #endregion

    #region "XTexture"

    /// <summary>
    /// Definisce un oggetto Texture per l'applicazione di esso ai poligoni
    /// </summary>
    public class XTexture:DevX
    {
        Texture texture;
        string filePath;
        private bool correct;

        /// <summary>
        /// Inizializza un oggetto Texture caricandolo da un file esterno di immagini
        /// </summary>
        /// <param name="FilePath"></param>
        public XTexture(string FilePath)
        {
            try
            {
                filePath = FilePath;
                texture = TextureLoader.FromFile(LogiX_Engine.Device, FilePath);
                correct = true;
            }
            catch
            {
                Error("LoadingTexture");
            }
        }

        /// <summary>
        /// Imposta la Texture come Texture Universale
        /// </summary>
        public bool SetTexture()
        {
            try
            {
                LogiX_Engine.Device.SetTexture(0, texture);
                return true;
            }
            catch
            {
                Error("SettingTexture");
                return false;
            }
        }

        /// <summary>
        /// Resetta tutte le Textures Universali
        /// </summary>
        /// <returns></returns>
        public static bool ResetTextures()
        {
            try
            {
                LogiX_Engine.Device.SetTexture(0, null);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Ritorna o imposta la corrispondente texture gestida da Microsoft DirectX
        /// </summary>
        public Texture DXTexture
        {
            get { return texture; }
            set { texture = value; }
        }

        /// <summary>
        /// Ritorna il percorso del file
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return filePath;
        }

        /// <summary>
        /// Ritorna "true" se non ci sono errori nell'oggetto
        /// </summary>
        public bool AmICorrect
        {
            get { return correct; }
        }

        /// <summary>
        /// Aggiunge un errore alla lista di errori del LogiX Engine
        /// </summary>
        /// <param name="ErrorName"></param>
        void Error(string ErrorName)
        {
            correct = false;
            LXE_Errors.AddError("XTexture", ErrorName);
        }
    }

    #endregion

    #region "Directional Light"

    /// <summary>
    /// Definisce una Luce Direzionale per l'illuminazione dei poligoni
    /// </summary>
    public class DirectionalLight : DevX
    {
        private bool enabled = true;
        private Color diffuse;
        private Color ambient;
        private Color specular;
        private VertexData direction;
        private bool correct;
        private int Count;

        /// <summary>
        /// Inizializza un oggetto Luce Direzionale
        /// </summary>
        /// <param name="Direction"></param>
        /// <param name="Ambient"></param>
        /// <param name="Diffuse"></param>
        /// <param name="Specular"></param>
        public DirectionalLight(VertexData Direction, Color Ambient, Color Diffuse, Color Specular)
        {
            try
            {
                ambient = Ambient;
                diffuse = Diffuse;
                direction = Direction;
                specular = Specular;
                LightCount++;
                Count = LightCount;
                correct = true;
            }
            catch
            {
                Error("OnCreateObject");
            }
        }

        /// <summary>
        /// Imposta e attiva la luce direzionale.
        /// </summary>
        /// <returns></returns>
        public bool SetDirectionalLight()
        {
            try
            {
                device.Lights[Count].Type = LightType.Directional;
                device.Lights[Count].Diffuse = Diffuse;
                device.Lights[Count].Ambient = Ambient;
                device.Lights[Count].Specular = Specular;
                device.Lights[Count].Direction = new Vector3(Direction.X, Direction.Y, Direction.Z);
                device.Lights[Count].Update();
                device.Lights[Count].Enabled = Enabled;
                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    Error("SettingLight");
                return false;
            }
        }

        /// <summary>
        /// Ritorna o imposta la componente Diffuse della Luce Direzionale
        /// </summary>
        public Color Diffuse
        {
            get { return diffuse; }
            set { diffuse = value; }
        }

        /// <summary>
        /// Ritorna o imposta la componente Ambient della Luce Direzionale
        /// </summary>
        public Color Ambient
        {
            get { return ambient; }
            set { ambient = value; }
        }

        /// <summary>
        /// Ritorna o imposta la componente specular della Luce Direzionale
        /// </summary>
        public Color Specular
        {
            get { return specular; }
            set { specular = value; }
        }

        /// <summary>
        /// Ritorna o imposta la direzione della Luce Direzionale
        /// </summary>
        public VertexData Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        /// <summary>
        /// Attiva o Disattiva la luce direzionale
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        /// <summary>
        /// Ritorna "true" se non ci sono errori nell'oggetto
        /// </summary>
        public bool AmICorrect
        {
            get { return correct; }
        }

        /// <summary>
        /// Aggiunge un errore alla lista di errori del LogiX Engine
        /// </summary>
        /// <param name="ErrorName"></param>
        void Error(string ErrorName)
        {
            correct = false;
            LXE_Errors.AddError("DirectionalLight", ErrorName);
        }
    }
    #endregion

    #region "Point Light"

    /// <summary>
    /// Definisce una Luce Puntiforme per l'illuminazione dei poligoni
    /// </summary>
    public class PointLight : DevX
    {
        private bool my_enabled = true;
        private Color my_diffuse;
        private Color my_ambient;
        private Color my_specular;
        private VertexData my_position;
        private bool correct;
        private float my_range;
        private float my_attenuation0;
        private float my_attenuation1;
        private float my_attenuation2;
        private int Count;

        /// <summary>
        /// Inizializza un oggetto Luce Puntiorme
        /// </summary>
        /// <param name="Position"></param>
        /// <param name="Ambient"></param>
        /// <param name="Diffuse"></param>
        /// <param name="Specular"></param>
        /// <param name="Range"></param>
        public PointLight(VertexData Position, Color Ambient, Color Diffuse, Color Specular, float Range)
        {
            try
            {
                my_ambient = Ambient;
                my_diffuse = Diffuse;
                my_position = Position;
                my_specular = Specular;
                my_range = Range;
                LightCount++;
                Count = LightCount;
                correct = true;
            }
            catch
            {
                Error("OnCreateObject");
            }
        }

        /// <summary>
        /// Imposta e attiva la Luce Puntiforme
        /// </summary>
        /// <returns></returns>
        public bool SetPointLight()
        {
            try
            {
                device.Lights[Count].Type = LightType.Point;
                device.Lights[Count].Diffuse = Diffuse;
                device.Lights[Count].Ambient = Ambient;
                device.Lights[Count].Specular = Specular;
                device.Lights[Count].Range = Range;
                device.Lights[Count].Attenuation0 = Attenuation0;
                device.Lights[Count].Attenuation1 = Attenuation1;
                device.Lights[Count].Attenuation2 = my_attenuation2;
                device.Lights[Count].Position = new Vector3(Position.X, Position.Y, Position.Z);
                device.Lights[Count].Update();
                device.Lights[Count].Enabled = Enabled;
                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    Error("SettingLight");
                return false;
            }
        }

        /// <summary>
        /// Ritorna o imposta la componente Diffuse della Luce Puntiforme
        /// </summary>
        public Color Diffuse
        {
            get { return my_diffuse; }
            set { my_diffuse = value; }
        }

        /// <summary>
        /// Ritorna o imposta la componente Ambient della Luce Puntiforme
        /// </summary>
        public Color Ambient
        {
            get { return my_ambient; }
            set { my_ambient = value; }
        }

        /// <summary>
        /// Ritorna o imposta la componente Specular della Luce Puntiforme
        /// </summary>
        public Color Specular
        {
            get { return my_specular; }
            set { my_specular = value; }
        }

        /// <summary>
        /// Ritorna o imposta la posizione della Luce Puntiforme
        /// </summary>
        public VertexData Position
        {
            get { return my_position; }
            set { my_position = value; }
        }

        /// <summary>
        /// Ritorna o imposta la distanza oltre la quale la Luce Puntiforme non ha più effetto
        /// </summary>
        public float Range
        {
            get { return my_range; }
            set { my_range = value; }
        }

        /// <summary>
        /// Ritorna o imposta l'attenuazione "0" della Luce Puntiforme
        /// </summary>
        public float Attenuation0
        {
            get { return my_attenuation0; }
            set { my_attenuation0 = value; }
        }

        /// <summary>
        /// Ritorna o imposta l'attenuazione "1" della Luce Puntiforme
        /// </summary>
        public float Attenuation1
        {
            get { return my_attenuation1; }
            set { my_attenuation1 = value; }
        }

        /// <summary>
        /// Ritorna o imposta l'attenuazione "2" della Luce Puntiforme
        /// </summary>
        public float Attenuation2
        {
            get { return my_attenuation2; }
            set { my_attenuation2 = value; }
        }

        /// <summary>
        /// Attiva o disattiva la Luce puntiforme
        /// </summary>
        public bool Enabled
        {
            get { return my_enabled; }
            set { my_enabled = value; }
        }

        /// <summary>
        /// Ritorna "true" se non ci sono errori nell'oggetto
        /// </summary>
        public bool AmICorrect
        {
            get { return correct; }
        }

        /// <summary>
        /// Aggiunge un errore alla lista di errori del LogiX Engine
        /// </summary>
        /// <param name="ErrorName"></param>
        void Error(string ErrorName)
        {
            correct = false;
            LXE_Errors.AddError("PointLight", ErrorName);
        }
    }
    #endregion

    #region "Spot Light"

    /// <summary>
    /// Definisce un oggetto Riflettore (Luce Conica) per l'illuminazione dei poligoni
    /// </summary>
    public class SpotLight : DevX
    {
        private bool enabled = true;
        private Color diffuse;
        private Color ambient;
        private Color specular;
        private VertexData position;
        private VertexData direction;
        private bool correct;
        private float range;
        private float attenuation0;
        private float attenuation1;
        private float attenuation2;
        private float falloff;
        private float innerconeangle;
        private float outerconeangle;
        private int Count;

        /// <summary>
        /// Inizializza un oggetto Riflettore
        /// </summary>
        /// <param name="Position"></param>
        /// <param name="Direction"></param>
        /// <param name="Ambient"></param>
        /// <param name="Diffuse"></param>
        /// <param name="Specular"></param>
        /// <param name="InnerConeAngle"></param>
        /// <param name="OuterConeAngle"></param>
        /// <param name="Falloff"></param>
        /// <param name="Range"></param>
        public SpotLight(VertexData Position, VertexData Direction, Color Ambient, Color Diffuse, Color Specular, float InnerConeAngle, float OuterConeAngle, float Falloff, float Range)
        {
            try
            {
                ambient = Ambient;
                diffuse = Diffuse;
                position = Position;
                direction = Direction;
                specular = Specular;
                range = Range;
                falloff = Falloff;
                innerconeangle = InnerConeAngle;
                outerconeangle = OuterConeAngle;
                LightCount++;
                Count = LightCount;
                correct = true;
            }
            catch
            {
                Error("OnCreateObject");
            }
        }

        /// <summary>
        /// Imposta e attiva il Riflettore
        /// </summary>
        /// <returns></returns>
        public bool SetSpotLight()
        {
            try
            {
                device.Lights[Count].Type = LightType.Spot;
                device.Lights[Count].Diffuse = Diffuse;
                device.Lights[Count].Ambient = Ambient;
                device.Lights[Count].Specular = Specular;
                device.Lights[Count].Range = Range;
                device.Lights[Count].Attenuation0 = Attenuation0;
                device.Lights[Count].Attenuation1 = Attenuation1;
                device.Lights[Count].Attenuation2 = attenuation2;
                device.Lights[Count].Falloff = Falloff;
                device.Lights[Count].InnerConeAngle = InnerConeAngle;
                device.Lights[Count].OuterConeAngle = OuterConeAngle;
                device.Lights[Count].Position = new Vector3(Position.X, Position.Y, Position.Z);
                device.Lights[Count].Direction = new Vector3(Direction.X, Direction.Y, Direction.Z);
                device.Lights[Count].Update();
                device.Lights[Count].Enabled = Enabled;
                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    Error("SettingLight");
                return false;
            }
        }

        /// <summary>
        /// Ritorna o imposta la componente Diffuse del Riflettore
        /// </summary>
        public Color Diffuse
        {
            get { return diffuse; }
            set { diffuse = value; }
        }

        /// <summary>
        /// Ritorna o imposta la componente Ambient del Riflettore
        /// </summary>
        public Color Ambient
        {
            get { return ambient; }
            set { ambient = value; }
        }

        /// <summary>
        /// Ritornao o imposta la componente Specular del Riflettore
        /// </summary>
        public Color Specular
        {
            get { return specular; }
            set { specular = value; }
        }

        /// <summary>
        /// Ritorna o imposta la posizione del Riflettore
        /// </summary>
        public VertexData Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Ritorna o imposta la Direzione del Riflettore
        /// </summary>
        public VertexData Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        /// <summary>
        /// Ritorna o imposta la distanza oltre la quale il Riflettore non ha più effetto
        /// </summary>
        public float Range
        {
            get { return range; }
            set { range = value; }
        }

        /// <summary>
        /// Ritorna o imposta l'attenuazione "0" del Riflettore
        /// </summary>
        public float Attenuation0
        {
            get { return attenuation0; }
            set { attenuation0 = value; }
        }

        /// <summary>
        /// Ritorna o imposta l'attenuazione "1" del Riflettore
        /// </summary>
        public float Attenuation1
        {
            get { return attenuation1; }
            set { attenuation1 = value; }
        }

        /// <summary>
        /// Ritorna o imposta l'attenuazione "2" del Riflettore
        /// </summary>
        public float Attenuation2
        {
            get { return attenuation2; }
            set { attenuation2 = value; }
        }

        /// <summary>
        /// Ritorna o imposta il decremento di illuminazione tra il cono interno (L'angolo specificato da LogiX_Technologies.SpotLight.InnerConeAngle) e il bordo del cono esterno (l'angolo specificato da LogiX_Technologies.SpotLight.OuterConeAngle) del Riflettore
        /// </summary>
        public float Falloff
        {
            get { return falloff; }
            set { falloff = value; }
        }

        /// <summary>
        /// Ritorna o imposta il valore dell'angolo, in radianti, del cono interno del Riflettore, che è il cono completamente illuinato.
        /// </summary>
        public float InnerConeAngle
        {
            get { return innerconeangle; }
            set { innerconeangle = value; }
        }

        /// <summary>
        /// Ritorna o imposta il valore dell'angolo, in radianti, che individua il bordo del cono esterno del Riflettore.
        /// </summary>
        public float OuterConeAngle
        {
            get { return outerconeangle; }
            set { outerconeangle = value; }
        }

        /// <summary>
        /// Attiva o disattiva il Riflettore
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        /// <summary>
        /// Ritorna "true" se non ci sono errori nell'oggetto
        /// </summary>
        public bool AmICorrect
        {
            get { return correct; }
        }

        /// <summary>
        /// Aggiunge un errore alla lista di errori del LogiX Engine
        /// </summary>
        /// <param name="ErrorName"></param>
        void Error(string ErrorName)
        {
            correct = false;
            LXE_Errors.AddError("SpotLight", ErrorName);
        }
    }
    #endregion

    #region "Model"

    /// <summary>
    /// Definisce un oggetto Modello 3D per caricare modelli poligonali da files esterni in formato "x"
    /// </summary>
    public class Model : DevX
    {
        public Mesh mesh = null;
        public Material[] meshMaterial;
        public Texture[] meshTexture;
        private VertexData position = new VertexData(0, 0, 0);
        private VertexData transformedPosition = new VertexData(0, 0, 0);
        private VertexData rotation = new VertexData(0, 0, 0);
        private VertexData rotationAxis = new VertexData(0, 0, 0);
        private VertexData scaling = new VertexData(1, 1, 1);
        private Matrix container = Matrix.Identity;
        private Matrix move = Matrix.Identity;
        private bool correct;
        private bool texturized;
        private int[] adia;

        /// <summary>
        /// Inizializza un oggetto Model caricando un modello poligonale in formato "x" dall'esterno
        /// </summary>
        /// <param name="ModelPath"></param>
        /// <param name="SubsetStart"></param>
        public Model(string ModelPath, int SubsetStart)
        {
            try
            {
                ExtendedMaterial[] my_ext = null;
                Directory.SetCurrentDirectory(Application.StartupPath);
                mesh = Mesh.FromFile(ModelPath, MeshFlags.Dynamic, device, out my_ext);

                if (meshTexture == null)
                {
                    meshTexture = new Texture[my_ext.Length];
                    meshMaterial = new Material[my_ext.Length];
                    for (int i = SubsetStart; i < my_ext.Length; i++)
                    {
                        meshMaterial[i] = my_ext[i].Material3D;
                        meshMaterial[i].Ambient = meshMaterial[i].Diffuse;
                        try
                        {
                            meshTexture[i] = TextureLoader.FromFile(device, ModelPath + "/../" + my_ext[i].TextureFilename);
                            texturized = true;
                        }
                        catch
                        {
                            texturized = false;
                        }
                    }
                }
                correct = true;
            }
            catch
            {
                Error("OnCreateObject");
            }

        }

        /// <summary>
        /// Renderizza il Modello senza tenere conto delle Trasformazioni Geometriche, quali traslazioni, dilatazioni e rotazioni. (Non tiene conto delle proprietà Position, Rotation, Rotation Axis e Scaling dell'oggetto stesso)
        /// </summary>
        /// <param name="SubsetStart"></param>
        /// <returns></returns>
        public bool RenderWithoutMatrices(int SubsetStart)
        {
            try
            {
                for (int i = SubsetStart; i < meshMaterial.Length; i++)
                {
                    if (meshTexture[i] == null)
                    {
                        device.SetTexture(0, null);
                        Material mat = new Material();
                        mat.Ambient = Color.DarkRed;
                        mat.Diffuse = Color.Red;
                        mat.Specular = Color.White;
                        mat.SpecularSharpness = 25;
                        device.Material = mat;
                        Error("TexturingObject");
                    }
                    else
                    {
                        device.Material = meshMaterial[i];
                        device.SetTexture(0, meshTexture[i]);
                    }
                    mesh.DrawSubset(i);
                }
                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    correct = false;
                Error("RenderingObject");
                return false;
            }

        }

        /// <summary>
        /// Renderizza il Modello calcolando le Trasformazioni Geometriche, quali traslazioni, rotazioni e dilatazioni. (Tiene conto delle proprietà Position, Rotation, Rotation Axis e Scaling dell'oggetto stesso)
        /// </summary>
        /// <param name="SubsetStart"></param>
        /// <returns></returns>
        public bool RenderMe(int SubsetStart)
        {
            try
            {
                for (int i = SubsetStart; i < meshMaterial.Length; i++)
                {
                    SetModelMatrices();
                    if (meshTexture[i] == null)
                    {
                        device.SetTexture(0, null);
                        Material mat = new Material();
                        mat.Ambient = Color.DarkRed;
                        mat.Diffuse = Color.Red;
                        mat.Specular = Color.White;
                        mat.SpecularSharpness = 25;
                        device.Material = mat;
                        Error("TexturingObject");
                    }
                    else
                    {
                        device.Material = meshMaterial[i];
                        device.SetTexture(0, meshTexture[i]);

                    }
                    mesh.DrawSubset(i);
                    UnSetModelMatrices();
                }
                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    correct = false;
                Error("RenderingObject");
                return false;
            }

        }

        /// <summary>
        /// Disegna una sezione del Modello senza tenere conto delle Trasformazioni Geometriche, quali traslazioni, dilatazioni e rotazioni. (Non tiene conto delle proprietà Position, Rotation, Rotation Axis e Scaling dell'oggetto stesso)
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool DrawSubset(int ID)
        {
            try
            {
                mesh.DrawSubset(ID);
                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    Error("DrawSubset Error");
                return false;
            }
        }

        /// <summary>
        /// Disegna più sezioni del Modello senza tenere conto delle Trasformazioni Geometriche, quali traslazioni, dilatazioni e rotazioni. (Non tiene conto delle proprietà Position, Rotation, Rotation Axis e Scaling dell'oggetto stesso)
        /// </summary>
        /// <param name="startID"></param>
        /// <param name="endID"></param>
        /// <returns></returns>
        public bool DrawSubsets(int startID, int endID)
        {
            try
            {
                for (int i = startID; i < endID + 1; i++)
                {
                    mesh.DrawSubset(i);
                }
                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    Error("DrawSubset Error");
                return false;
            }
        }

        /// <summary>
        /// Imposta le Trasformazioni Geometriche (definite dalle proprietà Position, Scaling, Rotation e Rotation Axis dell'oggetto stesso), quali traslazioni, dilatazioni e rotazioni, come Trasformazioni Geometriche universali.
        /// </summary>
        /// <returns></returns>
        public bool SetModelMatrices()
        {
            try
            {
                move.M41 = position.X;
                move.M42 = position.Y;
                move.M43 = position.Z;
                container = device.Transform.World;
                device.Transform.World = ((Matrix.RotationX(Rotation.X) * Matrix.RotationY(Rotation.Y) * Matrix.RotationZ(Rotation.Z))) * Matrix.Scaling(new Vector3(Scaling.X, Scaling.Y, Scaling.Z)) * move * (Matrix.RotationX(RotationAxis.X) * Matrix.RotationY(RotationAxis.Y) * Matrix.RotationZ(RotationAxis.Z)) * device.Transform.World;
                TransformModelPosition();
                return true;
            }
            catch
            {
                UnSetModelMatrices();
                if (AmICorrect == true)
                    Error("SetModelMatrices Error");
                return false;
            }
        }

        private void TransformModelPosition()
        {
            transformedPosition.X = device.Transform.World.M41;
            transformedPosition.Y = device.Transform.World.M42;
            transformedPosition.Z = device.Transform.World.M43;
        }

        /// <summary>
        /// Reimposta le Trasformazioni Geometriche universali. (Da utilizzare dopo il metodo LogiX_Technologies.Model.SetModelMatrices() dello stesso oggetto)
        /// </summary>
        /// <returns></returns>
        public bool UnSetModelMatrices()
        {
            try
            {
                device.Transform.World = container;
                move = Matrix.Identity;
                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    Error("UnSetModelMatrices Error");
                return false;
            }
        }

        /// <summary>
        /// Renderizza il Modello con un Materiale applicato, calcolando le Trasformazioni Geometriche, quali traslazioni, rotazioni e dilatazioni. (Tiene conto delle proprietà Position, Rotation, Rotation Axis e Scaling dell'oggetto stesso)
        /// </summary>
        /// <param name="XMaterial"></param>
        /// <param name="SubsetStart"></param>
        /// <returns></returns>
        public bool RenderMe(XMaterial XMaterial, int SubsetStart)
        {
            try
            {
                for (int i = SubsetStart; i < meshMaterial.Length; i++)
                {
                    SetModelMatrices();
                    device.SetTexture(0, null);
                    device.Material = XMaterial.DXMaterial;
                    mesh.DrawSubset(i);
                    UnSetModelMatrices();
                }
                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    Error("RenderingObject");
                return false;
            }

        }

        /// <summary>
        /// Converte il Modello in un Modello renderizzabile con l'effetto Rilievo (specificato da LogiX_Technologies.XEffect)
        /// </summary>
        /// <returns></returns>
        public bool ConvertToBumpMesh()
        {
            try
            {
                Mesh cMesh = mesh.Clone(MeshFlags.Managed, GetVertexElements, LogiX_Engine.Device);
                adia = new int[cMesh.NumberFaces * 3];
                cMesh.GenerateAdjacency(0.001f, adia);
                mesh = cMesh;
                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    Error("ConvertingToBump");
                return false;
            }
        }

        public static VertexElement[] GetVertexElements
        {
            get
            {
                VertexElement[] element = { new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0), new VertexElement(0, 12, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0), new VertexElement(0, 28, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 1), VertexElement.VertexDeclarationEnd };
                return element;
            }
        }

        /// <summary>
        /// Ritorna o imposta la posizione del Modello nello spazio cartesiano, per la Trasformazione Geometrica di traslazione.
        /// </summary>
        public VertexData Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Ritorna la posizione visualizzata a schermo del Modello nello spazio, dopo essere stata compiuta ogni Trasformazione Geometrica
        /// </summary>
        public VertexData TransformedPosition
        {
            get { return transformedPosition; }
        }

        /// <summary>
        /// Ritorna o imposta la rotazione del Modello su sè stesso, per la Trasformazione Geometrica di rotazione con sistema di riferimento traslato
        /// </summary>
        public VertexData Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        /// <summary>
        /// Ritorna o imposta la rotazione del Modello attorno agli assi cartesiani, per la Trasformazione Geometrica di rotazione attorno agli assi
        /// </summary>
        public VertexData RotationAxis
        {
            get { return rotationAxis; }
            set { rotationAxis = value; }
        }

        /// <summary>
        /// Ritorna o imposta la dilatazione del Modello nel riferimento cartesiano, per la Trasformazione Geometrica di dilatazione.
        /// </summary>
        public VertexData Scaling
        {
            get { return scaling; }
            set { scaling = value; }
        }

        /// <summary>
        /// Ritorna "true" se non ci sono errori nell'oggetto
        /// </summary>
        public bool AmICorrect
        {
            get { return correct; }
        }

        /// <summary>
        /// Ritorna le informazioni di posizione, dilatazione, e rotazione.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ("Position: " + Position.ToString() + "  Scaling: " + Scaling.ToString() + "  Rotation: " + Rotation.ToString() + "  RotationAxis: " + RotationAxis.ToString());
        }

        /// <summary>
        /// Aggiunge un errore alla lista di errori del LogiX Engine
        /// </summary>
        /// <param name="ErrorName"></param>
        void Error(string ErrorName)
        {
            correct = false;
            LXE_Errors.AddError("Model", ErrorName);
        }
    }
    #endregion

    #region "Print Screen"

    /// <summary>
    /// Definisce un oggetto testo da stampare a schermo
    /// </summary>
    public class PrintScreen : DevX
    {
        private Microsoft.DirectX.Direct3D.Font font;
        private bool correct;

        /// <summary>
        /// Inizializza un oggetto PrintScreen
        /// </summary>
        /// <param name="Font"></param>
        public PrintScreen(System.Drawing.Font Font)
        {
            try
            {
                font = new Microsoft.DirectX.Direct3D.Font(device, Font);
                correct = true;
            }
            catch
            {
                Error("OnCreateObject");
            }
        }

        /// <summary>
        /// Inizializza un oggetto PrintScreen
        /// </summary>
        /// <param name="FontName"></param>
        /// <param name="FontSize"></param>
        public PrintScreen(string FontName, float FontSize)
        {
            try
            {
                font = new Microsoft.DirectX.Direct3D.Font(device, new System.Drawing.Font(FontName, FontSize));
                correct = true;
            }
            catch
            {
                Error("OnCreateObject");
            }
        }

        /// <summary>
        /// Stampa a schermo il testo inserito
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Color"></param>
        /// <returns></returns>
        public bool Write(string Text, int X, int Y, Color Color)
        {
            try
            {
                font.DrawText(null, Text, X, Y, Color);
                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    Error("OnWriting");
                return false;
            }
        }

        /// <summary>
        /// Stampa a schermo il testo inserito
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Rectangle"></param>
        /// <param name="Format"></param>
        /// <param name="Color"></param>
        /// <returns></returns>
        public bool Write(string Text, Rectangle Rectangle, DrawTextFormat Format, Color Color)
        {
            try
            {
                font.DrawText(null, Text, Rectangle, Format, Color);
                return true;
            }

            catch
            {
                if (AmICorrect == true)
                    Error("OnWriting");
                return false;
            }
        }

        /// <summary>
        /// Ritorna "true" se non ci sono errori nell'oggetto
        /// </summary>
        bool AmICorrect
        {
            get { return correct; }
        }

        /// <summary>
        /// Aggiunge un errore alla lista di errori del LogiX Engine
        /// </summary>
        /// <param name="ErrorName"></param>
        void Error(string ErrorName)
        {
            correct = false;
            LXE_Errors.AddError("PrintScreen", ErrorName);
        }
    }

    #endregion

    #region "Mouse"

    /// <summary>
    /// Definisce un oggetto Mouse per la gestione dell'input da tale dispositivo
    /// </summary>
    public class Mouse : DevX
    {
        private Microsoft.DirectX.DirectInput.Device my_mouse;
        private Microsoft.DirectX.DirectInput.MouseState my_state;
        private bool correct;
        private float angleX;
        private float angleY;
        private float angleZ = -1;
        private float speedScaling;
        private Camera cam;
        private bool count;
        private float distanceForCamera;

        /// <summary>
        /// Inizializza un oggetto Mouse
        /// </summary>
        /// <param name="Handle"></param>
        public Mouse(Control Handle)
        {
            try
            {
                my_mouse = new Microsoft.DirectX.DirectInput.Device(SystemGuid.Mouse);
                if (LogiX_Engine.Device.PresentationParameters.Windowed)
                {
                    my_mouse.SetCooperativeLevel(Handle, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
                }
                else
                {
                    my_mouse.SetCooperativeLevel(Handle, CooperativeLevelFlags.Foreground | CooperativeLevelFlags.Exclusive);
                }
                my_mouse.SetDataFormat(DeviceDataFormat.Mouse);
                my_mouse.Acquire();
                correct = true;
            }
            catch
            {
                Error("OnCreateObject");
            }
        }

        /// <summary>
        /// Rileva lo stato del Mouse
        /// </summary>
        /// <returns></returns>
        public bool Acquire()
        {
            try
            {
                my_state = my_mouse.CurrentMouseState;
                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    Error("OnAcquire");
                return false;
            }
        }

        /// <summary>
        /// Controlla l'oggetto Camera (specificato da LogiX_Technologies.Camera) ruotando il suo punto di osservazione (specificato da LogiX_Technologies.Camera.Look) attorno al suo punto di posizione nello spazio (specificato da LogiX_Technologies.Camera.Position) con il Mouse. 
        /// </summary>
        /// <param name="Camera"></param>
        /// <returns></returns>
        public bool RotateCamera(Camera Camera)
        {
            try
            {
                cam = Camera;
                angleX = angleX + 0.005f * CurrentX;
                angleY = angleY + 0.005f * CurrentY;
                Camera.Look.X = Camera.Position.X - (float)Math.Cos(angleX) * (float)Math.Sin(angleY);
                if (angleY > 0)
                {
                    Camera.Look.Y = Camera.Position.Y + (float)Math.Cos(angleY);
                }
                if (angleY <= 0)
                {
                    angleY = 0.001f;
                    Camera.Look.Y = Camera.Position.Y + (float)Math.Cos(angleY);
                }
                if (angleY >= Math.PI)
                {
                    angleY = (float)Math.PI - 0.001f;
                    Camera.Look.Y = Camera.Position.Y + (float)Math.Cos(angleY);
                }
                Camera.Look.Z = Camera.Position.Z + (float)Math.Sin(angleX) * (float)Math.Sin(angleY);
                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    Error("OnRotateCamera");
                return false;
            }
        }

        /// <summary>
        /// Controlla l'oggetto Camera (specificato da LogiX_Technologies.Camera) ruotando il suo punto di posizione nello spazio (specificato da LogiX_Technologies.Camera.Position) attorno al suo punto di osservazione (specificato da LogiX_Technologies.Camera.Look) con il Mouse. 
        /// </summary>
        /// <param name="Camera"></param>
        /// <param name="Look"></param>
        /// <param name="Speed"></param>
        /// <returns></returns>
        public bool CameraTrackBall(Camera Camera, VertexData Look, float Speed, float SpeedScaling, float Sensibility)
        {
            try
            {
                cam = Camera;
                speedScaling = SpeedScaling;
                angleX = angleX + 0.005f * CurrentX * Sensibility;
                angleY = angleY + 0.005f * CurrentY * Sensibility;
                angleZ = angleZ + 0.005f * CurrentZ * Speed;
                Camera.Look = Look;
                if (angleY > 0)
                {
                    if (angleZ < 0)
                    {
                        if (angleY < (float)Math.PI)
                        {
                            Camera.Position.Y = Look.Y + 1000 * ((float)Math.Pow(angleZ, 5) / SpeedScaling) * (float)Math.Cos(angleY);
                            Camera.Position.X = Look.X + 1000 * ((float)Math.Pow(angleZ, 5) / SpeedScaling) * (float)Math.Cos(angleX) * (float)Math.Sin(angleY);
                            Camera.Position.Z = Look.Z + 1000 * ((float)Math.Pow(angleZ, 5) / SpeedScaling) * (float)Math.Sin(angleX) * (float)Math.Sin(angleY);
                        }
                        else
                        {
                            angleY = (float)Math.PI - 0.001f;
                            Camera.Position.Y = Look.Y + 1000 * ((float)Math.Pow(angleZ, 5) / SpeedScaling) * (float)Math.Cos(angleY);
                            Camera.Position.X = Look.X + 1000 * ((float)Math.Pow(angleZ, 5) / SpeedScaling) * (float)Math.Cos(angleX) * (float)Math.Sin(angleY);
                            Camera.Position.Z = Look.Z + 1000 * ((float)Math.Pow(angleZ, 5) / SpeedScaling) * (float)Math.Sin(angleX) * (float)Math.Sin(angleY);
                        }
                    }
                    else
                    {
                        angleZ = -0.00001f;
                    }
                }
                if (angleY <= 0)
                {
                    angleY = 0.01f;
                    Camera.Position.Y = Look.Y + 1000 * ((float)Math.Pow(angleZ, 5) / SpeedScaling) * (float)Math.Cos(angleY);
                    Camera.Position.X = Look.X + 1000 * ((float)Math.Pow(angleZ, 5) / SpeedScaling) * (float)Math.Cos(angleX) * (float)Math.Sin(angleY);
                    Camera.Position.Z = Look.Z + 1000 * ((float)Math.Pow(angleZ, 5) / SpeedScaling) * (float)Math.Sin(angleX) * (float)Math.Sin(angleY);
                }

                correct = true;
                return true;
            }
            catch
            {
                correct = false;
                return false;
            }
        }

        /// <summary>
        /// Ritorna la Camera allacciata al Mouse
        /// </summary>
        public Camera Camera
        {
            get { return cam; }
        }

        /// <summary>
        /// Ritorna la componente orizzontale corrente del movimento del Mouse
        /// </summary>
        public float CurrentX
        {
            get
            {
                return (float)my_state.X;
            }
        }

        /// <summary>
        /// Ritorna la componente verticale corrente del movimento del Mouse
        /// </summary>
        public float CurrentY
        {
            get
            {
                return (float)my_state.Y;
            }
        }

        /// <summary>
        /// Ritorna lo stato di movimento della rotellina del Mouse
        /// </summary>
        public float CurrentZ
        {
            get
            {
                return (float)my_state.Z;
            }
        }

        /// <summary>
        /// Ritorna "true" se non ci sono errori nell'oggetto
        /// </summary>
        public bool AmICorrect
        {
            get { return correct; }
        }

        /// <summary>
        /// Ritorna un valore derivante dal movimento orizzontale totale del Mouse. Generalmente viene utilizzato come valore "AngleX" dal metodo di allacciamento della camera alla tastiera [specificato da LogiX_Technologies.Keyboard.MoveCamera(Key Forward, Key Backward, Key Left, Key Right, Key Up, Key Down, Camera Camera, float AngleX, float AngleY, float Speed)]
        /// </summary>
        public float AngleX
        {
            get { return angleX; }
        }

        /// <summary>
        /// Ritorna un valore derivante dal movimento verticale totale del Mouse. Generalmente viene utilizzato come valore "AngleY" dal metodo di allacciamento della camera alla tastiera [specificato da LogiX_Technologies.Keyboard.MoveCamera(Key Forward, Key Backward, Key Left, Key Right, Key Up, Key Down, Camera Camera, float AngleX, float AngleY, float Speed)]
        /// </summary>
        public float AngleY
        {
            get { return angleY; }
        }

        /// <summary>
        /// Ritorna un valore derivante dal movimento totale della rotellina.
        /// </summary>
        public float AngleZ
        {
            get { return angleZ; }
        }

        /// <summary>
        /// Ritorna il valore, in byte, dei pulsanti del Mouse correntemente premuti.
        /// </summary>
        public byte[] CurrentMouseButton
        {
            get { return my_state.GetMouseButtons(); }
        }


        /// <summary>
        /// Ritorna o Imposta un moltiplicatore della distanza della Posizione della Camera allacciata al Mouse (specificata da LogiX_Technologies.Camera.Position) dal punto di osservazione della stessa (specificato da LogiX_Technologies.Camera.Look) nel caso di utilizzo del metodo di rotazione della Posizione della camera [specificato da LogiX_Technologies.Mouse.CameraTrackBall(Camera Camera, VertexData Look, float Speed)]
        /// </summary>
        public float DistanceForCameraTrackBall
        {
            get { return distanceForCamera; }
            set
            {
                distanceForCamera = value;
                angleZ = -distanceForCamera / 1000;
            }
        }

        /// <summary>
        /// Ritorna la distanza della Posizione della Camera allacciata al Mouse (specificata da LogiX_Technologies.Camera.Position) dal punto di osservazione della stessa (specificato da LogiX_Technologies.Camera.Look) in funzione della proprietà LogiX_Technologies.Mouse.DistanceForCameraTrackBall nel caso di utilizzo del metodo di rotazione della Posizione della camera [specificato da LogiX_Technologies.Mouse.CameraTrackBall(Camera Camera, VertexData Look, float Speed)]
        /// </summary>
        public float DistanceLookPosition
        {
            get
            {
                return 1000 * ((float)Math.Pow(angleZ, 5) / speedScaling);
            }
        }

        /// <summary>
        /// Aggiunge un errore alla lista di errori del LogiX Engine
        /// </summary>
        /// <param name="ErrorName"></param>
        void Error(string ErrorName)
        {
            correct = false;
            LXE_Errors.AddError("Mouse", ErrorName);
        }
    }

    #endregion

    #region "Keyboard"

    /// <summary>
    /// Definisce un oggetto Tastiera per la gestione dell'input da tale dispositivo
    /// </summary>
    public class Keyboard : DevX
    {

        private Microsoft.DirectX.DirectInput.Device keyboard;
        private bool correct;
        private VertexData movement;

        /// <summary>
        /// Inizializza un oggetto Tastiera
        /// </summary>
        /// <param name="Handle"></param>
        public Keyboard(Control Handle)
        {
            try
            {
                keyboard = new Microsoft.DirectX.DirectInput.Device(SystemGuid.Keyboard);
                movement = new VertexData(0, 0, 0);
                if (LogiX_Engine.Device.PresentationParameters.Windowed)
                {
                    keyboard.SetCooperativeLevel(Handle, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
                }
                else
                {
                    keyboard.SetCooperativeLevel(Handle, CooperativeLevelFlags.Foreground | CooperativeLevelFlags.NonExclusive);
                }
                keyboard.SetDataFormat(DeviceDataFormat.Keyboard);
                keyboard.Acquire();
                correct = true;
            }
            catch
            {
                Error("OnCreateObject");
            }
        }

        /// <summary>
        /// Ritorna lo stato delle informazioni immediatamente rilevate dalla tastiera (pulsanti premuti al momento)
        /// </summary>
        public KeyboardState KeyboardState
        {
            get { return keyboard.GetCurrentKeyboardState(); }
        }

        /// <summary>
        /// Rileva lo stato della Tastiera
        /// </summary>
        /// <returns></returns>
        public bool Acquire()
        {
            try
            {
                keyboard.Acquire();
                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    Error("OnAcquire");
                return false;
            }
        }

        /// <summary>
        /// Controlla l'oggetto Camera (specificato da LogiX_Technologies.Camera) traslando il suo punto di osservazione (specificato da LogiX_Technologies.Camera.Look) insieme con il suo punto di posizione (specificato da LogiX_Technologies.Camera.Position). Generalmente viene utilizzato contemporaneamente al metodo di rotazione della Camera da parte del Mouse [specificato da LogiX_Technologies.Mouse.RotateCamera(Camera Camera)], inserendo nei valori "float AngleX" e "float AngleY" di questo metodo le rispettive proprietà del mouse (specificate da LogiX_Technologies.Mouse.AngleX e da LogiX_Technologies.Mouse.AngleY)
        /// </summary>
        /// <param name="Forward"></param>
        /// <param name="Backward"></param>
        /// <param name="Left"></param>
        /// <param name="Right"></param>
        /// <param name="Up"></param>
        /// <param name="Down"></param>
        /// <param name="Camera"></param>
        /// <param name="AngleX"></param>
        /// <param name="AngleY"></param>
        /// <param name="Speed"></param>
        /// <returns></returns>
        public bool MoveCamera(Key Forward, Key Backward, Key Left, Key Right, Key Up, Key Down, Camera Camera, float AngleX, float AngleY, float Speed)
        {
            try
            {
                if (KeyboardState[Forward])
                {
                    movement.X = -(float)Math.Cos(AngleX) * (float)Math.Sin(AngleY) * Speed;
                    movement.Y = (float)Math.Cos(AngleY) * Speed;
                    movement.Z = (float)Math.Sin(AngleX) * (float)Math.Sin(AngleY) * Speed;
                    Camera.Look = Camera.Look + movement;
                    Camera.Position = Camera.Position + movement;
                }
                if (KeyboardState[Backward])
                {
                    movement.X = (float)Math.Cos(AngleX) * (float)Math.Sin(AngleY) * Speed;
                    movement.Y = -(float)Math.Cos(AngleY) * Speed;
                    movement.Z = -(float)Math.Sin(AngleX) * (float)Math.Sin(AngleY) * Speed;
                    Camera.Look = Camera.Look + movement;
                    Camera.Position = Camera.Position + movement;
                }
                if (KeyboardState[Left])
                {
                    movement.X = -(float)Math.Sin(AngleX) * (float)Math.Sin(AngleY) * Speed;
                    movement.Y = 0;
                    movement.Z = -(float)Math.Cos(AngleX) * (float)Math.Sin(AngleY) * Speed;
                    Camera.Look = Camera.Look + movement;
                    Camera.Position = Camera.Position + movement;
                }
                if (KeyboardState[Right])
                {
                    movement.X = (float)Math.Sin(AngleX) * (float)Math.Sin(AngleY) * Speed;
                    movement.Y = 0;
                    movement.Z = (float)Math.Cos(AngleX) * (float)Math.Sin(AngleY) * Speed;
                    Camera.Look = Camera.Look + movement;
                    Camera.Position = Camera.Position + movement;
                }
                if (KeyboardState[Up])
                {
                    Camera.Position.Y = Camera.Position.Y + 1 * Speed;
                    Camera.Look.Y = Camera.Look.Y + 1 * Speed;
                }
                if (KeyboardState[Down])
                {
                    Camera.Position.Y = Camera.Position.Y - 1 * Speed;
                    Camera.Look.Y = Camera.Look.Y - 1 * Speed;
                }

                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    Error("OnMoveCamera");
                return false;
            }
        }

        /// <summary>
        /// Ritorna "true" se non ci sono errori nell'oggetto
        /// </summary>
        public bool AmICorrect
        {
            get { return correct; }
        }

        /// <summary>
        /// Aggiunge un errore alla lista di errori del LogiX Engine
        /// </summary>
        /// <param name="ErrorName"></param>
        void Error(string ErrorName)
        {
            correct = false;
            LXE_Errors.AddError("Mouse", ErrorName);
        }
    }

    #endregion

    #region "XEffect"

    /// <summary>
    /// Definisce un oggetto Effetto per la visualizzazione di effetti grafici avanzati
    /// </summary>
    public class XEffect : DevX
    {
        bool correct;
        Microsoft.DirectX.Direct3D.Effect Effect;
        EffectHandle[] EffectHandles;
        EffectHandle[] EffectTechniques;
        BaseTexture[] BaseTextures;
        CubeTexture[] CubeTextures;
        Matrix matrix;

        /// <summary>
        /// Inizializza un'istanza dell'oggetto XEffect
        /// </summary>
        public XEffect()
        {

        }

        #region "Default Effect"

        /// <summary>
        /// Inizializza un effetto base tra quelli possibili (definiti da LogiX_Technologies.XEffects). Da utilizzare all'esterno del GameLoop
        /// </summary>
        /// <param name="baseTexturePath"></param>
        /// <param name="normalTexturePath"></param>
        /// <param name="cubeMap"></param>
        /// <param name="DefaultEffects"></param>
        public void DefaultEffect(string baseTexturePath, string normalTexturePath, CubeMap cubeMap, XEffects DefaultEffects)
        {
            switch (DefaultEffects)
            {
                case XEffects.PerPixelDirectionalLight:
                    try
                    {
                        EffectPool pool = new EffectPool();
                        Effect = Microsoft.DirectX.Direct3D.Effect.FromFile(LogiX_Engine.Device, "PerPixelDirectionalLight.fx", null, ShaderFlags.None, pool);
                        EffectHandles = new EffectHandle[8];
                        EffectTechniques = new EffectHandle[1];
                        EffectTechniques[0] = Effect.GetTechnique("t0");
                        EffectHandles[0] = Effect.GetParameter(null, "transform");
                        EffectHandles[1] = Effect.GetParameter(null, "world");
                        EffectHandles[2] = Effect.GetParameter(null, "LightDirection");
                        EffectHandles[3] = Effect.GetParameter(null, "ViewDirection");
                        EffectHandles[4] = Effect.GetParameter(null, "base_Tex");
                        EffectHandles[5] = Effect.GetParameter(null, "Specular");
                        EffectHandles[6] = Effect.GetParameter(null, "Diffuse");
                        EffectHandles[7] = Effect.GetParameter(null, "SpecularPower");
                        BaseTextures = new BaseTexture[1];
                        BaseTextures[0] = TextureLoader.FromFile(LogiX_Engine.Device, baseTexturePath);
                        correct = true;
                    }
                    catch
                    {
                        Error("OnCreateObject");
                    }
                    break;
                case XEffects.PerPixelPointLight:
                    try
                    {
                        EffectPool pool = new EffectPool();
                        Effect = Microsoft.DirectX.Direct3D.Effect.FromFile(LogiX_Engine.Device, Application.StartupPath + "/PerPixelPointLight.fx", null, ShaderFlags.None, pool);
                        EffectHandles = new EffectHandle[11];
                        EffectTechniques = new EffectHandle[1];
                        EffectTechniques[0] = Effect.GetTechnique("PerPixelLightning");
                        EffectHandles[0] = Effect.GetParameter(null, "World");
                        EffectHandles[1] = Effect.GetParameter(null, "View");
                        EffectHandles[2] = Effect.GetParameter(null, "Projection");
                        EffectHandles[3] = Effect.GetParameter(null, "LightPosition");
                        EffectHandles[4] = Effect.GetParameter(null, "ReflectionRatio");
                        EffectHandles[5] = Effect.GetParameter(null, "SpecularRatio");
                        EffectHandles[6] = Effect.GetParameter(null, "SpecularStyleLerp");
                        EffectHandles[7] = Effect.GetParameter(null, "SpecularPower");
                        EffectHandles[8] = Effect.GetParameter(null, "LightColor");
                        EffectHandles[9] = Effect.GetParameter(null, "MyTex_Tex");
                        EffectHandles[10] = Effect.GetParameter(null, "myCubemap_Tex");
                        BaseTextures = new BaseTexture[1];
                        CubeTextures = new CubeTexture[1];
                        BaseTextures[0] = TextureLoader.FromFile(LogiX_Engine.Device, baseTexturePath);
                        CubeTextures[0] = cubeMap.DXCubeTexture;
                    }
                    catch
                    {
                        Error("OnCreateObject");
                    }
                    break;
                case XEffects.BumpPerPixelPointLight:
                    try
                    {
                        EffectPool pool = new EffectPool();
                        Effect = Microsoft.DirectX.Direct3D.Effect.FromFile(LogiX_Engine.Device, Application.StartupPath + "PerPixelPointLight.fx", null, ShaderFlags.None, pool);
                        EffectHandles = new EffectHandle[12];
                        EffectTechniques = new EffectHandle[1];
                        EffectTechniques[0] = Effect.GetTechnique("PerPixelLightning");
                        EffectHandles[0] = Effect.GetParameter(null, "World");
                        EffectHandles[1] = Effect.GetParameter(null, "View");
                        EffectHandles[2] = Effect.GetParameter(null, "Projection");
                        EffectHandles[3] = Effect.GetParameter(null, "LightPosition");
                        EffectHandles[4] = Effect.GetParameter(null, "ReflectionRatio");
                        EffectHandles[5] = Effect.GetParameter(null, "SpecularRatio");
                        EffectHandles[6] = Effect.GetParameter(null, "SpecularStyleLerp");
                        EffectHandles[7] = Effect.GetParameter(null, "SpecularPower");
                        EffectHandles[8] = Effect.GetParameter(null, "LightColor");
                        EffectHandles[9] = Effect.GetParameter(null, "MyTex_Tex");
                        EffectHandles[10] = Effect.GetParameter(null, "MyNTex_Tex");
                        EffectHandles[11] = Effect.GetParameter(null, "myCubemap_Tex");
                        BaseTextures = new BaseTexture[2];
                        CubeTextures = new CubeTexture[1];
                        BaseTextures[0] = TextureLoader.FromFile(LogiX_Engine.Device, baseTexturePath);
                        BaseTextures[1] = TextureLoader.FromFile(LogiX_Engine.Device, normalTexturePath);
                        CubeTextures[0] = cubeMap.DXCubeTexture;
                        correct = true;
                    }
                    catch
                    {
                        Error("OnCreateObject");
                    }
                    break;
                case XEffects.Reflect:
                    try
                    {
                        EffectPool pool = new EffectPool();
                        Effect = Microsoft.DirectX.Direct3D.Effect.FromFile(LogiX_Engine.Device, "Reflect.fx", null, ShaderFlags.None, pool);
                        EffectHandles = new EffectHandle[7];
                        EffectTechniques = new EffectHandle[1];
                        EffectTechniques[0] = Effect.GetTechnique("Textured_Bump");
                        EffectHandles[0] = Effect.GetParameter(null, "fvEyePosition");
                        EffectHandles[1] = Effect.GetParameter(null, "matView");
                        EffectHandles[2] = Effect.GetParameter(null, "matViewProjection");
                        EffectHandles[3] = Effect.GetParameter(null, "ReflectionRatio");
                        EffectHandles[4] = Effect.GetParameter(null, "bump_Tex");
                        EffectHandles[5] = Effect.GetParameter(null, "cube_Tex");
                        EffectHandles[6] = Effect.GetParameter(null, "Bump");
                        BaseTextures = new BaseTexture[1];
                        CubeTextures = new CubeTexture[1];
                        BaseTextures[0] = TextureLoader.FromFile(LogiX_Engine.Device, normalTexturePath);
                        CubeTextures[0] = cubeMap.DXCubeTexture;
                        correct = true;
                    }
                    catch
                    {
                        Error("OnCreateObject");
                    }
                    break;
            }
        }

        /// <summary>
        /// Attiva l'effetto base PerPixelDirectionalLight
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="LightDirection"></param>
        /// <param name="mat"></param>
        /// <returns></returns>
        public bool BeginPerPixelDirectionalLightEffect(Camera cam, VertexData LightDirection, XMaterial mat)
        {
            try
            {
                Effect.Begin(FX.None);
                matrix = LogiX_Engine.Device.Transform.View * LogiX_Engine.Device.Transform.Projection;
                Matrix.TransposeMatrix(matrix);
                Effect.SetValue(EffectHandles[0], matrix);
                matrix = LogiX_Engine.Device.Transform.World;
                Matrix.TransposeMatrix(matrix);
                Effect.SetValue(EffectHandles[1], matrix);
                Effect.SetValue(EffectHandles[2], new float[3] { -LightDirection.X, -LightDirection.Y, -LightDirection.Z });
                Effect.SetValue(EffectHandles[3], new float[3] { cam.Position.X, cam.Position.Y, cam.Position.Z });
                Effect.SetValue(EffectHandles[4], BaseTextures[0]);
                Effect.SetValue(EffectHandles[5], new float[4] { mat.Specular.R / 255, mat.Specular.G / 255, mat.Specular.B / 255, mat.Specular.A / 255 });
                Effect.SetValue(EffectHandles[6], new float[4] { mat.Diffuse.R / 255, mat.Diffuse.G / 255, mat.Diffuse.B / 255, mat.Diffuse.A / 255 });
                Effect.SetValue(EffectHandles[7], mat.SpecularSharpness);
                Effect.CommitChanges();
                Effect.BeginPass(0);
                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    Error("OnBeginEffect");
                return false;
            }
        }

        /// <summary>
        /// Attiva l'effetto base PerPixelPointLightEffect
        /// </summary>
        /// <param name="light"></param>
        /// <param name="specularPower"></param>
        /// <param name="specularStyleLerp"></param>
        /// <param name="reflectionRatio"></param>
        /// <param name="specularRatio"></param>
        /// <returns></returns>
        public bool BeginPerPixelPointLightEffect(PointLight light, int specularPower, float specularStyleLerp, float reflectionRatio, float specularRatio)
        {
            try
            {
                Effect.Begin(FX.None);
                matrix = LogiX_Engine.Device.Transform.View;
                Matrix.TransposeMatrix(matrix);
                Effect.SetValue(EffectHandles[0], matrix);
                matrix = LogiX_Engine.Device.Transform.World;
                Matrix.TransposeMatrix(matrix);
                Effect.SetValue(EffectHandles[1], matrix);
                matrix = LogiX_Engine.Device.Transform.Projection;
                Matrix.TransposeMatrix(matrix);
                Effect.SetValue(EffectHandles[2], matrix);
                Vector4 v = Vector3.Transform(new Vector3(light.Position.X, light.Position.Y, light.Position.Z), LogiX_Engine.Device.Transform.View);
                VertexData vd = light.Position;
                light.Position = new VertexData(v.X, v.Y, v.Z);
                Effect.SetValue(EffectHandles[3], new float[3] { light.Position.X, light.Position.Y, light.Position.Z });
                light.Position = vd;
                Effect.SetValue(EffectHandles[4], reflectionRatio);
                Effect.SetValue(EffectHandles[5], specularRatio);
                Effect.SetValue(EffectHandles[6], specularStyleLerp);
                Effect.SetValue(EffectHandles[7], specularPower);
                Effect.SetValue(EffectHandles[8], new float[4] { (float)light.Diffuse.R / 255, (float)light.Diffuse.G / 255, (float)light.Diffuse.B / 255, (float)light.Diffuse.A / 255 });
                Effect.SetValue(EffectHandles[9], BaseTextures[0]);
                Effect.SetValue(EffectHandles[10], CubeTextures[0]);
                Effect.CommitChanges();
                Effect.BeginPass(0);
                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    Error("OnBeginEffect");
                return false;
            }
        }

        /// <summary>
        /// Attiva l'effetto base PerPixelPointLight
        /// </summary>
        /// <param name="light"></param>
        /// <param name="specularPower"></param>
        /// <returns></returns>
        public bool BeginPerPixelPointLightEffect(PointLight light, int specularPower)
        {
            try
            {
                Effect.Begin(FX.None);
                matrix = LogiX_Engine.Device.Transform.View;
                Matrix.TransposeMatrix(matrix);
                Effect.SetValue(EffectHandles[0], matrix);
                matrix = LogiX_Engine.Device.Transform.World;
                Matrix.TransposeMatrix(matrix);
                Effect.SetValue(EffectHandles[1], matrix);
                matrix = LogiX_Engine.Device.Transform.Projection;
                Matrix.TransposeMatrix(matrix);
                Effect.SetValue(EffectHandles[2], matrix);
                Vector4 v = Vector3.Transform(new Vector3(light.Position.X, light.Position.Y, light.Position.Z), LogiX_Engine.Device.Transform.View);
                VertexData vd = light.Position;
                light.Position = new VertexData(v.X, v.Y, v.Z);
                Effect.SetValue(EffectHandles[3], new float[3] { light.Position.X, light.Position.Y, light.Position.Z });
                light.Position = vd;
                Effect.SetValue(EffectHandles[4], 0);
                Effect.SetValue(EffectHandles[5], 0.15f);
                Effect.SetValue(EffectHandles[6], 1.01f);
                Effect.SetValue(EffectHandles[7], specularPower);
                Effect.SetValue(EffectHandles[8], new float[4] { (float)light.Diffuse.R / 255, (float)light.Diffuse.G / 255, (float)light.Diffuse.B / 255, (float)light.Diffuse.A / 255 });
                Effect.SetValue(EffectHandles[9], BaseTextures[0]);
                Effect.CommitChanges();
                Effect.BeginPass(0);
                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    Error("OnBeginEffect");
                return false;
            }
        }

        /// <summary>
        /// Attiva l'effetto base BumpPerPixelPointLight
        /// </summary>
        /// <param name="light"></param>
        /// <param name="specularPower"></param>
        /// <param name="specularStyleLerp"></param>
        /// <param name="reflectionRatio"></param>
        /// <param name="specularRatio"></param>
        /// <returns></returns>
        public bool BeginBumpPerPixelPointLightEffect(PointLight light, int specularPower, float specularStyleLerp, float reflectionRatio, float specularRatio)
        {
            try
            {
                Effect.Begin(FX.None);
                matrix = LogiX_Engine.Device.Transform.View;
                Matrix.TransposeMatrix(matrix);
                Effect.SetValue(EffectHandles[0], matrix);
                matrix = LogiX_Engine.Device.Transform.World;
                Matrix.TransposeMatrix(matrix);
                Effect.SetValue(EffectHandles[1], matrix);
                matrix = LogiX_Engine.Device.Transform.Projection;
                Matrix.TransposeMatrix(matrix);
                Effect.SetValue(EffectHandles[2], matrix);
                Vector4 v = Vector3.Transform(new Vector3(light.Position.X, light.Position.Y, light.Position.Z), LogiX_Engine.Device.Transform.View);
                VertexData vd = light.Position;
                light.Position = new VertexData(v.X, v.Y, v.Z);
                Effect.SetValue(EffectHandles[3], new float[3] { light.Position.X, light.Position.Y, light.Position.Z });
                light.Position = vd;
                Effect.SetValue(EffectHandles[4], reflectionRatio);
                Effect.SetValue(EffectHandles[5], specularRatio);
                Effect.SetValue(EffectHandles[6], specularStyleLerp);
                Effect.SetValue(EffectHandles[7], specularPower);
                Effect.SetValue(EffectHandles[8], new float[4] { (float)light.Diffuse.R / 255, (float)light.Diffuse.G / 255, (float)light.Diffuse.B / 255, (float)light.Diffuse.A / 255 });
                Effect.SetValue(EffectHandles[9], BaseTextures[0]);
                Effect.SetValue(EffectHandles[10], BaseTextures[1]);
                Effect.SetValue(EffectHandles[11], CubeTextures[0]);
                Effect.CommitChanges();
                Effect.BeginPass(0);
                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    Error("OnBeginEffect");
                return false;
            }
        }

        /// <summary>
        /// Attiva l'effetto base BumpPerPixelPointLight
        /// </summary>
        /// <param name="light"></param>
        /// <param name="specularPower"></param>
        /// <returns></returns>
        public bool BeginBumpPerPixelPointLightEffect(PointLight light, int specularPower)
        {
            try
            {
                Effect.Begin(FX.None);
                matrix = LogiX_Engine.Device.Transform.View;
                Matrix.TransposeMatrix(matrix);
                Effect.SetValue(EffectHandles[0], matrix);
                matrix = LogiX_Engine.Device.Transform.World;
                Matrix.TransposeMatrix(matrix);
                Effect.SetValue(EffectHandles[1], matrix);
                matrix = LogiX_Engine.Device.Transform.Projection;
                Matrix.TransposeMatrix(matrix);
                Effect.SetValue(EffectHandles[2], matrix);
                Vector4 v = Vector3.Transform(new Vector3(light.Position.X, light.Position.Y, light.Position.Z), LogiX_Engine.Device.Transform.View);
                VertexData vd = light.Position;
                light.Position = new VertexData(v.X, v.Y, v.Z);
                Effect.SetValue(EffectHandles[3], new float[3] { light.Position.X, light.Position.Y, light.Position.Z });
                light.Position = vd;
                Effect.SetValue(EffectHandles[4], 0);
                Effect.SetValue(EffectHandles[5], 0.15f);
                Effect.SetValue(EffectHandles[6], 1.01f);
                Effect.SetValue(EffectHandles[7], specularPower);
                Effect.SetValue(EffectHandles[8], new float[4] { (float)light.Diffuse.R / 255, (float)light.Diffuse.G / 255, (float)light.Diffuse.B / 255, (float)light.Diffuse.A / 255 });
                Effect.SetValue(EffectHandles[9], BaseTextures[0]);
                Effect.CommitChanges();
                Effect.BeginPass(0);
                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    Error("OnBeginEffect");
                return false;
            }
        }

        /// <summary>
        /// Attiva l'effetto base Reflect
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="ReflectionRatio"></param>
        /// <param name="BumpMapped"></param>
        /// <returns></returns>
        public bool BeginReflectEffect(Camera cam, float ReflectionRatio, bool BumpMapped)
        {
            try
            {
                Effect.Begin(FX.None);
                Effect.SetValue(EffectHandles[0], new float[3] { cam.Position.X, cam.Position.Y, cam.Position.Z });
                matrix = LogiX_Engine.Device.Transform.View;
                Matrix.TransposeMatrix(matrix);
                Effect.SetValue(EffectHandles[1], matrix);
                matrix = LogiX_Engine.Device.Transform.View;
                matrix = matrix * LogiX_Engine.Device.Transform.Projection;
                Matrix.TransposeMatrix(matrix);
                Effect.SetValue(EffectHandles[2], matrix);
                Effect.SetValue(EffectHandles[3], ReflectionRatio);
                Effect.SetValue(EffectHandles[4], BaseTextures[0]);
                Effect.SetValue(EffectHandles[5], CubeTextures[0]);
                Effect.SetValue(EffectHandles[6], BumpMapped);
                Effect.CommitChanges();
                Effect.BeginPass(0);
                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    Error("OnBeginEffect");
                return false;
            }
        }

        /// <summary>
        /// Attiva l'effetto base SkyBox
        /// </summary>
        /// <returns></returns>
        public bool BeginSkyBox()
        {
            try
            {
                Effect.Begin(FX.None);
                matrix = LogiX_Engine.Device.Transform.World * LogiX_Engine.Device.Transform.View * LogiX_Engine.Device.Transform.Projection;
                Matrix.Invert(matrix);
                Effect.SetValue(EffectHandles[0], matrix);
                Effect.SetValue(EffectHandles[1], CubeTextures[0]);
                Effect.CommitChanges();
                Effect.BeginPass(0);
                correct = true;
                return true;
            }
            catch
            {
                correct = false;
                return false;
            }
        }

        #endregion

        #region "Water Effect"

        /// <summary>
        /// Inizializza l'effetto avanzato Water (da utilizzare all'esterno del GameLoop)
        /// </summary>
        /// <param name="baseTex"></param>
        /// <param name="backTex"></param>
        /// <param name="normal1Tex"></param>
        /// <param name="worldTex"></param>
        /// <param name="terraIncognitaTex"></param>
        /// <param name="normal2Tex"></param>
        /// <param name="normal3Tex"></param>
        public void WaterEffect(string baseTex, string backTex, string normal1Tex, string worldTex, string terraIncognitaTex, string normal2Tex, string normal3Tex)
        {
            try
            {
                EffectPool pool = new EffectPool();
                Effect = Microsoft.DirectX.Direct3D.Effect.FromFile(LogiX_Engine.Device, "Water.fx", null, ShaderFlags.None, pool);
                EffectHandles = new EffectHandle[16];
                EffectTechniques = new EffectHandle[1];
                EffectTechniques[0] = Effect.GetTechnique("Water");
                EffectHandles[0] = Effect.GetParameter(null, "matWorldViewProjection");
                EffectHandles[1] = Effect.GetParameter(null, "LightPosition");
                EffectHandles[2] = Effect.GetParameter(null, "CameraPosition");
                EffectHandles[3] = Effect.GetParameter(null, "Time");
                EffectHandles[4] = Effect.GetParameter(null, "vAlpha");
                EffectHandles[5] = Effect.GetParameter(null, "Wave");
                EffectHandles[6] = Effect.GetParameter(null, "waveSpeed");
                EffectHandles[7] = Effect.GetParameter(null, "WaveLenght");
                EffectHandles[8] = Effect.GetParameter(null, "TexturesDimension");
                EffectHandles[9] = Effect.GetParameter(null, "BaseTex");
                EffectHandles[10] = Effect.GetParameter(null, "BackTex");
                EffectHandles[11] = Effect.GetParameter(null, "Normal1Tex");
                EffectHandles[12] = Effect.GetParameter(null, "WorldTex");
                EffectHandles[13] = Effect.GetParameter(null, "TerraIncognitaTex");
                EffectHandles[14] = Effect.GetParameter(null, "Normal2Tex");
                EffectHandles[15] = Effect.GetParameter(null, "Normal3Tex");

                BaseTextures = new BaseTexture[7];
                BaseTextures[0] = TextureLoader.FromFile(LogiX_Engine.Device, baseTex);
                BaseTextures[1] = TextureLoader.FromFile(LogiX_Engine.Device, backTex);
                BaseTextures[2] = TextureLoader.FromFile(LogiX_Engine.Device, normal1Tex);
                BaseTextures[3] = TextureLoader.FromFile(LogiX_Engine.Device, worldTex);
                BaseTextures[4] = TextureLoader.FromFile(LogiX_Engine.Device, terraIncognitaTex);
                BaseTextures[5] = TextureLoader.FromFile(LogiX_Engine.Device, normal2Tex);
                BaseTextures[6] = TextureLoader.FromFile(LogiX_Engine.Device, normal3Tex);

                correct = true;
            }
            catch
            {
                Error("OnCreateObject");
            }
        }

        /// <summary>
        /// Attiva l'effetto avanzato Water
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="LightPosition"></param>
        /// <param name="vAlpha"></param>
        /// <param name="WaveHeight"></param>
        /// <param name="WaveSpeed"></param>
        /// <param name="WaveLenght"></param>
        /// <param name="TexturesDimension"></param>
        /// <returns></returns>
        public bool BeginWaterEffect(Camera cam, VertexData LightPosition, float vAlpha, float WaveHeight, float WaveSpeed, float WaveLenght, float TexturesDimension)
        {
            try
            {
                Effect.Begin(FX.None);
                matrix = LogiX_Engine.Device.Transform.View * LogiX_Engine.Device.Transform.Projection * LogiX_Engine.Device.Transform.World;
                Matrix.TransposeMatrix(matrix);
                Effect.SetValue(EffectHandles[0], matrix);
                Effect.SetValue(EffectHandles[1], new float[3] { LightPosition.X, LightPosition.Y, LightPosition.Z });
                Effect.SetValue(EffectHandles[2], new float[4] { cam.Position.X, cam.Position.Y, cam.Position.Z, 0 });
                Effect.SetValue(EffectHandles[3], (float)Environment.TickCount / 1000);
                Effect.SetValue(EffectHandles[4], vAlpha);
                Effect.SetValue(EffectHandles[5], WaveHeight);
                Effect.SetValue(EffectHandles[6], WaveSpeed);
                Effect.SetValue(EffectHandles[7], WaveLenght);
                Effect.SetValue(EffectHandles[8], TexturesDimension);
                Effect.SetValue(EffectHandles[9], BaseTextures[0]);
                Effect.SetValue(EffectHandles[10], BaseTextures[1]);
                Effect.SetValue(EffectHandles[11], BaseTextures[2]);
                Effect.SetValue(EffectHandles[12], BaseTextures[3]);
                Effect.SetValue(EffectHandles[13], BaseTextures[4]);
                Effect.SetValue(EffectHandles[14], BaseTextures[5]);
                Effect.SetValue(EffectHandles[15], BaseTextures[6]);
                Effect.CommitChanges();
                Effect.BeginPass(0);
                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    Error("OnBeginEffect");
                return false;
            }
        }


        #endregion

        #region "Glass Effect"

        /// <summary>
        /// Inizializza l'effetto avanzato Glass (da utilizzare all'esterno del GameLoop)
        /// </summary>
        /// <param name="cubeMap"></param>
        public void GlassEffect(CubeMap cubeMap)
        {
            try
            {
                EffectPool pool = new EffectPool();
                Effect = Microsoft.DirectX.Direct3D.Effect.FromFile(LogiX_Engine.Device, "MEDIA//Glass.fx", null, ShaderFlags.None, pool);
                EffectHandles = new EffectHandle[10];
                EffectTechniques = new EffectHandle[1];
                EffectTechniques[0] = Effect.GetTechnique("Glass");
                EffectHandles[0] = Effect.GetParameter(null, "matViewProjection");
                EffectHandles[1] = Effect.GetParameter(null, "PointOfView");
                EffectHandles[2] = Effect.GetParameter(null, "refractionScale");
                EffectHandles[3] = Effect.GetParameter(null, "baseColor");
                EffectHandles[4] = Effect.GetParameter(null, "ambient");
                EffectHandles[5] = Effect.GetParameter(null, "indexOfRefractionRatio");
                EffectHandles[6] = Effect.GetParameter(null, "reflectionScale");
                EffectHandles[7] = Effect.GetParameter(null, "texCube");
                EffectHandles[8] = Effect.GetParameter(null, "alpha");
                EffectHandles[9] = Effect.GetParameter(null, "matWorld");

                CubeTextures = new CubeTexture[1];
                CubeTextures[0] = cubeMap.DXCubeTexture;

                correct = true;
            }
            catch
            {
                Error("OnCreateObject");
            }

        }

        /// <summary>
        /// Attiva l'effetto avanzato Glass
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="BaseColor"></param>
        /// <param name="RefractionScale"></param>
        /// <param name="Ambient"></param>
        /// <param name="IndexOfRefracrionRatio"></param>
        /// <param name="ReflectionScale"></param>
        /// <param name="Alpha"></param>
        /// <returns></returns>
        public bool BeginGlassEffect(Camera cam, Color BaseColor, float RefractionScale, float Ambient, float IndexOfRefracrionRatio, float ReflectionScale, float Alpha)
        {
            try
            {
                Effect.Begin(FX.None);
                matrix = LogiX_Engine.device.Transform.View * LogiX_Engine.device.Transform.Projection;
                Matrix.TransposeMatrix(matrix);
                Effect.SetValue(EffectHandles[0], matrix);
                Effect.SetValue(EffectHandles[1], new float[4] { cam.Position.X, cam.Position.Y, cam.Position.Z, 1.0f });
                Effect.SetValue(EffectHandles[2], RefractionScale);
                Effect.SetValue(EffectHandles[3], BaseColor.ToArgb());
                Effect.SetValue(EffectHandles[4], Ambient);
                Effect.SetValue(EffectHandles[5], IndexOfRefracrionRatio);
                Effect.SetValue(EffectHandles[6], ReflectionScale);
                Effect.SetValue(EffectHandles[7], CubeTextures[0]);
                Effect.SetValue(EffectHandles[8], Alpha);
                matrix = LogiX_Engine.device.Transform.World;
                Matrix.TransposeMatrix(matrix);
                Effect.SetValue(EffectHandles[9], matrix);
                Effect.CommitChanges();
                Effect.BeginPass(0);
                return true;
            }
            catch
            {
                Error("OnBeginEffect");
                return false;
            }
        }

        #endregion

        /// <summary>
        /// Disattiva l'effetto prima attivato
        /// </summary>
        /// <returns></returns>
        public bool EndEffect()
        {
            try
            {
                Effect.EndPass();
                Effect.End();
                return true;
            }
            catch
            {
                if (AmICorrect == true)
                    Error("OnEndEffect");
                return false;
            }
        }

        /// <summary>
        /// Ritorna "true" se non ci sono errori nell'oggetto
        /// </summary>
        public bool AmICorrect
        {
            get { return correct; }
            set { correct = value; }
        }

        /// <summary>
        /// Aggiunge un errore alla lista di errori del LogiX Engine
        /// </summary>
        /// <param name="ErrorName"></param>
        void Error(string ErrorName)
        {
            correct = false;
            LXE_Errors.AddError("XEffect", ErrorName);
        }
    }

    #endregion

    #region "CubeMap"

    /// <summary>
    /// Definisce un oggetto CubeMap per l'applicazione di esso agli effetti
    /// </summary>
    public class CubeMap : DevX
    {
        string path;
        string error;
        bool correct;
        CubeTexture cube;
        private RenderToEnvironmentMap renderToEnvy;
        private Viewport viewport;
        private Viewport backViewPort;
        public delegate void RenderFunction();
        private Camera cam;

        /// <summary>
        /// Inizializza un oggetto CubeMap caricandolo da un file esterno in formato "dds" 
        /// </summary>
        /// <param name="Path"></param>
        public CubeMap(string Path)
        {
            try
            {
                path = Path;
                cube = TextureLoader.FromCubeFile(LogiX_Engine.Device, Path);
                correct = true;
            }
            catch
            {
                Error("OnLoadCubeMap");
            }
        }

        /// <summary>
        /// Inizializza un'istanza dell'oggetto CubeMap
        /// </summary>
        public CubeMap()
        {
            correct = true;
        }

        /// <summary>
        /// Predispone l'oggetto CubeMap ad essere generato dall'ambiente circostante (Rendering to target) (da utilizzare all'esterno del GameLoop)
        /// </summary>
        /// <param name="EdgeLength"></param>
        /// <returns></returns>
        public bool InitializeRenderToCube(int EdgeLength)
        {
            try
            {
                viewport = new Viewport();
                viewport.MaxZ = 1;
                viewport.Width = EdgeLength;
                viewport.Height = EdgeLength;
                LogiX_Engine.Device.RenderState.NormalizeNormals = true;
                cube = new CubeTexture(LogiX_Engine.Device, EdgeLength, 1, Usage.RenderTarget, Format.X8R8G8B8, Pool.Default);
                renderToEnvy = new RenderToEnvironmentMap(LogiX_Engine.Device, EdgeLength, 1, Format.X8R8G8B8, true, DepthFormat.D16);
                cam = new Camera(new VertexData(0, 0, 0), new VertexData(0, 0, 0));
                cam.FarPlane = 100000;
                cam.AspectRatio = 1;
                cam.FieldOfView = (float)Math.PI / 2;
                path = null;
                correct = true;
                return true;
            }
            catch
            {
                Error("OnInitializeRenderToCube");
                return false;
            }
        }

        /// <summary>
        /// Inizializza la generazione (tramite rendering to target) della CubeMap. (Va utilizzato nel GameLoop prima dell'inizio del frame [specificato da LogiX_Technologies.LogiX_Engine.StartRender()] o dopo la fine del frame [specificata da LogiX_Technologies.LogiX_Engine.EndRender()])
        /// </summary>
        /// <param name="function"></param>
        /// <param name="CenterOfCube"></param>
        /// <returns></returns>
        public bool RenderToCube(RenderFunction function, VertexData CenterOfCube)
        {
            try
            {
                renderToEnvy.BeginCube(cube);
                backViewPort = LogiX_Engine.Device.Viewport;
                LogiX_Engine.Device.Viewport = viewport;
                Vector3 v = new Vector3();
                Vector3 upV = new Vector3();
                for (int i = 0; i < 6; i++)
                {
                    switch (i)
                    {
                        case (int)CubeMapFace.PositiveX:
                            cam.Position = CenterOfCube;
                            cam.Look = new VertexData(1, 0, 0) + CenterOfCube;
                            cam.UpVector = new VertexData(0, 1, 0);
                            break;
                        case (int)CubeMapFace.NegativeX:
                            cam.Position = CenterOfCube;
                            cam.Look = new VertexData(-1, 0, 0) + CenterOfCube;
                            cam.UpVector = new VertexData(0, 1, 0);
                            break;
                        case (int)CubeMapFace.PositiveY:
                            cam.Position = CenterOfCube;
                            cam.Look = new VertexData(0, 1, 0) + CenterOfCube;
                            cam.UpVector = new VertexData(0, 0, -1);
                            break;
                        case (int)CubeMapFace.NegativeY:
                            cam.Position = CenterOfCube;
                            cam.Look = new VertexData(0, -1, 0) + CenterOfCube;
                            cam.UpVector = new VertexData(0, 0, 1);
                            break;
                        case (int)CubeMapFace.PositiveZ:
                            cam.Position = CenterOfCube;
                            cam.Look = new VertexData(0, 0, 1) + CenterOfCube;
                            cam.UpVector = new VertexData(0, 1, 0);
                            break;
                        case (int)CubeMapFace.NegativeZ:
                            cam.Position = CenterOfCube;
                            cam.Look = new VertexData(0, 0, -1) + CenterOfCube;
                            cam.UpVector = new VertexData(0, 1, 0);
                            break;
                    }
                    cam.Capture();
                    SetFace((CubeMapFace)i);
                    function.Invoke();
                }
                renderToEnvy.End(1);
                LogiX_Engine.Device.Viewport = backViewPort;

                return true;
            }
            catch
            {
                Error("OnRenderToCube");
                return false;
            }
        }

        void SetFace(CubeMapFace face)
        {
            renderToEnvy.Face(face, 1);
        }

        /// <summary>
        /// Ritorna il percorso della CubeMap (in formato "dds") da importare o già importata.
        /// </summary>
        public string Path
        {
            get { return path; }
        }

        /// <summary>
        /// Ritorna il percorso della CubeMap importata o un messaggio di autogenerazione della CubeMap.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Path == null || Path == "")
            {
                return Path;
            }
            else
            {
                return "Auto-Generated CubeMap";
            }
        }

        /// <summary>
        /// Ritorna "true" se non ci sono errori nell'oggetto
        /// </summary>
        public bool AmICorrect
        {
            get { return correct; }
        }

        public CubeTexture DXCubeTexture
        {
            get { return cube; }
        }

        public RenderToEnvironmentMap RenderToEnvy
        {
            get { return renderToEnvy; }
        }

        /// <summary>
        /// Aggiunge un errore alla lista di errori del LogiX Engine
        /// </summary>
        /// <param name="ErrorName"></param>
        void Error(string ErrorName)
        {
            correct = false;
            LXE_Errors.AddError("CubeMap", ErrorName);
        }
    }

    #endregion

    #region "VertexData"
    /// <summary>
    /// Contenitore di dati float per le coordinate di punti nello spazio cartesiano.
    /// </summary>
    public class VertexData
    {
        private float my_x;
        private float my_y;
        private float my_z;

        /// <summary>
        /// Inizializza un oggetto VertexData.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public VertexData(float x, float y, float z)
        {
            my_x = x;
            my_y = y;
            my_z = z;
        }

        /// <summary>
        /// Ritorna o imposta la componente X del VertexData.
        /// </summary>
        public float X
        {
            get { return my_x; }
            set { my_x = value; }
        }

        /// <summary>
        /// Ritorna o imposta la componente Y del VertexData.
        /// </summary>
        public float Y
        {
            get { return my_y; }
            set { my_y = value; }
        }

        /// <summary>
        /// Ritorna o imposta la componente Z del VertexData.
        /// </summary>
        public float Z
        {
            get { return my_z; }
            set { my_z = value; }
        }

        /// <summary>
        /// Ritorna le coordinate cartesiane del VertexData.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "X = " + X.ToString() + "  Y = " + Y.ToString() + "  Z = " + Z.ToString();
        }

        /// <summary>
        /// Somma due VertexData.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        static public VertexData operator +(VertexData v1, VertexData v2)
        {
            return new VertexData(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        /// <summary>
        /// Sottrae due VertexData.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        static public VertexData operator -(VertexData v1, VertexData v2)
        {
            return new VertexData(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        /// <summary>
        /// Ritorna l'opposto del VertexData.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        static public VertexData operator -(VertexData v)
        {
            return new VertexData(-v.X, -v.Y, -v.Z);
        }

        /// <summary>
        /// Moltiplica un VertexData per un valore float.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        static public VertexData operator *(VertexData v, float i)
        {
            return new VertexData(v.X * i, v.Y * i, v.Z * i);
        }

        /// <summary>
        /// Moltiplica un VertexData per un valore int.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        static public VertexData operator *(VertexData v, int i)
        {
            return new VertexData(v.X * i, v.Y * i, v.Z * i);
        }

        /// <summary>
        /// Divide un VertexData per un valore float.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        static public VertexData operator /(VertexData v, float i)
        {
            return new VertexData(v.X / i, v.Y / i, v.Z / i);
        }

        /// <summary>
        /// Divide un VertexData per un valore int.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        static public VertexData operator /(VertexData v, int i)
        {
            return new VertexData(v.X / i, v.Y / i, v.Z / i);
        }

        /// <summary>
        /// Ritorna un oggetto VertexData vuoto.
        /// </summary>
        static public VertexData Empty
        {
            get { return new VertexData(0, 0, 0); }
        }

        /// <summary>
        /// Normalizza un VertexData.
        /// </summary>
        /// <param name="V"></param>
        /// <returns></returns>
        static public VertexData Normalize(VertexData V)
        {
            VertexData n;
            float f;
            f = 1 / ((float)Math.Sqrt((Math.Pow(V.X, 2) + Math.Pow(V.Y, 2) + Math.Pow(V.Z, 2))));
            n = V * f;
            return n;
        }

        /// <summary>
        /// Ritorna la distanza tra due punti le cui posizioni cartesiane sono definite da rispettivi VertexData
        /// </summary>
        /// <param name="V1"></param>
        /// <param name="V2"></param>
        /// <returns></returns>
        static public float Distance(VertexData V1, VertexData V2)
        {
            float f;
            f = (float)Math.Sqrt((Math.Pow((V1.X - V2.X), 2) + Math.Pow((V1.Y - V2.Y), 2) + Math.Pow((V1.Z - V2.Z), 2)));
            return f;
        }
    }
    #endregion

}