using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media.Imaging;
using MentorSystemWebRTC.MentorClasses;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.Storage;
using Windows.Graphics.Imaging;
using System.Text;
using Windows.UI.ViewManagement;
using Windows.Storage.Pickers;
using Windows.Media.Editing;

using Org.WebRtc;
using Windows.Media.Playback;
using Windows.Media.Core;
using WSAUnity;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MentorSystemWebRTC
{
    /// <summary>   
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        StarWebrtcContext starWebrtcContext;
        MediaPlayer _mediaPlayer;

        static double PI = 3.14159265358979323846f;

        private ControlCenter myController;
        private JSONManager myJsonManager;

        private SolidColorBrush redColor;
        private SolidColorBrush greenColor;
        private SolidColorBrush tealColor;
        private SolidColorBrush goldColor;
        private SolidColorBrush buttonCheckedColor;
        private SolidColorBrush buttonUncheckedColor;

        private Polyline LineAnnotation;
        private int AnnotationCounter;
        private int AnnotationThreshold;

        ScaleTransform transformationHandler = new ScaleTransform();

        ///////////////// Variables for the socket communication
        private StreamSocketListener tcpVideoListener;
        private const string videoPort = "8900";
        private StreamSocketListener tcpJsonListener;
        private const string jsonPort = "8988";
        StorageFolder rootFolder = ApplicationData.Current.LocalFolder;
        private bool connectionHappened = false;
        ///////////////////////

        Point newPointInBackgroundImage;

        public MainPage()
        {
            this.InitializeComponent();
            myController = new ControlCenter();
            myJsonManager = new JSONManager();

            Debug.WriteLine("MainPage()");

            redColor = new SolidColorBrush(Windows.UI.Colors.Red);
            greenColor = new SolidColorBrush(Windows.UI.Colors.Green);
            tealColor = new SolidColorBrush(Windows.UI.Colors.Teal);
            goldColor = new SolidColorBrush(Windows.UI.Colors.Gold);
            CreateButtonsColor();

            AnnotationCounter = 0;
            AnnotationThreshold = 0;
            ResetLineAnnotation();

            deleteTempFiles();

            //ColoredRectangle.PointerEntered += EnteringRectangle;

            //ColoredRectangle.Holding += HoldingRectangle;

            //////////////Threads//////////////
            Task jsonTask = Task.Run(() => JsonThread());
            ////////////////////////////////////

            _mediaPlayer = new MediaPlayer();
            mediaPlayerElement.SetMediaPlayer(_mediaPlayer);

            //DECOMMENT
            starWebrtcContext = StarWebrtcContext.CreateMentorContext(); 
            // right after creating the context (before starting the connections), we could edit some parameters such as the signalling server

            // comment these out if not needed
            Messenger.AddListener<string>(SympleLog.LogTrace, OnLog);
            Messenger.AddListener<string>(SympleLog.LogDebug, OnLog);
            Messenger.AddListener<string>(SympleLog.LogInfo, OnLog);
            Messenger.AddListener<string>(SympleLog.LogError, OnLog);

            Messenger.AddListener<IMediaSource>(SympleLog.CreatedMediaSource, OnCreatedMediaSource);
            Messenger.AddListener(SympleLog.DestroyedMediaSource, OnDestroyedMediaSource);
            Messenger.AddListener(SympleLog.RemoteAnnotationReceiverConnected, OnRemoteAnnotationReceiverConnected);
            Messenger.AddListener(SympleLog.RemoteAnnotationReceiverDisconnected, OnRemoteAnnotationReceiverDisconnected);

            starWebrtcContext.initAndStartWebRTC();

            mediaPlayerElement.ManipulationMode = ManipulationModes.Scale;

            BackgroundImage.RenderTransform = transformationHandler;
            mediaPlayerElement.RenderTransform = transformationHandler;
            imagesPanel.RenderTransform = transformationHandler;
            drawingPanel.RenderTransform = transformationHandler;

            myJsonManager.starWebrtcContext = starWebrtcContext;
        }

        private void OnDestroyedMediaSource()
        {
            Messenger.Broadcast(SympleLog.LogDebug, "OnDestroyedMediaSource");
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                /*
                MediaSource currentSource = _mediaPlayer.Source as MediaSource;
                if (currentSource != null)
                {
                    currentSource.Dispose();
                }
                */

                _mediaPlayer.Source = null;
            }
            );

        }

        private void OnCreatedMediaSource(IMediaSource source)
        {
            Messenger.Broadcast(SympleLog.LogDebug, "OnCreatedMediaSource");
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                MediaSource createdSource = MediaSource.CreateFromIMediaSource(source);

                _mediaPlayer.Source = createdSource;
                _mediaPlayer.Play();
            }
            );

        }

        private void OnLog(string msg)
        {
            Debug.WriteLine(msg);

            // http://stackoverflow.com/questions/19341591/the-application-called-an-interface-that-was-marshalled-for-a-different-thread
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                // Your UI update code goes here!
                Debug.WriteLine(msg + "\n");
            }
            );

        }

        private void OnRemoteAnnotationReceiverConnected()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                myJsonManager.JSONThroughWebRTC = true;
            }
            );
        }

        private void OnRemoteAnnotationReceiverDisconnected()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                myJsonManager.JSONThroughWebRTC = false;
            }
            );
        }

        /*private async void button_Click(object sender, RoutedEventArgs e)
        {
            buttonWebRTC.IsEnabled = false;

            starWebrtcContext.initAndStartWebRTC();
        }*/

        private async void OnConnected(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            Debug.WriteLine("Got Connected");
            connectionHappened = true;
            DataReader reader = new DataReader(args.Socket.InputStream);
            uint tabletResX = 640;
            uint tabletResY = 400;//400 for tablet //480 for drone
            uint orgNumChan = 3;
            uint targetNumChan = 4;
            uint orgRes = tabletResX * tabletResY * orgNumChan; 
            uint targetRes = tabletResX * tabletResY * targetNumChan;
            byte[] myInfo = new byte[orgRes];
            int deletionCntr = 0;

            try
            {
                while (true)
                {
                    // Read first 4 bytes (length of the subsequent string).
                    uint sizeFieldCount = await reader.LoadAsync(sizeof(byte) * orgRes);
                    if (sizeFieldCount != sizeof(byte) * orgRes)
                    {
                        // The underlying socket was closed before we were able to read the whole data.
                        return;
                    }
                    // Read the string.
                    reader.ReadBytes(myInfo);

                    byte[] newBytes = new byte[targetRes];
                    int position = 0;
                    for (int thiss = 0; thiss < orgRes;)
                    {
                        newBytes[position] = myInfo[thiss];
                        newBytes[position + 1] = myInfo[thiss + 1];
                        newBytes[position + 2] = myInfo[thiss + 2];
                        newBytes[position + 3] = 0;
                        position += 4;
                        thiss += 3;
                    }

                    if (deletionCntr == 10)
                    {
                        deleteTempFiles();
                        deletionCntr = 0;
                    }

                    // Display the string on the screen. The event is invoked on a non-UI thread, so we need to marshal
                    // the text back to the UI thread.
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    async () =>
                    {
                        Guid BitmapEncoderGuid = BitmapEncoder.PngEncoderId;

                        var file = await rootFolder.CreateFileAsync("Image.png", CreationCollisionOption.GenerateUniqueName);

                        using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                        {
                            BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoderGuid, stream);
                            encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, tabletResX, tabletResY, 96.0, 96.0, newBytes);
                            encoder.BitmapTransform.ScaledHeight = 1080;
                            encoder.BitmapTransform.ScaledWidth = 1920;
                            await encoder.FlushAsync();
                        }
                        using (var myStream = await file.OpenAsync(FileAccessMode.Read))
                        {
                            BitmapImage toDisplay = new BitmapImage();
                            toDisplay.SetSource(myStream);
                            BackgroundImage.Source = toDisplay;
                        }
                    });

                    deletionCntr++;
                }
            }
            catch (Exception exception)
            {
                // If this is an unknown status it means that the error is fatal and retry will likely fail.
            }
        }

        private async void jsonOnConnected(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            Debug.WriteLine("Also entered here");
            myJsonManager.receiveSocket(args.Socket);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Debug.WriteLine("Waiting for client to connect...");

            tcpVideoListener = new StreamSocketListener();
            tcpVideoListener.ConnectionReceived += OnConnected;
            await tcpVideoListener.BindEndpointAsync(null, videoPort);

            tcpJsonListener = new StreamSocketListener();
            tcpJsonListener.ConnectionReceived += jsonOnConnected;
            await tcpJsonListener.BindEndpointAsync(null, jsonPort);
        }
        ////////////////////////////////////////////////////////////////////// END OF SOCKET CODE

        private void iconPanelImageTapped(object sender, TappedRoutedEventArgs e)
        {
            buttonLines.IsChecked = false;
            buttonPoints.IsChecked = false;
            LinesButtonUnchecked(sender, e);
            PointsButtonUnchecked(sender, e);

            Image selectedImage = sender as Image;
            BitmapImage selectedSource = selectedImage.Source as BitmapImage;
            Uri selectedUri = selectedSource.UriSource;

            myController.SetIconAnnotationSelectedFlag(true);
            myController.SetSelectedIconPath(selectedUri.AbsoluteUri);
        }

        private void LineStarting(object sender, PointerRoutedEventArgs e)
        {
            newPointInBackgroundImage = new Point(e.GetCurrentPoint(BackgroundImage).Position.X, e.GetCurrentPoint(BackgroundImage).Position.Y);

            if (buttonLines.IsChecked.Value)
            {
                if(LineAnnotation.Points.Count > 0)
                {
                    // Prepare annotation to send
                    /*myJsonManager.createJSONable(AnnotationCounter, "CreateAnnotationCommand", getPointsFromLine(LineAnnotation.Points.ToArray(), null), null, null);
                    AnnotationCounter++;
                    ResetLineAnnotation();*/               
                }
            }
        }

        private void LineDrawing(object sender, PointerRoutedEventArgs e)
        {
             if (buttonLines.IsChecked.Value)
            {
                if(AnnotationThreshold%10==0)
                {
                    LineAnnotation.Points.Add(new Point(e.GetCurrentPoint(drawingPanel).Position.X, e.GetCurrentPoint(drawingPanel).Position.Y));
                }
                AnnotationThreshold++;
            }
        }

        private void imagesPanelTapped(object sender, TappedRoutedEventArgs e)
        {
            if (myController.GetIconAnnotationSelectedFlag())
            {
                CreateIconAnnotation(e);

                myController.SetIconAnnotationSelectedFlag(false);
                myController.SetSelectedIconPath("");
            }
            else if (buttonPoints.IsChecked.Value)
            {
                if (LineAnnotation.Points.Count > 0)
                {
                    // Prepare annotation to send
                    /*myJsonManager.createJSONable(AnnotationCounter, "CreateAnnotationCommand", getPointsFromLine(LineAnnotation.Points.ToArray(), null), null, null);
                    AnnotationCounter++;
                    ResetLineAnnotation();*/
                }
                    PreparePointAnnotation(e.GetPosition(drawingPanel));
            }
        }

        private void PointerOverTrashBin(object sender, PointerRoutedEventArgs e)
        {
            TrashBinOpenImage.Opacity = 1;
            TrashBinClosedImage.Opacity = 0;
        }

        private void PointerLeftTrashBin(object sender, PointerRoutedEventArgs e)
        {
            TrashBinOpenImage.Opacity = 0;
            TrashBinClosedImage.Opacity = 1;
        }

        private void LinesButtonChecked(object sender, RoutedEventArgs e)
        {
            buttonLinesBorder.Background = buttonCheckedColor;
            buttonPoints.IsChecked = false;
        }

        private void LinesButtonUnchecked(object sender, RoutedEventArgs e)
        {
            buttonLinesBorder.Background = buttonUncheckedColor;
        }

        private void PointsButtonChecked(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine(ApplicationData.Current.LocalFolder.Path);
            buttonPointsBorder.Background = buttonCheckedColor;
            buttonLines.IsChecked = false;
        }

        private void PointsButtonUnchecked(object sender, RoutedEventArgs e)
        {
            buttonPointsBorder.Background = buttonUncheckedColor;
        }

        private void EraseAllButtonClicked(object sender, RoutedEventArgs e)
        {
            foreach (UIElement element in imagesPanel.Children)
            {
                Image thisImage = element as Image;
                myJsonManager.createJSONable(Int32.Parse(thisImage.Name), "DeleteAnnotationCommand", null, null, null);
            }
            foreach (UIElement element in drawingPanel.Children)
            {
                Polyline thisLine = element as Polyline;
                myJsonManager.createJSONable(Int32.Parse(thisLine.Name), "DeleteAnnotationCommand", null, null, null);
            }

            imagesPanel.Children.Clear();
            drawingPanel.Children.Clear();
        }

        private async void ExitButtonClicked(object sender, RoutedEventArgs e)
        {
            if (connectionHappened)
            {
                try
                {
                    StorageFile file = await rootFolder.GetFileAsync("Image.png");
                    await file.DeleteAsync();
                }
                catch (IOException exception)
                {

                }

                deleteTempFiles();
            }

            Application.Current.Exit();
        }

        /*
         * Method Overview: Creates the color to represent button clicks
         * Parameters: None
         * Return: None
         */
        private void CreateButtonsColor()
        {
            Windows.UI.Color tempRGBAColor;
            tempRGBAColor.R = 29;
            tempRGBAColor.G = 122;
            tempRGBAColor.B = 216;
            tempRGBAColor.A = 255;
            buttonCheckedColor = new SolidColorBrush(tempRGBAColor);

            tempRGBAColor.R = 182;
            tempRGBAColor.G = 175;
            tempRGBAColor.B = 175;
            tempRGBAColor.A = 255;
            buttonUncheckedColor = new SolidColorBrush(tempRGBAColor);
        }

        /*
         * Method Overview: Resets the element used to represent lines
         * Parameters: None
         * Return: None
         */
        private void ResetLineAnnotation()
        {
            AnnotationThreshold = 0;

            // Touch manipulation handlers for the line
            LineAnnotation = new Polyline();
            LineAnnotation.Name = AnnotationCounter.ToString();
            LineAnnotation.Stroke = new SolidColorBrush(Windows.UI.Colors.Aquamarine);
            LineAnnotation.StrokeThickness = 7;
            LineAnnotation.HorizontalAlignment = HorizontalAlignment.Left;
            LineAnnotation.VerticalAlignment = VerticalAlignment.Top;

            // Touch manipulation handlers for the line
            LineAnnotation.ManipulationStarted += IconImage_ManipulationStarted;
            LineAnnotation.ManipulationDelta += IconImage_ManipulationDelta;
            LineAnnotation.ManipulationCompleted += IconImage_ManipulationCompleted;
            LineAnnotation.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            LineAnnotation.RenderTransform = new CompositeTransform();

            drawingPanel.Children.Add(LineAnnotation);
        }

        private async void deleteTempFiles()
        {
            StorageFile file;
            bool proceed = true;
            int fileCtr = 2;
            while (proceed)
            {
                string filePath = "Image (" + fileCtr.ToString() + ").png";

                try
                {
                    file = await rootFolder.GetFileAsync(filePath);
                }
                catch (IOException exception)
                {
                    //Debug.WriteLine("Deleted " + fileCtr.ToString() + " images");
                    break;
                }

                await file.DeleteAsync();
                fileCtr++;
            }
        }

        private void IconImage_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            buttonLines.IsChecked = false;
            buttonPoints.IsChecked = false;
            LinesButtonUnchecked(sender, e);
            PointsButtonUnchecked(sender, e);

            /*dynamic contextualizedObject = Convert.ChangeType(sender, sender.GetType());
            contextualizedObject.Opacity = 0.4;*/
            if ("Windows.UI.Xaml.Controls.Image" == sender.GetType().ToString())
            {
                Image selectedElement = sender as Image;
                selectedElement.Opacity = 0.4;
            }
            else
            {
                Polyline selectedElement = sender as Polyline;
                selectedElement.Opacity = 0.4;
            }
        }

        private void IconImage_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            CompositeTransform myTransform;
            /*dynamic contextualizedObject = Convert.ChangeType(sender, sender.GetType());
            myTransform = (CompositeTransform)contextualizedObject.RenderTransform;*/

            if ("Windows.UI.Xaml.Controls.Image" == sender.GetType().ToString())
            {
                Image selectedElement = sender as Image;
                myTransform = selectedElement.RenderTransform as CompositeTransform;
            }
            else
            {
                Polyline selectedElement = sender as Polyline;
                myTransform = selectedElement.RenderTransform as CompositeTransform;
            }

            myTransform.Rotation += e.Delta.Rotation;
            myTransform.ScaleX *= e.Delta.Scale;
            myTransform.ScaleY *= e.Delta.Scale;
            myTransform.TranslateX += e.Delta.Translation.X / transformationHandler.ScaleX;
            myTransform.TranslateY += e.Delta.Translation.Y / transformationHandler.ScaleY;
        }

        /*
         * Method Overview: Handler of the release touch event
         * Parameters: Object that generated the event, position of the event
         * Return: None
         */
        private void IconImage_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            // If the edited object was an icon annotation
            if ("Windows.UI.Xaml.Controls.Image" == sender.GetType().ToString())
            {
                // Return its opacity to full
                Image selectedElement = sender as Image;
                selectedElement.Opacity = 1;

                // Manipulation ended over the trash bin
                if (TrashBinOpenImage.Opacity == 1)
                {
                    // get the current position of the object
                    int idx = imagesPanel.Children.IndexOf(selectedElement);
                    imagesPanel.Children.RemoveAt(idx);

                    // Prepare annotation to send
                    myJsonManager.createJSONable(Int32.Parse(selectedElement.Name), "DeleteAnnotationCommand", null, null, null);
                }

                // Rest of the screen
                else
                {
                    // get the current position of the object
                    var ttv = selectedElement.TransformToVisual(imagesPanel);
                    Point screenCoords = ttv.TransformPoint(new Point(0, 0));

                    // Creates the information for the JSON
                    List<double> annotation_information = new List<double>();
                    annotation_information.Add(screenCoords.X);
                    annotation_information.Add(screenCoords.Y);
                    CompositeTransform myTransf = selectedElement.RenderTransform as CompositeTransform;
                    annotation_information.Add(myTransf.Rotation);
                    annotation_information.Add(myTransf.ScaleX);
                    BitmapImage myBitmap = selectedElement.Source as BitmapImage;

                    // Start JSON creation process
                    myJsonManager.createJSONable(Int32.Parse(selectedElement.Name), "UpdateAnnotationCommand", null, RetrieveAnnotationName(myBitmap.UriSource), annotation_information);
                }
            }

            // Same as before, but if the object is a Line
            else
            {
                Polyline selectedElement = sender as Polyline;
                selectedElement.Opacity = 1;

                if (TrashBinOpenImage.Opacity == 1)
                {
                    int idx = drawingPanel.Children.IndexOf(selectedElement);
                    drawingPanel.Children.RemoveAt(idx);

                    myJsonManager.createJSONable(Int32.Parse(selectedElement.Name), "DeleteAnnotationCommand", null, null, null);
                }
                else
                {
                    var ttv = selectedElement.TransformToVisual(drawingPanel);
                    Point screenCoords = ttv.TransformPoint(new Point(0, 0));


                    myJsonManager.createJSONable(Int32.Parse(selectedElement.Name), "UpdateAnnotationCommand", getPointsFromLine(selectedElement.Points.ToArray(), screenCoords), null, null);
                }
            }
        }

        /*
         * Method Overview: Extracts the name of the icon based on an Uri
         * Parameters: Uri the icon image
         * Return: Name of the icon image
         */
        private string RetrieveAnnotationName(Uri iconUri)
        {
            //greetingOutput.Text = iconUri.ToString();
            string path = iconUri.ToString();
            string[] parts = path.Split('.');
            string[] finalParts = parts.First().Split('/');
            return finalParts.Last();
        }

        private List<double> getPointsFromLine(Point[] myPoints, object offset)
        {
            double offsetX, offsetY;
            if (offset == null)
            {
                offsetX = 0;
                offsetY = 0;
            }
            else
            {
                Point myOffset = (Point)offset;
                offsetX = myOffset.X;
                offsetY = myOffset.Y;
            }

            List<double> pointsToJSON = new List<double>();

            foreach (Point myPoint in myPoints)
            {
                pointsToJSON.Add(myPoint.X + offsetX);
                pointsToJSON.Add(myPoint.Y + offsetY);
            }

            return pointsToJSON;
        }
        
        private void JsonThread()
        {
            myJsonManager.constructGeneralJSON();
        }

        /*
         * Method Overview: Given a tapped event position, create a icon annotation
         * Parameters: Tapped event
         * Return: None
         */
        private void CreateIconAnnotation(TappedRoutedEventArgs e)
        {
            // Annotation path retrieval
            Uri iconUri = new Uri(myController.GetSelectedIconPath());
            BitmapImage iconBitmap = new BitmapImage(iconUri);
            Image iconImage = new Image();
            iconImage.Source = iconBitmap;

            // Annotation dimension and position
            Point tappedPosition = e.GetPosition(imagesPanel);
            iconImage.HorizontalAlignment = HorizontalAlignment.Left;
            iconImage.VerticalAlignment = VerticalAlignment.Top;
            iconImage.Width = 1920 * 0.09f;
            iconImage.Name = AnnotationCounter.ToString();
            imagesPanel.Children.Add(iconImage);
            iconImage.Margin = new Thickness(tappedPosition.X, tappedPosition.Y, 0, 0);

            // Annotation manipulators
            iconImage.ManipulationStarted += IconImage_ManipulationStarted;
            iconImage.ManipulationDelta += IconImage_ManipulationDelta;
            iconImage.ManipulationCompleted += IconImage_ManipulationCompleted;
            iconImage.ManipulationMode = ManipulationModes.Rotate | ManipulationModes.Scale | ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            CompositeTransform myTransf = new CompositeTransform();
            myTransf.ScaleX = 1;
            myTransf.ScaleY = 1;
            myTransf.Rotation = -45;
            iconImage.RenderTransform = myTransf;

            // Stores the important info of the annotation for the JSON
            List<double> annotation_information = new List<double>();
            annotation_information.Add(tappedPosition.X);
            annotation_information.Add(tappedPosition.Y);
            annotation_information.Add(myTransf.Rotation);
            annotation_information.Add(myTransf.ScaleX);

            // Starts the JSON annotation creation process
            myJsonManager.createJSONable(AnnotationCounter, "CreateAnnotationCommand", null, RetrieveAnnotationName(iconUri), annotation_information);
            AnnotationCounter++;
        }

         /*
         * Method Overview: Given a tapped event position, create a circle around it
         * Parameters: Tapped event
         * Return: None
         */
        private void PreparePointAnnotation(Point e)
        {
            double initial_point_distance = 5.0;
            double angle;
            double val = PI / 180.0f;

            int counter;

            //create enough points to make a round shape
            for (counter = 0; counter <= 360; counter = counter + 18)
            {
                //gets the new angle value
                angle = counter * val;

                //Calculates trigonometric values of the point
                double cosComponent = Math.Cos(angle) * initial_point_distance;
                double senComponent = Math.Sin(angle) * initial_point_distance;

                //Calculates the new point
                double transfX = ((cosComponent) - (senComponent));
                double transfY = ((senComponent) + (cosComponent));

                //assigns the results
                LineAnnotation.Points.Add(new Point((transfX) + (e.X), (transfY) + (e.Y)));
            }
        }

        private void ZoomBackgroundImage(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            transformationHandler.CenterX = newPointInBackgroundImage.X;
            transformationHandler.CenterY = newPointInBackgroundImage.Y;

            if (transformationHandler.ScaleX * e.Delta.Scale < 1.0)
            {
                transformationHandler.ScaleX = 1.0;
                transformationHandler.ScaleY = 1.0;
            }
            else
            {
                transformationHandler.ScaleX *= e.Delta.Scale;
                transformationHandler.ScaleY *= e.Delta.Scale;
            }
        }

        private void FingerLeft(object sender, PointerRoutedEventArgs e)
        {
            if(buttonLines.IsChecked.Value || buttonPoints.IsChecked.Value)
            {
                myJsonManager.createJSONable(AnnotationCounter, "CreateAnnotationCommand", getPointsFromLine(LineAnnotation.Points.ToArray(), null), null, null);
                AnnotationCounter++;
                ResetLineAnnotation();
            }
        }
    }
}
